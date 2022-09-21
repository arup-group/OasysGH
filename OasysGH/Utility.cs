using OasysGH.Units.UI.MainMenu;
using System.Collections.Generic;
using System.Linq;
using Yak;
using System.Threading.Tasks;

namespace OasysGH
{
  public static class Utility
  {
    private static object UnitsLoaded = false;
    public static void InitialiseMainMenuAndDefaultUnits(bool installGH_UnitNumber = true)
    {
      lock (UnitsLoaded) // lock so that only one plugin will load this
      {
        if (!(bool)UnitsLoaded)
        {
          Grasshopper.Instances.CanvasCreated += LoadMainMenu.OnStartup;
          Units.Utility.SetupUnitsDuringLoad();
          UnitsLoaded = true;
          if (installGH_UnitNumber)
            YakInstall.InstallGH_UnitNumberPackageAsync("GH_UnitNumber");
        }
      }
    }
  }

  internal static class YakInstall
  {
    internal static async Task InstallGH_UnitNumberPackageAsync(string plugin)
    {
      if (Rhino.RhinoApp.ExeVersion < 7)
        return;

      // initialise yak client and set package install folder for this version of rhino
      var yak = new YakClient(
        PackageRepositoryFactory.Create(
          "https://yak.rhino3d.com",
          new ProductHeaderValue("OasysAutomation")))
      {
        PackageFolder = Rhino.Runtime.HostUtils.AutoInstallPlugInFolder(true) // user
      };

      Package pack = await yak.Package.Get(plugin);
      Version[] versions = await yak.Version.GetAll(plugin);
      versions = versions.Where(v => v.Distributions.Where(d => d.IsCompatible()).Any()).ToArray();

      IEnumerable<Package> installed = yak.List(); // list installed packages
      foreach (Package package in installed)
      {
        if (package.Name == pack.Name)
        {
          System.Version latestVersion = new System.Version(versions[0].Number.Replace("-beta", string.Empty));
          if (versions[0].Number.Contains("-beta"))
            latestVersion = new System.Version(latestVersion.Major, latestVersion.Minor, latestVersion.Build, latestVersion.Revision + 1);
          
          System.Version installedVersion = new System.Version(package.Version.Replace("-beta", string.Empty));
          if (package.Version.Contains("-beta"))
            installedVersion = new System.Version(installedVersion.Major, installedVersion.Minor, installedVersion.Build, installedVersion.Revision + 1);

          if (latestVersion > installedVersion)
          {
            var tmp_path = await yak.Version.Download(plugin, versions[0].Number);
            yak.Install(tmp_path);
          }
          return; // latest version already installed
        }
      }
      var tmp_path2 = await yak.Version.Download(plugin, versions[0].Number);
      yak.Install(tmp_path2);
    }
  }
}
