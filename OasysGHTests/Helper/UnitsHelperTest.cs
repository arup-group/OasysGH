using System;
using System.Globalization;
using OasysGH.Units.Helpers;
using OasysUnits;
using OasysUnits.Units;
using Xunit;

namespace OasysGHTests.Helper
{
  public class UnitsHelperTest
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
    [InlineData("en-US")]
    [InlineData("ru-RU")]
    public void CultureInfoTest(string name)
    {
      // Arrange
      CultureInfo culture = new CultureInfo(name);
      string accelerationAbbreviation = Acceleration.GetAbbreviation(AccelerationUnit.InchPerSecondSquared, culture);
      string angleAbbreviation = Angle.GetAbbreviation(AngleUnit.Radian, culture);
      string areaMomentOfInertiaAbbreviation = AreaMomentOfInertia.GetAbbreviation(AreaMomentOfInertiaUnit.FootToTheFourth, culture);

      string lengthAbbreviation = Length.GetAbbreviation(LengthUnit.Meter, culture);

      // Act
      AccelerationUnit accelerationUnit = (AccelerationUnit)UnitsHelper.Parse(typeof(AccelerationUnit), accelerationAbbreviation, culture);
      AngleUnit angleUnit = (AngleUnit)UnitsHelper.Parse(typeof(AngleUnit), angleAbbreviation, culture);
      AreaMomentOfInertiaUnit areaMomentOfInertiaUnit = (AreaMomentOfInertiaUnit)UnitsHelper.Parse(typeof(AreaMomentOfInertiaUnit), areaMomentOfInertiaAbbreviation, culture);


      LengthUnit lengthUnit = (LengthUnit)UnitsHelper.Parse(typeof(LengthUnit), lengthAbbreviation, culture);


      // Assert
      Assert.Equal(AccelerationUnit.InchPerSecondSquared, accelerationUnit);
      Assert.Equal(AngleUnit.Radian, angleUnit);
      Assert.Equal(AreaMomentOfInertiaUnit.FootToTheFourth, areaMomentOfInertiaUnit);

      Assert.Equal(LengthUnit.Meter, lengthUnit);
    }
  }
}
