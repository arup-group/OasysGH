using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Data.Sqlite;
using OasysGH.Helpers;
using Xunit;

namespace OasysGHTests.Helpers {
  [Collection("GrasshopperFixture collection")]
  public class SqlReaderTests {
    private static readonly string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "lib", "sectlib.db3");

    [Theory]
    [InlineData("IPE100", new double[5] { 0.1, 0.055, 0.0041, 0.0057, 0.007 })] // x x x x x
    [InlineData("CHS457x12.5", new double[2] { 0.457, 0.0125 })] // - x x - -
    [InlineData("RHS100x50x10", new double[3] { 0.1, 0.05, 0.01 })] // x x x - -
    [InlineData("RHS100x50x10.0", new double[3] { 0.1, 0.05, 0.01 })]
    [InlineData("EA80x80x8", new double[4] { 0.080000, 0.080000, 0.008, 0.01 })] // x x x - x 
    [InlineData("EHS200x100x6.3", new double[3] { 0.2, 0.1, 0.0063 })] // x x x - - 
    [InlineData("45x45x3EA", new double[5] { 0.045, 0.045, 0.003, 0.003, 0.005 })]
    [InlineData("HSS20x4x0.25", new double[3] { 0.508, 0.1016, 0.005918 })] // x x x - - 
    [InlineData("UB686x254x140", new double[5] { 0.6835001, 0.2537, 0.0124, 0.019, 0.0152 })]
    [InlineData("HE550.B", new double[5] { 0.55, 0.3, 0.015, 0.029, 0.027 })]
    [InlineData("UA75x50x8.0", new double[5] { 0.075, 0.05, 0.008, 0.008, 0.007 })]
    [InlineData("ISMB450", new double[5] { 0.45, 0.15, 0.0094, 0.0174, 0.015 })]
    [InlineData("70.B1", new double[5] { 0.691, 0.26, 0.012, 0.0155, 0.024 })]
    [InlineData("TUB419x457x194", new double[5] { 0.4602, 0.4205, 0.0215, 0.0366, 0.0241 })]
    [InlineData("SHS400x400x16", new double[3] { 0.4, 0.4, 0.016 })]
    [InlineData("E-HP200x200x43", new double[5] { 0.2, 0.205, 0.009000001, 0.009000001, 0.01 })]
    [InlineData("CHS508x12.5", new double[2] { 0.508, 0.0125 })]
    [InlineData("EA250x250x35", new double[4] { 0.25, 0.25, 0.035, 0.02 })]
    [InlineData("CUB914x419x388", new double[4] { 1.3777, 0.4205, 0.0215, 0.0366 })]
    [InlineData("PX0.5", new double[2] { 0.021336, 0.0037338 })] // - x x - -
    [InlineData("10RND", new double[1] { 0.01 })] // - x - - - 
    [InlineData("HSS14x0.197", new double[2] { 0.3556, 0.0050038 })] // empty x x - -
    public void GetCatalogueProfileValuesTest(string profileString, double[] expectedValues) {
      List<double> values = SqlReader.Instance.GetCatalogueProfileValues(profileString, filePath);
      Assert.Equal(expectedValues.Length, values.Count);

      for (int i = 0; i < expectedValues.Length; i++)
        Assert.True(Math.Abs(expectedValues[i] - values[i]) < 1e-6,
          $"Expected: {expectedValues[i]}, actual: {values[i]}");
    }

    [Fact]
    public void GetCatalogueProfileValues_InvalidFilePath_ThrowsException() =>
      Assert.Throws<SqliteException>(() => SqlReader.Instance.GetCatalogueProfileValues("IPE100", "invalid_path.db3"));

    [Fact]
    public void GetCataloguesDataFromSQLiteTest() {
      Tuple<List<string>, List<int>> result = SqlReader.Instance.GetCataloguesDataFromSQLite(filePath);

      Assert.NotNull(result);
      Assert.NotNull(result.Item1);
      Assert.NotNull(result.Item2);
      Assert.True(result.Item1.Count > 0);
      Assert.True(result.Item2.Count > 0);
      Assert.Equal("All", result.Item1[0]);
      Assert.Equal(-1, result.Item2[0]);
    }

    [Fact]
    public void GetCataloguesDataFromSQLite_InvalidFilePath_ThrowsException() =>
      Assert.Throws<SqliteException>(() => SqlReader.Instance.GetCataloguesDataFromSQLite("invalid_path.db3"));

    [Fact]
    public void GetTypesDataFromSQLiteTest() {
      Tuple<List<string>, List<int>> result = SqlReader.Instance.GetTypesDataFromSQLite(-1, filePath);

      Assert.NotNull(result);
      Assert.NotNull(result.Item1);
      Assert.NotNull(result.Item2);
      Assert.True(result.Item1.Count > 0);
      Assert.True(result.Item2.Count > 0);
      Assert.Equal("All", result.Item1[0]);
      Assert.Equal(-1, result.Item2[0]);
    }

    [Fact]
    public void GetTypesDataFromSQLite_WithSuperseeded_ReturnsMoreResults() {
      Tuple<List<string>, List<int>> resultWithoutSuperseeded =
        SqlReader.Instance.GetTypesDataFromSQLite(-1, filePath, false);
      Tuple<List<string>, List<int>> resultWithSuperseeded =
        SqlReader.Instance.GetTypesDataFromSQLite(-1, filePath, true);

      Assert.True(resultWithSuperseeded.Item1.Count >= resultWithoutSuperseeded.Item1.Count);
    }

    [Fact]
    public void GetTypesDataFromSQLite_SpecificCatalogue_ReturnsFilteredResults() {
      Tuple<List<string>, List<int>> catalogues = SqlReader.Instance.GetCataloguesDataFromSQLite(filePath);
      if (catalogues.Item2.Count > 1) {
        int validCatalogueNumber = catalogues.Item2[1]; // Skip "All" (-1)

        Tuple<List<string>, List<int>> result =
          SqlReader.Instance.GetTypesDataFromSQLite(validCatalogueNumber, filePath);

        Assert.NotNull(result);
        Assert.True(result.Item1.Count > 0);
      }
    }

    [Fact]
    public void GetSectionsDataFromSQLiteTest() {
      var types = new List<int> { -1 };
      List<string> result = SqlReader.Instance.GetSectionsDataFromSQLite(types, filePath);

      Assert.NotNull(result);
      Assert.True(result.Count > 0);
      Assert.Equal("All", result[0]);
    }

    [Fact]
    public void GetSectionsDataFromSQLite_SpecificTypes_ReturnsFilteredResults() {
      Tuple<List<string>, List<int>> typesData = SqlReader.Instance.GetTypesDataFromSQLite(-1, filePath);
      Assert.True(typesData.Item1.Count > 1);
      var typeNumbers = new List<int> { typesData.Item2[1] };
      List<string> result = SqlReader.Instance.GetSectionsDataFromSQLite(typeNumbers, filePath);
      Assert.NotNull(result);
      Assert.True(result.Count > 0);
    }

    [Fact]
    public void GetSectionsDataFromSQLite_WithSuperseeded_ReturnsMoreResults() {
      var types = new List<int> { -1 };
      List<string> resultWithoutSuperseeded = SqlReader.Instance.GetSectionsDataFromSQLite(types, filePath, false);
      List<string> resultWithSuperseeded = SqlReader.Instance.GetSectionsDataFromSQLite(types, filePath, true);

      Assert.True(resultWithSuperseeded.Count >= resultWithoutSuperseeded.Count);
    }

    [Fact]
    public void ConnectionTest() {
      using (SqliteConnection connection = SqlReader.Instance.Connection(filePath)) {
        Assert.NotNull(connection);
        Assert.Contains(filePath, connection.ConnectionString);
        Assert.Contains("Mode=ReadOnly", connection.ConnectionString);
      }
    }

    [Fact]
    public void Connection_InvalidPath_ThrowsOnOpen() {
      using (SqliteConnection connection = SqlReader.Instance.Connection("invalid_path.db3")) {
        Assert.Throws<SqliteException>(() => connection.Open());
      }
    }

    [Fact]
    public void AppDomainTest() {
      string codeBase = Assembly.GetCallingAssembly().CodeBase;
      var uri = new UriBuilder(codeBase);
      string codeBasePath = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));

      AppDomain appDomain = SqlReader.CreateSecondAppDomain(codeBasePath);

      Assert.NotNull(appDomain);
    }

    [Fact]
    public void InitializeLifetimeServiceTest() {
      Assert.Null(SqlReader.Instance.InitializeLifetimeService());
    }

    [Fact]
    public void InitializeTest() {
      var reader = SqlReader.Initialize();
      Assert.NotNull(reader);
    }

    [Fact]
    public void SingletonInstanceTest() {
      SqlReader instance1 = SqlReader.Instance;
      SqlReader instance2 = SqlReader.Instance;

      Assert.NotNull(instance1);
      Assert.NotNull(instance2);
      Assert.Same(instance1, instance2);
    }
  }
}
