using OasysGH.Units.UI.MainMenu;
using OasysGH.Units.Helpers;

namespace OasysGH
{
  public static class Utility
  {
    private static object unitsLoaded = false;
    public static void InitialiseMainMenuAndDefaultUnits()
    {
      if (!(bool)unitsLoaded)
      {
        lock (unitsLoaded) // lock so that only one plugin will load this:
        {
          Grasshopper.Instances.CanvasCreated += LoadMainMenu.OnStartup;
          Units.Utility.SetupUnitsDuringLoad();
          unitsLoaded = true;
        }
      }
    }
  }
}
