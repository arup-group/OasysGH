using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Grasshopper.GUI.Canvas;
using OasysGH.Versions.UI;
using Rhino;

namespace OasysGH.Versions {
  public class UpdateDependentPlugins {
    private static bool checkHasBeenDone = false;

    internal static void OnStartup(GH_Canvas canvas) {
      if (checkHasBeenDone)
        return;

      CheckAndShowDialogue();
      checkHasBeenDone = true;
      Grasshopper.Instances.CanvasCreated -= OnStartup;
    }

    internal static void CheckAndShowDialogue() {
      bool isAdSecOutdated = Versions.IsPluginOutdated(Versions.AdSecGuid);
      bool isComposOutdated = Versions.IsPluginOutdated(Versions.ComposGuid);
      bool isGsaOutdated = Versions.IsPluginOutdated(Versions.GsaGuid);
      if (RhinoApp.IsRunningHeadless || (!isAdSecOutdated && !isComposOutdated && !isGsaOutdated)) {
        return;
      }

      CreatePluginUpdateDialogue(isAdSecOutdated, isComposOutdated, isGsaOutdated).ShowDialog();
    }

    internal static UpdatePluginsBox CreatePluginUpdateDialogue(
      bool isAdSecOutdated, bool isComposOutdated, bool isGsaOutdated) {
      string process = @"rhino://package/search?name=guid:";
      string text = "An update is available for ";
      string header = "Update ";
      Bitmap icon = Properties.Resources.OasysGHUpdate3;

      switch (isAdSecOutdated, isComposOutdated, isGsaOutdated) {
        case (true, false, false):
          text += "AdSec";
          process += Versions.AdSecGuid;
          header += "AdSec";
          icon = Properties.Resources.AdSecGHUpdate;
          break;

        case (false, true, false):
          text += "Compos";
          process += Versions.ComposGuid;
          header += "Compos";
          icon = Properties.Resources.ComposGHUpdate;
          break;

        case (false, false, true):
          text += "GSA";
          process += Versions.GsaGuid;
          header += "GSA";
          icon = Properties.Resources.GsaGHUpdate;
          break;

        default:
          var names = new List<string>();
          if (isAdSecOutdated) {
            names.Add("AdSec");
          }

          if (isComposOutdated) {
            names.Add("Compos");
          }

          if (isGsaOutdated) {
            names.Add("GSA");
          }

          var first = names.ToList();
          first.RemoveAt(first.Count - 1);
          text = $"Updates are available for {string.Join(", ", first)} and {names.Last()}";
          process = @"rhino://package/search?name=oasys";
          header += "Oasys Plugins";
          if (names.Count == 2) {
            icon = Properties.Resources.OasysGHUpdate2;
          }

          break;
      }

      text += ".\n\nClick OK to update now.";
      var updateComposBox = new UpdatePluginsBox(header, text, process, icon);
      return updateComposBox;
    }
  }
}
