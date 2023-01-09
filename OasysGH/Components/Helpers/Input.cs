using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using OasysUnits;
using OasysGH.Parameters;
using System.Globalization;
using OasysUnits.Units;

namespace OasysGH.Helpers
{
  public class Input
  {
    /// <summary>
    /// Helper method to get UnitNumber input parameter item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="owner"></param>
    /// <param name="DA"></param>
    /// <param name="inputid"></param>
    /// <param name="unit"></param>
    /// <param name="isOptional"></param>
    /// <returns></returns>
    public static IQuantity UnitNumber<T>(GH_Component owner, IGH_DataAccess DA, int inputid, T unit, bool isOptional = false) where T : Enum
    {
      GH_UnitNumber unitNumber = null;
      IQuantity zeroReturn = null;
      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(inputid, ref gh_typ))
      {
        // try cast directly to quantity type
        if (gh_typ.Value is GH_UnitNumber)
        {
          unitNumber = (GH_UnitNumber)gh_typ.Value;
          // check that unit is of right type
          if (!unitNumber.Value.QuantityInfo.UnitType.Equals(typeof(T)))
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error in " + owner.Params.Input[inputid].NickName + " input: Wrong unit type"
                + Environment.NewLine + "Unit type is " + unitNumber.Value.QuantityInfo.Name + " but must be " + typeof(T));
            Quantity.TryFrom(0, unit, out zeroReturn);
            return zeroReturn;
          }
          else
            return unitNumber.Value;
        }

        // try cast to double
        else if (GH_Convert.ToDouble(gh_typ.Value, out double val, GH_Conversion.Both))
        {
          // create new quantity from default units
          if (Quantity.TryFrom(val, unit, out IQuantity quantity))
            return quantity;
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
            Quantity.TryFrom(0, unit, out zeroReturn);
            return zeroReturn;
          }
        }

        // try cast to string
        else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        {
          Quantity.TryFrom(0, unit, out IQuantity zeroType);
          Type valueType = zeroType.QuantityInfo.ValueType;
          if (Quantity.TryParse(valueType, txt, out IQuantity quantity))
            return quantity;
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
            Quantity.TryFrom(0, unit, out zeroReturn);
            return zeroReturn;
          }
        }
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
          Quantity.TryFrom(0, unit, out zeroReturn);
          return zeroReturn;
        }
      }
      else if (!owner.Params.Input[inputid].Optional)
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");

      Quantity.TryFrom(0, unit, out zeroReturn);
      return zeroReturn;
    }

    /// <summary>
    /// Helper method to get UnitNumber input parameter list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="owner"></param>
    /// <param name="DA"></param>
    /// <param name="inputid"></param>
    /// <param name="unit"></param>
    /// <returns></returns>
    public static List<IQuantity> UnitNumberList<T>(GH_Component owner, IGH_DataAccess DA, int inputid, T unit) where T : Enum
    {
      List<IQuantity> items = new List<IQuantity>();
      List<GH_ObjectWrapper> gh_typs = new List<GH_ObjectWrapper>();
      IQuantity zeroReturn = null;

      if (DA.GetDataList(inputid, gh_typs))
      {
        for (int i = 0; i < gh_typs.Count; i++)
        {
          GH_UnitNumber unitNumber = null;
          // try cast directly to quantity type
          if (gh_typs[i].Value is GH_UnitNumber)
          {
            unitNumber = (GH_UnitNumber)gh_typs[i].Value;
            // check that unit is of right type
            if (!unitNumber.Value.QuantityInfo.UnitType.Equals(typeof(T)))
            {
              owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Error in " + owner.Params.Input[inputid].NickName + " input (index " + i + " (" + gh_typs[i].Value.GetType().Name + ")): Wrong unit type " + Environment.NewLine + "Unit type is " + unitNumber.Value.QuantityInfo.Name + " but must be " + typeof(T));
              Quantity.TryFrom(0, unit, out zeroReturn);
              items.Add(zeroReturn);
            }
            else
              items.Add(unitNumber.Value);
          }

          // try cast to double
          else if (GH_Convert.ToDouble(gh_typs[i].Value, out double val, GH_Conversion.Both))
          {
            // create new quantity from default units
            if (Quantity.TryFrom(val, unit, out IQuantity quantity))
              items.Add(quantity);
            else
            {
              owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Unable to convert " + owner.Params.Input[inputid].NickName + " (index " + i + ") to UnitNumber");
              Quantity.TryFrom(0, unit, out zeroReturn);
              items.Add(zeroReturn);
            }
          }

          // try cast to string
          else if (GH_Convert.ToString(gh_typs[i].Value, out string txt, GH_Conversion.Both))
          {
            Quantity.TryFrom(0, unit, out IQuantity zeroType);
            Type valueType = zeroType.QuantityInfo.ValueType;
            if (Quantity.TryParse(valueType, txt, out IQuantity quantity))
              items.Add(quantity);
            else
            {
              owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Unable to convert " + owner.Params.Input[inputid].NickName + " (index " + i + ") to UnitNumber");
              Quantity.TryFrom(0, unit, out zeroReturn);
              items.Add(zeroReturn);
            }
          }

          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Unable to convert " + owner.Params.Input[inputid].NickName + " (index " + i + ") to UnitNumber");
            Quantity.TryFrom(0, unit, out zeroReturn);
            items.Add(zeroReturn);
          }
        }
        return items;
      }
      else if (!owner.Params.Input[inputid].Optional)
      {
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      }
      return items;
    }

    /// <summary>
    /// Helper method to get custom parameter input item
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    /// <param name="owner"></param>
    /// <param name="DA"></param>
    /// <param name="inputid"></param>
    /// <returns></returns>
    public static object GenericGoo<Type>(GH_Component owner, IGH_DataAccess DA, int inputid)
    {
      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (owner.Params.Input[inputid].Sources.Count == 0 & owner.Params.Input[inputid].Optional)
        return null;

      if (DA.GetData(inputid, ref gh_typ))
      {
        if (gh_typ.Value is Type)
        {
          return (Type)gh_typ.Value;
        }
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " input (" + gh_typ.Value.GetType().Name + ") to " + typeof(Type).Name.Replace("Goo", string.Empty));
          return null;
        }
      }
      else if (!owner.Params.Input[inputid].Optional)
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");

      return null;
    }

    /// <summary>
    /// Helper method to get custom parameter input list
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    /// <param name="owner"></param>
    /// <param name="DA"></param>
    /// <param name="inputid"></param>
    /// <returns></returns>
    public static List<Type> GenericGooList<Type>(GH_Component owner, IGH_DataAccess DA, int inputid)
    {
      List<Type> items = new List<Type>();
      List<GH_ObjectWrapper> gh_typs = new List<GH_ObjectWrapper>();

      if (owner.Params.Input[inputid].Sources.Count == 0 & owner.Params.Input[inputid].Optional)
        return null;

      if (DA.GetDataList(inputid, gh_typs))
      {
        for (int i = 0; i < gh_typs.Count; i++)
        {
          if (gh_typs[i].Value is Type)
          {
            items.Add((Type)gh_typs[i].Value);
          }
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Unable to convert " + owner.Params.Input[inputid].NickName + " input (index " + i + " (" + gh_typs[i].Value.GetType().Name + ")) to " + typeof(Type).Name.Replace("Goo", string.Empty));
            continue;
          }
        }
        return items;
      }
      else if (!owner.Params.Input[inputid].Optional)
      {
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      }
      return null;
    }

    /// <summary>
    /// Helper method to get either Length or Ratio input
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="DA"></param>
    /// <param name="inputid"></param>
    /// <param name="lengthUnit"></param>
    /// <param name="isOptional"></param>
    /// <returns></returns>
    public static IQuantity LengthOrRatio(GH_Component owner, IGH_DataAccess DA, int inputid, LengthUnit lengthUnit, bool isOptional = false)
    {
      GH_UnitNumber unitNumber = null;
      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(inputid, ref gh_typ))
      {
        // try cast directly to quantity type
        if (gh_typ.Value is GH_UnitNumber)
        {
          unitNumber = (GH_UnitNumber)gh_typ.Value;
          // check that unit is of right type
          if (!unitNumber.Value.QuantityInfo.UnitType.Equals(typeof(LengthUnit)) &&
            !unitNumber.Value.QuantityInfo.UnitType.Equals(typeof(RatioUnit)))
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error in " + owner.Params.Input[inputid].NickName + " input: Wrong unit type"
                + Environment.NewLine + "Unit type is " + unitNumber.Value.QuantityInfo.Name + " but must be Length or Ratio");
            return null;
          }
        }
        // try cast to double
        else if (GH_Convert.ToDouble(gh_typ.Value, out double val, GH_Conversion.Both))
        {
          // create new quantity from default units
          if (val < 0)
            unitNumber = new GH_UnitNumber(new Ratio(Math.Abs(val), RatioUnit.DecimalFraction).ToUnit(RatioUnit.Percent));
          else
            unitNumber = new GH_UnitNumber(new Length(val, lengthUnit));
        }
        // try cast to text
        else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        {
          if (txt.EndsWith("%"))
          {
            NumberFormatInfo noComma = CultureInfo.InvariantCulture.NumberFormat;
            unitNumber = new GH_UnitNumber(new Ratio(Convert.ToDouble(txt.Replace("%", string.Empty).Replace(" ", string.Empty), noComma), RatioUnit.Percent));
          }
          else if (Length.TryParse(txt, out Length parsed))
            unitNumber = new GH_UnitNumber(parsed);
          else if (Length.TryParseFeetInches(txt, out Length parsedFeetInches))
            unitNumber = new GH_UnitNumber(parsedFeetInches);
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
            return Length.Zero;
          }
        }
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
          return null;
        }
      }
      else if (!isOptional)
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");

      if (unitNumber == null)
        return null;

      return unitNumber.Value;
    }

    /// <summary>
    /// Helper method to get a list of either Length or Ratio inputs
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="DA"></param>
    /// <param name="inputid"></param>
    /// <param name="lengthUnit"></param>
    /// <param name="isOptional"></param>
    /// <returns></returns>
    public static List<IQuantity> LengthsOrRatios(GH_Component owner, IGH_DataAccess DA, int inputid, LengthUnit lengthUnit, bool isOptional = false)
    {
      List<IQuantity> lengths = new List<IQuantity>();
      List<GH_ObjectWrapper> gh_typs = new List<GH_ObjectWrapper>();
      if (DA.GetDataList(inputid, gh_typs))
      {
        for (int i = 0; i < gh_typs.Count; i++)
        {
          GH_UnitNumber unitNumber = null;
          // try cast directly to quantity type
          if (gh_typs[i].Value is GH_UnitNumber)
          {
            unitNumber = (GH_UnitNumber)gh_typs[i].Value;
            // check that unit is of right type
            if (!unitNumber.Value.QuantityInfo.UnitType.Equals(typeof(LengthUnit)) &&
              !unitNumber.Value.QuantityInfo.UnitType.Equals(typeof(RatioUnit)))
            {
              owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Error in " + owner.Params.Input[inputid].NickName + " (item " + i + ") input: Wrong unit type"
                  + Environment.NewLine + "Unit type is " + unitNumber.Value.QuantityInfo.Name + " but must be Length or Ratio");
            }
            else
            {
              lengths.Add(unitNumber.Value);
            }
          }
          // try cast to double
          else if (GH_Convert.ToDouble(gh_typs[i].Value, out double val, GH_Conversion.Both))
          {
            // create new quantity from default units
            if (val < 0)
              lengths.Add(new Ratio(Math.Abs(val), RatioUnit.DecimalFraction));
            else
              lengths.Add(new Length(val, lengthUnit));
          }
          // try cast to text
          else if (GH_Convert.ToString(gh_typs[i].Value, out string txt, GH_Conversion.Both))
          {
            NumberFormatInfo noComma = CultureInfo.InvariantCulture.NumberFormat;
            if (txt.EndsWith("%"))
            {
              lengths.Add(new Ratio(Convert.ToDouble(txt.Replace("%", string.Empty).Replace(" ", string.Empty), noComma), RatioUnit.Percent));
            }
            else if (Length.TryParse(txt, noComma, out Length parsed))
              lengths.Add(parsed);
            else if (Length.TryParseFeetInches(txt, out Length parsedFeetInches, noComma))
              lengths.Add(parsedFeetInches);
            else
            {
              owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Unable to convert " + owner.Params.Input[inputid].NickName + " (item " + i + ") to UnitNumber");
              continue;
            }
          }
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Unable to convert " + owner.Params.Input[inputid].NickName + " (item " + i + ") to UnitNumber");
            continue;
          }
        }
        return lengths;
      }
      else if (!isOptional)
      {
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      }
      return null;
    }
  }
}
