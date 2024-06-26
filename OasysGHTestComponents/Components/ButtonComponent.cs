﻿using System;
using Grasshopper.Kernel;
using OasysGH.UI;
using static OasysGHTestComponents.OasysGHTestComponentsInfo;

namespace OasysGH.Components.Tests {
  public class ButtonComponent : GH_OasysDropDownComponent {
    public override Guid ComponentGuid => new Guid("c5815cf0-465b-443d-8a62-fa1be6175530");
    public override GH_Exposure Exposure => GH_Exposure.primary;
    public override OasysPluginInfo PluginInfo => OasysGHTestComponentsPluginInfo.Instance;
    private bool _wasClicked = false;

    public ButtonComponent()
      : base("ButtonComponent", "But", "A button component", "OasysGH", "Test") { }

    public override void CreateAttributes() {
      m_attributes = new ButtonComponentAttributes(this, "Click", Clicked, "Simulate a mouse click");
    }

    public void Clicked() {
      _wasClicked = true;
      base.UpdateUI();
    }
    public override void SetSelected(int i, int j) { }

    protected override void InitialiseDropdowns() { }

    protected override void RegisterInputParams(GH_InputParamManager pManager) {
      pManager.AddBooleanParameter("Dummy", "D", "A dummy input", GH_ParamAccess.item, true);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddBooleanParameter("Clicked", "C", "The button was clicked", GH_ParamAccess.item);
    }

    protected override void SolveInternal(IGH_DataAccess da) {
      da.SetData(0, _wasClicked);
    }
  }
}
