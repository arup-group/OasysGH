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
      // for PostHog to work these json properties need to be lower case!
      [JsonProperty("api_key")]
      private string _apiKey;

      [JsonProperty("event")]
      private string _event;

      [JsonProperty("properties")]
      private Dictionary<string, object> _properties;

      [JsonProperty("timestamp")]
      private DateTime _timestamp;

      public PhContainer(string apiKey, string eventName, Dictionary<string, object> properties) {
        _apiKey = apiKey;
        _event = eventName;
        _properties = properties;
        _timestamp = DateTime.UtcNow;
      }
    }

    internal static User currentUser = new User();
    private static HttpClient phClient = new HttpClient();

    public static void AddedToDocument(GH_OasysComponent component) {
      AddedToDocument(component, component.PluginInfo);
    }

    public static void AddedToDocument(GH_Component component, OasysPluginInfo pluginInfo) {
      string eventName = "AddedToDocument";
      var properties = new Dictionary<string, object>() {
        { "componentName", component.Name },
      };
      _ = SendToPostHog(pluginInfo, eventName, properties);
    }

    public static void ModelIO(OasysPluginInfo pluginInfo, string interactionType, int size = 0) {
      string eventName = "ModelIO";
      var properties = new Dictionary<string, object>() {
        { "interactionType", interactionType },
        { "size", size },
      };
      _ = SendToPostHog(pluginInfo, eventName, properties);
    }

    public static void PluginLoaded(OasysPluginInfo pluginInfo, string error = "") {
      string eventName = "PluginLoaded";

      var properties = new Dictionary<string, object>() {
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
        var properties = new Dictionary<string, object>() {
          { "componentName", component.Name },
          { "runCount", component.RunCount },
        };
        _ = SendToPostHog(pluginInfo, eventName, properties);
      }
    }

    public static async Task<HttpResponseMessage> SendToPostHog(OasysPluginInfo pluginInfo, string eventName, Dictionary<string, object> additionalProperties = null) {
      // posthog ADS plugin requires a user object
      User user = currentUser;

      var properties = new Dictionary<string, object>() {
        { "distinct_id", user.UserName },
        { "user", user },
        { "pluginName", pluginInfo.PluginName },
        { "version", pluginInfo.Version },
        { "isBeta", pluginInfo.IsBeta },
      };

      if (additionalProperties != null) {
        foreach (string key in additionalProperties.Keys) {
          properties.Add(key, additionalProperties[key]);
        }
      }

      var container = new PhContainer(pluginInfo.PostHogApiKey, eventName, properties);
      string body = JsonConvert.SerializeObject(container);
      var content = new StringContent(body, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await phClient.PostAsync("https://posthog.insights.arup.com/capture/", content);
      return response;
    }
  }

  internal class User {
    public string Email { get; set; }
    public string UserName { get; set; }

    internal User() {
      UserName = Environment.UserName.ToLower();
      try {
        var task = Task.Run(() => UserPrincipal.Current.EmailAddress);
        if (task.Wait(TimeSpan.FromSeconds(2))) {
          if (task.Result.EndsWith("arup.com"))
            Email = task.Result;
          else {
            Email = task.Result.GetHashCode().ToString();
            UserName = UserName.GetHashCode().ToString();
          }

          return;
        }
      } catch (Exception) { }

      if (Environment.UserDomainName.ToLower() == "global") {
        Email = UserName + "@arup.com";
      } else {
        UserName = UserName.GetHashCode().ToString();
      }
    }
  }
}
