using System;
using System.IO;
using System.Reflection;
using Grasshopper;
using Grasshopper.Kernel;

namespace OasysGH.Versions {
  internal class Versions {
    public static readonly Guid AdSecGuid = new Guid("f815c29a-e1eb-4ca6-9e56-0554777ff9c9");
    public static readonly Guid ComposGuid = new Guid("c3884cdc-ac5b-4151-afc2-93590cef4f8f");
    public static readonly Guid GsaGuid = new Guid("a3b08c32-f7de-4b00-b415-f8b466f05e9f");
    private static Version oasysGhVersion = null;

    internal static Version OasysGhVersion {
      get {
        if (oasysGhVersion == null) oasysGhVersion = GetOasysGhVersion();

        return oasysGhVersion;
      }
    }


    internal static Version CreateVersion(string version) {
      var v = new Version(version.Replace("-beta", string.Empty));
      if (version.Contains("-beta")) v = new Version(v.Major, v.Minor, v.Build, v.Revision + 1);

      return v;
    }

    internal static Version GetOasysGhVersion() {
      string v = OasysGHVersion.Version;
      if (OasysGHVersion.IsBeta) {
#pragma warning disable CS0162 // Keep this check if we return to publishing beta versions
        v += "-beta";
#pragma warning restore CS0162 // Unreachable code detected
      }

      return CreateVersion(v);
    }

    internal static bool IsPluginOutdated(Guid guid) {
      GH_AssemblyInfo pluginInfo = Instances.ComponentServer.FindAssembly(guid);
      return pluginInfo != null && IsVersionOutdated(GetOasyGhVersion(pluginInfo.Location));
    }

    internal static bool IsVersionOutdated(Version pluginOasysGhVersion) =>
      pluginOasysGhVersion.CompareTo(OasysGhVersion) < 0;

    private static Version GetOasyGhVersion(string path) {
      string file = Path.Combine(Path.GetDirectoryName(path), "OasysGH.dll");
      var oasyGh = AssemblyName.GetAssemblyName(file);
      return oasyGh.Version;
    }
  }
}
