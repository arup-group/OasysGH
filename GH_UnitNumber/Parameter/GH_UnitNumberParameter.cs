using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace GH_UnitNumber
{
  /// <summary>
  /// This class provides a Parameter interface for the CustomGoo type.
  /// </summary>
  public class GH_UnitNumberParameter : GH_PersistentParam<OasysGH.Parameters.GH_UnitNumber>
  {
    public GH_UnitNumberParameter()
      : base(new GH_InstanceDescription(
        OasysGH.Parameters.GH_UnitNumber.Name, 
        OasysGH.Parameters.GH_UnitNumber.NickName, 
        OasysGH.Parameters.GH_UnitNumber.Description, 
        "Params", 
        "Primitive"))
    { }
    public override string InstanceDescription => this.m_data.DataCount == 0 ? "Empty " + OasysGH.Parameters.GH_UnitNumber.Name + " parameter" : base.InstanceDescription;
    public override string TypeName => this.SourceCount == 0 ? OasysGH.Parameters.GH_UnitNumber.Name : base.TypeName;
    public override Guid ComponentGuid => new Guid("007d8fa3-aeb6-492a-b885-4736925f22a8");
    public override GH_Exposure Exposure => GH_Exposure.quinary | GH_Exposure.obscure;
    protected override System.Drawing.Bitmap Icon => Properties.Resources.UnitParam;
    protected override GH_GetterResult Prompt_Plural(ref List<OasysGH.Parameters.GH_UnitNumber> values)
    {
      return GH_GetterResult.cancel;
    }
    protected override GH_GetterResult Prompt_Singular(ref OasysGH.Parameters.GH_UnitNumber value)
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
