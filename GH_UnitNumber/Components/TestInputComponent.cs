using System;
using Grasshopper.Kernel;
using OasysGH;
using OasysGH.Components;
using OasysUnits.Units;

namespace GH_UnitNumber.Components
{
  public class TestInputComponent : GH_OasysComponent
  {
    public override Guid ComponentGuid => new Guid("0dec5da5-93ed-4d3e-b3c0-9dc7727327ad");
    public override GH_Exposure Exposure => GH_Exposure.hidden;
    public override OasysPluginInfo PluginInfo => GH_UnitNumberPluginInfo.Instance;
    public TestInputComponent() : base("Test Component", "Test", "A component to test unitsnumber inputs", "Test", "Test") { } 
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter("Test", "UN", "Test item GH_UnitNumber input", GH_ParamAccess.item);
      pManager.AddGenericParameter("Test", "UNs", "Test list GH_UnitNumber input", GH_ParamAccess.list);
      pManager.AddGenericParameter("Test", "GG", "Test item GenericGoo input", GH_ParamAccess.item);
      pManager.AddGenericParameter("Test", "GGs", "Test list GenericGoo input", GH_ParamAccess.list);
      pManager.AddGenericParameter("Test", "LorR", "Test item LengthOrRatio input", GH_ParamAccess.item);
      pManager.AddGenericParameter("Test", "LorRs", "Test list LengthOrRatio input", GH_ParamAccess.list);
      pManager.AddGenericParameter("Test", "UR", "Test item UnitNumberOrDoubleAsRatioToPercentage input", GH_ParamAccess.item);
      pManager.AddGenericParameter("Test", "UR", "Test item RatioInDecimalFractionToPercentage input", GH_ParamAccess.item);
      pManager.AddGenericParameter("Test", "UR", "Test item RatioInDecimalFractionToDecimalFraction input", GH_ParamAccess.item);
      for (int i = 0; i < pManager.ParamCount; i++)
        pManager[i].Optional = true;
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter("Test", "T", "Test item output", GH_ParamAccess.item);
      pManager.AddGenericParameter("Test", "T", "Test list output", GH_ParamAccess.list);
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
      if (this.Params.Input[0].SourceCount > 0)
      {
        DA.SetData(0, OasysGH.Helpers.Input.UnitNumber(this, DA, 0, MomentUnit.KilonewtonMeter));
      }
      if (this.Params.Input[1].SourceCount > 0)
      {
        DA.SetDataList(1, OasysGH.Helpers.Input.UnitNumberList(this, DA, 1, ForceUnit.Kilonewton));
      }

      if (this.Params.Input[2].SourceCount > 0)
      {
        DA.SetData(0, OasysGH.Helpers.Input.GenericGoo<OasysGH.Parameters.GH_UnitNumber>(this, DA, 2));
      }
      if (this.Params.Input[3].SourceCount > 0)
      {
        DA.SetDataList(1, OasysGH.Helpers.Input.GenericGooList< OasysGH.Parameters.GH_UnitNumber>(this, DA, 3));
      }

      if (this.Params.Input[4].SourceCount > 0)
      {
        DA.SetData(0, OasysGH.Helpers.Input.LengthOrRatio(this, DA, 4, LengthUnit.Meter));
      }
      if (this.Params.Input[5].SourceCount > 0)
      {
        DA.SetDataList(1, OasysGH.Helpers.Input.LengthsOrRatios(this, DA, 5, LengthUnit.Meter));
      }

      if (this.Params.Input[6].SourceCount > 0)
      {
        DA.SetData(0, OasysGH.Helpers.Input.UnitNumberOrDoubleAsRatioToPercentage(this, DA, 6));
      }
      if (this.Params.Input[7].SourceCount > 0)
      {
        DA.SetData(0, OasysGH.Helpers.Input.RatioInDecimalFractionToPercentage(this, DA, 7));
      }
      if (this.Params.Input[8].SourceCount > 0)
      {
        DA.SetData(0, OasysGH.Helpers.Input.RatioInDecimalFractionToDecimalFraction(this, DA, 8));
      }
    }
  }
}
