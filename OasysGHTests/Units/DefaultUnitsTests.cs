using OasysGH.Units;
using OasysUnits;
using OasysUnits.Units;
using Xunit;

namespace OasysGHTests.Units {
  [Collection("GrasshopperFixture collection")]
  public class DefaultUnitsTests {
    [Fact]
    public static void GetRhinoTolerance() {
      DefaultUnits.UseRhinoTolerance = true;
      Assert.Equal(
        new Length(0.01, LengthUnit.Meter),
        DefaultUnits.Tolerance);
      DefaultUnits.UseRhinoTolerance = false;
    }

    [Fact]
    public static void GetRhinoLength() {
      DefaultUnits.UseRhinoLengthGeometryUnit = true;
      Assert.Equal(LengthUnit.Meter, DefaultUnits.LengthUnitGeometry);
      DefaultUnits.UseRhinoLengthGeometryUnit = false;
    }

    [Fact]
    public static void DefaultUnitsTest() {
      // this test may fail locally if you have overwritten the default units in Grasshopper

      Assert.Equal(
        AccelerationUnit.MeterPerSecondSquared,
        DefaultUnits.AccelerationUnit);

      Assert.Equal(
        AngleUnit.Radian,
        DefaultUnits.AngleUnit);

      Assert.Equal(
        AxialStiffnessUnit.Kilonewton,
        DefaultUnits.AxialStiffnessUnit);

      Assert.Equal(
        BendingStiffnessUnit.KilonewtonSquareMeter,
        DefaultUnits.BendingStiffnessUnit);

      Assert.Equal(
        CurvatureUnit.PerMeter,
        DefaultUnits.CurvatureUnit);

      Assert.Equal(
        DensityUnit.KilogramPerCubicMeter,
        DefaultUnits.DensityUnit);

      Assert.Equal(
        EnergyUnit.Megajoule,
        DefaultUnits.EnergyUnit);

      Assert.Equal(
        PressureUnit.KilonewtonPerSquareMeter,
        DefaultUnits.ForcePerAreaUnit);

      Assert.Equal(
        ForcePerLengthUnit.KilonewtonPerMeter,
        DefaultUnits.ForcePerLengthUnit);

      Assert.Equal(
        ForceUnit.Kilonewton,
        DefaultUnits.ForceUnit);

      Assert.False(DefaultUnits.UseRhinoLengthGeometryUnit);

      Assert.Equal(
        LengthUnit.Meter,
        DefaultUnits.LengthUnitGeometry);

      Assert.Equal(
        LengthUnit.Centimeter,
        DefaultUnits.LengthUnitSection);

      Assert.Equal(
        LinearDensityUnit.KilogramPerMeter,
        DefaultUnits.LinearDensityUnit);

      Assert.Equal(
        MassUnit.Tonne,
        DefaultUnits.MassUnit);

      Assert.Equal(
        StrainUnit.Ratio,
        DefaultUnits.MaterialStrainUnit);

      Assert.Equal(
        PressureUnit.Megapascal,
        DefaultUnits.MaterialStrengthUnit);

      Assert.Equal(
        MomentUnit.KilonewtonMeter,
        DefaultUnits.MomentUnit);

      Assert.Equal(
        RatioUnit.DecimalFraction,
        DefaultUnits.RatioUnit);

      Assert.Equal(
        AreaMomentOfInertiaUnit.CentimeterToTheFourth,
        DefaultUnits.SectionAreaMomentOfInertiaUnit);

      Assert.Equal(
        AreaUnit.SquareCentimeter,
        DefaultUnits.SectionAreaUnit);

      Assert.Equal(
        SectionModulusUnit.CubicCentimeter,
        DefaultUnits.SectionModulusUnit);

      Assert.Equal(
        VolumeUnit.CubicCentimeter,
        DefaultUnits.SectionVolumeUnit);

      Assert.Equal(
        StrainUnit.Ratio,
        DefaultUnits.StrainUnitResult);

      Assert.Equal(
        PressureUnit.Megapascal,
        DefaultUnits.StressUnitResult);

      Assert.Equal(
        TemperatureUnit.DegreeCelsius,
        DefaultUnits.TemperatureUnit);

      Assert.Equal(
        DurationUnit.Day,
        DefaultUnits.TimeLongUnit);

      Assert.Equal(
        DurationUnit.Minute,
        DefaultUnits.TimeMediumUnit);

      Assert.Equal(
        DurationUnit.Second,
        DefaultUnits.TimeShortUnit);

      Assert.False(DefaultUnits.UseRhinoTolerance);

      Assert.Equal(
        new Length(1, LengthUnit.Centimeter),
        DefaultUnits.Tolerance);

      Assert.Equal(
        SpeedUnit.MeterPerSecond,
        DefaultUnits.VelocityUnit);

      Assert.Equal(
        VolumePerLengthUnit.CubicMeterPerMeter,
        DefaultUnits.VolumePerLengthUnit);

      Assert.Equal(
        PressureUnit.Gigapascal,
        DefaultUnits.YoungsModulusUnit);

      Assert.Equal(
        LengthUnit.Millimeter,
        DefaultUnits.LengthUnitResult);
    }
  }
}
