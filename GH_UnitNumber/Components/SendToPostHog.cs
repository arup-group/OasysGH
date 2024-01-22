using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Newtonsoft.Json;
using OasysGH;
using OasysGH.Components;
using OasysGH.Helpers;

namespace GH_UnitNumber.Components {
  public class SendToPostHog : GH_OasysComponent {
    public override Guid ComponentGuid => new Guid("9da7ba33-62fc-4f36-81a3-7e24a09a644b");
    public override GH_Exposure Exposure => GH_Exposure.hidden;
    public override OasysPluginInfo PluginInfo => GH_UnitNumberPluginInfo.Instance;
    protected override Bitmap Icon => null;

    public SendToPostHog() : base("Send to PostHog", "PostHog",
      "Send an event to a PostHog server", "Params", "Util") {
      Hidden = true;
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager) {
      pManager.AddTextParameter("API key", "K", "PostHog API key", GH_ParamAccess.item);
      pManager.AddTextParameter("Event name", "E", "PostHog event name", GH_ParamAccess.item);
      pManager.AddTextParameter("Event properties", "P", "[Optional] PostHog event properties as JSON string", GH_ParamAccess.item);
      pManager.AddBooleanParameter("Enabled", "On", "[Optional] If true, the component will try to send the PostHog event", GH_ParamAccess.item, true);
      pManager[2].Optional = true;
      pManager[3].Optional = true;
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
    }

    protected override void SolveInstance(IGH_DataAccess da) {
      string apiKey = string.Empty;
      da.GetData(0, ref apiKey);

      string eventName = string.Empty;
      da.GetData(1, ref eventName);

      string json = string.Empty;
      da.GetData(2, ref json);

      bool enabled = true;
      da.GetData(3, ref enabled);

      if (enabled) {
        IDictionary<string, object> properties = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        _ = PostHog.SendToPostHog(apiKey, GH_UnitNumberPluginInfo.Instance.PluginName, GH_UnitNumberPluginInfo.Instance.Version, GH_UnitNumberPluginInfo.Instance.IsBeta, eventName, properties);
      }
    }
  }
}
