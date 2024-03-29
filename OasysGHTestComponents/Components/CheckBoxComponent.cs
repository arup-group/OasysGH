﻿using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using OasysGH.UI;
using static OasysGHTestComponents.OasysGHTestComponentsInfo;

namespace OasysGH.Components.Tests {
  public class CheckBoxComponent : GH_OasysDropDownComponent {
    public override Guid ComponentGuid => new Guid("f58b0461-4f16-4051-bc00-1ac45d4d8b46");
    public override GH_Exposure Exposure => GH_Exposure.primary;
    public override OasysPluginInfo PluginInfo => OasysGHTestComponentsPluginInfo.Instance;
    private bool _x;
    private bool _xx;
    private bool _y;
    private bool _yy;
    private bool _z;
    private bool _zz;

    public CheckBoxComponent()
      : base("CheckBoxComponent", "CB", "A checkbox component", "OasysGH", "Test") { }

    public override void CreateAttributes() {
      m_attributes = new CheckBoxComponentAttributes(this, SetReleases,
        new List<string>() {
          "Set 6 DOF",
        },
        new List<List<bool>>() {
          new List<bool>() {
            _x, _y, _z, _xx, _yy, _zz,
            },
          },
        new List<List<string>>() {
          new List<string>() {
            "x", "y", "z", "xx", "yy", "zz",
          },
        });
    }

    public void SetReleases(List<List<bool>> bool6) {
      _x = bool6[0][0];
      _y = bool6[0][1];
      _z = bool6[0][2];
      _xx = bool6[0][3];
      _yy = bool6[0][4];
      _zz = bool6[0][5];
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
