using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using UnitsNet;
using OasysGH.Units;

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
            return null;
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
            return null;
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
            return null;
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
      return null;
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
    internal static List<IQuantity> UnitNumberList<T>(GH_Component owner, IGH_DataAccess DA, int inputid, T unit) where T : Enum
    {
      List<IQuantity> items = new List<IQuantity>();
      List<GH_ObjectWrapper> gh_typs = new List<GH_ObjectWrapper>();

      if (owner.Params.Input[inputid].Sources.Count == 0 & owner.Params.Input[inputid].Optional)
        return null;

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
              items.Add(null);
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
              items.Add(null);
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
              items.Add(null);
            }
          }

          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Unable to convert " + owner.Params.Input[inputid].NickName + " (index " + i + ") to UnitNumber");
            items.Add(null);
          }

        }
        return items;
      }
      else
      {
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      }
      return null;
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
      else
      {
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      }
      return null;
    }
  }
}
