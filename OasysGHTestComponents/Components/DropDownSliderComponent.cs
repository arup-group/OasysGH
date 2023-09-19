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
  public class DropDownSliderComponent : GH_OasysDropDownComponent {
    public override Guid ComponentGuid => new Guid("dbcce9f9-2028-42ed-b392-73cba819218a");
    public override GH_Exposure Exposure => GH_Exposure.hidden;
    public override OasysPluginInfo PluginInfo => OasysGHTestComponentsPluginInfo.Instance;
    private LengthUnit _lengthUnit = DefaultUnits.LengthUnitGeometry;
    private double _value = 500;
    private double _maxValue = 1000;
    private double _minValue = -250;

    public DropDownSliderComponent()
      : base("DropDownSliderComponent", "DDS", "A DropDown and Slider component", "OasysGH", "Test") { }

    public override void CreateAttributes() {
      if (!_isInitialised) {
        InitialiseDropdowns();
      }

      m_attributes = new DropDownSliderComponentAttributes(this, SetSelected, _dropDownItems,
        _selectedItems, true, SetVal, SetMaxMin, _value, _maxValue, _minValue, 3,
        _spacerDescriptions);
    }

    public void SetVal(double value) {
      _value = value;
    }
    public void SetMaxMin(double max, double min) {
      _maxValue = max;
      _minValue = min;
    }

    protected override void InitialiseDropdowns() {
      _spacerDescriptions = new List<string>(new[] {
        "Dropdown",
        "Slider",
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
    protected override void RegisterInputParams(GH_InputParamManager pManager) {
      pManager.AddBooleanParameter("Dummy", "D", "A dummy input", GH_ParamAccess.item, true);
    }
    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddNumberParameter("Value", "Val", "The slider's value", GH_ParamAccess.item);
      pManager.AddNumberParameter("MaxValue", "Max", "The slider's upper bound value", GH_ParamAccess.item);
      pManager.AddNumberParameter("MinValue", "Min", "The slider's lower bound value", GH_ParamAccess.item);
    }

    protected override void SolveInternal(IGH_DataAccess da) {
      da.SetData(0, _value);
      da.SetData(1, _maxValue);
      da.SetData(2, _minValue);
    }
  }
}
