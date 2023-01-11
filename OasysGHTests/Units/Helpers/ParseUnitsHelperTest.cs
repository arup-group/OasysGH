using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OasysGH.Units.Helpers;
using OasysUnits;
using OasysUnits.Units;
using Xunit;

namespace OasysGHTests.Units.Helpers
{
  public class ParseUnitsHelperTest
  {
    [Theory]
    [InlineData("mm", typeof(LengthUnit), LengthUnit.Millimeter)]
    [InlineData("cm", typeof(LengthUnit), LengthUnit.Centimeter)]
    [InlineData("m", typeof(LengthUnit), LengthUnit.Meter)]
    [InlineData("in", typeof(LengthUnit), LengthUnit.Inch)]
    [InlineData("Millimeter", typeof(LengthUnit), LengthUnit.Millimeter)]
    [InlineData("millimeter", typeof(LengthUnit), LengthUnit.Millimeter)]
    [InlineData("Centimeter", typeof(LengthUnit), LengthUnit.Centimeter)]
    [InlineData("Meter", typeof(LengthUnit), LengthUnit.Meter)]
    [InlineData("Inch", typeof(LengthUnit), LengthUnit.Inch)]
    [InlineData("slug/ft³", typeof(DensityUnit), DensityUnit.SlugPerCubicFoot)]
    [InlineData("lb/ft²", typeof(PressureUnit), PressureUnit.PoundForcePerSquareFoot)]
    public void ParseTest(string value, Type unitType, Enum expectedUnit)
    {
      // Act
      var unit = UnitsHelper.Parse(unitType, value);

      // Assert
      Assert.Equal(expectedUnit, unit);
    }

    [Theory]
    [InlineData("Müllimeter")]
    public void ParseExceptionTest(string value)
    {
      // Act & Assert
      Assert.Throws<UnitNotFoundException>(() => UnitsHelper.Parse(typeof(LengthUnit), value));
    }

    [Theory]
    [InlineData("zh-hk")]
    [InlineData("zh-cn")]
    [InlineData("zh-sg")]
    [InlineData("zh-tw")]
    [InlineData("ru-ru")]
    public void CultureInfoTest(string name)
    {
      // Arrange
      CultureInfo culture = new CultureInfo(name);
      string accelerationAbbreviation = Acceleration.GetAbbreviation(AccelerationUnit.InchPerSecondSquared, culture);
      string angleAbbreviation = Angle.GetAbbreviation(AngleUnit.Radian, culture);
      string areaMomentOfInertiaAbbreviation = AreaMomentOfInertia.GetAbbreviation(AreaMomentOfInertiaUnit.FootToTheFourth, culture);
      string areaAbbreviation = Area.GetAbbreviation(AreaUnit.SquareDecimeter, culture);
      string axialStiffnessAbbreviation = AxialStiffness.GetAbbreviation(AxialStiffnessUnit.Newton, culture);
      string bendingStiffnessAbbreviation = BendingStiffness.GetAbbreviation(BendingStiffnessUnit.NewtonSquareMeter, culture);
      string coefficientOfThermalExpansionAbbreviation = CoefficientOfThermalExpansion.GetAbbreviation(CoefficientOfThermalExpansionUnit.InverseDegreeCelsius, culture);
      string curvatureAbbreviation = Curvature.GetAbbreviation(CurvatureUnit.PerMillimeter, culture);
      string densityAbbreviation = Density.GetAbbreviation(DensityUnit.KilogramPerCubicMeter, culture);
      string durationAbbreviation = Duration.GetAbbreviation(DurationUnit.Second, culture);
      string energyAbbreviation = Energy.GetAbbreviation(EnergyUnit.Joule, culture);
      string forcePerLengthAbbreviation = ForcePerLength.GetAbbreviation(ForcePerLengthUnit.NewtonPerMeter, culture);
      string forceAbbreviation = Force.GetAbbreviation(ForceUnit.Newton, culture);
      string lengthAbbreviation = Length.GetAbbreviation(LengthUnit.Meter, culture);
      string linearDensityAbbreviation = LinearDensity.GetAbbreviation(LinearDensityUnit.GramPerMeter, culture);
      string massAbbreviation = Mass.GetAbbreviation(MassUnit.Kilogram, culture);
      string momentAbbreviation = Moment.GetAbbreviation(MomentUnit.KilonewtonMeter, culture);
      string pressureAbbreviation = Pressure.GetAbbreviation(PressureUnit.Bar, culture);
      string ratioAbbreviation = Ratio.GetAbbreviation(RatioUnit.Percent, culture);
      string speedAbbreviation = Speed.GetAbbreviation(SpeedUnit.KilometerPerHour, culture);
      string strainAbbreviation = Strain.GetAbbreviation(StrainUnit.MilliStrain, culture);
      string temperatureAbbreviation = Temperature.GetAbbreviation(TemperatureUnit.DegreeCelsius, culture);
      string volumePerLengthAbbreviation = VolumePerLength.GetAbbreviation(VolumePerLengthUnit.CubicYardPerFoot, culture);
      string volumeAbbreviation = Volume.GetAbbreviation(VolumeUnit.Liter, culture);

      // Act
      AccelerationUnit accelerationUnit = (AccelerationUnit)UnitsHelper.Parse(typeof(AccelerationUnit), accelerationAbbreviation, culture);
      AngleUnit angleUnit = (AngleUnit)UnitsHelper.Parse(typeof(AngleUnit), angleAbbreviation, culture);
      AreaMomentOfInertiaUnit areaMomentOfInertiaUnit = (AreaMomentOfInertiaUnit)UnitsHelper.Parse(typeof(AreaMomentOfInertiaUnit), areaMomentOfInertiaAbbreviation, culture);
      AreaUnit areaUnit = (AreaUnit)UnitsHelper.Parse(typeof(AreaUnit), areaAbbreviation, culture);
      AxialStiffnessUnit axialStiffnessUnit = (AxialStiffnessUnit)UnitsHelper.Parse(typeof(AxialStiffnessUnit), axialStiffnessAbbreviation, culture);
      BendingStiffnessUnit bendingStiffnessUnit = (BendingStiffnessUnit)UnitsHelper.Parse(typeof(BendingStiffnessUnit), bendingStiffnessAbbreviation, culture);
      CoefficientOfThermalExpansionUnit coefficientOfThermalExpansionUnit = (CoefficientOfThermalExpansionUnit)UnitsHelper.Parse(typeof(CoefficientOfThermalExpansionUnit), coefficientOfThermalExpansionAbbreviation, culture);
      CurvatureUnit curvatureUnit = (CurvatureUnit)UnitsHelper.Parse(typeof(CurvatureUnit), curvatureAbbreviation, culture);
      DensityUnit densityUnit = (DensityUnit)UnitsHelper.Parse(typeof(DensityUnit), densityAbbreviation, culture);
      DurationUnit durationUnit = (DurationUnit)UnitsHelper.Parse(typeof(DurationUnit), durationAbbreviation, culture);
      EnergyUnit energyUnit = (EnergyUnit)UnitsHelper.Parse(typeof(EnergyUnit), energyAbbreviation, culture);
      ForcePerLengthUnit forcePerLengthUnit = (ForcePerLengthUnit)UnitsHelper.Parse(typeof(ForcePerLengthUnit), forcePerLengthAbbreviation, culture);
      ForceUnit forceUnit = (ForceUnit)UnitsHelper.Parse(typeof(ForceUnit), forceAbbreviation, culture);
      LengthUnit lengthUnit = (LengthUnit)UnitsHelper.Parse(typeof(LengthUnit), lengthAbbreviation, culture);
      LinearDensityUnit linearDensityUnit = (LinearDensityUnit)UnitsHelper.Parse(typeof(LinearDensityUnit), linearDensityAbbreviation, culture);
      MassUnit massUnit = (MassUnit)UnitsHelper.Parse(typeof(MassUnit), massAbbreviation, culture);
      MomentUnit momentUnit = (MomentUnit)UnitsHelper.Parse(typeof(MomentUnit), momentAbbreviation, culture);
      PressureUnit pressureUnit = (PressureUnit)UnitsHelper.Parse(typeof(PressureUnit), pressureAbbreviation, culture);
      RatioUnit ratioUnit = (RatioUnit)UnitsHelper.Parse(typeof(RatioUnit), ratioAbbreviation, culture);
      SpeedUnit speedUnit = (SpeedUnit)UnitsHelper.Parse(typeof(SpeedUnit), speedAbbreviation, culture);
      StrainUnit strainUnit = (StrainUnit)UnitsHelper.Parse(typeof(StrainUnit), strainAbbreviation, culture);
      TemperatureUnit temperatureUnit = (TemperatureUnit)UnitsHelper.Parse(typeof(TemperatureUnit), temperatureAbbreviation, culture);
      VolumePerLengthUnit volumePerLengthUnit = (VolumePerLengthUnit)UnitsHelper.Parse(typeof(VolumePerLengthUnit), volumePerLengthAbbreviation, culture);
      VolumeUnit volumeUnit = (VolumeUnit)UnitsHelper.Parse(typeof(VolumeUnit), volumeAbbreviation, culture);

      // Assert
      Assert.Equal(AccelerationUnit.InchPerSecondSquared, accelerationUnit);
      Assert.Equal(AngleUnit.Radian, angleUnit);
      Assert.Equal(AreaMomentOfInertiaUnit.FootToTheFourth, areaMomentOfInertiaUnit);
      Assert.Equal(AreaUnit.SquareDecimeter, areaUnit);
      Assert.Equal(AxialStiffnessUnit.Newton, axialStiffnessUnit);
      Assert.Equal(BendingStiffnessUnit.NewtonSquareMeter, bendingStiffnessUnit);
      Assert.Equal(CoefficientOfThermalExpansionUnit.InverseDegreeCelsius, coefficientOfThermalExpansionUnit);
      Assert.Equal(CurvatureUnit.PerMillimeter, curvatureUnit);
      Assert.Equal(DensityUnit.KilogramPerCubicMeter, densityUnit);
      Assert.Equal(DurationUnit.Second, durationUnit);
      Assert.Equal(EnergyUnit.Joule, energyUnit);
      Assert.Equal(ForcePerLengthUnit.NewtonPerMeter, forcePerLengthUnit);
      Assert.Equal(ForceUnit.Newton, forceUnit);
      Assert.Equal(LengthUnit.Meter, lengthUnit);
      Assert.Equal(LinearDensityUnit.GramPerMeter, linearDensityUnit);
      Assert.Equal(MassUnit.Kilogram, massUnit);
      Assert.Equal(MomentUnit.KilonewtonMeter, momentUnit);
      Assert.Equal(PressureUnit.Bar, pressureUnit);
      Assert.Equal(RatioUnit.Percent, ratioUnit);
      Assert.Equal(SpeedUnit.KilometerPerHour, speedUnit);
      Assert.Equal(StrainUnit.MilliStrain, strainUnit);
      Assert.Equal(TemperatureUnit.DegreeCelsius, temperatureUnit);
      Assert.Equal(VolumePerLengthUnit.CubicYardPerFoot, volumePerLengthUnit);
      Assert.Equal(VolumeUnit.Liter, volumeUnit);
    }


  }
}
