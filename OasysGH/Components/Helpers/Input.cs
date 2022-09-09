using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using UnitsNet;
using UnitsNet.Units;
using Oasys.Units;
using OasysGH.Units;
using System.Globalization;

namespace OasysGH.Helpers
{
  class Input
  {
    #region UnitsNet
    internal static Density Density(GH_Component owner, IGH_DataAccess DA, int inputid, DensityUnit densityUnit, bool isOptional = false)
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
          if (!unitNumber.Value.QuantityInfo.UnitType.Equals(typeof(DensityUnit)))
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error in " + owner.Params.Input[inputid].NickName + " input: Wrong unit type"
                + Environment.NewLine + "Unit type is " + unitNumber.Value.QuantityInfo.Name + " but must be Density");
            return UnitsNet.Density.Zero;
          }
        }
        // try cast to double
        else if (GH_Convert.ToDouble(gh_typ.Value, out double val, GH_Conversion.Both))
        {
          // create new quantity from default units
          unitNumber = new GH_UnitNumber(new Density(val, densityUnit));
        }
        // try cast to string
        else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        {
          if (UnitsNet.Density.TryParse(txt, out Density quantity))
            unitNumber = new GH_UnitNumber(quantity);
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
            return UnitsNet.Density.Zero;
          }
        }
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
          return UnitsNet.Density.Zero;
        }
      }
      else if (!isOptional)
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      else
      {
        if (unitNumber == null)
          return UnitsNet.Density.Zero;
      }

      return (Density)unitNumber.Value;
    }

    internal static Frequency Frequency(GH_Component owner, IGH_DataAccess DA, int inputid, FrequencyUnit frequencyUnit, bool isOptional = false)
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
          if (!unitNumber.Value.QuantityInfo.UnitType.Equals(typeof(DensityUnit)))
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error in " + owner.Params.Input[inputid].NickName + " input: Wrong unit type"
                + Environment.NewLine + "Unit type is " + unitNumber.Value.QuantityInfo.Name + " but must be Density");
            return UnitsNet.Frequency.Zero;
          }
        }
        // try cast to double
        else if (GH_Convert.ToDouble(gh_typ.Value, out double val, GH_Conversion.Both))
        {
          // create new quantity from default units
          unitNumber = new GH_UnitNumber(new Frequency(val, frequencyUnit));
        }
        // try cast to string
        else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        {
          if (UnitsNet.Frequency.TryParse(txt, out Frequency quantity))
            unitNumber = new GH_UnitNumber(quantity);
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
            return UnitsNet.Frequency.Zero;
          }
        }
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
          return UnitsNet.Frequency.Zero;
        }
      }
      else if (!isOptional)
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      else
      {
        if (unitNumber == null)
          return UnitsNet.Frequency.Zero;
      }

      return (Frequency)unitNumber.Value;
    }
    internal static Length Length(GH_Component owner, IGH_DataAccess DA, int inputid, LengthUnit docLengthUnit, bool isOptional = false)
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
          if (!unitNumber.Value.QuantityInfo.UnitType.Equals(typeof(LengthUnit)))
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error in " + owner.Params.Input[inputid].NickName + " input: Wrong unit type"
                + Environment.NewLine + "Unit type is " + unitNumber.Value.QuantityInfo.Name + " but must be Length");
            return UnitsNet.Length.Zero;
          }
        }
        // try cast to double
        else if (GH_Convert.ToDouble(gh_typ.Value, out double val, GH_Conversion.Both))
        {
          // create new quantity from default units
          unitNumber = new GH_UnitNumber(new Length(val, docLengthUnit));
        }
        // try cast to text
        else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        {
          if (UnitsNet.Length.TryParse(txt, out UnitsNet.Length parsed))
            unitNumber = new GH_UnitNumber(parsed);
          else if (UnitsNet.Length.TryParseFeetInches(txt, out UnitsNet.Length parsedFeetInches))
            unitNumber = new GH_UnitNumber(parsedFeetInches);
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
            return UnitsNet.Length.Zero;
          }
        }
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
          return UnitsNet.Length.Zero;
        }
      }
      else if (!isOptional)
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      else
      {
        if (unitNumber == null)
          return UnitsNet.Length.Zero;
      }

      return (Length)unitNumber.Value;
    }

    internal static IQuantity LengthOrRatio(GH_Component owner, IGH_DataAccess DA, int inputid, LengthUnit docLengthUnit, bool isOptional = false)
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
            unitNumber = new GH_UnitNumber(new Length(val, docLengthUnit));
        }
        // try cast to text
        else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        {
          if (txt.EndsWith("%"))
          {
            NumberFormatInfo noComma = CultureInfo.InvariantCulture.NumberFormat;
            unitNumber = new GH_UnitNumber(new Ratio(Convert.ToDouble(txt.Replace("%", string.Empty).Replace(" ", string.Empty), noComma), RatioUnit.Percent));
          }
          else if (UnitsNet.Length.TryParse(txt, out UnitsNet.Length parsed))
            unitNumber = new GH_UnitNumber(parsed);
          else if (UnitsNet.Length.TryParseFeetInches(txt, out UnitsNet.Length parsedFeetInches))
            unitNumber = new GH_UnitNumber(parsedFeetInches);
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
            return UnitsNet.Length.Zero;
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
      else
      {
        if (unitNumber == null)
          return null;
      }

      return unitNumber.Value;
    }
    internal static List<IQuantity> LengthsOrRatios(GH_Component owner, IGH_DataAccess DA, int inputid, LengthUnit docLengthUnit, bool isOptional = false)
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
              lengths.Add(new Length(val, docLengthUnit));
          }
          // try cast to text
          else if (GH_Convert.ToString(gh_typs[i].Value, out string txt, GH_Conversion.Both))
          {
            NumberFormatInfo noComma = CultureInfo.InvariantCulture.NumberFormat;
            if (txt.EndsWith("%"))
            {
              lengths.Add(new Ratio(Convert.ToDouble(txt.Replace("%", string.Empty).Replace(" ", string.Empty), noComma), RatioUnit.Percent));
            }
            else if (UnitsNet.Length.TryParse(txt, noComma, out UnitsNet.Length parsed))
              lengths.Add(parsed);
            else if (UnitsNet.Length.TryParseFeetInches(txt, out UnitsNet.Length parsedFeetInches, noComma))
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
    internal static List<Length> Lengths(GH_Component owner, IGH_DataAccess DA, int inputid, LengthUnit docLengthUnit, bool isOptional = false)
    {
      List<Length> lengths = new List<Length>();
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
            if (!unitNumber.Value.QuantityInfo.UnitType.Equals(typeof(LengthUnit)))
            {
              owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Error in " + owner.Params.Input[inputid].NickName + " (item " + i + ") input: Wrong unit type"
                  + Environment.NewLine + "Unit type is " + unitNumber.Value.QuantityInfo.Name + " but must be Length");
            }
            else
            {
              lengths.Add((Length)unitNumber.Value);
            }
          }
          // try cast to double
          else if (GH_Convert.ToDouble(gh_typs[i].Value, out double val, GH_Conversion.Both))
          {
            // create new quantity from default units
            lengths.Add(new Length(val, docLengthUnit));
          }
          // try cast to text
          else if (GH_Convert.ToString(gh_typs[i].Value, out string txt, GH_Conversion.Both))
          {
            if (UnitsNet.Length.TryParse(txt, out UnitsNet.Length parsed))
              lengths.Add(parsed);
            else if (UnitsNet.Length.TryParseFeetInches(txt, out UnitsNet.Length parsedFeetInches))
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
    internal static Pressure Stress(GH_Component owner, IGH_DataAccess DA, int inputid, PressureUnit stressUnit, bool isOptional = false)
    {
      Pressure stressFib = new Pressure();

      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(inputid, ref gh_typ))
      {
        GH_UnitNumber inStress;

        // try cast directly to quantity type
        if (gh_typ.Value is GH_UnitNumber)
        {
          inStress = (GH_UnitNumber)gh_typ.Value;
          if (!inStress.Value.QuantityInfo.UnitType.Equals(typeof(PressureUnit)))
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error in " + owner.Params.Input[inputid].NickName + " input: Wrong unit type"
                + Environment.NewLine + "Unit type is " + inStress.Value.QuantityInfo.Name + " but must be Stress (Pressure)");
            return Pressure.Zero;
          }
          stressFib = (Pressure)inStress.Value.ToUnit(stressUnit);
        }
        // try cast to double
        else if (GH_Convert.ToDouble(gh_typ.Value, out double val, GH_Conversion.Both))
        {
          // create new quantity from default units
          inStress = new GH_UnitNumber(new Pressure(val, stressUnit));
          stressFib = (Pressure)inStress.Value;
        }
        // try cast to string
        else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        {
          if (UnitsNet.Pressure.TryParse(txt, out Pressure quantity))
            return quantity;
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
            return Pressure.Zero;
          }
        }
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber of Stress");
          return Pressure.Zero;
        }
        return stressFib;
      }
      else if (!isOptional)
      {
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      }
      return Pressure.Zero;
    }

    internal static Ratio Ratio(GH_Component owner, IGH_DataAccess DA, int inputid, RatioUnit ratioUnit, bool isOptional = false)
    {
      Ratio ratio = new Ratio();

      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(inputid, ref gh_typ))
      {
        GH_UnitNumber inRatio;

        // try cast directly to quantity type
        if (gh_typ.Value is GH_UnitNumber)
        {
          inRatio = (GH_UnitNumber)gh_typ.Value;
          if (!inRatio.Value.QuantityInfo.UnitType.Equals(typeof(RatioUnit)))
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error in " + owner.Params.Input[inputid].NickName + " input: Wrong unit type"
                + Environment.NewLine + "Unit type is " + inRatio.Value.QuantityInfo.Name + " but must be Ratio");
            inRatio = new GH_UnitNumber(new Ratio(0, ratioUnit));
            ratio = (Ratio)inRatio.Value;
          }
          ratio = (Ratio)inRatio.Value.ToUnit(ratioUnit);
        }
        // try cast to double
        else if (GH_Convert.ToDouble(gh_typ.Value, out double val, GH_Conversion.Both))
        {
          // create new quantity from default units
          inRatio = new GH_UnitNumber(new Ratio(val, ratioUnit));
          ratio = (Ratio)inRatio.Value;
        }
        // try cast to string
        else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        {
          if (txt.EndsWith("%"))
          {
            NumberFormatInfo noComma = CultureInfo.InvariantCulture.NumberFormat;
            ratio = new Ratio(Convert.ToDouble(txt.Replace("%", string.Empty).Replace(" ", string.Empty), noComma), RatioUnit.Percent);
          }
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber of Ratio");
            inRatio = new GH_UnitNumber(new Ratio(0, ratioUnit));
            ratio = (Ratio)inRatio.Value;
          }
        }
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber of Ratio");
          inRatio = new GH_UnitNumber(new Ratio(0, ratioUnit));
          ratio = (Ratio)inRatio.Value;
        }
        return ratio;
      }
      else if (!isOptional)
      {
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      }
      ratio = new Ratio(0, ratioUnit);
      return ratio;
    }

    internal static Strain Strain(GH_Component owner, IGH_DataAccess DA, int inputid, StrainUnit strainUnit, bool isOptional = false)
    {
      Strain strainFib = new Strain();

      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(inputid, ref gh_typ))
      {
        GH_UnitNumber inStrain;

        // try cast directly to quantity type
        if (gh_typ.Value is GH_UnitNumber)
        {
          inStrain = (GH_UnitNumber)gh_typ.Value;
          if (!inStrain.Value.QuantityInfo.UnitType.Equals(typeof(StrainUnit)))
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error in " + owner.Params.Input[inputid].NickName + " input: Wrong unit type"
                + Environment.NewLine + "Unit type is " + inStrain.Value.QuantityInfo.Name + " but must be Strain");
            return Oasys.Units.Strain.Zero;
          }
          strainFib = (Strain)inStrain.Value.ToUnit(strainUnit);
        }
        // try cast to double
        else if (GH_Convert.ToDouble(gh_typ.Value, out double val, GH_Conversion.Both))
        {
          // create new quantity from default units
          inStrain = new GH_UnitNumber(new Strain(val, strainUnit));
          strainFib = (Strain)inStrain.Value;
        }
        // try cast to string
        // awaiting bug fix in Oasys Units to be able to parse unit from string
        //else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        //{
        //  if (Oasys.Units.Strain.TryParse(txt, out Strain quantity))
        //    return quantity;
        //  else
        //  {
        //    owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
        //    return Oasys.Units.Strain.Zero;
        //  }
        //}
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber of Strain");
          return Oasys.Units.Strain.Zero;
        }
        return strainFib;
      }
      else if (!isOptional)
      {
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      }
      return Oasys.Units.Strain.Zero;
    }

    internal static Curvature Curvature(GH_Component owner, IGH_DataAccess DA, int inputid, CurvatureUnit curvatureUnit, bool isOptional = false)
    {
      Curvature crvature = new Curvature();

      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(inputid, ref gh_typ))
      {
        GH_UnitNumber inStrain;

        // try cast directly to quantity type
        if (gh_typ.Value is GH_UnitNumber)
        {
          inStrain = (GH_UnitNumber)gh_typ.Value;
          if (!inStrain.Value.QuantityInfo.UnitType.Equals(typeof(CurvatureUnit)))
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error in " + owner.Params.Input[inputid].NickName + " input: Wrong unit type"
                + Environment.NewLine + "Unit type is " + inStrain.Value.QuantityInfo.Name + " but must be Curvature");
            return Oasys.Units.Curvature.Zero;
          }
          crvature = (Curvature)inStrain.Value.ToUnit(curvatureUnit);
        }
        // try cast to double
        else if (GH_Convert.ToDouble(gh_typ.Value, out double val, GH_Conversion.Both))
        {
          // create new quantity from default units
          inStrain = new GH_UnitNumber(new Curvature(val, curvatureUnit));
          crvature = (Curvature)inStrain.Value;
        }
        // try cast to string
        //else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        //{
        //  if (Oasys.Units.Curvature.TryParse(txt, out Curvature quantity))
        //    return quantity;
        //  else
        //  {
        //    owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
        //    return Oasys.Units.Curvature.Zero;
        //  }
        //}
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber of Curvature");
          return Oasys.Units.Curvature.Zero;
        }
        return crvature;
      }
      else if (!isOptional)
      {
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      }
      return Oasys.Units.Curvature.Zero;
    }

    internal static Force Force(GH_Component owner, IGH_DataAccess DA, int inputid, ForceUnit forceUnit, bool isOptional = false)
    {
      Force force = new Force();

      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(inputid, ref gh_typ))
      {
        GH_UnitNumber inForce;

        // try cast directly to quantity type
        if (gh_typ.Value is GH_UnitNumber)
        {
          inForce = (GH_UnitNumber)gh_typ.Value;
          if (!inForce.Value.QuantityInfo.UnitType.Equals(typeof(ForceUnit)))
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error in " + owner.Params.Input[inputid].NickName + " input: Wrong unit type"
                + Environment.NewLine + "Unit type is " + inForce.Value.QuantityInfo.Name + " but must be Force");
            return UnitsNet.Force.Zero;
          }
          force = (Force)inForce.Value.ToUnit(forceUnit);
        }
        // try cast to double
        else if (GH_Convert.ToDouble(gh_typ.Value, out double val, GH_Conversion.Both))
        {
          // create new quantity from default units
          inForce = new GH_UnitNumber(new Force(val, forceUnit));
          force = (Force)inForce.Value;
        }
        // try cast from string
        else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        {
          if (UnitsNet.Force.TryParse(txt, out Force quantity))
            return quantity;
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
            return UnitsNet.Force.Zero;
          }
        }
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber of Force");
          return UnitsNet.Force.Zero;
        }
        return force;
      }
      else if (!isOptional)
      {
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      }
      return UnitsNet.Force.Zero;
    }

    internal static Moment Moment(GH_Component owner, IGH_DataAccess DA, int inputid, MomentUnit momentUnit, bool isOptional = false)
    {
      Moment moment = new Moment();

      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(inputid, ref gh_typ))
      {
        GH_UnitNumber inMoment;

        // try cast directly to quantity type
        if (gh_typ.Value is GH_UnitNumber)
        {
          inMoment = (GH_UnitNumber)gh_typ.Value;
          if (!inMoment.Value.QuantityInfo.UnitType.Equals(typeof(MomentUnit)))
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error in " + owner.Params.Input[inputid].NickName + " input: Wrong unit type"
                + Environment.NewLine + "Unit type is " + inMoment.Value.QuantityInfo.Name + " but must be Moment");
            return Oasys.Units.Moment.Zero;
          }
          moment = (Moment)inMoment.Value.ToUnit(momentUnit);
        }
        // try cast to double
        else if (GH_Convert.ToDouble(gh_typ.Value, out double val, GH_Conversion.Both))
        {
          // create new quantity from default units
          inMoment = new GH_UnitNumber(new Moment(val, momentUnit));
          moment = (Moment)inMoment.Value;
        }
        // try cast from string
        //else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        //{
        //  if (Oasys.Units.Moment.TryParse(txt, out Moment quantity))
        //    return quantity;
        //  else
        //  {
        //    owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
        //    return Oasys.Units.Moment.Zero;
        //  }
        //}
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber of Moment");
          return Oasys.Units.Moment.Zero;
        }
        return moment;
      }
      else if (!isOptional)
      {
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      }
      return Oasys.Units.Moment.Zero;
    }

    internal static ForcePerLength ForcePerLength(GH_Component owner, IGH_DataAccess DA, int inputid, ForcePerLengthUnit forceUnit, bool isOptional = false)
    {
      ForcePerLength force = new ForcePerLength();

      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(inputid, ref gh_typ))
      {
        GH_UnitNumber inForce;

        // try cast directly to quantity type
        if (gh_typ.Value is GH_UnitNumber)
        {
          inForce = (GH_UnitNumber)gh_typ.Value;
          if (!inForce.Value.QuantityInfo.UnitType.Equals(typeof(ForceUnit)))
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error in " + owner.Params.Input[inputid].NickName + " input: Wrong unit type"
                + Environment.NewLine + "Unit type is " + inForce.Value.QuantityInfo.Name + " but must be ForcePerLength");
            return UnitsNet.ForcePerLength.Zero;
          }
          force = (ForcePerLength)inForce.Value.ToUnit(forceUnit);
        }
        // try cast to double
        else if (GH_Convert.ToDouble(gh_typ.Value, out double val, GH_Conversion.Both))
        {
          // create new quantity from default units
          inForce = new GH_UnitNumber(new ForcePerLength(val, forceUnit));
          force = (ForcePerLength)inForce.Value;
        }
        // try cast from string
        else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        {
          if (UnitsNet.ForcePerLength.TryParse(txt, out ForcePerLength quantity))
            return quantity;
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
            return UnitsNet.ForcePerLength.Zero;
          }
        }
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber of ForcePerLength");
          return UnitsNet.ForcePerLength.Zero;
        }
        return force;
      }
      else if (!isOptional)
      {
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      }
      return UnitsNet.ForcePerLength.Zero;
    }

    internal static Angle Angle(GH_Component owner, IGH_DataAccess DA, int inputid, AngleUnit angleUnit, bool isOptional = false)
    {
      GH_UnitNumber a1 = new GH_UnitNumber(new Angle(0, angleUnit));
      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(inputid, ref gh_typ))
      {
        // try cast directly to quantity type
        if (gh_typ.Value is GH_UnitNumber)
        {
          a1 = (GH_UnitNumber)gh_typ.Value;
          // check that unit is of right type
          if (!a1.Value.QuantityInfo.UnitType.Equals(typeof(AngleUnit)))
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Error in " + owner.Params.Input[inputid].NickName + " input: Wrong unit type"
                + Environment.NewLine + "Unit type is " + a1.Value.QuantityInfo.Name + " but must be Angle");
            return UnitsNet.Angle.Zero;
          }
        }
        // try cast to double
        else if (GH_Convert.ToDouble(gh_typ.Value, out double val, GH_Conversion.Both))
        {
          // create new quantity from default units
          a1 = new GH_UnitNumber(new Angle(val, angleUnit));
        }// try cast from string
        else if (GH_Convert.ToString(gh_typ.Value, out string txt, GH_Conversion.Both))
        {
          if (UnitsNet.Angle.TryParse(txt, out Angle quantity))
            return quantity;
          else
          {
            owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to UnitNumber");
            return UnitsNet.Angle.Zero;
          }
        }
        else
        {
          owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + owner.Params.Input[inputid].NickName + " to Angle");
          return UnitsNet.Angle.Zero;
        }
        return (Angle)a1.Value;
      }
      else if (!isOptional)
      {
        owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter " + owner.Params.Input[inputid].NickName + " failed to collect data!");
      }
      return UnitsNet.Angle.Zero;
    }
    #endregion

    internal static object GenericGoo<Type>(GH_Component owner, IGH_DataAccess DA, int inputid)
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

    internal static List<Type> GenericGooList<Type>(GH_Component owner, IGH_DataAccess DA, int inputid)
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
