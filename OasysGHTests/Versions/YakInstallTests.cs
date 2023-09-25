using System.Linq;
using OasysGH.Versions;
using Xunit;
using Yak;

namespace OasysGHTests.Versions {
  [Collection("GrasshopperFixture collection")]
  public class YakInstallTests {

    private static YakClient Yak => yakClient ?? (yakClient = YakInstall.CreateYakClient());
    private static YakClient yakClient = null;

    [Fact]
    public void CreateYakClientTest() {
      Assert.NotNull(Yak);
    }

    [Fact]
    public async void GetPackageFromVersionTest() {
      string name = "UnitNumber";
      var version = new System.Version(0, 6, 13);
      Yak.Version yakVersion = await YakInstall.GetPackageFromVersion(Yak, name, version, true);
      Assert.NotNull(yakVersion);
      Assert.Equal(name, yakVersion.Name);
      Assert.True(yakVersion.Prerelease);
      Assert.Equal("0.6.13-beta", yakVersion.Number);
    }

    [Fact]
    public async void GetPackageFromVersionThatDoesntExistTest() {
      string name = "UnitNumber";
      var version = new System.Version(0, 6, 13);
      Yak.Version yakVersion = await YakInstall.GetPackageFromVersion(Yak, name, version, false);
      Assert.Null(yakVersion);
    }

    [Fact]
    public async void InstallYakPackageAsyncTest() {
      string name = "UnitNumber";
      var version = new System.Version(0, 6, 13);
      Yak.Version yakVersion = await YakInstall.GetPackageFromVersion(Yak, name, version, true);
      await YakInstall.InstallYakPackageAsync(Yak, name, yakVersion);
      Package installedPackage = Yak.List().Where(p => p.Name == name).FirstOrDefault();
      Assert.NotNull(installedPackage);
      Assert.Equal(name, installedPackage.Name);
      Assert.True(Yak.Remove(name));
    }

    [Fact]
    public async void InstallGH_UnitNumberPackageAsyncTest() {
      await YakInstall.InstallGH_UnitNumberPackageAsync();
      string name = "UnitNumber";
      Package installedPackage = Yak.List().Where(p => p.Name == name).FirstOrDefault();

      // We should never be able to download a compatible version of UnitNumber as
      // this test should be run from an un-released version of OasysGH and the
      // Install method should therefore not be able to find and install it
      Assert.Null(installedPackage);
    }
  }
}
