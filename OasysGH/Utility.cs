using OasysGH.Units.UI.MainMenu;
using OasysGH.Versions;

namespace OasysGH {
  public static class Utility {
    private static object initialised = false;

    public static void InitialiseMainMenuUnitsAndDependentPluginsCheck(bool installGH_UnitNumber = true) {
      // lock so that only one plugin will load this
      lock (initialised) {
        if (!(bool)initialised) {
          Grasshopper.Instances.CanvasCreated += LoadMainMenu.OnStartup;
          Units.Utility.SetupUnitsDuringLoad();
          initialised = true;
          if (installGH_UnitNumber) {
            try {
              // if triggered by Rhino6 this will fail (which is ok):
              _ = YakInstall.InstallGH_UnitNumberPackageAsync();
            } catch (System.Exception) {
              // do nothing.
            }
          }

          Grasshopper.Instances.CanvasCreated += UpdateDependentPlugins.OnStartup;
        }
      }
    }
  }
}
