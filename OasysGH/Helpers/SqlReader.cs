using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Data.SQLite;

namespace OasysGH.Helpers {
  /// <summary>
  /// Singleton that reads data from a SQLite .db3 file.
  /// When running inside Grasshopper/Rhino, SQLite is loaded in an isolated AppDomain to avoid
  /// version conflicts with other plugins. Method calls are forwarded to that domain via reflection.
  /// </summary>
  public class SqlReader {
    public static SqlReader Instance => lazy.Value;
    private static readonly Lazy<SqlReader> lazy = new Lazy<SqlReader>(() => new SqlReader());

    private SqlReader() {
    }

    
    /// <summary>Opens a read-only SQLite connection to <paramref name="filePath"/>.</summary>
    public SQLiteConnection Connection(string filePath) {
      string connectionString = $"Data Source={filePath};Version=3;Read Only=True;";
      return new SQLiteConnection(connectionString);
    }

    /// <summary>
    /// Returns section dimensions (m) for a profile name: [0] depth, [1] width, [2] web thk,
    /// [3] flange thk, [4] root radius (welded sections omit [4]).
    /// </summary>
    public List<double> GetCatalogueProfileValues(string profileString, string filePath) {
       var values = new List<double>();

      using (SQLiteConnection db = Connection(filePath)) {
        db.Open();
        SQLiteCommand cmd = db.CreateCommand();

        cmd.CommandText =
          "Select SECT_DEPTH_DIAM || ' -- ' || IFNULL(SECT_WIDTH, '') || ' -- ' || IFNULL(SECT_WEB_THICK, '') || ' -- ' || IFNULL(SECT_FLG_THICK, '') || ' -- ' || IFNULL(SECT_ROOT_RAD, '') as SECT_NAME from Sect INNER JOIN Types ON Sect.SECT_TYPE_NUM = Types.TYPE_NUM where SECT_NAME = @profile ORDER BY SECT_DATE_ADDED;";

        cmd.CommandType = CommandType.Text;

        cmd.Parameters.AddWithValue("@profile", profileString);

        var data = new List<string>();

        using (SQLiteDataReader r = cmd.ExecuteReader()) {
          while (r.Read()) {
            string sqlData = Convert.ToString(r["SECT_NAME"]);
            data.Add(sqlData);
          }
        }

        db.Close();

        // Guard statement to prevent data[0] crash if something goes wrong
        if (data.Count == 0) return values;

        string[] vals = data[0].Split(new string[] { " -- " }, StringSplitOptions.None);

        NumberFormatInfo noComma = CultureInfo.InvariantCulture.NumberFormat;
        values.AddRange(vals.Where(val => val != "").Select(val => Convert.ToDouble(val, noComma)));
      }

      return values;
    }


    /// <summary>
    /// Returns all catalogues in the db3 file as (names, numbers).
    /// First entry of each list is ("All", -1).
    /// </summary>
    /// <param name="filePath">Path to SecLib.db3</param>
    public Tuple<List<string>, List<int>> GetCataloguesDataFromSQLite(string filePath) {
      // Create empty lists to work on:
      var catNames = new List<string>();
      var catNumber = new List<int>();

      using (SQLiteConnection db = Connection(filePath)) {
        db.Open();
        SQLiteCommand cmd = db.CreateCommand();
        cmd.CommandText = @"Select CAT_NAME || ' -- ' || CAT_NUM as CAT_NAME from Catalogues";

        cmd.CommandType = CommandType.Text;
        SQLiteDataReader r = cmd.ExecuteReader();
        while (r.Read()) {
          // get data
          string sqlData = Convert.ToString(r["CAT_NAME"]);

          // split text string
          // example: British -- 2
          catNames.Add(sqlData.Split(new string[] { " -- " }, StringSplitOptions.None)[0]);
          catNumber.Add(int.Parse(sqlData.Split(new string[] { " -- " }, StringSplitOptions.None)[1]));
        }

        db.Close();
      }

      catNames.Insert(0, "All");
      catNumber.Insert(0, -1);
      return new Tuple<List<string>, List<int>>(catNames, catNumber);
    }

    /// <summary>Returns section profile strings (with GSA type abbreviation) for the given type numbers.</summary>
    /// <param name="type_numbers">Type numbers to query; pass -1 as first element for all types.</param>
    /// <param name="filePath">Path to SecLib.db3</param>
    /// <param name="inclSuperseeded">Include superseded sections when true.</param>
    public List<string> GetSectionsDataFromSQLite(List<int> type_numbers, string filePath, bool inclSuperseeded = false) {
      // Create empty list to work on:
      var sections = new List<string>();

      List<int> types;
      if (type_numbers[0] == -1) {
        Tuple<List<string>, List<int>> typeData = GetTypesDataFromSQLite(-1, filePath, inclSuperseeded);
        types = typeData.Item2;
        types.RemoveAt(0); // remove -1 from beginning of list
      }
      else {
        types = type_numbers;
      }

      using (SQLiteConnection db = Connection(filePath)) {
        // get section name
        for (int i = 0; i < types.Count; i++) {
          int type = types[i];
          db.Open();
          SQLiteCommand cmd = db.CreateCommand();

          if (inclSuperseeded)
            cmd.CommandText = $"Select Types.TYPE_ABR || ' ' || SECT_NAME || ' -- ' || SECT_DATE_ADDED as SECT_NAME from Sect INNER JOIN Types ON Sect.SECT_TYPE_NUM = Types.TYPE_NUM where SECT_TYPE_NUM = {type} ORDER BY SECT_AREA";
          else
            cmd.CommandText = $"Select Types.TYPE_ABR || ' ' || SECT_NAME as SECT_NAME from Sect INNER JOIN Types ON Sect.SECT_TYPE_NUM = Types.TYPE_NUM where SECT_TYPE_NUM = {type} and not (SECT_SUPERSEDED = True or SECT_SUPERSEDED = TRUE or SECT_SUPERSEDED = 1) ORDER BY SECT_AREA";

          cmd.CommandType = CommandType.Text;
          SQLiteDataReader r = cmd.ExecuteReader();
          while (r.Read())
            if (inclSuperseeded) {
              string full = Convert.ToString(r["SECT_NAME"]);
              // BSI-IPE IPEAA80 -- 2017-09-01 00:00:00.000
              string profile = full.Split(new string[] { " -- " }, StringSplitOptions.None)[0];
              string date = full.Split(new string[] { " -- " }, StringSplitOptions.None)[1];
              date = date.Replace("-", "");
              date = date.Substring(0, 8);
              sections.Add(profile + " " + date);
            }
            else {
              string profile = Convert.ToString(r["SECT_NAME"]);
              // BSI-IPE IPEAA80
              sections.Add(profile);
            }

          db.Close();
        }
      }

      sections.Sort();

      sections.Insert(0, "All");

      return sections;
    }

    /// <summary>
    /// Returns section types for a catalogue as (names, numbers).
    /// First entry of each list is ("All", -1).
    /// </summary>
    /// <param name="catalogue_number">Catalogue to query; pass -1 for all catalogues.</param>
    /// <param name="filePath">Path to SecLib.db3</param>
    /// <param name="inclSuperseeded">Include superseded types when true.</param>
    public Tuple<List<string>, List<int>> GetTypesDataFromSQLite(int catalogue_number, string filePath, bool inclSuperseeded = false) {
       // Create empty lists to work on:
      var typeNames = new List<string>();
      var typeNumber = new List<int>();

      // get Catalogue numbers if input is -1 (All catalogues)
      var catNumbers = new List<int>();
      if (catalogue_number == -1) {
        Tuple<List<string>, List<int>> catalogueData = GetCataloguesDataFromSQLite(filePath);
        catNumbers = catalogueData.Item2;
        catNumbers.RemoveAt(0); // remove -1 from beginning of list
      }
      else {
        catNumbers.Add(catalogue_number);
      }

      using (SQLiteConnection db = Connection(filePath)) {
        for (int i = 0; i < catNumbers.Count; i++) {
          int cat = catNumbers[i];

          db.Open();
          SQLiteCommand cmd = db.CreateCommand();
          if (inclSuperseeded)
            cmd.CommandText = $"Select TYPE_NAME || ' -- ' || TYPE_NUM as TYPE_NAME from Types where TYPE_CAT_NUM = {cat}";
          else
            cmd.CommandText = $"Select TYPE_NAME || ' -- ' || TYPE_NUM as TYPE_NAME from Types where TYPE_CAT_NUM = {cat} and not (TYPE_SUPERSEDED = True or TYPE_SUPERSEDED = TRUE or TYPE_SUPERSEDED = 1)";
          cmd.CommandType = CommandType.Text;
          SQLiteDataReader r = cmd.ExecuteReader();
          while (r.Read()) {
            // get data
            string sqlData = Convert.ToString(r["TYPE_NAME"]);

            // split text string
            // example: Universal Beams -- 51
            typeNames.Add(sqlData.Split(new string[] { " -- " }, StringSplitOptions.None)[0]);
            typeNumber.Add(int.Parse(sqlData.Split(new string[] { " -- " }, StringSplitOptions.None)[1]));
          }

          db.Close();
        }
      }

      typeNames.Insert(0, "All");
      typeNumber.Insert(0, -1);
      return new Tuple<List<string>, List<int>>(typeNames, typeNumber);
    }
  }
}
