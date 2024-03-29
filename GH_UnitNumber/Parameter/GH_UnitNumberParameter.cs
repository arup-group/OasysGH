﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using OasysUnits;

namespace GH_UnitNumber {
  /// <summary>
  /// This class provides a Parameter interface for <see cref="GH_UnitNumber"/>.
  /// </summary>
  public class GH_UnitNumberParameter : GH_PersistentParam<OasysGH.Parameters.GH_UnitNumber> {
    public override Guid ComponentGuid => new Guid("007d8fa3-aeb6-492a-b885-4736925f22a8");
    public override GH_Exposure Exposure => GH_Exposure.quinary | GH_Exposure.obscure;
    public bool Hidden {
      get { return true; }
    }

    public override string InstanceDescription => m_data.DataCount == 0 ? "Empty " + OasysGH.Parameters.GH_UnitNumber.Name + " parameter" : base.InstanceDescription;
    public bool IsPreviewCapable {
      get { return false; }
    }

    public override string TypeName => SourceCount == 0 ? OasysGH.Parameters.GH_UnitNumber.Name : base.TypeName;
    protected override System.Drawing.Bitmap Icon => Properties.Resources.UnitParam;

    public GH_UnitNumberParameter() : base(new GH_InstanceDescription(
      OasysGH.Parameters.GH_UnitNumber.Name,
      OasysGH.Parameters.GH_UnitNumber.NickName,
      OasysGH.Parameters.GH_UnitNumber.Description,
      "Params",
      "Primitive")) {
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

    protected override OasysGH.Parameters.GH_UnitNumber PreferredCast(object data) {
      switch (data) {
        case IQuantity quantity:
          return new OasysGH.Parameters.GH_UnitNumber(quantity);

        case GH_ObjectWrapper gooWrapper:
          object val = gooWrapper.Value;
          if (typeof(IQuantity).IsAssignableFrom(val.GetType())) {
            return new OasysGH.Parameters.GH_UnitNumber((IQuantity)val);
          }

          break;
      }

      GH_Convert.ToString(data, out string txt, GH_Conversion.Both);
      var types = Quantity.Infos.Select(x => x.ValueType).ToList();

      /// remove some types that have overlapping abbreviations like "m" for minute and meter
      Type axialStiffness = types.Where(t => t.Name == "AxialStiffness").ToList()[0];
      types.Remove(axialStiffness);
      Type bendingStiffness = types.Where(t => t.Name == "BendingStiffness").ToList()[0];
      types.Remove(bendingStiffness);
      Type duration = types.Where(t => t.Name == "Duration").ToList()[0];
      Type massFraction = types.Where(t => t.Name == "MassFraction").ToList()[0];
      types.Remove(duration);

      foreach (Type type in types) {
        if (Quantity.TryParse(type, txt, out IQuantity quantity))
          return new OasysGH.Parameters.GH_UnitNumber(quantity);
      }

      var alternativeTypes = new List<Type> {
          axialStiffness,
          bendingStiffness,
          duration,
          massFraction
        };
      foreach (Type type in alternativeTypes) {
        if (Quantity.TryParse(type, txt, out IQuantity quantity)) {
          return new OasysGH.Parameters.GH_UnitNumber(quantity);
        }
      }

      AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to cast " + txt + " to any know quantity. Input a string like '15 mm' or '12.4 slug'");
      return base.PreferredCast(data);
    }

    protected override GH_GetterResult Prompt_Plural(ref List<OasysGH.Parameters.GH_UnitNumber> values) {
      return GH_GetterResult.cancel;
    }

    protected override GH_GetterResult Prompt_Singular(ref OasysGH.Parameters.GH_UnitNumber value) {
      return GH_GetterResult.cancel;
    }
  }
}
