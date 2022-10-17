using Grasshopper.Kernel;
using OasysGH;
using System;

namespace GH_UnitNumber
{
  public class AddReferencePriority : GH_AssemblyPriority
  {
    public override GH_LoadingInstruction PriorityLoad()
    {
      // ### Load OasysGH ###
      Utility.InitialiseMainMenuAndDefaultUnits(false);
      return GH_LoadingInstruction.Proceed;
    }
  }
  internal sealed class GH_UnitNumberPluginInfo
  {
    private static readonly Lazy<OasysPluginInfo> lazy =
        new Lazy<OasysPluginInfo>(() => new OasysPluginInfo(
          AssemblyInfo.ProductName, 
          AssemblyInfo.PluginName, 
          AssemblyInfo.Vers,
          AssemblyInfo.isBeta, 
          "phc_alOp3OccDM3D18xJTWDoW44Y1cJvbEScm5LJSX8qnhs"
          ));

    public static OasysPluginInfo Instance { get { return lazy.Value; } }

    private GH_UnitNumberPluginInfo()
    {
    }
  }
  public class AssemblyInfo : GH_AssemblyInfo
  {
    internal static Guid GUID = new Guid("6080a841-4f35-4182-9922-f40a66977a69");
    internal const string Company = "Oasys";
    internal const string Copyright = "Copyright © Oasys 1985 - 2022";
    internal const string Contact = "https://www.oasys-software.com/";
    internal const string Vers = "0.3.7";
    internal static bool isBeta = true;
    internal const string ProductName = "UnitNumber";
    internal const string PluginName = "UnitNumber";

    public override string Name
    {
      get
      {
        return ProductName;
      }
    }
    public override string Description
    {
      get
      {
        //Return a short string describing the purpose of this GHA library.
        return "A small plugin enabling use of units in Grasshopper through UnitsNets and OasysUnits";
      }
    }
    public override Guid Id
    {
      get
      {
        return GUID;
      }
    }

    public override string AuthorName
    {
      get
      {
        //Return a string identifying you or your company.
        return Company;
      }
    }
    public override string AuthorContact
    {
      get
      {
        //Return a string representing your preferred contact details.
        return Contact;
      }
    }

    public override string Version
    {
      get
      {
        if (isBeta)
          return Vers + "-beta";
        else
          return Vers;
      }
    }
  }
}
