using Grasshopper.Kernel;
using OasysGH.Helpers;
using OasysGH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GH_UnitNumber
{
  public class AddReferencePriority : GH_AssemblyPriority
  {
    public override GH_LoadingInstruction PriorityLoad()
    {
      // ### Setup OasysGH ###
      GH_PluginInfo.PluginName = GH_UnitNumberAssemblyInfo.PluginName;
      GH_PluginInfo.ProductName = GH_UnitNumberAssemblyInfo.ProductName;
      GH_PluginInfo.PostHogApiKey = "phc_alOp3OccDM3D18xJTWDoW44Y1cJvbEScm5LJSX8qnhs";

      // ### Setup Units ###
      //Units.SetupUnitsDuringLoad();

      PostHog.PluginLoaded();

      // subscribe to rhino closing event
      //Rhino.RhinoApp.Closing += CloseFile;

      return GH_LoadingInstruction.Proceed;
    }
  }
  public class GH_UnitNumberAssemblyInfo : GH_AssemblyInfo
  {
    internal static Guid GUID = new Guid("6080a841-4f35-4182-9922-f40a66977a69");
    internal const string Company = "Oasys";
    internal const string Copyright = "Copyright © Oasys 1985 - 2022";
    internal const string Contact = "https://www.oasys-software.com/";
    internal const string Vers = "0.1.0";
    internal static bool isBeta = true;
    internal const string ProductName = "Units";
    internal const string PluginName = "GH_UnitNumber";

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
