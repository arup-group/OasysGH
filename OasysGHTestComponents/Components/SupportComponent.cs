using System;
using Grasshopper.Kernel;
using OasysGH;
using OasysGH.Components;
using OasysGH.UI;
using static OasysGHTestComponents.OasysGHTestComponentsInfo;

namespace OasysGH.Components.Tests {
  public class SupportComponent : GH_OasysDropDownComponent {
    public override Guid ComponentGuid => new Guid("351bcef4-bf86-46c8-a9a8-8a3ef23248a2");
    public override GH_Exposure Exposure => GH_Exposure.hidden;
    public override OasysPluginInfo PluginInfo => OasysGHTestComponentsPluginInfo.Instance;
    private bool _x;
    private bool _xx;
    private bool _y;
    private bool _yy;
    private bool _z;
    private bool _zz;

    public SupportComponent()
      : base("SupportComponent", "Sup", "A support component", "OasysGH", "Test") { }

    public override void CreateAttributes() {
      m_attributes = new SupportComponentAttributes(this, SetRestraints, "Restraints",
        _x, _y, _z, _xx, _yy, _zz);
    }

    public void SetRestraints(bool resx, bool resy, bool resz, bool resxx, bool resyy, bool reszz) {
      _x = resx;
      _y = resy;
      _z = resz;
      _xx = resxx;
      _yy = resyy;
      _zz = reszz;

      base.UpdateUI();
    }
    public override void SetSelected(int i, int j) { }
    protected override void InitialiseDropdowns() { }
    protected override void RegisterInputParams(GH_InputParamManager pManager) {
      pManager.AddBooleanParameter("Dummy", "D", "A dummy input", GH_ParamAccess.item, true);
    }
    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddBooleanParameter("X", "X", "X", GH_ParamAccess.item);
      pManager.AddBooleanParameter("Y", "Y", "Y", GH_ParamAccess.item);
      pManager.AddBooleanParameter("Z", "Z", "Z", GH_ParamAccess.item);
      pManager.AddBooleanParameter("XX", "XX", "XX", GH_ParamAccess.item);
      pManager.AddBooleanParameter("YY", "YY", "YY", GH_ParamAccess.item);
      pManager.AddBooleanParameter("ZZ", "ZZ", "ZZ", GH_ParamAccess.item);
    }

    protected override void SolveInternal(IGH_DataAccess da) {
      da.SetData(0, _x);
      da.SetData(1, _y);
      da.SetData(2, _z);
      da.SetData(3, _xx);
      da.SetData(4, _yy);
      da.SetData(5, _zz);
    }
  }
}
