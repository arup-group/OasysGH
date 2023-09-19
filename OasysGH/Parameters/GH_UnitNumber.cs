using System;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using OasysUnits;

namespace OasysGH.Parameters {
  /// <summary>
  /// Goo wrapper class, makes sure OasysUnits <see cref="IQuantity"/> can be used in Grasshopper.
  /// </summary>
  public class GH_UnitNumber : GH_OasysGoo<IQuantity> {
    public static string Description => "A value with a unit measure. Note that this is not a text but an actual object, you can convert this into other units using the 'ConvertUnitNumber' component.";
    public static string Name => "UnitNumber";
    public static string NickName => "UN";
    public override OasysPluginInfo PluginInfo => OasysGH.PluginInfo.Instance;

    public GH_UnitNumber(IQuantity item) : base(item) {
    }

    public override bool CastFrom(object source) {
      if (source == null) {
        return false;
      }

      if (typeof(GH_UnitNumber).IsAssignableFrom(source.GetType())) {
        var num = (GH_UnitNumber)source;
        Value = num.m_value;
        return true;
      }

      if (GH_Convert.ToString(source, out string txt, GH_Conversion.Both)) {
        var types = Quantity.Infos.Select(x => x.ValueType).ToList();
        foreach (Type type in types) {
          if (Quantity.TryParse(type, txt, out IQuantity quantity)) {
            Value = quantity;
            return true;
          }
        }
      }

      return false;
    }

    public override bool CastTo<Q>(ref Q target) {
      if (typeof(Q).IsAssignableFrom(typeof(GH_UnitNumber))) {
        target = (Q)(object)new GH_UnitNumber(Value);
        return true;
      }

      if (typeof(Q).IsAssignableFrom(typeof(GH_Number))) {
        if (Value == null) {
          target = default;
        } else {
          target = (Q)(object)new GH_Number(Value.Value);
        }

        return true;
      }

      target = default;
      return false;
    }

    public override IGH_Goo Duplicate() => new GH_UnitNumber(Value);

    public override string ToString() {
      if (Value == null) {
        return "Null";
      }

      return Value.ToString().Replace(",", string.Empty).Replace(" ", string.Empty);
    }
  }
}
