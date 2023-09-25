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
      if (checkHasBeenDone) return;

      CheckAndShowDialogue();
      checkHasBeenDone = true;
      Instances.CanvasCreated -= OnStartup;
    }

    internal static void CheckAndShowDialogue() {
      bool isAdSecOutdated = Versions.IsPluginOutdated(Versions.AdSecGuid);
      bool isComposOutdated = Versions.IsPluginOutdated(Versions.ComposGuid);
      bool isGsaOutdated = Versions.IsPluginOutdated(Versions.GsaGuid);
      if (RhinoApp.IsRunningHeadless || (!isAdSecOutdated && !isComposOutdated && !isGsaOutdated)) return;

      CreatePluginUpdateDialogue(isAdSecOutdated, isComposOutdated, isGsaOutdated).ShowDialog();
    }

    internal static UpdatePluginsBox CreatePluginUpdateDialogue(bool isAdSecOutdated, bool isComposOutdated,
                                                                bool isGsaOutdated) {
      var processesToUpdate = new List<UpdateProcessHelper>();
      if (isAdSecOutdated) processesToUpdate.Add(new AdSecUpdateHelper());
      if (isComposOutdated) processesToUpdate.Add(new ComposUpdateHelper());
      if (isGsaOutdated) processesToUpdate.Add(new GsaUpdateHelper());

      switch (processesToUpdate.Count) {
        case 0:
          return null;
        case 1: {
          UpdateProcessHelper updateProcessHelper = processesToUpdate.FirstOrDefault();
          string desc = updateProcessHelper?.Text + ".\n\nClick OK to update now.";

          return new UpdatePluginsBox(updateProcessHelper?.Header, desc, updateProcessHelper?.Process,
            updateProcessHelper?.Icon);
        }
        default:
          var multiplePluginsToUpdate = new MultipleVersionsToUpdate(processesToUpdate);
          string description = multiplePluginsToUpdate.Text + ".\n\nClick OK to update now.";

          return new UpdatePluginsBox(multiplePluginsToUpdate.Header, description, multiplePluginsToUpdate.Process,
            multiplePluginsToUpdate.Icon);
      }
    }
  }
}
