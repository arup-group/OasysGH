using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grasshopper.Kernel;
using OasysUnits;

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

    protected override OasysGH.Parameters.GH_UnitNumber PreferredCast(object data)
    {
      if (GH_Convert.ToString(data, out string txt, GH_Conversion.Both))
      {
        List<Type> types = Quantity.Infos.Select(x => x.ValueType).ToList();
        
        Type axialStiffness = types.Where(t => t.Name == "AxialStiffness").ToList()[0];
        types.Remove(axialStiffness);
        Type bendingStiffness = types.Where(t => t.Name == "BendingStiffness").ToList()[0];
        types.Remove(bendingStiffness);
        
        foreach (Type type in types)
        {
          if (Quantity.TryParse(type, txt, out IQuantity quantity))
            return new OasysGH.Parameters.GH_UnitNumber(quantity);
        }

        List<Type> alternativeTypes = new List<Type>();
        types.Add(axialStiffness);
        types.Add(bendingStiffness);
        foreach (Type type in alternativeTypes)
        {
          if (Quantity.TryParse(type, txt, out IQuantity quantity))
            return new OasysGH.Parameters.GH_UnitNumber(quantity);
        }

        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to cast " + txt + " to any know quantity. Input a string like '15 mm' or '12.4 slug'");
        return null;
      }
      return base.PreferredCast(data);
    }
  }
}
