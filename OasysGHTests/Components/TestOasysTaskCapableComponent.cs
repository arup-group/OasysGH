using System;
using Grasshopper.Kernel;
using OasysGH;
using OasysGH.Components;

namespace OasysGHTests.Components {
  internal class TestOasysTaskCapableComponent : GH_OasysTaskCapableComponent<Type> {
    public override Guid ComponentGuid => new Guid("2f5ead72-7582-49cc-a61e-412c0bef7049");
    public override OasysPluginInfo PluginInfo => OasysGH.PluginInfo.Instance;

    public TestOasysTaskCapableComponent() : base("name", "nickname", "description", "category", "subCategory") {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager) {
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
    }

    protected override void SolveInstance(IGH_DataAccess DA) {
    }
  }
}
