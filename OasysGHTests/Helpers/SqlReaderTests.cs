using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OasysGH.Helpers;
using Xunit;

namespace OasysGHTests.Helpers {
  [Collection("GrasshopperFixture collection")]
  public class SqlReaderTests {
    private static readonly string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "lib", "sectlib.db3");

    [Theory]
    [InlineData("IPE100", new double[5] { 0.1, 0.055, 0.0041, 0.0057, 0.007 })]
    [InlineData("CHS457x12.5", new double[2] { 0.457, 0.0125 })]
    [InlineData("RHS100x50x10", new double[3] { 0.1, 0.05, 0.01 })]
    [InlineData("RHS100x50x10.0", new double[3] { 0.1, 0.05, 0.01 })]
    public void GetCatalogueProfileValuesTest(string profileString, double[] expectedValues) {
      List<double> values = SqlReader.Instance.GetCatalogueProfileValues(profileString, filePath);

      Assert.Equal(expectedValues.Length, values.Count);

      for (int i = 0; i < expectedValues.Length; i++)
        Assert.True(Math.Abs(expectedValues[i] - values[i]) < 1e-6,
          $"Expected: {expectedValues[i]}, actual: {values[i]}");
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
  }
}
