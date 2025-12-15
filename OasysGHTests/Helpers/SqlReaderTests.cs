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
    private static readonly string filePath = GetDatabaseFilePath();
    
    private static string GetDatabaseFilePath() {
      // First check if the database file is in the current directory (copied during build)
      string currentDirFile = Path.Combine(Directory.GetCurrentDirectory(), "sectlib.db3");
      if (File.Exists(currentDirFile)) {
        return currentDirFile;
      }
      
      // Fall back to the original logic
      return Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "lib", "sectlib.db3");
    }

    [Theory]
    [InlineData("IPE100", new double[5] { 0.1, 0.055, 0.0041, 0.0057, 0.007 })]
    [InlineData("CHS457x12.5", new double[2] { 0.457, 0.0125 })]
    public void GetCatalogueProfileValuesTest(string profileString, double[] expectedValues) {
      List<double> values = SqlReader.Instance.GetCatalogueProfileValues(profileString, filePath);
      Assert.Equal(expectedValues, values); 
    }
  
    [Fact]
    public void GetCatalogueProfileValues_InvalidFilePath_ThrowsException() {
      Assert.Throws<SqliteException>(() => 
        SqlReader.Instance.GetCatalogueProfileValues("IPE100", "invalid_path.db3"));
    }

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
    public void GetCataloguesDataFromSQLite_InvalidFilePath_ThrowsException() {
      Assert.Throws<SqliteException>(() => 
        SqlReader.Instance.GetCataloguesDataFromSQLite("invalid_path.db3"));
    }

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
      Tuple<List<string>, List<int>> resultWithoutSuperseeded = SqlReader.Instance.GetTypesDataFromSQLite(-1, filePath, false);
      Tuple<List<string>, List<int>> resultWithSuperseeded = SqlReader.Instance.GetTypesDataFromSQLite(-1, filePath, true);
      
      Assert.True(resultWithSuperseeded.Item1.Count >= resultWithoutSuperseeded.Item1.Count);
    }
    
    [Fact]
    public void GetTypesDataFromSQLite_SpecificCatalogue_ReturnsFilteredResults() {
      Tuple<List<string>, List<int>> catalogues = SqlReader.Instance.GetCataloguesDataFromSQLite(filePath);
      if (catalogues.Item2.Count > 1) {
        int validCatalogueNumber = catalogues.Item2[1]; // Skip "All" (-1)

        Tuple<List<string>, List<int>> result = SqlReader.Instance.GetTypesDataFromSQLite(validCatalogueNumber, filePath);
        
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
      if (typesData.Item2.Count > 1) {
        var typeNumbers = new List<int> { typesData.Item2[1] }; // Skip "All" (-1)

        List<string> result = SqlReader.Instance.GetSectionsDataFromSQLite(typeNumbers, filePath);
        
        Assert.NotNull(result);
        Assert.True(result.Count > 0);
      }
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
