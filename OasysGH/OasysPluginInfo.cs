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
  public static class OasysGHVersion
  {
    // this is the one place to set the version in VS:
    // also update the version manually in OasysGH.csproj
    public const string Version = "0.4.4";
    public const bool IsBeta = true;
  }

  internal sealed class PluginInfo
  {
    private static readonly Lazy<OasysPluginInfo> lazy =
        new Lazy<OasysPluginInfo>(() => new OasysPluginInfo(
          "Oasys Shared Grasshopper", "OasysGH", OasysGHVersion.Version, OasysGHVersion.IsBeta, "phc_alOp3OccDM3D18xJTWDoW44Y1cJvbEScm5LJSX8qnhs"
          ));

    public static OasysPluginInfo Instance { get { return lazy.Value; } }

    private PluginInfo()
    {
    }
  }
}
