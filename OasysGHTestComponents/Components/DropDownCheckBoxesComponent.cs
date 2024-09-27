using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using OasysGH.UI;
using OasysGH.Units.Helpers;
using OasysUnits.Units;
using OasysUnits;
using OasysGH.Units;
using static OasysGHTestComponents.OasysGHTestComponentsInfo;

namespace OasysGH.Components.Tests {
  public class DropDownCheckBoxesComponent : GH_OasysDropDownComponent {
    public override Guid ComponentGuid => new Guid("1d8ef8e7-2a17-4fb9-bab1-2527a7bfc7b9");
    public override GH_Exposure Exposure => GH_Exposure.primary;
    public override OasysPluginInfo PluginInfo => OasysGHTestComponentsPluginInfo.Instance;
    private bool _isChecked = false;
    private LengthUnit _lengthUnit = DefaultUnits.LengthUnitGeometry;
    private List<bool> _initialCheckState = new List<bool>() {
      false,
    };
    private readonly List<string> _checkboxTexts = new List<string>() {
      "CheckBox",
    };
    public DropDownCheckBoxesComponent()
      : base("DropDownCheckBoxes", "DDC", "A DropDown and CheckBoxes component", "OasysGH", "Test") { }

    public override void CreateAttributes() {
      if (!IsInitialised) {
        InitialiseDropdowns();
      }

      m_attributes = new DropDownCheckBoxesComponentAttributes(this, SetSelected, DropDownItems,
        SelectedItems, CheckBox, _initialCheckState, _checkboxTexts, SpacerDescriptions);
    }

    public void CheckBox(List<bool> value) {
      _isChecked = value[0];
    }

    protected override void InitialiseDropdowns() {
      SpacerDescriptions = new List<string>(new[] {
        "Dropdown",
        "CheckBox",
      });

      DropDownItems = new List<List<string>>();
      SelectedItems = new List<string>();

      DropDownItems.Add(UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Length));
      SelectedItems.Add(Length.GetAbbreviation(_lengthUnit));

      IsInitialised = true;
    }
    public override void SetSelected(int i, int j) {
      SelectedItems[i] = DropDownItems[i][j];
      _lengthUnit = (LengthUnit)UnitsHelper.Parse(typeof(LengthUnit), SelectedItems[i]);
      base.UpdateUI();
    }
    protected override void RegisterInputParams(GH_InputParamManager pManager) {
      pManager.AddBooleanParameter("Dummy", "D", "A dummy input", GH_ParamAccess.item, true);
    }
    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddBooleanParameter("Checked", "C", "The check box is checked", GH_ParamAccess.item);
    }

    protected override void SolveInternal(IGH_DataAccess da) {
      da.SetData(0, _isChecked);
    }
  }
}
