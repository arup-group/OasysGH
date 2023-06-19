using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using OasysGH;
using OasysGH.Components;
using OasysGH.UI;

namespace OasysGHTests.Components {
  internal class TestCheckBoxComponentComponent : GH_OasysDropDownComponent {
    public override Guid ComponentGuid => new Guid("de53ffb2-fe5d-47dc-a2a8-ac98feede489");
    public override OasysPluginInfo PluginInfo => OasysGH.PluginInfo.Instance;

    public TestCheckBoxComponentComponent() : base("name", "nickname", "description", "category", "subCategory") {
    }

    public override void CreateAttributes() {
      m_attributes = new CheckBoxComponentComponentAttributes(this, UpdateHandle,
        new List<string>() { "spacerText" },
        new List<List<bool>>() {
          new List<bool>() { false },
        },
        new List<List<string>>() {
          new List<string>() { "text" },
        });
    }

    public override void SetSelected(int i, int j) {
    }

    protected internal override void InitialiseDropdowns() {
      _isInitialised = true;
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager) {
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
    }

    protected override void SolveInstance(IGH_DataAccess DA) {
    }

    private void UpdateHandle(List<List<bool>> bool6) {
    }
  }
}
