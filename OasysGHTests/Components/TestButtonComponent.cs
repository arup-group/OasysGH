using System;
using Grasshopper.Kernel;
using OasysGH;
using OasysGH.Components;

namespace OasysGHTests.Components {
  internal class TestButtonComponent : GH_OasysDropDownComponent {

    public override OasysPluginInfo PluginInfo => OasysGH.PluginInfo.Instance;

    public override Guid ComponentGuid => new Guid("019d3045-2f5a-4224-9386-b4b1a0bee79a");

    public TestButtonComponent() : base("name", "nickname", "description", "category", "subCategory") {
    }

    public override void SetSelected(int i, int j) {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager) {
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
    }

    protected override void SolveInstance(IGH_DataAccess DA) {
    }

    protected internal override void InitialiseDropdowns() {
    }
  }
}
