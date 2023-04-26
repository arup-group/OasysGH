using System;
using Grasshopper.Kernel;
using OasysGH;

namespace GH_UnitNumber {
  public class AddReferencePriority : GH_AssemblyPriority {

    public override GH_LoadingInstruction PriorityLoad() {
      // ### Load OasysGH ###
      Utility.InitialiseMainMenuAndDefaultUnits(false);
      return GH_LoadingInstruction.Proceed;
    }
  }

  public class AssemblyInfo : GH_AssemblyInfo {
    public override string AuthorContact {
      get {
        //Return a string representing your preferred contact details.
        return Contact;
      }
    }

    public override string AuthorName {
      get {
        //Return a string identifying you or your company.
        return Company;
      }
    }

    public override string Description {
      get {
        //Return a short string describing the purpose of this GHA library.
        return "A small plugin enabling use of units in Grasshopper through UnitsNets and OasysUnits";
      }
    }

    public override Guid Id {
      get {
        return gUID;
      }
    }

    public override string Name {
      get {
        return ProductName;
      }
    }

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
    internal const string Copyright = "Copyright © Oasys 1985 - 2022";
    internal const string PluginName = "UnitNumber";
    internal const string ProductName = "UnitNumber";
    internal const string Vers = OasysGHVersion.Version;
    internal static Guid gUID = new Guid("6080a841-4f35-4182-9922-f40a66977a69");
    internal static bool isBeta = OasysGHVersion.IsBeta;
  }

  internal sealed class GH_UnitNumberPluginInfo {
    public static OasysPluginInfo Instance { get { return lazy.Value; } }

    private static readonly Lazy<OasysPluginInfo> lazy =
            new Lazy<OasysPluginInfo>(() => new OasysPluginInfo(
          AssemblyInfo.ProductName,
          AssemblyInfo.PluginName,
          AssemblyInfo.Vers,
          AssemblyInfo.isBeta,
          "phc_alOp3OccDM3D18xJTWDoW44Y1cJvbEScm5LJSX8qnhs"
          ));

    private GH_UnitNumberPluginInfo() {
    }
  }
}
