using System;
using Rhino;
using OasysUnitsNet;
using OasysUnitsNet.Units;
using OasysGH.Units.Helpers;

namespace OasysGH.Units
{
  public static class DefaultUnits
  {
    public static AngleUnit AngleUnit { get; set; } = AngleUnit.Radian;
    public static Length Tolerance
    {
      get
      {
        if (UseRhinoTolerance)
          return RhinoUnit.GetRhinoTolerance();
        else
          return m_tolerance;
      }
      set
      {
        m_tolerance = value;
      }
    }
    private static Length m_tolerance = new Length(1, LengthUnit.Centimeter);
    public static bool UseRhinoTolerance { get; set; } = true;
    public static LengthUnit LengthUnitGeometry
    {
      get
      {
        if (UseRhinoLengthGeometryUnit)
          if (RhinoDoc.ActiveDoc != null)
            m_length_geometry = RhinoUnit.GetRhinoLengthUnit(RhinoDoc.ActiveDoc.ModelUnitSystem);
        return m_length_geometry;
      }
      set
      {
        UseRhinoLengthGeometryUnit = false;
        m_length_geometry = value;
      }
    }
    private static LengthUnit m_length_geometry = LengthUnit.Meter;
    public static bool UseRhinoLengthGeometryUnit { get; set; } = true;
    public static LengthUnit LengthUnitSection { get; set; } = LengthUnit.Centimeter;
    public static LengthUnit LengthUnitResult = LengthUnit.Millimeter;
    public static ForceUnit ForceUnit { get; set; } = ForceUnit.Kilonewton;
    public static MomentUnit MomentUnit { get; set; } = MomentUnit.KilonewtonMeter;
    public static PressureUnit StressUnitResult { get; set; } = PressureUnit.Megapascal;
    public static PressureUnit MaterialStrengthUnit { get; set; } = PressureUnit.Megapascal;
    public static StrainUnit StrainUnitResult { get; set; } = StrainUnit.Ratio;
    public static StrainUnit MaterialStrainUnit { get; set; } = StrainUnit.Ratio;
    public static AxialStiffnessUnit AxialStiffnessUnit { get; set; } = AxialStiffnessUnit.Kilonewton;
    public static ForcePerLengthUnit ForcePerLengthUnit { get; set; } = ForcePerLengthUnit.KilonewtonPerMeter;
    public static PressureUnit ForcePerAreaUnit { get; set; } = PressureUnit.KilonewtonPerSquareMeter;
    public static BendingStiffnessUnit BendingStiffnessUnit { get; set; } = BendingStiffnessUnit.KilonewtonSquareMeter;
    public static MassUnit MassUnit { get; set; } = MassUnit.Tonne;
    public static TemperatureUnit TemperatureUnit { get; set; } = TemperatureUnit.DegreeCelsius;
    public static SpeedUnit VelocityUnit { get; set; } = SpeedUnit.MeterPerSecond;
    public static AccelerationUnit AccelerationUnit { get; set; } = AccelerationUnit.MeterPerSecondSquared;
    public static EnergyUnit EnergyUnit { get; set; } = EnergyUnit.Megajoule;
    public static RatioUnit RatioUnit { get; set; } = RatioUnit.DecimalFraction;
    public static DurationUnit TimeShortUnit { get; set; } = DurationUnit.Second;
    public static DurationUnit TimeMediumUnit { get; set; } = DurationUnit.Minute;
    public static DurationUnit TimeLongUnit { get; set; } = DurationUnit.Day;
    public static AreaUnit SectionAreaUnit { get; set; } = AreaUnit.SquareCentimeter;
    public static VolumeUnit SectionVolumeUnit { get; set; } = VolumeUnit.CubicCentimeter;
    public static PressureUnit YoungsModulusUnit { get; set; } = PressureUnit.Gigapascal;
    public static AreaMomentOfInertiaUnit SectionAreaMomentOfInertiaUnit { get; set; } = AreaMomentOfInertiaUnit.CentimeterToTheFourth;
    public static VolumePerLengthUnit VolumePerLengthUnit { get; set; } = VolumePerLengthUnit.CubicMeterPerMeter;
    public static DensityUnit DensityUnit { get; set; } = DensityUnit.KilogramPerCubicMeter;
    public static LinearDensityUnit LinearDensityUnit { get; set; } = LinearDensityUnit.KilogramPerMeter;
    public static CurvatureUnit CurvatureUnit { get; set; } = CurvatureUnit.PerMeter;
  }
}
