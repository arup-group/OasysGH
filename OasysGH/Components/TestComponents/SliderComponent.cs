using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using OasysGH.UI;
using OasysGH.Units.Helpers;
using OasysUnits.Units;
using OasysUnits;
using OasysGH.Units;

namespace OasysGH.Components.TestComponents {
  public class SliderComponent : GH_OasysDropDownComponent {
    public override Guid ComponentGuid => new Guid("3d185ae4-3013-425c-bfe0-029cf8b46d29");
    public override GH_Exposure Exposure => GH_Exposure.hidden;
    public override OasysPluginInfo PluginInfo => OasysGH.PluginInfo.Instance;
    private double _value = 500;
    private double _maxValue = 1000;
    private double _minValue = -250;

    public SliderComponent()
      : base("SliderComponent", "Sli", "A slider component", "OasysGH", "Test") { }

    public override void CreateAttributes() {
      m_attributes =
        new SliderComponentAttributes(this, SetVal, SetMaxMin, _value, _maxValue, _minValue, 3);
    }

    public void SetVal(double value) {
      _value = value;
    }
    public void SetMaxMin(double max, double min) {
      _maxValue = max;
      _minValue = min;
    }

    protected internal override void InitialiseDropdowns() { }
    public override void SetSelected(int i, int j) { }
    protected override void RegisterInputParams(GH_InputParamManager pManager) { }
    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddNumberParameter("Value", "Val", "The slider's value", GH_ParamAccess.item);
      pManager.AddNumberParameter("MaxValue", "Max", "The slider's upper bound value", GH_ParamAccess.item);
      pManager.AddNumberParameter("MinValue", "Min", "The slider's lower bound value", GH_ParamAccess.item);
    }

    protected override void SolveInstance(IGH_DataAccess da) {
      da.SetData(0, _value);
      da.SetData(1, _maxValue);
      da.SetData(2, _minValue);
    }
  }
}
