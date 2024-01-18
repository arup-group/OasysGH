using OasysGH.Units.Helpers;
using OasysUnits;
using OasysUnits.Units;
using Rhino;

namespace OasysGH.Units {
  public static class DefaultUnits {
    public static AccelerationUnit AccelerationUnit { get; set; } = AccelerationUnit.MeterPerSecondSquared;
    public static AngleUnit AngleUnit { get; set; } = AngleUnit.Radian;
    public static AxialStiffnessUnit AxialStiffnessUnit { get; set; } = AxialStiffnessUnit.Kilonewton;
    public static BendingStiffnessUnit BendingStiffnessUnit { get; set; } = BendingStiffnessUnit.KilonewtonSquareMeter;
    public static CoefficientOfThermalExpansionUnit CoefficientOfThermalExpansionUnit => UnitsHelper.GetCoefficientOfThermalExpansionUnit(DefaultUnits.TemperatureUnit);
    public static CurvatureUnit CurvatureUnit { get; set; } = CurvatureUnit.PerMeter;
    public static DensityUnit DensityUnit { get; set; } = DensityUnit.KilogramPerCubicMeter;
    public static EnergyUnit EnergyUnit { get; set; } = EnergyUnit.Megajoule;
    public static PressureUnit ForcePerAreaUnit { get; set; } = PressureUnit.KilonewtonPerSquareMeter;
    public static ForcePerLengthUnit ForcePerLengthUnit { get; set; } = ForcePerLengthUnit.KilonewtonPerMeter;
    public static ForceUnit ForceUnit { get; set; } = ForceUnit.Kilonewton;
    public static RotationalStiffnessUnit RotationalStiffnessUnit { get; set; } = RotationalStiffnessUnit.NewtonMeterPerRadian;
    public static LengthUnit LengthUnitGeometry {
      get {
        if (UseRhinoLengthGeometryUnit)
          if (RhinoDoc.ActiveDoc != null)
            lengthGeometry = RhinoUnit.GetRhinoLengthUnit(RhinoDoc.ActiveDoc.ModelUnitSystem);
        return lengthGeometry;
      }
      set {
        UseRhinoLengthGeometryUnit = false;
        lengthGeometry = value;
      }
    }
    public static LengthUnit LengthUnitSection { get; set; } = LengthUnit.Centimeter;
    public static LinearDensityUnit LinearDensityUnit { get; set; } = LinearDensityUnit.KilogramPerMeter;
    public static MassUnit MassUnit { get; set; } = MassUnit.Tonne;
    public static StrainUnit MaterialStrainUnit { get; set; } = StrainUnit.Ratio;
    public static PressureUnit MaterialStrengthUnit { get; set; } = PressureUnit.Megapascal;
    public static MomentUnit MomentUnit { get; set; } = MomentUnit.KilonewtonMeter;
    public static RatioUnit RatioUnit { get; set; } = RatioUnit.DecimalFraction;
    public static AreaMomentOfInertiaUnit SectionAreaMomentOfInertiaUnit { get; set; } = AreaMomentOfInertiaUnit.CentimeterToTheFourth;
    public static AreaUnit SectionAreaUnit { get; set; } = AreaUnit.SquareCentimeter;
    public static SectionModulusUnit SectionModulusUnit { get; set; } = SectionModulusUnit.CubicCentimeter;
    public static VolumeUnit SectionVolumeUnit { get; set; } = VolumeUnit.CubicCentimeter;
    public static StrainUnit StrainUnitResult { get; set; } = StrainUnit.Ratio;
    public static PressureUnit StressUnitResult { get; set; } = PressureUnit.Megapascal;
    public static TemperatureUnit TemperatureUnit { get; set; } = TemperatureUnit.DegreeCelsius;
    public static DurationUnit TimeLongUnit { get; set; } = DurationUnit.Day;
    public static DurationUnit TimeMediumUnit { get; set; } = DurationUnit.Minute;
    public static DurationUnit TimeShortUnit { get; set; } = DurationUnit.Second;
    public static Length Tolerance {
      get {
        if (UseRhinoTolerance)
          return RhinoUnit.GetRhinoTolerance();
        else
          return tolerance;
      }
      set => tolerance = value;
    }

    public static bool UseRhinoLengthGeometryUnit { get; set; } = false;
    public static bool UseRhinoTolerance { get; set; } = false;
    public static SpeedUnit VelocityUnit { get; set; } = SpeedUnit.MeterPerSecond;
    public static VolumePerLengthUnit VolumePerLengthUnit { get; set; } = VolumePerLengthUnit.CubicMeterPerMeter;
    public static PressureUnit YoungsModulusUnit { get; set; } = PressureUnit.Gigapascal;
    public static LengthUnit LengthUnitResult = LengthUnit.Millimeter;
    private static LengthUnit lengthGeometry = LengthUnit.Meter;
    private static Length tolerance = new Length(1, LengthUnit.Centimeter);
  }
}
