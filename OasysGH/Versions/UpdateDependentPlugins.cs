using System.Collections.Generic;
using System.Linq;
using Grasshopper;
using Grasshopper.GUI.Canvas;
using OasysGH.Versions.UI;
using Rhino;

namespace OasysGH.Versions {
  public class UpdateDependentPlugins {
    private static bool checkHasBeenDone = false;

    internal static void OnStartup(GH_Canvas canvas) {
      if (checkHasBeenDone) {
        return;
      }

      CheckAndShowDialogue();
      checkHasBeenDone = true;
      Instances.CanvasCreated -= OnStartup;
    }

    internal static void CheckAndShowDialogue() {
      bool isAdSecOutdated = Versions.IsPluginOutdated(Versions.AdSecGuid);
      bool isComposOutdated = Versions.IsPluginOutdated(Versions.ComposGuid);
      bool isGsaOutdated = Versions.IsPluginOutdated(Versions.GsaGuid);
      if (RhinoApp.IsRunningHeadless || (!isAdSecOutdated && !isComposOutdated && !isGsaOutdated))
        return;

      CreatePluginUpdateDialogue(isAdSecOutdated, isComposOutdated, isGsaOutdated).ShowDialog();
    }

    internal static UpdatePluginsBox CreatePluginUpdateDialogue(
      bool isAdSecOutdated, bool isComposOutdated, bool isGsaOutdated) {
      if (!isAdSecOutdated && !isComposOutdated && !isGsaOutdated) {
        return null;
      }

      var processesToUpdate = new List<UpdatePluginHelper>();
      if (isAdSecOutdated) {
        processesToUpdate.Add(UpdatePluginHelper.AdSec());
      }

      if (isComposOutdated) {
        processesToUpdate.Add(UpdatePluginHelper.Compos());
      }

      if (isGsaOutdated) {
        processesToUpdate.Add(UpdatePluginHelper.Gsa());
      }

      if (processesToUpdate.Count > 1) {
        var multiplePluginsToUpdate = new UpdatePluginHelper(processesToUpdate);
        string description = multiplePluginsToUpdate.Text + ".\n\nClick OK to update now.";

        return new UpdatePluginsBox(multiplePluginsToUpdate.Header, description, multiplePluginsToUpdate.Process,
          multiplePluginsToUpdate.Icon);
      }

      UpdatePluginHelper updateProcessHelper = processesToUpdate.FirstOrDefault();
      string desc = updateProcessHelper?.Text + ".\n\nClick OK to update now.";

      return new UpdatePluginsBox(updateProcessHelper?.Header, desc, updateProcessHelper?.Process,
        updateProcessHelper?.Icon);
    }
  }
}
