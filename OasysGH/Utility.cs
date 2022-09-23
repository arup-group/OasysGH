using OasysGH.Units.UI.MainMenu;

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
          {
            try
            {
              // if triggered by Rhino6 this will fail (which is ok):
              //YakInstall.InstallGH_UnitNumberPackageAsync("UnitNumber");
            }
            catch (System.Exception)
            {
              // do nothing.
            }
          }
        }
      }
    }
  }
}
