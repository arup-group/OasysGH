using System;
using System.Runtime.CompilerServices;

using Grasshopper.Kernel;

using OasysGH;

#if RELEASEFORTESTING || DEBUG
[assembly: InternalsVisibleToAttribute("GH_UnitNumberTests")]
#endif

namespace GH_UnitNumber {
  public class AddReferencePriority : GH_AssemblyPriority {

    public override GH_LoadingInstruction PriorityLoad() {
      Utility.InitialiseMainMenuUnitsAndDependentPluginsCheck(false);
      return GH_LoadingInstruction.Proceed;
    }
  }

  public class GH_UnitNumberInfo : GH_AssemblyInfo {
    public override string AuthorContact => Contact;

    public override string AuthorName => Company;

    public override string Description
      => "A small plugin enabling use of units in Grasshopper through UnitsNet and OasysUnits";

    public override Guid Id => gUID;

    public override string Name => ProductName;

    public override string Version {
      get {
        if (isBeta)
          return Vers + "-beta";
        else
          return Vers;
      }
    }

    internal const string Company = "Oasys";
    internal const string Contact = "https://www.oasys-software.com/";
    internal const string Copyright = "Copyright © Oasys 1985 - 2024";
    internal const string PluginName = "UnitNumber";
    internal const string ProductName = "UnitNumber";
    internal const string Vers = OasysGHVersion.Version;
    internal static Guid gUID = new Guid("6080a841-4f35-4182-9922-f40a66977a69");
    internal static bool isBeta = OasysGHVersion.IsBeta;
  }

  internal sealed class GH_UnitNumberPluginInfo {
    public static OasysPluginInfo Instance => lazy.Value;

    private static readonly Lazy<OasysPluginInfo> lazy = new Lazy<OasysPluginInfo>(()
      => new OasysPluginInfo(GH_UnitNumberInfo.ProductName, GH_UnitNumberInfo.PluginName, GH_UnitNumberInfo.Vers,
        GH_UnitNumberInfo.isBeta, "phc_QjmqOoe8GqTMi3u88ynRR3WWvrJA9zAaqcQS1FDVnJD"));
  }
}
