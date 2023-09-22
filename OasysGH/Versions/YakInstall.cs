using System.Linq;
using System.Threading.Tasks;
using Yak;
using Version = Yak.Version;

namespace OasysGH.Versions {
  internal class YakInstall {

    internal static async Task InstallGH_UnitNumberPackageAsync() {
      if (Rhino.RhinoApp.ExeVersion < 7)
        return;

      System.Version oasysGhVersion = Versions.GetOasysGhVersion();
      bool isPrerelease = OasysGHVersion.IsBeta;


      string name = "UnitNumber";
      YakClient yak = CreateYakClient();
      Version yakVersion = await GetPackageFromVersion(yak, name, oasysGhVersion, isPrerelease);
      if (yakVersion == null) {
        // no compatible yak version available, nothing to install
        return;
      }

      // list all installed yak packages
      Package installedPackage = yak.List().Where(p => p.Name == name).FirstOrDefault();
      if (installedPackage == null) {
        // no package installed, so we install
        _ = InstallYakPackageAsync(yak, name, yakVersion);
        return;
      }

      System.Version installedVersion = Versions.CreateVersion(installedPackage.Version);

      if (oasysGhVersion > installedVersion) {
        // installed version is outdated, so we install
        _ = InstallYakPackageAsync(yak, name, yakVersion);
        return;
      }
    }

    private static async Task InstallYakPackageAsync(YakClient yak, string name, Version yakVersion) {
      string path = await yak.Version.Download(name, yakVersion.Number);
      yak.Install(path);
    }

    private static async Task<Version> GetPackageFromVersion(
      YakClient yak, string name, System.Version desiredVersion, bool isPrerelease = false) {
      Version[] versions = await yak.Version.GetAll(name);
      // get all versions compatible with installed Rhino version
      versions = versions.Where(v => v.Distributions.Where(d => d.IsCompatible()).Any()).ToArray();
      // remove pre-releases if current dll is not beta
      if (!isPrerelease) {
        versions = versions.Where(v => !v.Prerelease).ToArray();
      }

      // return the version that matches desired version number
      return versions.Where(v => Versions.CreateVersion(v.Number) == desiredVersion).FirstOrDefault();
    }

    private static YakClient CreateYakClient() {
      // initialise yak client and set package install folder for this version of rhino
      return new YakClient(
        PackageRepositoryFactory.Create(
          "https://yak.rhino3d.com",
          new ProductHeaderValue("OasysAutomation"))) {
        PackageFolder = Rhino.Runtime.HostUtils.AutoInstallPlugInFolder(true) // user
      };
    }
  }
}
