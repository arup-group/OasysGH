using System;
using OasysGH.Units.Helpers;
using OasysUnits.Units;
using Xunit;

namespace OasysGHTests.Helper
{
  public class UnitsHelperTest
  {
    [Theory]
    [InlineData("mm", LengthUnit.Millimeter)]
    [InlineData("cm", LengthUnit.Centimeter)]
    [InlineData("m", LengthUnit.Meter)]
    [InlineData("in", LengthUnit.Inch)]
    [InlineData("Millimeter", LengthUnit.Millimeter)]
    [InlineData("Centimeter", LengthUnit.Centimeter)]
    [InlineData("Meter", LengthUnit.Meter)]
    [InlineData("Inch", LengthUnit.Inch)]
    public void ParseTest(string value, LengthUnit expectedUnit)
    {
      // Act
      var unit = UnitsHelper.Parse(value, typeof(LengthUnit));

      // Assert
      Assert.Equal(expectedUnit, unit);
    }

    [Theory]
    [InlineData("millimeter")]
    [InlineData("Müllimeter")]
    public void ParseExceptionTest(string value)
    {
      // Act & Assert
      Assert.Throws<ArgumentException>(() => UnitsHelper.Parse(value, typeof(LengthUnit)));
    }
  }
}
