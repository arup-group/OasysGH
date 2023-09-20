using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Newtonsoft.Json;
using OasysGH.Components;

namespace OasysGH.Helpers {
  public class PostHog {
    private class PhContainer {
      [JsonProperty("api_key")]
      string api_key { get; set; }

      [JsonProperty("event")]
      string ph_event { get; set; }

      [JsonProperty("timestamp")]
      DateTime ph_timestamp { get; set; }
      public Dictionary<string, object> properties { get; set; }

      public PhContainer(OasysPluginInfo pluginInfo, string eventName, Dictionary<string, object> properties) {
        this.ph_event = eventName;
        this.properties = properties;
        this.ph_timestamp = DateTime.UtcNow;
        this.api_key = pluginInfo.PostHogApiKey;
      }
    }

    internal static User CurrentUser = new User();
    private static HttpClient _phClient = new HttpClient();

    public static void AddedToDocument(GH_OasysComponent component) {
      AddedToDocument(component, component.PluginInfo);
    }

    public static void AddedToDocument(GH_Component component, OasysPluginInfo pluginInfo) {
      string eventName = "AddedToDocument";
      Dictionary<string, object> properties = new Dictionary<string, object>()
      {
        { "componentName", component.Name },
      };
      _ = SendToPostHog(pluginInfo, eventName, properties);
    }

    public static void ModelIO(OasysPluginInfo pluginInfo, string interactionType, int size = 0) {
      string eventName = "ModelIO";
      Dictionary<string, object> properties = new Dictionary<string, object>()
      {
        { "interactionType", interactionType },
        { "size", size },
      };
      _ = SendToPostHog(pluginInfo, eventName, properties);
    }

    public static void PluginLoaded(OasysPluginInfo pluginInfo, string error = "") {
      string eventName = "PluginLoaded";

      Dictionary<string, object> properties = new Dictionary<string, object>()
      {
        { "rhinoVersion", Rhino.RhinoApp.Version.ToString().Split('.')
                          + "." + Rhino.RhinoApp.Version.ToString().Split('.')[1] },
        { "rhinoMajorVersion", Rhino.RhinoApp.ExeVersion },
        { "rhinoServiceRelease", Rhino.RhinoApp.ExeServiceRelease },
        { "loadingError", error },
      };
      _ = SendToPostHog(pluginInfo, eventName, properties);
    }

    public static void RemovedFromDocument(GH_OasysComponent component) {
      RemovedFromDocument(component, component.PluginInfo);
    }

    public static void RemovedFromDocument(GH_Component component, OasysPluginInfo pluginInfo) {
      if (component.Attributes.Selected) {
        string eventName = "RemovedFromDocument";
        Dictionary<string, object> properties = new Dictionary<string, object>()
        {
          { "componentName", component.Name },
          { "runCount", component.RunCount },
        };
        _ = SendToPostHog(pluginInfo, eventName, properties);
      }
    }

    public static async Task<HttpResponseMessage> SendToPostHog(OasysPluginInfo pluginInfo, string eventName, Dictionary<string, object> additionalProperties = null) {
      // posthog ADS plugin requires a user object
      User user = CurrentUser;

      Dictionary<string, object> properties = new Dictionary<string, object>() {
        { "distinct_id", user.userName },
        { "user", user },
        { "pluginName", pluginInfo.PluginName },
        { "version", pluginInfo.Version },
        { "isBeta", pluginInfo.IsBeta },
      };

      if (additionalProperties != null) {
        foreach (string key in additionalProperties.Keys)
          properties.Add(key, additionalProperties[key]);
      }

      var container = new PhContainer(pluginInfo, eventName, properties);
      var body = JsonConvert.SerializeObject(container);
      var content = new StringContent(body, Encoding.UTF8, "application/json");
      var response = await _phClient.PostAsync("https://posthog.insights.arup.com/capture/", content);
      return response;
    }
  }

  internal class User {
    public string email { get; set; }
    public string userName { get; set; }

    internal User() {
      userName = Environment.UserName.ToLower();
      try {
        var task = Task.Run(() => UserPrincipal.Current.EmailAddress);
        if (task.Wait(TimeSpan.FromSeconds(2))) {
          if (task.Result.EndsWith("arup.com"))
            email = task.Result;
          else {
            email = task.Result.GetHashCode().ToString();
            userName = userName.GetHashCode().ToString();
          }
          return;
        }
      } catch (Exception) { }

      if (Environment.UserDomainName.ToLower() == "global")
        email = userName + "@arup.com";
      else
        userName = userName.GetHashCode().ToString();
    }
  }
}
