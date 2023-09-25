using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OasysGH.Properties;

namespace OasysGH.Versions {
  public class UpdatePluginHelper {
    public string Header { get; private set; } = "Update ";
    public string Name { get; private set; } =  string.Empty;
    public string Process { get; private set; } = @"rhino://package/search?name=guid:";
    public string Text { get; private set; } = "An update is available for ";
    public Bitmap Icon { get; private set; } = Resources.OasysGHUpdate3;
    public UpdatePluginHelper() { }

    public UpdatePluginHelper(List<UpdatePluginHelper> processes) {
      var withoutLast = new List<UpdatePluginHelper>();
      withoutLast.AddRange(processes.GetRange(0, processes.Count - 1));

      Process = @"rhino://package/search?name=oasys";
      Text = $"Updates are available for { string.Join(", ", withoutLast.Select(n=>n.Name)) }" +
        $" and {processes.Last().Name}";
      Header = "Update Oasys Plugins";
      Icon = processes.Count switch {
        2 => Resources.OasysGHUpdate2,
        3 => Resources.OasysGHUpdate3,
        _ => Icon
      };
    }

    public static UpdatePluginHelper AdSec() {
      var updateProcessHelper = new UpdatePluginHelper();
      updateProcessHelper.SetName("AdSec");
      updateProcessHelper.Process += Versions.AdSecGuid;
      updateProcessHelper.Icon = Resources.AdSecGHUpdate;
      return updateProcessHelper;
    }

    public static UpdatePluginHelper Compos() {
      var updateProcessHelper = new UpdatePluginHelper();
      updateProcessHelper.SetName("Compos");
      updateProcessHelper.Process += Versions.ComposGuid;
      updateProcessHelper.Icon = Resources.ComposGHUpdate;
      return updateProcessHelper;
    }

    public static UpdatePluginHelper Gsa() {
      var updateProcessHelper = new UpdatePluginHelper();
      updateProcessHelper.SetName("GSA");
      updateProcessHelper.Process += Versions.GsaGuid;
      updateProcessHelper.Icon = Resources.GsaGHUpdate;
      return updateProcessHelper;
    }

    private void SetName(string name) {
      Header += name;
      Text += name;
      Name = name;
    }
  }
}
