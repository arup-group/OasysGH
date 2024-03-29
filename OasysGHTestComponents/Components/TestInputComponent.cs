﻿using System;
using System.Diagnostics.CodeAnalysis;
using Grasshopper.Kernel;
using OasysGH;
using OasysGH.Components;
using OasysUnits.Units;
using static OasysGHTestComponents.OasysGHTestComponentsInfo;

namespace OasysGHTestComponents.Components {
  [ExcludeFromCodeCoverage]
  public class TestInputComponent : GH_OasysComponent {
    public override Guid ComponentGuid => new Guid("0dec5da5-93ed-4d3e-b3c0-9dc7727327ad");
    public override GH_Exposure Exposure => GH_Exposure.primary;
    public override OasysPluginInfo PluginInfo => OasysGHTestComponentsPluginInfo.Instance;

    public TestInputComponent() : base("Input Test Component", "Input",
      "A component to test unitsnumber inputs", "OasysGH", "Test") {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager) {
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

    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddGenericParameter("Test", "T", "Test item output", GH_ParamAccess.item);
      pManager.AddGenericParameter("Test", "T", "Test list output", GH_ParamAccess.list);
    }

    protected override void SolveInstance(IGH_DataAccess da) {
      if (Params.Input[0].SourceCount > 0) {
        da.SetData(0, OasysGH.Helpers.Input.UnitNumber(this, da, 0, MomentUnit.KilonewtonMeter));
      }

      if (Params.Input[1].SourceCount > 0) {
        da.SetDataList(1, OasysGH.Helpers.Input.UnitNumberList(this, da, 1, ForceUnit.Kilonewton));
      }

      if (Params.Input[2].SourceCount > 0) {
        da.SetData(0, OasysGH.Helpers.Input.GenericGoo<OasysGH.Parameters.GH_UnitNumber>(this, da, 2));
      }

      if (Params.Input[3].SourceCount > 0) {
        da.SetDataList(1, OasysGH.Helpers.Input.GenericGooList<OasysGH.Parameters.GH_UnitNumber>(this, da, 3));
      }

      if (Params.Input[4].SourceCount > 0) {
        da.SetData(0, OasysGH.Helpers.Input.LengthOrRatio(this, da, 4, LengthUnit.Meter));
      }

      if (Params.Input[5].SourceCount > 0) {
        da.SetDataList(1, OasysGH.Helpers.Input.LengthsOrRatios(this, da, 5, LengthUnit.Meter));
      }

      if (Params.Input[6].SourceCount > 0) {
        da.SetData(0, OasysGH.Helpers.Input.UnitNumberOrDoubleAsRatioToPercentage(this, da, 6));
      }

      if (Params.Input[7].SourceCount > 0) {
        da.SetData(0, OasysGH.Helpers.Input.RatioInDecimalFractionToPercentage(this, da, 7));
      }

      if (Params.Input[8].SourceCount > 0) {
        da.SetData(0, OasysGH.Helpers.Input.RatioInDecimalFractionToDecimalFraction(this, da, 8));
      }
    }
  }
}
