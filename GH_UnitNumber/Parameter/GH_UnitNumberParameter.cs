using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using OasysGH.Parameters;

namespace GH_UnitNumber
{
  /// <summary>
  /// This class provides a Parameter interface for the CustomGoo type.
  /// </summary>
  public class GH_UnitNumberParameter : GH_PersistentParam<GH_UnitNumber>
  {
    public GH_UnitNumberParameter()
      : base(new GH_InstanceDescription(GH_UnitNumber.Name, GH_UnitNumber.NickName, "Maintains a collection of " + GH_UnitNumber.Description + " data", "Params", "Primitive"))
    {
    }

    public override Guid ComponentGuid => new Guid("007d8fa3-aeb6-492a-b885-4736925f22a8");

    public override GH_Exposure Exposure => GH_Exposure.quinary | GH_Exposure.obscure;

    protected override System.Drawing.Bitmap Icon => Properties.Resources.UnitParam;

    protected override GH_GetterResult Prompt_Plural(ref List<GH_UnitNumber> values)
    {
      return GH_GetterResult.cancel;
    }
    protected override GH_GetterResult Prompt_Singular(ref GH_UnitNumber value)
    {
      return GH_GetterResult.cancel;
    }
    protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomSingleValueItem()
    {
      System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem
      {
        Text = "Not available",
        Visible = false
      };
      return item;
    }
    protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomMultiValueItem()
    {
      System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem
      {
        Text = "Not available",
        Visible = false
      };
      return item;
    }

    #region preview methods
    public bool Hidden
    {
      get { return true; }
    }
    public bool IsPreviewCapable
    {
      get { return false; }
    }
    #endregion
  }

}
