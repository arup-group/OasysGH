using OasysGH.Units.UI;
using OasysUnits;
using OasysUnits.Units;
using Xunit;
using UnitSystem = OasysGH.Units.UnitSystem;

namespace OasysGHTests.Units {
  [Collection("GrasshopperFixture collection")]
  public class DefaultUnitsFormTests {
    [Fact]
    public static void kipFtButtonClickTest() {
      var unitBox = new DefaultUnitsForm();
      Assert.NotNull(unitBox);
      unitBox.kipft_Click(null, null);
      TestUnitBox(kipft(), unitBox);
    }

    [Fact]
    public static void kipInButtonClickTest() {
      var unitBox = new DefaultUnitsForm();
      Assert.NotNull(unitBox);
      unitBox.kipin_Click(null, null);
      TestUnitBox(kipin(), unitBox);
    }

    [Fact]
    public static void kNmButtonClickTest() {
      var unitBox = new DefaultUnitsForm();
      Assert.NotNull(unitBox);
      unitBox.kNm_Click(null, null);
      TestUnitBox(kNm(), unitBox);
    }

    [Fact]
    public static void SiButtonClickTest() {
      var unitBox = new DefaultUnitsForm();
      Assert.NotNull(unitBox);
      unitBox.SI_Click(null, null);
      TestUnitBox(SI(), unitBox);
    }

    [Fact]
    public static void UseRhinoLengthUnitCheckBoxTest() {
      var unitBox = new DefaultUnitsForm();
      Assert.NotNull(unitBox);
      // initialt state is false
      Assert.False(unitBox.useRhinoLengthUnit.Checked);
      unitBox.useRhinoLengthUnit_CheckedChanged(null, null);
      Assert.True(unitBox.lengthComboBox.Enabled);
      // toggle to true
      unitBox.useRhinoLengthUnit.Checked = true;
      unitBox.useRhinoLengthUnit_CheckedChanged(null, null);
      Assert.False(unitBox.lengthComboBox.Enabled);
    }

    [Fact]
    public static void UseRhinoToleranceCheckBoxTest() {
      var unitBox = new DefaultUnitsForm();
      Assert.NotNull(unitBox);
      // initialt state is false
      Assert.False(unitBox.useRhinoTolerance.Checked);
      // toggle to true
      unitBox.useRhinoTolerance_CheckedChanged(null, null);
      Assert.True(unitBox.toleranceUpDown.Enabled);
      
      unitBox.useRhinoTolerance.Checked = true;
      // toggle to false
      unitBox.useRhinoTolerance_CheckedChanged(null, null);
      Assert.False(unitBox.toleranceUpDown.Enabled);
    }

    internal static void TestUnitBox(UnitSystem unitSystem, DefaultUnitsForm unitBox) {
      Assert.Equal(
        Length.GetAbbreviation(unitSystem.SectionLengthUnit),
        unitBox.lengthSectionComboBox.Text);

      Assert.Equal(
        Area.GetAbbreviation(unitSystem.SectionAreaUnit),
        unitBox.areaComboBox.Text);

      Assert.Equal(
        SectionModulus.GetAbbreviation(unitSystem.SectionModulusUnit),
        unitBox.volumeComboBox.Text);

      Assert.Equal(
        AreaMomentOfInertia.GetAbbreviation(unitSystem.SectionAreaMomentOfInertiaUnit),
        unitBox.momentOfInertiaComboBox.Text);

      Assert.Equal(
        Mass.GetAbbreviation(unitSystem.MassUnit),
        unitBox.massComboBox.Text);

      Assert.Equal(
        Density.GetAbbreviation(unitSystem.DensityUnit),
        unitBox.densityComboBox.Text);

      Assert.Equal(
        LinearDensity.GetAbbreviation(unitSystem.LinearDensityUnit),
        unitBox.linearDensityComboBox.Text);

      Assert.Equal(
        VolumePerLength.GetAbbreviation(unitSystem.VolumePerLengthUnit),
        unitBox.volumePerLengthComboBox.Text);

      Assert.Equal(
        Pressure.GetAbbreviation(unitSystem.MaterialStrengthUnit),
        unitBox.materialStrengthComboBox.Text);

      Assert.Equal(
        Strain.GetAbbreviation(unitSystem.MaterialStrainUnit),
        unitBox.materialStrainComboBox.Text);

      Assert.Equal(
        Pressure.GetAbbreviation(unitSystem.YoungsModulusUnit),
        unitBox.youngsModulusComboBox.Text);

      Assert.Equal(
        Length.GetAbbreviation(unitSystem.LengthUnit),
        unitBox.lengthComboBox.Text);

      Assert.Equal(
        Force.GetAbbreviation(unitSystem.ForceUnit),
        unitBox.forceComboBox.Text);

      Assert.Equal(
        ForcePerLength.GetAbbreviation(unitSystem.ForcePerLengthUnit),
        unitBox.forcePerLengthComboBox.Text);

      Assert.Equal(
        Pressure.GetAbbreviation(unitSystem.ForcePerAreaUnit),
        unitBox.forcePerAreaComboBox.Text);

      Assert.Equal(
        Moment.GetAbbreviation(unitSystem.MomentUnit),
        unitBox.momentComboBox.Text);

      Assert.Equal(
        Temperature.GetAbbreviation(unitSystem.TemperatureUnit),
        unitBox.temperatureComboBox.Text);

      Assert.Equal(
        Length.GetAbbreviation(unitSystem.LengthUnitResult),
        unitBox.displacementComboBox.Text);

      Assert.Equal(
        Pressure.GetAbbreviation(unitSystem.StressUnitResult),
        unitBox.stressComboBox.Text);

      Assert.Equal(
        Strain.GetAbbreviation(unitSystem.StrainUnitResult),
        unitBox.strainComboBox.Text);

      Assert.Equal(
        AxialStiffness.GetAbbreviation(unitSystem.AxialStiffnessUnit),
        unitBox.axialStiffnessComboBox.Text);

      Assert.Equal(
        BendingStiffness.GetAbbreviation(unitSystem.BendingStiffnessUnit),
        unitBox.bendingStiffnessComboBox.Text);

      Assert.Equal(
        Speed.GetAbbreviation(unitSystem.VelocityUnit),
        unitBox.velocityComboBox.Text);

      Assert.Equal(
        Acceleration.GetAbbreviation(unitSystem.AccelerationUnit),
        unitBox.accelerationComboBox.Text);

      Assert.Equal(
        Energy.GetAbbreviation(unitSystem.EnergyUnit),
        unitBox.energyComboBox.Text);

      Assert.Equal(
        Curvature.GetAbbreviation(unitSystem.CurvatureUnit),
        unitBox.curvatureComboBox.Text);
    }

    internal static UnitSystem kipft() {
      return new UnitSystem(
        LengthUnit.Inch,
        AreaUnit.SquareInch,
        VolumeUnit.CubicFoot,
        AreaMomentOfInertiaUnit.InchToTheFourth,
        MassUnit.Pound,
        DensityUnit.PoundPerCubicFoot,
        LinearDensityUnit.PoundPerFoot,
        VolumePerLengthUnit.CubicYardPerFoot,
        PressureUnit.KilopoundForcePerSquareInch,
        StrainUnit.Percent,
        PressureUnit.KilopoundForcePerSquareInch,
        LengthUnit.Foot,
        ForceUnit.KilopoundForce,
        ForcePerLengthUnit.KilopoundForcePerFoot,
        PressureUnit.KilopoundForcePerSquareFoot,
        MomentUnit.KilopoundForceFoot,
        TemperatureUnit.DegreeFahrenheit,
        LengthUnit.Inch,
        PressureUnit.KilopoundForcePerSquareInch,
        StrainUnit.Percent,
        AxialStiffnessUnit.KilopoundForce,
        BendingStiffnessUnit.PoundForceSquareFoot,
        SpeedUnit.FootPerSecond,
        AccelerationUnit.FootPerSecondSquared,
        EnergyUnit.FootPound,
        CurvatureUnit.PerInch,
        SectionModulusUnit.CubicFoot);
    }

    internal static UnitSystem kipin() {
      return new UnitSystem(
        LengthUnit.Inch,
        AreaUnit.SquareInch,
        VolumeUnit.CubicInch,
        AreaMomentOfInertiaUnit.InchToTheFourth,
        MassUnit.Pound,
        DensityUnit.PoundPerCubicFoot,
        LinearDensityUnit.PoundPerInch,
        VolumePerLengthUnit.CubicYardPerFoot,
        PressureUnit.KilopoundForcePerSquareInch,
        StrainUnit.Percent,
        PressureUnit.KilopoundForcePerSquareInch,
        LengthUnit.Inch,
        ForceUnit.KilopoundForce,
        ForcePerLengthUnit.KilopoundForcePerInch,
        PressureUnit.KilopoundForcePerSquareInch,
        MomentUnit.KilopoundForceInch,
        TemperatureUnit.DegreeFahrenheit,
        LengthUnit.Inch,
        PressureUnit.KilopoundForcePerSquareInch,
        StrainUnit.Percent,
        AxialStiffnessUnit.KilopoundForce,
        BendingStiffnessUnit.PoundForceSquareInch,
        SpeedUnit.FootPerSecond,
        AccelerationUnit.FootPerSecondSquared,
        EnergyUnit.FootPound,
        CurvatureUnit.PerInch,
        SectionModulusUnit.CubicInch);
    }

    internal static UnitSystem kNm() {
      return new UnitSystem(
        LengthUnit.Centimeter,
        AreaUnit.SquareCentimeter,
        VolumeUnit.CubicCentimeter,
        AreaMomentOfInertiaUnit.CentimeterToTheFourth,
        MassUnit.Tonne,
        DensityUnit.KilogramPerCubicMeter,
        LinearDensityUnit.KilogramPerMeter,
        VolumePerLengthUnit.CubicMeterPerMeter,
        PressureUnit.Megapascal,
        StrainUnit.MilliStrain,
        PressureUnit.Gigapascal,
        LengthUnit.Meter,
        ForceUnit.Kilonewton,
        ForcePerLengthUnit.KilonewtonPerMeter,
        PressureUnit.KilonewtonPerSquareMeter,
        MomentUnit.KilonewtonMeter,
        TemperatureUnit.DegreeCelsius,
        LengthUnit.Millimeter,
        PressureUnit.Megapascal,
        StrainUnit.MilliStrain,
        AxialStiffnessUnit.Kilonewton,
        BendingStiffnessUnit.KilonewtonSquareMeter,
        SpeedUnit.MeterPerSecond,
        AccelerationUnit.MeterPerSecondSquared,
        EnergyUnit.Megajoule,
        CurvatureUnit.PerMeter,
        SectionModulusUnit.CubicCentimeter);
    }

    internal static UnitSystem SI() {
      return new UnitSystem(
        LengthUnit.Meter,
        AreaUnit.SquareMeter,
        VolumeUnit.CubicMeter,
        AreaMomentOfInertiaUnit.MeterToTheFourth,
        MassUnit.Kilogram,
        DensityUnit.KilogramPerCubicMeter,
        LinearDensityUnit.KilogramPerMeter,
        VolumePerLengthUnit.CubicMeterPerMeter,
        PressureUnit.Pascal,
        StrainUnit.Ratio,
        PressureUnit.Pascal,
        LengthUnit.Meter,
        ForceUnit.Newton,
        ForcePerLengthUnit.NewtonPerMeter,
        PressureUnit.NewtonPerSquareMeter,
        MomentUnit.NewtonMeter,
        TemperatureUnit.Kelvin,
        LengthUnit.Meter,
        PressureUnit.Pascal,
        StrainUnit.Ratio,
        AxialStiffnessUnit.Newton,
        BendingStiffnessUnit.NewtonSquareMeter,
        SpeedUnit.MeterPerSecond,
        AccelerationUnit.MeterPerSecondSquared,
        EnergyUnit.Joule,
        CurvatureUnit.PerMeter,
        SectionModulusUnit.CubicMeter);
    }
  }
}
