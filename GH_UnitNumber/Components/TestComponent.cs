using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using OasysGH;
using OasysGH.Components;
using OasysGH.Helpers;
using OasysGH.Units;
using OasysGH.Units.Helpers;
using OasysUnitsNet;
using OasysUnitsNet.Units;

namespace GH_UnitNumber.Components
{
  public class TestComponent : GH_OasysDropDownComponent
  {
    #region Name and Ribbon Layout
    // This region handles how the component in displayed on the ribbon including name, exposure level and icon
    public override Guid ComponentGuid => new Guid("2a867d34-0226-4411-b11d-9d616a792dd0");
    public TestComponent()
      : base("TestName",
          "T",
          "Test component for input and output UnitNumber",
            "Test",
            "Test")
    { this.Hidden = true; } // sets the initial state of the component to hidden

    public override GH_Exposure Exposure => GH_Exposure.primary;
    public override OasysPluginInfo PluginInfo => GH_UnitNumberPluginInfo.Instance;
    #endregion

    #region Input and output
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter("Name", "N", "Item", GH_ParamAccess.item);
      pManager.AddGenericParameter("Name", "N", "List", GH_ParamAccess.list);
      pManager.AddGenericParameter("Object", "O", "Object", GH_ParamAccess.item);
      pManager[0].Optional = true;
      pManager[1].Optional = true;
      pManager[2].Optional = true;
    }
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddParameter(new GH_UnitNumberParameter());
      pManager.AddParameter(new GH_UnitNumberParameter(), "Name", "N", "List", GH_ParamAccess.list);
      pManager.AddParameter(new GH_UnitNumberParameter(), "Name", "N", "Tree", GH_ParamAccess.tree);
      pManager.AddGenericParameter("Object", "O", "Object", GH_ParamAccess.item);
    }
    #endregion

    protected override void SolveInstance(IGH_DataAccess DA)
    {
      Length first = (Length)Input.UnitNumber(this, DA, 0, this.LengthUnit);
      List<Length> second = Input.UnitNumberList(this, DA, 1, LengthUnit).Select(x => (Length)x).ToList();

      Output.SetItem(this, DA, 0, new OasysGH.Units.GH_UnitNumber(first));
      Output.SetList(this, DA, 1, new List<OasysGH.Units.GH_UnitNumber>(second.Select(x => new OasysGH.Units.GH_UnitNumber(x))));
      Output.SetTree(this, DA, 2, new DataTree<OasysGH.Units.GH_UnitNumber>(new List<OasysGH.Units.GH_UnitNumber>(second.Select(x => new OasysGH.Units.GH_UnitNumber(x)))));

      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(2, ref gh_typ))
      {
        Output.SetItem(this, DA, 3, gh_typ);
      }
    }
    #region Custom UI
    private LengthUnit LengthUnit = DefaultUnits.LengthUnitSection;
    private MomentUnit MomentUnit = DefaultUnits.MomentUnit;

    public override void InitialiseDropdowns()
    {
      this.SpacerDescriptions = new List<string>(new string[] { "Length Unit", "Moment Unit" });

      this.DropDownItems = new List<List<string>>();
      this.SelectedItems = new List<string>();

      // length
      this.DropDownItems.Add(FilteredUnits.FilteredLengthUnits);
      this.SelectedItems.Add(this.LengthUnit.ToString());

      // moment
      this.DropDownItems.Add(FilteredUnits.FilteredMomentUnits);
      this.SelectedItems.Add(this.MomentUnit.ToString());

      this.IsInitialised = true;
    }

    public override void SetSelected(int i, int j)
    {
      this.SelectedItems[i] = this.DropDownItems[i][j];
      if (i == 0)
        this.LengthUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), this.SelectedItems[i]);
      if (i == 1)
        this.MomentUnit = (MomentUnit)Enum.Parse(typeof(MomentUnit), this.SelectedItems[i]);
      base.UpdateUI();
    }

    public override void UpdateUIFromSelectedItems()
    {
      this.LengthUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), this.SelectedItems[0]);
      this.MomentUnit = (MomentUnit)Enum.Parse(typeof(MomentUnit), this.SelectedItems[1]);
      base.UpdateUIFromSelectedItems();
    }

    public override void VariableParameterMaintenance()
    {
      string lengthUnitAbbreviation = Length.GetAbbreviation(this.LengthUnit);
      string momenthUnitAbbreviation = Moment.GetAbbreviation(this.MomentUnit);
      this.Params.Input[0].Name = "Length [" + lengthUnitAbbreviation + "]";
      this.Params.Input[1].Name = "Moment [" + momenthUnitAbbreviation + "]";
    }
    #endregion
  }
}
