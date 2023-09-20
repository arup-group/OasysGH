using System;

namespace OasysGH {
  public static class OasysGHVersion {
    public const bool IsBeta = true;
    // this is the one place to set the version in VS:
    // also update the version manually in OasysGH.csproj
    public const string Version = "0.9.0";
  }

  public class OasysPluginInfo {
    public bool IsBeta { get; }
    public string PluginName { get; }
    public string PostHogApiKey { get; }
    public string ProductName { get; }
    public string Version { get; }

    public OasysPluginInfo(string productName, string pluginName, string version, bool isBeta, string postHogApiKey) {
      ProductName = productName;
      PluginName = pluginName;
      Version = version;
      IsBeta = isBeta;
      PostHogApiKey = postHogApiKey;
    }
  }

  internal sealed class PluginInfo {
    public static OasysPluginInfo Instance => lazy.Value;

    private static readonly Lazy<OasysPluginInfo> lazy =
            new Lazy<OasysPluginInfo>(() => new OasysPluginInfo(
          "Oasys Shared Grasshopper", "OasysGH", OasysGHVersion.Version, OasysGHVersion.IsBeta, "phc_alOp3OccDM3D18xJTWDoW44Y1cJvbEScm5LJSX8qnhs"
          ));
  }
}
