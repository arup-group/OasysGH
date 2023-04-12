using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Yak;

namespace OasysGH {

  internal static class YakInstall {
    internal static async Task InstallGH_UnitNumberPackageAsync() {
      if (Rhino.RhinoApp.ExeVersion < 7)
        return;

      // initialise yak client and set package install folder for this version of rhino
      var yak = new YakClient(
        PackageRepositoryFactory.Create(
          "https://yak.rhino3d.com",
          new ProductHeaderValue("OasysAutomation"))) {
        PackageFolder = Rhino.Runtime.HostUtils.AutoInstallPlugInFolder(true) // user
      };

      string name = "UnitNumber";
      Package pack = await yak.Package.Get(name);
      Version[] versions = await yak.Version.GetAll(name);
      versions = versions.Where(v => v.Distributions.Where(d => d.IsCompatible()).Any()).ToArray();

      IEnumerable<Package> installed = yak.List(); // list installed packages
      foreach (Package package in installed) {
        if (package.Name == pack.Name) {
          var installedVersion = new System.Version(package.Version.Replace("-beta", string.Empty));
          if (package.Version.Contains("-beta"))
            installedVersion = new System.Version(installedVersion.Major, installedVersion.Minor, installedVersion.Build, installedVersion.Revision + 1);

          // OasysGH and GH_UnitNumber share the same version number!
          System.Version version = Assembly.GetExecutingAssembly().GetName().Version;
          if (version == null) {
            var latestVersion = new System.Version(versions[0].Number.Replace("-beta", string.Empty));
            if (versions[0].Number.Contains("-beta"))
              latestVersion = new System.Version(latestVersion.Major, latestVersion.Minor, latestVersion.Build, latestVersion.Revision + 1);
            version = latestVersion;
          }

          string versionNumber = version.Major + "." + version.Minor + "." + version.Build + "-beta";

          // don´t install a newer version than the version of OasysGH calling this
          if (version > installedVersion) {
            string tmp_path = await yak.Version.Download(name, versionNumber);
            yak.Install(tmp_path);
          }
          return; // latest version already installed
        }
      }
      string tmp_path2 = await yak.Version.Download(name, versions[0].Number);
      yak.Install(tmp_path2);
    }
  }
}
