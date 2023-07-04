﻿using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using OasysGH.UI;

namespace OasysGH.Components.TestComponents {
  public class ThreeButtonComponent : GH_OasysDropDownComponent {
    public override Guid ComponentGuid => new Guid("e6e42b78-1255-42be-88cd-2b8043e376da");
    public override GH_Exposure Exposure => GH_Exposure.hidden;
    public override OasysPluginInfo PluginInfo => OasysGH.PluginInfo.Instance;
    private bool _firstWasClicked = false;
    private bool _secondWasClicked = false;
    private bool _thirdWasClicked = false;
    public ThreeButtonComponent()
      : base("ThreeButtonComponent", "3B", "A three button component", "OasysGH", "Test") { }

    public override void CreateAttributes() {
      m_attributes = new ThreeButtonAtrributes(this, "Button1", "Button2", "Button3",
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
    protected internal override void InitialiseDropdowns() { }
    protected override void RegisterInputParams(GH_InputParamManager pManager) { }
    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddBooleanParameter(
        "1Clicked", "C1", "The first button was clicked", GH_ParamAccess.item);
      pManager.AddBooleanParameter(
        "2Clicked", "C2", "The second button was clicked", GH_ParamAccess.item);
      pManager.AddBooleanParameter(
        "3Clicked", "C3", "The third button was clicked", GH_ParamAccess.item);
    }

    protected override void SolveInstance(IGH_DataAccess da) {
      da.SetData(0, _firstWasClicked);
      da.SetData(1, _secondWasClicked);
      da.SetData(2, _thirdWasClicked);
    }
  }
}
