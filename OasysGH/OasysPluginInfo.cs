using System;

namespace OasysGH
{
  public class OasysPluginInfo
  {
    public string ProductName { get; }
    public string PluginName { get; }
    public string Version { get; }
    public bool IsBeta { get; }
    public string PostHogApiKey { get; }

    public OasysPluginInfo(string productName, string pluginName, string version, bool isBeta, string postHogApiKey)
    {
      ProductName = productName;
      PluginName = pluginName;
      Version = version;
      IsBeta = isBeta;
      PostHogApiKey = postHogApiKey;
    }
  }

  internal sealed class PluginInfo
  {
    internal const string Version = "0.3.2";

    private static readonly Lazy<OasysPluginInfo> lazy =
        new Lazy<OasysPluginInfo>(() => new OasysPluginInfo(
          "Oasys Shared Grasshopper", "OasysGH", Version, true, "phc_alOp3OccDM3D18xJTWDoW44Y1cJvbEScm5LJSX8qnhs"
          ));

    public static OasysPluginInfo Instance { get { return lazy.Value; } }

    private PluginInfo()
    {
    }
  }
}
