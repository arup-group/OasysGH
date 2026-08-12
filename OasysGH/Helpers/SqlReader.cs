using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.Data.Sqlite;

namespace OasysGH.Helpers {
  /// <summary>
  /// Singleton that reads data from a SQLite .db3 file.
  /// When running inside Grasshopper/Rhino, SQLite is loaded in an isolated AppDomain to avoid
  /// version conflicts with other plugins. Method calls are forwarded to that domain via reflection.
  /// </summary>
  public class SqlReader : MarshalByRefObject {
    public static SqlReader Instance => lazy.Value;
    private static readonly Lazy<SqlReader> lazy = new Lazy<SqlReader>(() => Initialize());

    // Non-null only in the main-domain wrapper. Null when this instance IS the remote worker.
    private readonly object _remoteProxy;

    static SqlReader() {
      AppDomain.CurrentDomain.AssemblyResolve += ResolveSQLitePCLRaw;
    }

    public SqlReader() {
      try {
        SQLitePCL.Batteries.Init();
      }
      catch {
      }
    }

    private SqlReader(object remoteProxy) {
      _remoteProxy = remoteProxy;
    }

    /// <summary>
    /// Calls <paramref name="method"/> on the remote-domain proxy via reflection.
    /// The transparent proxy intercepts the call and routes it to the isolated AppDomain,
    /// where SQLite executes with the correct library version. The return value crosses back
    /// as a serializable copy. Only valid when <see cref="_remoteProxy"/> is non-null.
    /// dynamic dispatch cannot be used here — it does not work on transparent proxies in .NET Framework.
    /// </summary>
    private T Invoke<T>(string method, params object[] args) {
      MethodInfo methodInfo = _remoteProxy.GetType().GetMethod(method);
      object result;
      try {
        result = methodInfo.Invoke(_remoteProxy, args);
      }
      catch (TargetInvocationException ex) when (ex.InnerException != null) {
        ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
        throw;
      }
      return (T)result;
    }

    // Handles any assembly version mismatch: loads whatever version is present on disk.
    private static Assembly ResolveSQLitePCLRaw(object sender, ResolveEventArgs args) {
      string dir = Path.GetDirectoryName(typeof(SqlReader).Assembly.Location)
                   ?? AppDomain.CurrentDomain.BaseDirectory;
      string path = Path.Combine(dir, new AssemblyName(args.Name).Name + ".dll");
      return File.Exists(path) ? Assembly.LoadFrom(path) : null;
    }

    public static SqlReader Initialize() {
      string codeBasePath = Path.GetDirectoryName(typeof(SqlReader).Assembly.Location);
      if (string.IsNullOrEmpty(codeBasePath)) {
        codeBasePath = AppDomain.CurrentDomain.BaseDirectory;
      }

      // Use isolated AppDomain only in Rhino host process. Testhost/VS can load Grasshopper
      // assemblies too, but does not need plugin-style isolation and should stay in-process.
      bool inPluginHost = AppDomain.CurrentDomain.GetAssemblies()
        .Any(a => a.GetName().Name == "Grasshopper" || a.GetName().Name == "RhinoCommon");
     
      if (!inPluginHost) {
        try {
          Assembly.LoadFile(Path.Combine(codeBasePath, "Microsoft.Data.Sqlite.dll"));
          using (var testConnection = new SqliteConnection("Data Source=:memory:")) {
            testConnection.Open();
            testConnection.Close();
          }

          return new SqlReader();
        }
        catch (Exception) { }
      }

      // Retrieve the remote-domain instance as 'object' — never cast to SqlReader.
      // .NET Framework resolves transparent-proxy casts via Assembly.Load (not LoadFrom),
      // which can be intercepted by Grasshopper's resolver and return a different binary,
      // making the identity check fail. Wrapping in a local SqlReader avoids any cast.
      string assemblyFile = typeof(SqlReader).Assembly.Location;
      AppDomain appDomain = CreateSecondAppDomain(codeBasePath);
      object proxy = appDomain.CreateInstanceFromAndUnwrap(assemblyFile, typeof(SqlReader).FullName);
      return new SqlReader(proxy);
    }

    /// <summary>Opens a read-only SQLite connection to <paramref name="filePath"/>.</summary>
    public SqliteConnection Connection(string filePath) {
      string connectionString = $"Data Source={filePath};Mode=ReadOnly";
      return new SqliteConnection(connectionString);
    }

    /// <summary>
    /// Returns section dimensions (m) for a profile name: [0] depth, [1] width, [2] web thk,
    /// [3] flange thk, [4] root radius (welded sections omit [4]).
    /// </summary>
    public List<double> GetCatalogueProfileValues(string profileString, string filePath) {
      if (_remoteProxy != null)
        return Invoke<List<double>>(nameof(GetCatalogueProfileValues), profileString, filePath);

      var values = new List<double>();

      using (SqliteConnection db = Connection(filePath)) {
        db.Open();
        SqliteCommand cmd = db.CreateCommand();

        cmd.CommandText =
          "Select SECT_DEPTH_DIAM || ' -- ' || IFNULL(SECT_WIDTH, '') || ' -- ' || IFNULL(SECT_WEB_THICK, '') || ' -- ' || IFNULL(SECT_FLG_THICK, '') || ' -- ' || IFNULL(SECT_ROOT_RAD, '') as SECT_NAME from Sect INNER JOIN Types ON Sect.SECT_TYPE_NUM = Types.TYPE_NUM where SECT_NAME = @profile ORDER BY SECT_DATE_ADDED;";

        cmd.CommandType = CommandType.Text;

        cmd.Parameters.AddWithValue("@profile", profileString);

        var data = new List<string>();

        using (SqliteDataReader r = cmd.ExecuteReader()) {
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
      if (_remoteProxy != null)
        return Invoke<Tuple<List<string>, List<int>>>(nameof(GetCataloguesDataFromSQLite), filePath);

      // Create empty lists to work on:
      var catNames = new List<string>();
      var catNumber = new List<int>();

      using (SqliteConnection db = Connection(filePath)) {
        db.Open();
        SqliteCommand cmd = db.CreateCommand();
        cmd.CommandText = @"Select CAT_NAME || ' -- ' || CAT_NUM as CAT_NAME from Catalogues";

        cmd.CommandType = CommandType.Text;
        SqliteDataReader r = cmd.ExecuteReader();
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
      if (_remoteProxy != null)
        return Invoke<List<string>>(nameof(GetSectionsDataFromSQLite), type_numbers, filePath, inclSuperseeded);

      // Create empty list to work on:
      var sections = new List<string>();

      List<int> types;
      if (type_numbers[0] == -1) {
        Tuple<List<string>, List<int>> typeData = GetTypesDataFromSQLite(-1, filePath, inclSuperseeded);
        types = typeData.Item2;
        types.RemoveAt(0); // remove -1 from beginning of list
      } else {
        types = type_numbers;
      }

      using (SqliteConnection db = Connection(filePath)) {
        // get section name
        for (int i = 0; i < types.Count; i++) {
          int type = types[i];
          db.Open();
          SqliteCommand cmd = db.CreateCommand();

          if (inclSuperseeded)
            cmd.CommandText = $"Select Types.TYPE_ABR || ' ' || SECT_NAME || ' -- ' || SECT_DATE_ADDED as SECT_NAME from Sect INNER JOIN Types ON Sect.SECT_TYPE_NUM = Types.TYPE_NUM where SECT_TYPE_NUM = {type} ORDER BY SECT_AREA";
          else
            cmd.CommandText = $"Select Types.TYPE_ABR || ' ' || SECT_NAME as SECT_NAME from Sect INNER JOIN Types ON Sect.SECT_TYPE_NUM = Types.TYPE_NUM where SECT_TYPE_NUM = {type} and not (SECT_SUPERSEDED = True or SECT_SUPERSEDED = TRUE or SECT_SUPERSEDED = 1) ORDER BY SECT_AREA";

          cmd.CommandType = CommandType.Text;
          SqliteDataReader r = cmd.ExecuteReader();
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
      if (_remoteProxy != null)
        return Invoke<Tuple<List<string>, List<int>>>(nameof(GetTypesDataFromSQLite), catalogue_number, filePath, inclSuperseeded);

      // Create empty lists to work on:
      var typeNames = new List<string>();
      var typeNumber = new List<int>();

      // get Catalogue numbers if input is -1 (All catalogues)
      var catNumbers = new List<int>();
      if (catalogue_number == -1) {
        Tuple<List<string>, List<int>> catalogueData = GetCataloguesDataFromSQLite(filePath);
        catNumbers = catalogueData.Item2;
        catNumbers.RemoveAt(0); // remove -1 from beginning of list
      } else {
        catNumbers.Add(catalogue_number);
      }

      using (SqliteConnection db = Connection(filePath)) {
        for (int i = 0; i < catNumbers.Count; i++) {
          int cat = catNumbers[i];

          db.Open();
          SqliteCommand cmd = db.CreateCommand();
          if (inclSuperseeded)
            cmd.CommandText = $"Select TYPE_NAME || ' -- ' || TYPE_NUM as TYPE_NAME from Types where TYPE_CAT_NUM = {cat}";
          else
            cmd.CommandText = $"Select TYPE_NAME || ' -- ' || TYPE_NUM as TYPE_NAME from Types where TYPE_CAT_NUM = {cat} and not (TYPE_SUPERSEDED = True or TYPE_SUPERSEDED = TRUE or TYPE_SUPERSEDED = 1)";
          cmd.CommandType = CommandType.Text;
          SqliteDataReader r = cmd.ExecuteReader();
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

    public override object InitializeLifetimeService() {
      // keep proxy object lives until the AppDomain unloads.
      return null;
    }

    internal static AppDomain CreateSecondAppDomain(string codeBasePath) {
      var ads = new AppDomainSetup {
        ApplicationBase = codeBasePath,
        DisallowBindingRedirects = false,
        DisallowCodeDownload = true,
        ConfigurationFile = null,
      };

      return AppDomain.CreateDomain("SQLite AppDomain", null, ads);
    }
  }
}
