using OasysGH.Units.UI.MainMenu;

namespace OasysGH {
  public static class Utility {
    private static object unitsLoaded = false;
    public static void InitialiseMainMenuAndDefaultUnits(bool installGH_UnitNumber = true) {
      lock (unitsLoaded) // lock so that only one plugin will load this
      {
        if (!(bool)unitsLoaded) {
          Grasshopper.Instances.CanvasCreated += LoadMainMenu.OnStartup;
          Units.Utility.SetupUnitsDuringLoad();
          unitsLoaded = true;
          if (installGH_UnitNumber) {
            try {
              // if triggered by Rhino6 this will fail (which is ok):
              YakInstall.InstallGH_UnitNumberPackageAsync();
            }
            catch (System.Exception) {
              // do nothing.
            }
          }
        }
      }
    }
  }
}
