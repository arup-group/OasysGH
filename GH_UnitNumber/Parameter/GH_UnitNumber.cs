using GH_IO.Types;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using OasysGH.Parameters;
using UnitsNet;

namespace GH_UnitNumber
{
  /// <summary>
  /// Goo wrapper class, makes sure UnitsNet and OasysUnits <see cref="IQuantity"/> can be used in Grasshopper.
  /// </summary>
  public class GH_UnitNumber : GH_OasysGoo<IQuantity>
  {
    public static string Name => "UnitNumber";
    public static string NickName => "UN";
    public static string Description => "A value with a unit measure";
    public GH_UnitNumber(IQuantity item) : base(item) { }
    public override IGH_Goo Duplicate() => new GH_UnitNumber(this.Value);

    public override string ToString()
    {
      return this.Value.ToString().Replace(",", string.Empty).Replace(" ", string.Empty);
    }

    public override bool CastTo<Q>(ref Q target)
    {
      // This function is called when Grasshopper needs to convert this 
      // instance of UnitNumber into some other type Q.            

      if (typeof(Q).IsAssignableFrom(typeof(GH_UnitNumber)))
      {
        target = (Q)(object)new GH_UnitNumber(Value);
        return true;
      }

      if (typeof(Q).IsAssignableFrom(typeof(GH_Number)))
      {
        if (Value == null)
          target = default;
        else
          target = (Q)(object)new GH_Number(Value.Value);
        return true;
      }

      target = default;
      return false;
    }
    public override bool CastFrom(object source)
    {
      // This function is called when Grasshopper needs to convert other data 
      // into this parameter.

      if (source == null) { return false; }

      //Cast from own type
      if (typeof(GH_UnitNumber).IsAssignableFrom(source.GetType()))
      {
        GH_UnitNumber num = (GH_UnitNumber)source;
        Value = num.m_value;
        return true;
      }

      if (GH_Convert.ToString(source, out string txt, GH_Conversion.Both))
      {
        // TODO: a way to generically Parse text into quantity without knowing unit
        
      }

      return false;
    }
  }
}
