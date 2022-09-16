using OasysGH.Units.UI.MainMenu;

namespace OasysGH
{
  public static class Utility
  {
    private static object UnitsLoaded = false;
    public static void InitialiseMainMenuAndDefaultUnits()
    {
      lock (UnitsLoaded) // lock so that only one plugin will load this
      {
        if (!(bool)UnitsLoaded)
        {

          Grasshopper.Instances.CanvasCreated += LoadMainMenu.OnStartup;
          Units.Utility.SetupUnitsDuringLoad();
          UnitsLoaded = true;
        }
      }
    }
  }
}
