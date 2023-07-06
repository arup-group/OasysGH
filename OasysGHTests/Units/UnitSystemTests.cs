using OasysGH.Units;
using OasysUnits.Units;
using Xunit;

namespace OasysGHTests.Units {
  [Collection("GrasshopperFixture collection")]
  public class UnitSystemTests {
    [Fact]
    public static void UnitSystemTest() {
      var us = new UnitSystem(
        LengthUnit.Centimeter,
        AreaUnit.SquareMeter,
        VolumeUnit.CubicMillimeter,
        AreaMomentOfInertiaUnit.FootToTheFourth,
        MassUnit.Slug,
        DensityUnit.KilogramPerCubicMeter,
        LinearDensityUnit.KilogramPerMeter,
        VolumePerLengthUnit.CubicMeterPerMeter,
        PressureUnit.Gigapascal,
        StrainUnit.Percent,
        PressureUnit.Megapascal,
        LengthUnit.Inch,
        ForceUnit.Kilonewton,
        ForcePerLengthUnit.KilonewtonPerMeter,
        PressureUnit.KilonewtonPerSquareMeter,
        MomentUnit.KilonewtonMeter,
        TemperatureUnit.SolarTemperature,
        LengthUnit.Millimeter,
        PressureUnit.Kilopascal,
        StrainUnit.MilliStrain,
        AxialStiffnessUnit.KilogramForce,
        BendingStiffnessUnit.PoundForceSquareInch,
        SpeedUnit.KilometerPerHour,
        AccelerationUnit.StandardGravity,
        EnergyUnit.Calorie,
        CurvatureUnit.PerMillimeter,
        SectionModulusUnit.CubicCentimeter);

      Assert.Equal(LengthUnit.Centimeter, us.SectionLengthUnit);
        Assert.Equal(AreaUnit.SquareMeter, us.SectionAreaUnit);
      Assert.Equal(VolumeUnit.CubicMillimeter, us.SectionVolumeUnit);
      Assert.Equal(AreaMomentOfInertiaUnit.FootToTheFourth, us.SectionAreaMomentOfInertiaUnit);
      Assert.Equal(MassUnit.Slug, us.MassUnit);
      Assert.Equal(DensityUnit.KilogramPerCubicMeter, us.DensityUnit);
      Assert.Equal(LinearDensityUnit.KilogramPerMeter, us.LinearDensityUnit);
      Assert.Equal(VolumePerLengthUnit.CubicMeterPerMeter, us.VolumePerLengthUnit);
      Assert.Equal(PressureUnit.Gigapascal, us.MaterialStrengthUnit);
      Assert.Equal(StrainUnit.Percent, us.MaterialStrainUnit);
      Assert.Equal(PressureUnit.Megapascal, us.YoungsModulusUnit);
      Assert.Equal(LengthUnit.Inch, us.LengthUnit);
      Assert.Equal(ForceUnit.Kilonewton, us.ForceUnit);
      Assert.Equal(ForcePerLengthUnit.KilonewtonPerMeter, us.ForcePerLengthUnit);
      Assert.Equal(PressureUnit.KilonewtonPerSquareMeter, us.ForcePerAreaUnit);
      Assert.Equal(MomentUnit.KilonewtonMeter, us.MomentUnit);
      Assert.Equal(TemperatureUnit.SolarTemperature, us.TemperatureUnit);
      Assert.Equal(LengthUnit.Millimeter, us.LengthUnitResult);
      Assert.Equal(PressureUnit.Kilopascal, us.StressUnitResult);
      Assert.Equal(StrainUnit.MilliStrain, us.StrainUnitResult);
      Assert.Equal(AxialStiffnessUnit.KilogramForce, us.AxialStiffnessUnit);
      Assert.Equal(BendingStiffnessUnit.PoundForceSquareInch, us.BendingStiffnessUnit);
      Assert.Equal(SpeedUnit.KilometerPerHour, us.VelocityUnit);
      Assert.Equal(AccelerationUnit.StandardGravity, us.AccelerationUnit);
      Assert.Equal(EnergyUnit.Calorie, us.EnergyUnit);
      Assert.Equal(CurvatureUnit.PerMillimeter, us.CurvatureUnit);
      Assert.Equal(SectionModulusUnit.CubicCentimeter, us.SectionModulusUnit);
    }
  }
}
