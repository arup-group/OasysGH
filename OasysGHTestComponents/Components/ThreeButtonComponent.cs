using System;
using Grasshopper.Kernel;
using OasysGH.UI;
using static OasysGHTestComponents.OasysGHTestComponentsInfo;

namespace OasysGH.Components.Tests {
  public class ThreeButtonComponent : GH_OasysDropDownComponent {
    public override Guid ComponentGuid => new Guid("e6e42b78-1255-42be-88cd-2b8043e376da");
    public override GH_Exposure Exposure => GH_Exposure.primary;
    public override OasysPluginInfo PluginInfo => OasysGHTestComponentsPluginInfo.Instance;
    private bool _firstWasClicked = false;
    private bool _secondWasClicked = false;
    private bool _thirdWasClicked = false;
    public ThreeButtonComponent()
      : base("ThreeButtonComponent", "3B", "A three button component", "OasysGH", "Test") { }

    public override void CreateAttributes() {
      m_attributes = new ThreeButtonComponentAttributes(this, "Button1", "Button2", "Button3",
        ClickedFirst, ClickedSecond, ClickedThird, true, "Three buttons");
    }

    public void ClickedFirst() {
      _firstWasClicked = true;
      base.UpdateUI();
    }
    public void ClickedSecond() {
      _secondWasClicked = true;
      base.UpdateUI();
    }
    public void ClickedThird() {
      _thirdWasClicked = true;
      base.UpdateUI();
    }
    public override void SetSelected(int i, int j) { }
    protected override void InitialiseDropdowns() { }
    protected override void RegisterInputParams(GH_InputParamManager pManager) {
      pManager.AddBooleanParameter("Dummy", "D", "A dummy input", GH_ParamAccess.item, true);
    }
    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddBooleanParameter(
        "1Clicked", "C1", "The first button was clicked", GH_ParamAccess.item);
      pManager.AddBooleanParameter(
        "2Clicked", "C2", "The second button was clicked", GH_ParamAccess.item);
      pManager.AddBooleanParameter(
        "3Clicked", "C3", "The third button was clicked", GH_ParamAccess.item);
    }

    protected override void SolveInternal(IGH_DataAccess da) {
      da.SetData(0, _firstWasClicked);
      da.SetData(1, _secondWasClicked);
      da.SetData(2, _thirdWasClicked);
    }
  }
}
