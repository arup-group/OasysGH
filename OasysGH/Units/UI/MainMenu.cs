using System.Threading;
using System.Windows.Forms;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;

namespace OasysGH.Units.UI.MainMenu {

  internal class LoadMainMenu {
    private static bool menuLoaded = false;
    private static ToolStripMenuItem oasysMenu;
    internal static void OnStartup(GH_Canvas canvas) {
      if (menuLoaded)
        return;
      oasysMenu = new ToolStripMenuItem("Oasys");
      oasysMenu.Name = "Oasys";

      PopulateSub(oasysMenu);

      GH_DocumentEditor editor = null;

      while (editor == null) {
        editor = Grasshopper.Instances.DocumentEditor;
        Thread.Sleep(321);
      }

      if (!editor.MainMenuStrip.Items.ContainsKey("Oasys"))
        editor.MainMenuStrip.Items.Add(oasysMenu);
      else {
        oasysMenu = (ToolStripMenuItem)editor.MainMenuStrip.Items["Oasys"];
        lock (oasysMenu) {
          oasysMenu.DropDown.Items.Add(new ToolStripSeparator());
          PopulateSub(oasysMenu);
        }
      }
      menuLoaded = true;
      Grasshopper.Instances.CanvasCreated -= OnStartup;
    }

    private static void PopulateSub(ToolStripMenuItem menuItem) {
      menuItem.DropDown.Items.Add("Oasys Units", Properties.Resources.Units1, (s, a) => {
        var unitBox = new DefaultUnitsForm();
        unitBox.ShowDialog();
      });
    }
  }
}
