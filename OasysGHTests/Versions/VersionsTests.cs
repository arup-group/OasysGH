using System;
using Xunit;
using OasysGH;

namespace OasysGHTests.Versions {
  [Collection("GrasshopperFixture collection")]
  public class VersionsTests {

    [Fact]
    public void IsPluginOutdatedTest() {
      var unitNumberGuid = new Guid("6080a841-4f35-4182-9922-f40a66977a69");
      Assert.False(OasysGH.Versions.Versions.IsPluginOutdated(unitNumberGuid));
    }

    [Theory]
    [InlineData("0.0.1", true)]
    [InlineData("0.2.2147483647", true)]
    [InlineData("2.0", false)]
    public void IsVersionOutdatedTest(string version, bool expected) {
      var v = new Version(version);
      Assert.Equal(expected, OasysGH.Versions.Versions.IsVersionOutdated(v));
    }

    [Theory]
    [InlineData("0.6.13-beta", 0, 6, 13, 0)]
    [InlineData("1.0.0", 1, 0, 0, -1)]
    public void CreateVersionTest(string versionString, int major, int minor, int build, int revision) {
      Version versionFromString = OasysGH.Versions.Versions.CreateVersion(versionString);
      Assert.Equal(major, versionFromString.Major);
      Assert.Equal(minor, versionFromString.Minor);
      Assert.Equal(build, versionFromString.Build);
      Assert.Equal(revision, versionFromString.Revision);
    }

    [Fact]
    public void GetOasysGhVersion() {
      Version oasyGhVersion = OasysGH.Versions.Versions.GetOasysGhVersion();
      Assert.Equal(OasysGHVersion.Version, oasyGhVersion.ToString());
    }
  }
}
