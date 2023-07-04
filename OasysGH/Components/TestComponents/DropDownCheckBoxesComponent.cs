using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using OasysGH.UI;
using OasysGH.Units.Helpers;
using OasysUnits.Units;
using OasysUnits;
using OasysGH.Units;

namespace OasysGH.Components.TestComponents {
  public class DropDownCheckBoxesComponent : GH_OasysDropDownComponent {
    public override Guid ComponentGuid => new Guid("1d8ef8e7-2a17-4fb9-bab1-2527a7bfc7b9");
    public override GH_Exposure Exposure => GH_Exposure.hidden;
    public override OasysPluginInfo PluginInfo => OasysGH.PluginInfo.Instance;
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
      if (!_isInitialised) {
        InitialiseDropdowns();
      }

      m_attributes = new DropDownCheckBoxesComponentAttributes(this, SetSelected, _dropDownItems,
        _selectedItems, CheckBox, _initialCheckState, _checkboxTexts, _spacerDescriptions);
    }

    public void CheckBox(List<bool> value) {
      _isChecked = value[0];
    }

    protected internal override void InitialiseDropdowns() {
      _spacerDescriptions = new List<string>(new[] {
        "Dropdown",
        "CheckBox",
      });

      _dropDownItems = new List<List<string>>();
      _selectedItems = new List<string>();

      _dropDownItems.Add(UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Length));
      _selectedItems.Add(Length.GetAbbreviation(_lengthUnit));

      _isInitialised = true;
    }
    public override void SetSelected(int i, int j) {
      _selectedItems[i] = _dropDownItems[i][j];
      _lengthUnit = (LengthUnit)UnitsHelper.Parse(typeof(LengthUnit), _selectedItems[i]);
      base.UpdateUI();
    }
    protected override void RegisterInputParams(GH_InputParamManager pManager) { }
    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddBooleanParameter("Checked", "C", "The check box is checked", GH_ParamAccess.item);
    }

    protected override void SolveInstance(IGH_DataAccess da) {
      da.SetData(0, _isChecked);
    }
  }
}
