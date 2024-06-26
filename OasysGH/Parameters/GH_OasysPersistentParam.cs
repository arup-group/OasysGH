﻿using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace OasysGH.Parameters {
  public abstract class GH_OasysPersistentParam<T> : GH_PersistentParam<T> where T : class, IGH_Goo {
    public virtual bool Hidden {
      get => true;
      set { }
    }
    public virtual bool IsPreviewCapable => false;
    protected GH_OasysPersistentParam(GH_InstanceDescription nTag) : base(nTag) {
    }

    protected override ToolStripMenuItem Menu_CustomMultiValueItem() {
      var item = new ToolStripMenuItem {
        Text = "Not available",
        Visible = false
      };
      return item;
    }

    protected override ToolStripMenuItem Menu_CustomSingleValueItem() {
      var item = new ToolStripMenuItem {
        Text = "Not available",
        Visible = false
      };
      return item;
    }

    protected override GH_GetterResult Prompt_Plural(ref List<T> values) {
      return GH_GetterResult.cancel;
    }

    protected override GH_GetterResult Prompt_Singular(ref T value) {
      return GH_GetterResult.cancel;
    }
  }
}
