using System;
using OasysGH.Units.Helpers;
using OasysUnits;
using OasysUnits.Units;
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
          LengthUnitGeometry = LengthUnit.Meter;
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

      DefaultUnits.AngleUnit = (AngleUnit)Enum.Parse(typeof(AngleUnit),
        Grasshopper.Instances.Settings.GetValue("OasysAngle", string.Empty));

      DefaultUnits.LengthUnitGeometry = (LengthUnit)Enum.Parse(typeof(LengthUnit),
      Grasshopper.Instances.Settings.GetValue("OasysLengthUnitGeometry", string.Empty));
      DefaultUnits.UseRhinoLengthGeometryUnit = Grasshopper.Instances.Settings.GetValue("OasysUseRhinoLengthGeometryUnit", false);

      DefaultUnits.Tolerance = new Length(Grasshopper.Instances.Settings.GetValue("OasysTolerance", double.NaN), LengthUnitGeometry);
      DefaultUnits.UseRhinoTolerance = Grasshopper.Instances.Settings.GetValue("OasysUseRhinoTolerance", false);

      DefaultUnits.LengthUnitSection = (LengthUnit)Enum.Parse(typeof(LengthUnit),
      Grasshopper.Instances.Settings.GetValue("OasysLengthUnitSection", string.Empty));

      DefaultUnits.LengthUnitResult = (LengthUnit)Enum.Parse(typeof(LengthUnit),
      Grasshopper.Instances.Settings.GetValue("OasysLengthUnitResult", string.Empty));

      DefaultUnits.ForceUnit = (ForceUnit)Enum.Parse(typeof(ForceUnit),
      Grasshopper.Instances.Settings.GetValue("OasysForceUnit", string.Empty));

      DefaultUnits.MomentUnit = (MomentUnit)Enum.Parse(typeof(MomentUnit),
      Grasshopper.Instances.Settings.GetValue("OasysMomentUnit", string.Empty));

      DefaultUnits.StressUnitResult = (PressureUnit)Enum.Parse(typeof(PressureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysStressUnit", string.Empty));

      DefaultUnits.MaterialStrengthUnit = (PressureUnit)Enum.Parse(typeof(PressureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysMaterialStrengthUnit", string.Empty));

      DefaultUnits.YoungsModulusUnit = (PressureUnit)Enum.Parse(typeof(PressureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysYoungsModulusUnit", string.Empty));

      DefaultUnits.MaterialStrainUnit = (StrainUnit)Enum.Parse(typeof(StrainUnit),
      Grasshopper.Instances.Settings.GetValue("MaterialStrainUnit", string.Empty));

      DefaultUnits.StrainUnitResult = (StrainUnit)Enum.Parse(typeof(StrainUnit),
      Grasshopper.Instances.Settings.GetValue("StrainUnitResult", string.Empty));

      DefaultUnits.AxialStiffnessUnit = (AxialStiffnessUnit)Enum.Parse(typeof(AxialStiffnessUnit),
      Grasshopper.Instances.Settings.GetValue("OasysAxialStiffnessUnit", string.Empty));

      DefaultUnits.BendingStiffnessUnit = (BendingStiffnessUnit)Enum.Parse(typeof(BendingStiffnessUnit),
      Grasshopper.Instances.Settings.GetValue("OasysBendingStiffnessUnit", string.Empty));

      DefaultUnits.CurvatureUnit = (CurvatureUnit)Enum.Parse(typeof(CurvatureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysCurvatureUnit", string.Empty));

      DefaultUnits.MassUnit = (MassUnit)Enum.Parse(typeof(MassUnit),
      Grasshopper.Instances.Settings.GetValue("OasysMassUnit", string.Empty));

      DefaultUnits.TemperatureUnit = (TemperatureUnit)Enum.Parse(typeof(TemperatureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysTemperatureUnit", string.Empty));

      DefaultUnits.VelocityUnit = (SpeedUnit)Enum.Parse(typeof(SpeedUnit),
      Grasshopper.Instances.Settings.GetValue("OasysVelocityUnit", string.Empty));

      DefaultUnits.AccelerationUnit = (AccelerationUnit)Enum.Parse(typeof(AccelerationUnit),
      Grasshopper.Instances.Settings.GetValue("OasysAccelerationUnit", string.Empty));

      DefaultUnits.EnergyUnit = (EnergyUnit)Enum.Parse(typeof(EnergyUnit),
      Grasshopper.Instances.Settings.GetValue("OasysEnergyUnit", string.Empty));

      DefaultUnits.RatioUnit = (RatioUnit)Enum.Parse(typeof(RatioUnit),
      Grasshopper.Instances.Settings.GetValue("OasysRatioUnit", string.Empty));

      DefaultUnits.TimeShortUnit = (DurationUnit)Enum.Parse(typeof(DurationUnit),
      Grasshopper.Instances.Settings.GetValue("OasysTimeShortUnit", string.Empty));

      DefaultUnits.TimeMediumUnit = (DurationUnit)Enum.Parse(typeof(DurationUnit),
      Grasshopper.Instances.Settings.GetValue("OasysTimeMediumUnit", string.Empty));

      DefaultUnits.TimeLongUnit = (DurationUnit)Enum.Parse(typeof(DurationUnit),
      Grasshopper.Instances.Settings.GetValue("OasysTimeLongUnit", string.Empty));

      DefaultUnits.ForcePerLengthUnit = (ForcePerLengthUnit)Enum.Parse(typeof(ForcePerLengthUnit),
      Grasshopper.Instances.Settings.GetValue("OasysForcePerLengthUnit", string.Empty));

      DefaultUnits.ForcePerAreaUnit = (PressureUnit)Enum.Parse(typeof(PressureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysForcePerAreaUnit", string.Empty));

      DefaultUnits.SectionAreaUnit = (AreaUnit)Enum.Parse(typeof(AreaUnit),
      Grasshopper.Instances.Settings.GetValue("OasysSectionAreaUnit", string.Empty));

      DefaultUnits.SectionVolumeUnit = (VolumeUnit)Enum.Parse(typeof(VolumeUnit),
      Grasshopper.Instances.Settings.GetValue("OasysSectionVolumeUnit", string.Empty));

      DefaultUnits.SectionAreaMomentOfInertiaUnit = (AreaMomentOfInertiaUnit)Enum.Parse(typeof(AreaMomentOfInertiaUnit),
      Grasshopper.Instances.Settings.GetValue("OasysSectionAreaMomentOfInertiaUnit", string.Empty));

      DefaultUnits.DensityUnit = (DensityUnit)Enum.Parse(typeof(DensityUnit),
      Grasshopper.Instances.Settings.GetValue("OasysDensityUnit", string.Empty));

      DefaultUnits.LinearDensityUnit = (LinearDensityUnit)Enum.Parse(typeof(LinearDensityUnit),
      Grasshopper.Instances.Settings.GetValue("OasysLinearDensityUnit", string.Empty));
      return true;
    }
  }
}
