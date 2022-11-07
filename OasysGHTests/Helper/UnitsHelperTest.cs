using System;
using OasysGH.Units.Helpers;
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
    [InlineData("millimeter")]
    [InlineData("Müllimeter")]
    public void ParseExceptionTest(string value)
    {
      // Act & Assert
      Assert.Throws<ArgumentException>(() => UnitsHelper.Parse(typeof(LengthUnit), value));
    }
  }
}
