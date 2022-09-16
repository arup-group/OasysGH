using System;
using Rhino;
using static OasysGH.Units.DefaultUnits;

namespace OasysGH.Units
{
  public static class Utility
  {
    public static void SetupUnitsDuringLoad(bool headless = false)
    {
      bool settingsExist = ReadSettings();
      if (!settingsExist)
      {
        // get rhino document length unit
        if (headless)
          LengthUnitGeometry = UnitsNet.Units.LengthUnit.Meter;
        else
          LengthUnitGeometry = RhinoUnit.GetRhinoLengthUnit(RhinoDoc.ActiveDoc.ModelUnitSystem);
        SaveSettings();
      }
    }

    internal static void SaveSettings()
    {
      Grasshopper.Instances.Settings.SetValue("OasysAngle", DefaultUnits.AngleUnit.ToString());

      Grasshopper.Instances.Settings.SetValue("OasysTolerance", DefaultUnits.Tolerance.As(LengthUnitGeometry));
      Grasshopper.Instances.Settings.SetValue("OasysUseRhinoTolerance", DefaultUnits.UseRhinoTolerance);
      
      Grasshopper.Instances.Settings.SetValue("OasysLengthUnitGeometry", DefaultUnits.LengthUnitGeometry.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysUseRhinoLengthGeometryUnit", DefaultUnits.UseRhinoLengthGeometryUnit);
      
      Grasshopper.Instances.Settings.SetValue("OasysLengthUnitSection", DefaultUnits.LengthUnitSection.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysSectionAreaUnit", DefaultUnits.SectionAreaUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysSectionVolumeUnit", DefaultUnits.SectionVolumeUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysSectionAreaMomentOfInertiaUnit", DefaultUnits.SectionAreaMomentOfInertiaUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysDensityUnit", DefaultUnits.DensityUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysLinearDensityUnit", DefaultUnits.LinearDensityUnit.ToString());

      Grasshopper.Instances.Settings.SetValue("OasysLengthUnitResult", DefaultUnits.LengthUnitResult.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysForceUnit", DefaultUnits.ForceUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysForcePerLengthUnit", DefaultUnits.ForcePerLengthUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysForcePerAreaUnit", DefaultUnits.ForcePerAreaUnit.ToString());

      Grasshopper.Instances.Settings.SetValue("OasysMomentUnit", DefaultUnits.MomentUnit.ToString());

      Grasshopper.Instances.Settings.SetValue("OasysStressUnit", DefaultUnits.StressUnitResult.ToString());

      Grasshopper.Instances.Settings.SetValue("OasysMaterialStrengthUnit", DefaultUnits.MaterialStrengthUnit.ToString());

      Grasshopper.Instances.Settings.SetValue("OasysYoungsModulusUnit", DefaultUnits.YoungsModulusUnit.ToString());

      Grasshopper.Instances.Settings.SetValue("MaterialStrainUnit", DefaultUnits.MaterialStrainUnit.ToString());
      
      Grasshopper.Instances.Settings.SetValue("StrainUnitResult", DefaultUnits.StrainUnitResult.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysAxialStiffnessUnit", DefaultUnits.AxialStiffnessUnit.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysBendingStiffnessUnit", DefaultUnits.BendingStiffnessUnit.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysCurvatureUnit", DefaultUnits.CurvatureUnit.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysMassUnit", DefaultUnits.MassUnit.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysTemperatureUnit", DefaultUnits.TemperatureUnit.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysVelocityUnit", DefaultUnits.VelocityUnit.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysAccelerationUnit", DefaultUnits.AccelerationUnit.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysEnergyUnit", DefaultUnits.EnergyUnit.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysRatioUnit", DefaultUnits.RatioUnit.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysTimeShortUnit", DefaultUnits.TimeShortUnit.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysTimeMediumUnit", DefaultUnits.TimeMediumUnit.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysTimeLongUnit", DefaultUnits.TimeLongUnit.ToString());

      Grasshopper.Instances.Settings.WritePersistentSettings();
    }
    internal static bool ReadSettings()
    {
      if (!Grasshopper.Instances.Settings.ConstainsEntry("OasysLengthUnitGeometry"))
        return false;

      DefaultUnits.AngleUnit = (UnitsNet.Units.AngleUnit)Enum.Parse(typeof(UnitsNet.Units.AngleUnit), 
        Grasshopper.Instances.Settings.GetValue("OasysAngle", string.Empty));

      DefaultUnits.LengthUnitGeometry = (UnitsNet.Units.LengthUnit)Enum.Parse(typeof(UnitsNet.Units.LengthUnit),
      Grasshopper.Instances.Settings.GetValue("OasysLengthUnitGeometry", string.Empty));
      DefaultUnits.UseRhinoLengthGeometryUnit = Grasshopper.Instances.Settings.GetValue("OasysUseRhinoLengthGeometryUnit", false);

      DefaultUnits.Tolerance = new UnitsNet.Length(Grasshopper.Instances.Settings.GetValue("OasysTolerance", double.NaN), LengthUnitGeometry);
      UseRhinoTolerance = Grasshopper.Instances.Settings.GetValue("OasysUseRhinoTolerance", false);

      DefaultUnits.LengthUnitSection = (UnitsNet.Units.LengthUnit)Enum.Parse(typeof(UnitsNet.Units.LengthUnit),
      Grasshopper.Instances.Settings.GetValue("OasysLengthUnitSection", string.Empty));

      DefaultUnits.LengthUnitResult = (UnitsNet.Units.LengthUnit)Enum.Parse(typeof(UnitsNet.Units.LengthUnit),
      Grasshopper.Instances.Settings.GetValue("OasysLengthUnitResult", string.Empty));

      DefaultUnits.ForceUnit = (UnitsNet.Units.ForceUnit)Enum.Parse(typeof(UnitsNet.Units.ForceUnit),
      Grasshopper.Instances.Settings.GetValue("OasysForceUnit", string.Empty));

      DefaultUnits.MomentUnit = (Oasys.Units.MomentUnit)Enum.Parse(typeof(Oasys.Units.MomentUnit),
      Grasshopper.Instances.Settings.GetValue("OasysMomentUnit", string.Empty));

      DefaultUnits.StressUnitResult = (UnitsNet.Units.PressureUnit)Enum.Parse(typeof(UnitsNet.Units.PressureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysStressUnit", string.Empty));

      DefaultUnits.MaterialStrengthUnit = (UnitsNet.Units.PressureUnit)Enum.Parse(typeof(UnitsNet.Units.PressureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysMaterialStrengthUnit", string.Empty));

      DefaultUnits.YoungsModulusUnit = (UnitsNet.Units.PressureUnit)Enum.Parse(typeof(UnitsNet.Units.PressureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysYoungsModulusUnit", string.Empty));

      DefaultUnits.MaterialStrainUnit = (Oasys.Units.StrainUnit)Enum.Parse(typeof(Oasys.Units.StrainUnit),
      Grasshopper.Instances.Settings.GetValue("MaterialStrainUnit", string.Empty));

      DefaultUnits.StrainUnitResult = (Oasys.Units.StrainUnit)Enum.Parse(typeof(Oasys.Units.StrainUnit),
      Grasshopper.Instances.Settings.GetValue("StrainUnitResult", string.Empty));

      DefaultUnits.AxialStiffnessUnit = (Oasys.Units.AxialStiffnessUnit)Enum.Parse(typeof(Oasys.Units.AxialStiffnessUnit),
      Grasshopper.Instances.Settings.GetValue("OasysAxialStiffnessUnit", string.Empty));

      DefaultUnits.BendingStiffnessUnit = (Oasys.Units.BendingStiffnessUnit)Enum.Parse(typeof(Oasys.Units.BendingStiffnessUnit),
      Grasshopper.Instances.Settings.GetValue("OasysBendingStiffnessUnit", string.Empty));

      DefaultUnits.CurvatureUnit = (Oasys.Units.CurvatureUnit)Enum.Parse(typeof(Oasys.Units.CurvatureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysCurvatureUnit", string.Empty));

      DefaultUnits.MassUnit = (UnitsNet.Units.MassUnit)Enum.Parse(typeof(UnitsNet.Units.MassUnit),
      Grasshopper.Instances.Settings.GetValue("OasysMassUnit", string.Empty));

      DefaultUnits.TemperatureUnit = (UnitsNet.Units.TemperatureUnit)Enum.Parse(typeof(UnitsNet.Units.TemperatureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysTemperatureUnit", string.Empty));

      DefaultUnits.VelocityUnit = (UnitsNet.Units.SpeedUnit)Enum.Parse(typeof(UnitsNet.Units.SpeedUnit),
      Grasshopper.Instances.Settings.GetValue("OasysVelocityUnit", string.Empty));

      DefaultUnits.AccelerationUnit = (UnitsNet.Units.AccelerationUnit)Enum.Parse(typeof(UnitsNet.Units.AccelerationUnit),
      Grasshopper.Instances.Settings.GetValue("OasysAccelerationUnit", string.Empty));

      DefaultUnits.EnergyUnit = (UnitsNet.Units.EnergyUnit)Enum.Parse(typeof(UnitsNet.Units.EnergyUnit),
      Grasshopper.Instances.Settings.GetValue("OasysEnergyUnit", string.Empty));

      DefaultUnits.RatioUnit = (UnitsNet.Units.RatioUnit)Enum.Parse(typeof(UnitsNet.Units.RatioUnit),
      Grasshopper.Instances.Settings.GetValue("OasysRatioUnit", string.Empty));

      DefaultUnits.TimeShortUnit = (UnitsNet.Units.DurationUnit)Enum.Parse(typeof(UnitsNet.Units.DurationUnit),
      Grasshopper.Instances.Settings.GetValue("OasysTimeShortUnit", string.Empty));

      DefaultUnits.TimeMediumUnit = (UnitsNet.Units.DurationUnit)Enum.Parse(typeof(UnitsNet.Units.DurationUnit),
      Grasshopper.Instances.Settings.GetValue("OasysTimeMediumUnit", string.Empty));

      DefaultUnits.TimeLongUnit = (UnitsNet.Units.DurationUnit)Enum.Parse(typeof(UnitsNet.Units.DurationUnit),
      Grasshopper.Instances.Settings.GetValue("OasysTimeLongUnit", string.Empty));

      DefaultUnits.ForcePerLengthUnit = (UnitsNet.Units.ForcePerLengthUnit)Enum.Parse(typeof(UnitsNet.Units.ForcePerLengthUnit),
      Grasshopper.Instances.Settings.GetValue("OasysForcePerLengthUnit", string.Empty));

      DefaultUnits.ForcePerAreaUnit = (UnitsNet.Units.PressureUnit)Enum.Parse(typeof(UnitsNet.Units.PressureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysForcePerAreaUnit", string.Empty));

      DefaultUnits.SectionAreaUnit = (UnitsNet.Units.AreaUnit)Enum.Parse(typeof(UnitsNet.Units.AreaUnit),
      Grasshopper.Instances.Settings.GetValue("OasysSectionAreaUnit", string.Empty));

      DefaultUnits.SectionVolumeUnit = (UnitsNet.Units.VolumeUnit)Enum.Parse(typeof(UnitsNet.Units.VolumeUnit),
      Grasshopper.Instances.Settings.GetValue("OasysSectionVolumeUnit", string.Empty));

      DefaultUnits.SectionAreaMomentOfInertiaUnit = (UnitsNet.Units.AreaMomentOfInertiaUnit)Enum.Parse(typeof(UnitsNet.Units.AreaMomentOfInertiaUnit),
      Grasshopper.Instances.Settings.GetValue("OasysSectionAreaMomentOfInertiaUnit", string.Empty));

      DefaultUnits.DensityUnit = (UnitsNet.Units.DensityUnit)Enum.Parse(typeof(UnitsNet.Units.DensityUnit),
      Grasshopper.Instances.Settings.GetValue("OasysDensityUnit", string.Empty));

      DefaultUnits.LinearDensityUnit = (UnitsNet.Units.LinearDensityUnit)Enum.Parse(typeof(UnitsNet.Units.LinearDensityUnit),
      Grasshopper.Instances.Settings.GetValue("OasysLinearDensityUnit", string.Empty));
      return true;
    }
  }
}
