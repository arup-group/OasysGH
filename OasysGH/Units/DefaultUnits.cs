using System;
using Rhino;
using UnitsNet;
using UnitsNet.Units;
using Oasys.Units;
using OasysGH.Units.Helpers;

namespace OasysGH.Units
{
  public class DefaultUnits
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
    internal static bool UseRhinoLengthGeometryUnit { get; set; } = true;

    public static LengthUnit LengthUnitSection { get; set; } = LengthUnit.Centimeter;
    public static LengthUnit LengthUnitResult = LengthUnit.Millimeter;

    public static AreaUnit SectionAreaUnit = UnitsHelper.GetAreaUnit(LengthUnitSection);
    public static VolumeUnit VolumeUnit = UnitsHelper.GetVolumeUnit(LengthUnitSection);

    public static AreaMomentOfInertiaUnit SectionAreaMomentOfInertiaUnit = UnitsHelper.GetAreaMomentOfInertiaUnit(LengthUnitSection);

    public static VolumePerLengthUnit VolumePerLengthUnit = UnitsHelper.GetVolumePerLengthUnit(LengthUnitSection);

    public static ForceUnit ForceUnit { get; set; } = ForceUnit.Kilonewton;

    public static ForcePerLengthUnit ForcePerLengthUnit = UnitsHelper.GetForcePerLengthUnit(ForceUnit, LengthUnitGeometry);
    public static PressureUnit ForcePerAreaUnit = UnitsHelper.GetForcePerAreaUnit(ForceUnit, LengthUnitGeometry);

    public static MomentUnit MomentUnit { get; set; } = MomentUnit.KilonewtonMeter;

    public static PressureUnit StressUnit { get; set; } = PressureUnit.Megapascal;

    public static StrainUnit StrainUnit { get; set; } = StrainUnit.MilliStrain;

    public static AxialStiffnessUnit AxialStiffnessUnit { get; set; } = AxialStiffnessUnit.Kilonewton;

    public static BendingStiffnessUnit BendingStiffnessUnit { get; set; } = BendingStiffnessUnit.KilonewtonSquareMeter;

    public static CurvatureUnit CurvatureUnit { get; set; } = (CurvatureUnit)Enum.Parse(typeof(CurvatureUnit), "Per" + LengthUnitGeometry.ToString());

    public static MassUnit MassUnit { get; set; } = MassUnit.Tonne;
    public static DensityUnit DensityUnit = UnitsHelper.GetDensityUnit(MassUnit, LengthUnitGeometry);

    public static LinearDensityUnit LinearDensityUnit = UnitsHelper.GetLinearDensityUnit(MassUnit, LengthUnitGeometry);

    public static TemperatureUnit TemperatureUnit { get; set; } = TemperatureUnit.DegreeCelsius;

    public static SpeedUnit VelocityUnit { get; set; } = SpeedUnit.MeterPerSecond;

    public static AccelerationUnit AccelerationUnit { get; set; } = AccelerationUnit.MeterPerSecondSquared;

    public static EnergyUnit EnergyUnit { get; set; } = EnergyUnit.Megajoule;

    public static RatioUnit RatioUnit { get; set; } = RatioUnit.DecimalFraction;

    public static DurationUnit TimeShortUnit { get; set; } = DurationUnit.Second;

    public static DurationUnit TimeMediumUnit { get; set; } = DurationUnit.Minute;

    public static DurationUnit TimeLongUnit { get; set; } = DurationUnit.Day;
  }
}
