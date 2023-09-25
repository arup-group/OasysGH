using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OasysGH.Properties;

namespace OasysGH.Versions {
  internal abstract class UpdateProcessHelper {
    public string Process { get; protected set; } = @"rhino://package/search?name=guid:";
    public string Text { get; protected set; } = "An update is available for ";
    public string Header { get; protected set; } = "Update ";
    public Bitmap Icon { get; protected set; } = Resources.OasysGHUpdate3;
  }

  internal class AdSecUpdateHelper : UpdateProcessHelper {
    public AdSecUpdateHelper() {
      Process += "AdSec";
      Text += Versions.AdSecGuid;
      Header += "AdSec";
      Icon = Resources.AdSecGHUpdate;
    }
  }

  internal class GsaUpdateHelper : UpdateProcessHelper {
    public GsaUpdateHelper() {
      Process += "GSA";
      Text += Versions.GsaGuid;
      Header += "GSA";
      Icon = Resources.GsaGHUpdate;
    }
  }

  internal class ComposUpdateHelper : UpdateProcessHelper {
    public ComposUpdateHelper() {
      Process += "Compos";
      Text += Versions.ComposGuid;
      Header += "Compos";
      Icon = Resources.ComposGHUpdate;
    }
  }

  internal class MultipleVersionsToUpdate : UpdateProcessHelper {
    public MultipleVersionsToUpdate(List<UpdateProcessHelper> processes) {
      var withoutLast = new List<UpdateProcessHelper>();
      withoutLast.AddRange(processes.GetRange(0, processes.Count - 1));

      Process = @"rhino://package/search?name=oasys";
      Text = $"Updates are available for {string.Join(", ", withoutLast)} and {processes.Last()}";
      Header = "Oasys Plugins";
      Icon = processes.Count switch {
        2 => Resources.OasysGHUpdate2,
        3 => Resources.OasysGHUpdate3,
        _ => Icon
      };
    }
  }
}
