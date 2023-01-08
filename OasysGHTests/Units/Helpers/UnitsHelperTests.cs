using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OasysGH.Units;
using OasysGH.Units.Helpers;
using OasysUnits;
using OasysUnits.Units;
using Xunit;

namespace OasysGHTests.Units.Helpers
{
  [Collection("GrasshopperFixture collection")]
  public class UnitsHelperTests
  {
    [Fact]
    public void SignificantDigitsTest() 
    {
      // Assemble
      Length currentTolerance = DefaultUnits.Tolerance;
      
      // Act
      DefaultUnits.Tolerance = new Length(0.12345678, DefaultUnits.LengthUnitGeometry);

      // Assert
      Assert.Equal(8, UnitsHelper.SignificantDigits);

      // Revert
      DefaultUnits.Tolerance = currentTolerance;
    }

    [Theory]
    [InlineData(AreaUnit.SquareMillimeter, LengthUnit.Millimeter)]
    [InlineData(AreaUnit.SquareCentimeter, LengthUnit.Centimeter)]
    [InlineData(AreaUnit.SquareMeter, LengthUnit.Meter)]
    [InlineData(AreaUnit.SquareFoot, LengthUnit.Foot)]
    [InlineData(AreaUnit.SquareInch, LengthUnit.Inch)]
    [InlineData(AreaUnit.SquareKilometer, LengthUnit.Kilometer)]
    public void GetAreaUnitTest(AreaUnit expected, LengthUnit lengthUnit)
    {
      // Act
      AreaUnit unit = UnitsHelper.GetAreaUnit(lengthUnit);

      // Assert
      Assert.Equal(expected, unit);
    }

    [Theory]
    [InlineData(VolumeUnit.CubicMillimeter, LengthUnit.Millimeter)]
    [InlineData(VolumeUnit.CubicCentimeter, LengthUnit.Centimeter)]
    [InlineData(VolumeUnit.CubicMeter, LengthUnit.Meter)]
    [InlineData(VolumeUnit.CubicFoot, LengthUnit.Foot)]
    [InlineData(VolumeUnit.CubicInch, LengthUnit.Inch)]
    public void GetVolumeUnitTest(VolumeUnit expected, LengthUnit lengthUnit)
    {
      // Act
      VolumeUnit unit = UnitsHelper.GetVolumeUnit(lengthUnit);

      // Assert
      Assert.Equal(expected, unit);
    }

    [Theory]
    [InlineData(AreaMomentOfInertiaUnit.MillimeterToTheFourth, LengthUnit.Millimeter)]
    [InlineData(AreaMomentOfInertiaUnit.CentimeterToTheFourth, LengthUnit.Centimeter)]
    [InlineData(AreaMomentOfInertiaUnit.MeterToTheFourth, LengthUnit.Meter)]
    [InlineData(AreaMomentOfInertiaUnit.FootToTheFourth, LengthUnit.Foot)]
    [InlineData(AreaMomentOfInertiaUnit.InchToTheFourth, LengthUnit.Inch)]
    public void GetAreaMomentOfInertiaUnitTest(AreaMomentOfInertiaUnit expected, LengthUnit lengthUnit)
    {
      // Act
      AreaMomentOfInertiaUnit unit = UnitsHelper.GetAreaMomentOfInertiaUnit(lengthUnit);

      // Assert
      Assert.Equal(expected, unit);
    }

    [Theory]
    [InlineData(ForcePerLengthUnit.NewtonPerMillimeter, ForceUnit.Newton, LengthUnit.Millimeter)]
    [InlineData(ForcePerLengthUnit.NewtonPerCentimeter, ForceUnit.Newton, LengthUnit.Centimeter)]
    [InlineData(ForcePerLengthUnit.NewtonPerMeter, ForceUnit.Newton, LengthUnit.Meter)]
    [InlineData(ForcePerLengthUnit.KilonewtonPerMillimeter, ForceUnit.Kilonewton, LengthUnit.Millimeter)]
    [InlineData(ForcePerLengthUnit.KilonewtonPerCentimeter, ForceUnit.Kilonewton, LengthUnit.Centimeter)]
    [InlineData(ForcePerLengthUnit.KilonewtonPerMeter, ForceUnit.Kilonewton, LengthUnit.Meter)]
    [InlineData(ForcePerLengthUnit.MeganewtonPerMillimeter, ForceUnit.Meganewton, LengthUnit.Millimeter)]
    [InlineData(ForcePerLengthUnit.MeganewtonPerCentimeter, ForceUnit.Meganewton, LengthUnit.Centimeter)]
    [InlineData(ForcePerLengthUnit.MeganewtonPerMeter, ForceUnit.Meganewton, LengthUnit.Meter)]
    [InlineData(ForcePerLengthUnit.KilopoundForcePerInch, ForceUnit.KilopoundForce, LengthUnit.Inch)]
    [InlineData(ForcePerLengthUnit.KilopoundForcePerFoot, ForceUnit.KilopoundForce, LengthUnit.Foot)]
    [InlineData(ForcePerLengthUnit.PoundForcePerInch, ForceUnit.PoundForce, LengthUnit.Inch)]
    [InlineData(ForcePerLengthUnit.PoundForcePerFoot, ForceUnit.PoundForce, LengthUnit.Foot)]

    public void GetForcePerLengthUnitTest(ForcePerLengthUnit expected, ForceUnit forceUnit, LengthUnit lengthUnit)
    {
      // Act
      ForcePerLengthUnit unit = UnitsHelper.GetForcePerLengthUnit(forceUnit, lengthUnit);

      // Assert
      Assert.Equal(expected, unit);
    }

    [Theory]
    [InlineData(PressureUnit.NewtonPerSquareMillimeter, ForceUnit.Newton, LengthUnit.Millimeter)]
    [InlineData(PressureUnit.NewtonPerSquareCentimeter, ForceUnit.Newton, LengthUnit.Centimeter)]
    [InlineData(PressureUnit.NewtonPerSquareMeter, ForceUnit.Newton, LengthUnit.Meter)]
    [InlineData(PressureUnit.KilonewtonPerSquareMillimeter, ForceUnit.Kilonewton, LengthUnit.Millimeter)]
    [InlineData(PressureUnit.KilonewtonPerSquareCentimeter, ForceUnit.Kilonewton, LengthUnit.Centimeter)]
    [InlineData(PressureUnit.KilonewtonPerSquareMeter, ForceUnit.Kilonewton, LengthUnit.Meter)]
    [InlineData(PressureUnit.MeganewtonPerSquareMeter, ForceUnit.Meganewton, LengthUnit.Meter)]
    [InlineData(PressureUnit.KilopoundForcePerSquareInch, ForceUnit.KilopoundForce, LengthUnit.Inch)]
    [InlineData(PressureUnit.KilopoundForcePerSquareFoot, ForceUnit.KilopoundForce, LengthUnit.Foot)]
    [InlineData(PressureUnit.PoundForcePerSquareInch, ForceUnit.PoundForce, LengthUnit.Inch)]
    [InlineData(PressureUnit.PoundForcePerSquareFoot, ForceUnit.PoundForce, LengthUnit.Foot)]

    public void GetForcePerAreaUnitTest(PressureUnit expected, ForceUnit forceUnit, LengthUnit lengthUnit)
    {
      // Act
      PressureUnit unit = UnitsHelper.GetForcePerAreaUnit(forceUnit, lengthUnit);

      // Assert
      Assert.Equal(expected, unit);
    }

    [Theory]
    [InlineData(BendingStiffnessUnit.NewtonSquareMillimeter, ForceUnit.Newton, LengthUnit.Millimeter)]
    [InlineData(BendingStiffnessUnit.NewtonSquareMeter, ForceUnit.Newton, LengthUnit.Meter)]
    [InlineData(BendingStiffnessUnit.KilonewtonSquareMillimeter, ForceUnit.Kilonewton, LengthUnit.Millimeter)]
    [InlineData(BendingStiffnessUnit.KilonewtonSquareMeter, ForceUnit.Kilonewton, LengthUnit.Meter)]
    [InlineData(BendingStiffnessUnit.PoundForceSquareInch, ForceUnit.PoundForce, LengthUnit.Inch)]
    [InlineData(BendingStiffnessUnit.PoundForceSquareFoot, ForceUnit.PoundForce, LengthUnit.Foot)]

    public void GetBendingStiffnessUnitTest(BendingStiffnessUnit expected, ForceUnit forceUnit, LengthUnit lengthUnit)
    {
      // Act
      BendingStiffnessUnit unit = UnitsHelper.GetBendingStiffnessUnit(forceUnit, lengthUnit);

      // Assert
      Assert.Equal(expected, unit);
    }

    [Theory]
    [InlineData(MomentUnit.NewtonMillimeter, ForceUnit.Newton, LengthUnit.Millimeter)]
    [InlineData(MomentUnit.NewtonCentimeter, ForceUnit.Newton, LengthUnit.Centimeter)]
    [InlineData(MomentUnit.NewtonMeter, ForceUnit.Newton, LengthUnit.Meter)]
    [InlineData(MomentUnit.KilonewtonMillimeter, ForceUnit.Kilonewton, LengthUnit.Millimeter)]
    [InlineData(MomentUnit.KilonewtonCentimeter, ForceUnit.Kilonewton, LengthUnit.Centimeter)]
    [InlineData(MomentUnit.KilonewtonMeter, ForceUnit.Kilonewton, LengthUnit.Meter)]
    [InlineData(MomentUnit.MeganewtonMillimeter, ForceUnit.Meganewton, LengthUnit.Millimeter)]
    [InlineData(MomentUnit.MeganewtonCentimeter, ForceUnit.Meganewton, LengthUnit.Centimeter)]
    [InlineData(MomentUnit.MeganewtonMeter, ForceUnit.Meganewton, LengthUnit.Meter)]
    [InlineData(MomentUnit.KilopoundForceInch, ForceUnit.KilopoundForce, LengthUnit.Inch)]
    [InlineData(MomentUnit.KilopoundForceFoot, ForceUnit.KilopoundForce, LengthUnit.Foot)]
    [InlineData(MomentUnit.PoundForceInch, ForceUnit.PoundForce, LengthUnit.Inch)]
    [InlineData(MomentUnit.PoundForceFoot, ForceUnit.PoundForce, LengthUnit.Foot)]

    public void GetMomentUnitTest(MomentUnit expected, ForceUnit forceUnit, LengthUnit lengthUnit)
    {
      // Act
      MomentUnit unit = UnitsHelper.GetMomentUnit(forceUnit, lengthUnit);

      // Assert
      Assert.Equal(expected, unit);
    }

    [Theory]
    [InlineData(AxialStiffnessUnit.Newton, ForceUnit.Newton)]
    [InlineData(AxialStiffnessUnit.Kilonewton, ForceUnit.Kilonewton)]
    [InlineData(AxialStiffnessUnit.Meganewton, ForceUnit.Meganewton)]
    [InlineData(AxialStiffnessUnit.PoundForce, ForceUnit.PoundForce)]
    [InlineData(AxialStiffnessUnit.KilopoundForce, ForceUnit.KilopoundForce)]
    public void GetAxialStiffnessUnitTest(AxialStiffnessUnit expected, ForceUnit forceUnit)
    {
      // Act
      AxialStiffnessUnit unit = UnitsHelper.GetAxialStiffnessUnit(forceUnit);

      // Assert
      Assert.Equal(expected, unit);
    }

    [Theory]
    [InlineData(DensityUnit.GramPerCubicMillimeter, MassUnit.Gram, LengthUnit.Millimeter)]
    [InlineData(DensityUnit.GramPerCubicCentimeter, MassUnit.Gram, LengthUnit.Centimeter)]
    [InlineData(DensityUnit.GramPerCubicMeter, MassUnit.Gram, LengthUnit.Meter)]
    [InlineData(DensityUnit.KilogramPerCubicMillimeter, MassUnit.Kilogram, LengthUnit.Millimeter)]
    [InlineData(DensityUnit.KilogramPerCubicCentimeter, MassUnit.Kilogram, LengthUnit.Centimeter)]
    [InlineData(DensityUnit.KilogramPerCubicMeter, MassUnit.Kilogram, LengthUnit.Meter)]
    [InlineData(DensityUnit.TonnePerCubicMillimeter, MassUnit.Tonne, LengthUnit.Millimeter)]
    [InlineData(DensityUnit.TonnePerCubicCentimeter, MassUnit.Tonne, LengthUnit.Centimeter)]
    [InlineData(DensityUnit.TonnePerCubicMeter, MassUnit.Tonne, LengthUnit.Meter)]
    [InlineData(DensityUnit.PoundPerCubicFoot, MassUnit.Pound, LengthUnit.Foot)]
    [InlineData(DensityUnit.PoundPerCubicInch, MassUnit.Pound, LengthUnit.Inch)]
    [InlineData(DensityUnit.KilopoundPerCubicFoot, MassUnit.Kilopound, LengthUnit.Foot)]
    [InlineData(DensityUnit.KilopoundPerCubicInch, MassUnit.Kilopound, LengthUnit.Inch)]
    [InlineData(DensityUnit.SlugPerCubicFoot, MassUnit.Slug, LengthUnit.Foot)]
    public void GetDensityUnitTest(DensityUnit expected, MassUnit massUnit, LengthUnit lengthUnit)
    {
      // Act
      DensityUnit unit = UnitsHelper.GetDensityUnit(massUnit, lengthUnit);

      // Assert
      Assert.Equal(expected, unit);
    }

    [Theory]
    [InlineData(LinearDensityUnit.KilogramPerMillimeter, MassUnit.Kilogram, LengthUnit.Millimeter)]
    [InlineData(LinearDensityUnit.KilogramPerCentimeter, MassUnit.Kilogram, LengthUnit.Centimeter)]
    [InlineData(LinearDensityUnit.KilogramPerMeter, MassUnit.Kilogram, LengthUnit.Meter)]
    [InlineData(LinearDensityUnit.PoundPerFoot, MassUnit.Pound, LengthUnit.Foot)]
    [InlineData(LinearDensityUnit.PoundPerInch, MassUnit.Pound, LengthUnit.Inch)]
    public void GetLinearDensityUnitTest(LinearDensityUnit expected, MassUnit massUnit, LengthUnit lengthUnit)
    {
      // Act
      LinearDensityUnit unit = UnitsHelper.GetLinearDensityUnit(massUnit, lengthUnit);

      // Assert
      Assert.Equal(expected, unit);
    }

    [Theory]
    [InlineData(CoefficientOfThermalExpansionUnit.InverseKelvin, TemperatureUnit.Kelvin)]
    [InlineData(CoefficientOfThermalExpansionUnit.InverseDegreeFahrenheit, TemperatureUnit.DegreeFahrenheit)]
    [InlineData(CoefficientOfThermalExpansionUnit.InverseDegreeCelsius, TemperatureUnit.DegreeCelsius)]
    public void GetCoefficientOfThermalExpansionUnitTest(CoefficientOfThermalExpansionUnit expected, TemperatureUnit temperatureUnit)
    {
      // Act
      CoefficientOfThermalExpansionUnit unit = UnitsHelper.GetCoefficientOfThermalExpansionUnit(temperatureUnit);

      // Assert
      Assert.Equal(expected, unit);
    }

    [Theory]
    [InlineData(EngineeringUnits.Angle, typeof(AngleUnit))]
    [InlineData(EngineeringUnits.Length, typeof(LengthUnit))]
    [InlineData(EngineeringUnits.Area, typeof(AreaUnit))]
    [InlineData(EngineeringUnits.Volume, typeof(VolumeUnit))]
    [InlineData(EngineeringUnits.AreaMomentOfInertia, typeof(AreaMomentOfInertiaUnit))]
    [InlineData(EngineeringUnits.Force, typeof(ForceUnit))]
    [InlineData(EngineeringUnits.ForcePerLength, typeof(ForcePerLengthUnit))]
    [InlineData(EngineeringUnits.ForcePerArea, typeof(PressureUnit))]
    [InlineData(EngineeringUnits.Moment, typeof(MomentUnit))]
    [InlineData(EngineeringUnits.Stress, typeof(PressureUnit))]
    [InlineData(EngineeringUnits.Strain, typeof(StrainUnit))]
    [InlineData(EngineeringUnits.AxialStiffness, typeof(AxialStiffnessUnit))]
    [InlineData(EngineeringUnits.BendingStiffness, typeof(BendingStiffnessUnit))]
    [InlineData(EngineeringUnits.Curvature, typeof(CurvatureUnit))]
    [InlineData(EngineeringUnits.Mass, typeof(MassUnit))]
    [InlineData(EngineeringUnits.Density, typeof(DensityUnit))]
    [InlineData(EngineeringUnits.LinearDensity, typeof(LinearDensityUnit))]
    [InlineData(EngineeringUnits.VolumePerLength, typeof(VolumePerLengthUnit))]
    [InlineData(EngineeringUnits.Temperature, typeof(TemperatureUnit))]
    [InlineData(EngineeringUnits.Velocity, typeof(SpeedUnit))]
    [InlineData(EngineeringUnits.Acceleration, typeof(AccelerationUnit))]
    [InlineData(EngineeringUnits.Energy, typeof(EnergyUnit))]
    [InlineData(EngineeringUnits.Ratio, typeof(RatioUnit))]
    [InlineData(EngineeringUnits.Time, typeof(DurationUnit))]
    public void GetFilteredAbbreviationsTest(EngineeringUnits unit, Type type) 
    {
      // Act
      List<string> unitsAbbs = UnitsHelper.GetFilteredAbbreviations(unit);

      for (int i = 0; i < unitsAbbs.Count; i++)
      {
        Enum parsedUnit = UnitsHelper.Parse(type, unitsAbbs[i]);
        Assert.True(parsedUnit != null);
      }
    }
  }
}
