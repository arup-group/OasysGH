using Grasshopper.GUI.Canvas;
using OasysGH.Units.UI.MainMenu;
using OasysGH.Units.Helpers;

namespace OasysGH
{
  public class Initialise
  {
    private static object unitsLoaded = false;
    public static void MainMenuAndDefaultUnits()
    {
      if (!(bool)unitsLoaded)
      {
        lock (unitsLoaded) // lock so that only one plugin will load this:
        {
          Grasshopper.Instances.CanvasCreated += LoadMainMenu.OnStartup;
          Setup.SetupUnitsDuringLoad();
          unitsLoaded = true;
        }
      }
    }
  }
}
