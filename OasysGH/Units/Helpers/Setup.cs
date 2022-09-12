using Rhino;
using System;
using static OasysGH.Units.DefaultUnits;

namespace OasysGH.Units.Helpers
{
  public class Setup
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
      Grasshopper.Instances.Settings.SetValue("OasysAngle", AngleUnit.ToString());

      Grasshopper.Instances.Settings.SetValue("OasysTolerance", Tolerance.As(LengthUnitGeometry));
      Grasshopper.Instances.Settings.SetValue("OasysUseRhinoTolerance", UseRhinoTolerance);
      
      Grasshopper.Instances.Settings.SetValue("OasysLengthUnitGeometry", LengthUnitGeometry.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysUseRhinoLengthGeometryUnit", UseRhinoLengthGeometryUnit);
      
      Grasshopper.Instances.Settings.SetValue("OasysLengthUnitSection", LengthUnitSection.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysLengthUnitResult", LengthUnitResult.ToString());
      
      Grasshopper.Instances.Settings.SetValue("OasysForceUnit", ForceUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysMomentUnit", MomentUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysStressUnit", StressUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysStrainUnit", StrainUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysAxialStiffnessUnit", AxialStiffnessUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysBendingStiffnessUnit", BendingStiffnessUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysCurvatureUnit", CurvatureUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysMassUnit", MassUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysTemperatureUnit", TemperatureUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysVelocityUnit", VelocityUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysAccelerationUnit", AccelerationUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysEnergyUnit", EnergyUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysRatioUnit", RatioUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysTimeShortUnit", TimeShortUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysTimeMediumUnit", TimeMediumUnit.ToString());
      Grasshopper.Instances.Settings.SetValue("OasysTimeLongUnit", TimeLongUnit.ToString());

      Grasshopper.Instances.Settings.WritePersistentSettings();
    }
    internal static bool ReadSettings()
    {
      if (!Grasshopper.Instances.Settings.ConstainsEntry("OasysLengthUnitGeometry"))
        return false;

      AngleUnit = (UnitsNet.Units.AngleUnit)Enum.Parse(typeof(UnitsNet.Units.AngleUnit), 
        Grasshopper.Instances.Settings.GetValue("OasysAngle", string.Empty));

      LengthUnitGeometry = (UnitsNet.Units.LengthUnit)Enum.Parse(typeof(UnitsNet.Units.LengthUnit),
      Grasshopper.Instances.Settings.GetValue("OasysLengthUnitGeometry", string.Empty));
      UseRhinoLengthGeometryUnit = Grasshopper.Instances.Settings.GetValue("OasysUseRhinoLengthGeometryUnit", false);

      Tolerance = new UnitsNet.Length(Grasshopper.Instances.Settings.GetValue("OasysTolerance", double.NaN), LengthUnitGeometry);
      UseRhinoTolerance = Grasshopper.Instances.Settings.GetValue("OasysUseRhinoTolerance", false);

      LengthUnitSection = (UnitsNet.Units.LengthUnit)Enum.Parse(typeof(UnitsNet.Units.LengthUnit),
      Grasshopper.Instances.Settings.GetValue("OasysLengthUnitSection", string.Empty));
      LengthUnitResult = (UnitsNet.Units.LengthUnit)Enum.Parse(typeof(UnitsNet.Units.LengthUnit),
      Grasshopper.Instances.Settings.GetValue("OasysLengthUnitResult", string.Empty));

      ForceUnit = (UnitsNet.Units.ForceUnit)Enum.Parse(typeof(UnitsNet.Units.ForceUnit),
      Grasshopper.Instances.Settings.GetValue("OasysForceUnit", string.Empty));
      MomentUnit = (Oasys.Units.MomentUnit)Enum.Parse(typeof(Oasys.Units.MomentUnit),
      Grasshopper.Instances.Settings.GetValue("OasysMomentUnit", string.Empty));
      StressUnit = (UnitsNet.Units.PressureUnit)Enum.Parse(typeof(UnitsNet.Units.PressureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysStressUnit", string.Empty));
      StrainUnit = (Oasys.Units.StrainUnit)Enum.Parse(typeof(Oasys.Units.StrainUnit),
      Grasshopper.Instances.Settings.GetValue("OasysStrainUnit", string.Empty));
      AxialStiffnessUnit = (Oasys.Units.AxialStiffnessUnit)Enum.Parse(typeof(Oasys.Units.AxialStiffnessUnit),
      Grasshopper.Instances.Settings.GetValue("OasysAxialStiffnessUnit", string.Empty));
      BendingStiffnessUnit = (Oasys.Units.BendingStiffnessUnit)Enum.Parse(typeof(Oasys.Units.BendingStiffnessUnit),
      Grasshopper.Instances.Settings.GetValue("OasysBendingStiffnessUnit", string.Empty));
      CurvatureUnit = (Oasys.Units.CurvatureUnit)Enum.Parse(typeof(Oasys.Units.CurvatureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysCurvatureUnit", string.Empty));
      MassUnit = (UnitsNet.Units.MassUnit)Enum.Parse(typeof(UnitsNet.Units.MassUnit),
      Grasshopper.Instances.Settings.GetValue("OasysMassUnit", string.Empty));
      TemperatureUnit = (UnitsNet.Units.TemperatureUnit)Enum.Parse(typeof(UnitsNet.Units.TemperatureUnit),
      Grasshopper.Instances.Settings.GetValue("OasysTemperatureUnit", string.Empty));
      VelocityUnit = (UnitsNet.Units.SpeedUnit)Enum.Parse(typeof(UnitsNet.Units.SpeedUnit),
      Grasshopper.Instances.Settings.GetValue("OasysVelocityUnit", string.Empty));
      AccelerationUnit = (UnitsNet.Units.AccelerationUnit)Enum.Parse(typeof(UnitsNet.Units.AccelerationUnit),
      Grasshopper.Instances.Settings.GetValue("OasysAccelerationUnit", string.Empty));
      EnergyUnit = (UnitsNet.Units.EnergyUnit)Enum.Parse(typeof(UnitsNet.Units.EnergyUnit),
      Grasshopper.Instances.Settings.GetValue("OasysEnergyUnit", string.Empty));
      RatioUnit = (UnitsNet.Units.RatioUnit)Enum.Parse(typeof(UnitsNet.Units.RatioUnit),
      Grasshopper.Instances.Settings.GetValue("OasysRatioUnit", string.Empty));
      TimeShortUnit = (UnitsNet.Units.DurationUnit)Enum.Parse(typeof(UnitsNet.Units.DurationUnit),
      Grasshopper.Instances.Settings.GetValue("OasysTimeShortUnit", string.Empty));
      TimeMediumUnit = (UnitsNet.Units.DurationUnit)Enum.Parse(typeof(UnitsNet.Units.DurationUnit),
      Grasshopper.Instances.Settings.GetValue("OasysTimeMediumUnit", string.Empty));
      TimeLongUnit = (UnitsNet.Units.DurationUnit)Enum.Parse(typeof(UnitsNet.Units.DurationUnit),
      Grasshopper.Instances.Settings.GetValue("OasysTimeLongUnit", string.Empty));
      
      return true;
    }
  }
}
