using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OasysGH.Units.Helpers;
using OasysUnits;
using OasysUnits.Units;

namespace OasysGH.Units.UI {

  public partial class DefaultUnitsForm : Form {
    private readonly List<string> _accelerationAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Acceleration);
    private readonly List<string> _areaAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Area);
    private readonly List<string> _axialStffAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.AxialStiffness);
    private readonly List<string> _bendingStffAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.BendingStiffness);
    private readonly List<string> _curvatureAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Curvature);
    private readonly List<string> _densityAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Density);
    private readonly List<string> _energyAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Energy);
    private readonly List<string> _forceAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Force);
    private readonly List<string> _forcePerAreaAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.ForcePerArea);
    private readonly List<string> _forcePerLengthAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.ForcePerLength);
    private readonly List<string> _inertiaAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.AreaMomentOfInertia);
    private readonly List<string> _lengthAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Length);
    private readonly List<string> _linearDensityAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.LinearDensity);
    private readonly List<string> _massAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Mass);
    private readonly List<string> _momentAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Moment);
    private readonly List<string> _pressureAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Stress);
    private readonly List<string> _strainAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Strain);
    private readonly List<string> _temperatureAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Temperature);
    private readonly List<string> _velocityAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Velocity);
    private readonly List<string> _volPerLengthAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.VolumePerLength);
    private readonly List<string> _volumeAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Volume);
    private Length _tempTolerance = DefaultUnits.Tolerance;

    public DefaultUnitsForm() {
      InitializeComponent();

      // Properties
      InitialiseDropdown(lengthSectionComboBox, _lengthAbbr, Length.GetAbbreviation(DefaultUnits.LengthUnitSection));
      InitialiseDropdown(areaComboBox, _areaAbbr, Area.GetAbbreviation(DefaultUnits.SectionAreaUnit));
      InitialiseDropdown(volumeComboBox, _volumeAbbr, Volume.GetAbbreviation(DefaultUnits.SectionVolumeUnit));
      InitialiseDropdown(momentOfInertiaComboBox, _inertiaAbbr, AreaMomentOfInertia.GetAbbreviation(DefaultUnits.SectionAreaMomentOfInertiaUnit));
      InitialiseDropdown(massComboBox, _massAbbr, Mass.GetAbbreviation(DefaultUnits.MassUnit));
      InitialiseDropdown(densityComboBox, _densityAbbr, Density.GetAbbreviation(DefaultUnits.DensityUnit));
      InitialiseDropdown(linearDensityComboBox, _linearDensityAbbr, LinearDensity.GetAbbreviation(DefaultUnits.LinearDensityUnit));
      InitialiseDropdown(volumePerLengthComboBox, _volPerLengthAbbr, VolumePerLength.GetAbbreviation(DefaultUnits.VolumePerLengthUnit));
      InitialiseDropdown(materialStrengthComboBox, _pressureAbbr, Pressure.GetAbbreviation(DefaultUnits.MaterialStrengthUnit));
      InitialiseDropdown(materialStrainComboBox, _strainAbbr, Strain.GetAbbreviation(DefaultUnits.MaterialStrainUnit));
      InitialiseDropdown(youngsModulusComboBox, _pressureAbbr, Pressure.GetAbbreviation(DefaultUnits.YoungsModulusUnit));

      // Geometry
      toleranceUpDown.Value = (decimal)_tempTolerance.As(Length.ParseUnit(Length.GetAbbreviation(DefaultUnits.LengthUnitGeometry)));
      useRhinoTolerance.Checked = DefaultUnits.UseRhinoTolerance;
      toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(DefaultUnits.LengthUnitGeometry) + "]";

      if (useRhinoTolerance.Checked) {
        toleranceUpDown.Value = (decimal)RhinoUnit.GetRhinoTolerance().As(DefaultUnits.LengthUnitGeometry);
        toleranceUpDown.Enabled = false;
        toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(RhinoUnit.GetRhinoLengthUnit()) + "]";
      }

      InitialiseDropdown(lengthComboBox, _lengthAbbr, Length.GetAbbreviation(DefaultUnits.LengthUnitGeometry));
      lengthComboBox.Enabled = !DefaultUnits.UseRhinoLengthGeometryUnit;
      useRhinoLengthUnit.Checked = DefaultUnits.UseRhinoLengthGeometryUnit;

      // Loads
      InitialiseDropdown(forceComboBox, _forceAbbr, Force.GetAbbreviation(DefaultUnits.ForceUnit));
      InitialiseDropdown(forcePerLengthComboBox, _forcePerLengthAbbr, ForcePerLength.GetAbbreviation(DefaultUnits.ForcePerLengthUnit));
      InitialiseDropdown(forcePerAreaComboBox, _forcePerAreaAbbr, Pressure.GetAbbreviation(DefaultUnits.ForcePerAreaUnit));
      InitialiseDropdown(momentComboBox, _momentAbbr, Moment.GetAbbreviation(DefaultUnits.MomentUnit));
      InitialiseDropdown(temperatureComboBox, _temperatureAbbr, Temperature.GetAbbreviation(DefaultUnits.TemperatureUnit));

      // Results
      InitialiseDropdown(displacementComboBox, _lengthAbbr, Length.GetAbbreviation(DefaultUnits.LengthUnitResult));
      InitialiseDropdown(stressComboBox, _pressureAbbr, Pressure.GetAbbreviation(DefaultUnits.StressUnitResult));
      InitialiseDropdown(strainComboBox, _strainAbbr, Strain.GetAbbreviation(DefaultUnits.StrainUnitResult));
      InitialiseDropdown(axialStiffnessComboBox, _axialStffAbbr, AxialStiffness.GetAbbreviation(DefaultUnits.AxialStiffnessUnit));
      InitialiseDropdown(bendingStiffnessComboBox, _bendingStffAbbr, BendingStiffness.GetAbbreviation(DefaultUnits.BendingStiffnessUnit));
      InitialiseDropdown(velocityComboBox, _velocityAbbr, Speed.GetAbbreviation(DefaultUnits.VelocityUnit));
      InitialiseDropdown(accelerationComboBox, _accelerationAbbr, Acceleration.GetAbbreviation(DefaultUnits.AccelerationUnit));
      InitialiseDropdown(energyComboBox, _energyAbbr, Energy.GetAbbreviation(DefaultUnits.EnergyUnit));
      InitialiseDropdown(curvatureComboBox, _curvatureAbbr, Curvature.GetAbbreviation(DefaultUnits.CurvatureUnit));
    }

    private void DefaultUnitsForm_Load(object sender, EventArgs e) {
    }

    private void InitialiseDropdown(ComboBox comboBox, List<string> dataSource, string selected) {
      comboBox.DataSource = dataSource.ToList();
      comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBox.SelectedIndex = dataSource.IndexOf(selected);
    }

    private void kipft_Click(object sender, EventArgs e) {
      UpdateSelectedFromUnitSystem(new UnitSystem(
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
        CurvatureUnit.PerInch));
    }

    private void kipin_Click(object sender, EventArgs e) {
      UpdateSelectedFromUnitSystem(new UnitSystem(
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
        CurvatureUnit.PerInch));
    }

    private void kNm_Click(object sender, EventArgs e) {
      UpdateSelectedFromUnitSystem(new UnitSystem(
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
        CurvatureUnit.PerMeter));
    }

    private void lengthComboBox_SelectedIndexChanged(object sender, EventArgs e) {
      if (!useRhinoTolerance.Checked) {
        toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(Length.ParseUnit(lengthComboBox.Text)) + "]";

        LengthUnit unit = Length.ParseUnit(lengthComboBox.Text);

        SetTempTolerance(unit);
      }
    }

    private void OK_Click(object sender, EventArgs e) {
      DefaultUnits.LengthUnitSection = Length.ParseUnit(lengthSectionComboBox.Text);
      DefaultUnits.SectionAreaUnit = Area.ParseUnit(areaComboBox.Text);
      DefaultUnits.SectionVolumeUnit = Volume.ParseUnit(volumeComboBox.Text);
      DefaultUnits.SectionAreaMomentOfInertiaUnit = AreaMomentOfInertia.ParseUnit(momentOfInertiaComboBox.Text);
      DefaultUnits.MassUnit = Mass.ParseUnit(massComboBox.Text);
      DefaultUnits.DensityUnit = Density.ParseUnit(densityComboBox.Text);
      DefaultUnits.LinearDensityUnit = LinearDensity.ParseUnit(linearDensityComboBox.Text);
      DefaultUnits.VolumePerLengthUnit = VolumePerLength.ParseUnit(volumePerLengthComboBox.Text);
      DefaultUnits.MaterialStrengthUnit = Pressure.ParseUnit(materialStrengthComboBox.Text);
      DefaultUnits.MaterialStrainUnit = Strain.ParseUnit(materialStrainComboBox.Text);
      DefaultUnits.YoungsModulusUnit = Pressure.ParseUnit(youngsModulusComboBox.Text);
      DefaultUnits.LengthUnitGeometry = Length.ParseUnit(lengthComboBox.Text);
      DefaultUnits.UseRhinoLengthGeometryUnit = useRhinoLengthUnit.Checked;
      DefaultUnits.ForceUnit = Force.ParseUnit(forceComboBox.Text);
      DefaultUnits.ForcePerLengthUnit = ForcePerLength.ParseUnit(forcePerLengthComboBox.Text);
      DefaultUnits.ForcePerAreaUnit = Pressure.ParseUnit(forcePerAreaComboBox.Text);
      DefaultUnits.MomentUnit = Moment.ParseUnit(momentComboBox.Text);
      DefaultUnits.TemperatureUnit = Temperature.ParseUnit(temperatureComboBox.Text);
      DefaultUnits.LengthUnitResult = Length.ParseUnit(displacementComboBox.Text);
      DefaultUnits.StressUnitResult = Pressure.ParseUnit(stressComboBox.Text);
      DefaultUnits.StrainUnitResult = Strain.ParseUnit(strainComboBox.Text);
      DefaultUnits.AxialStiffnessUnit = AxialStiffness.ParseUnit(axialStiffnessComboBox.Text);
      DefaultUnits.BendingStiffnessUnit = BendingStiffness.ParseUnit(bendingStiffnessComboBox.Text);
      DefaultUnits.VelocityUnit = Speed.ParseUnit(velocityComboBox.Text);
      DefaultUnits.AccelerationUnit = Acceleration.ParseUnit(accelerationComboBox.Text);
      DefaultUnits.EnergyUnit = Energy.ParseUnit(energyComboBox.Text);
      DefaultUnits.CurvatureUnit = Curvature.ParseUnit(curvatureComboBox.Text);
      DefaultUnits.Tolerance = new Length((double)toleranceUpDown.Value, useRhinoTolerance.Checked ? RhinoUnit.GetRhinoLengthUnit() : DefaultUnits.LengthUnitGeometry);
      DefaultUnits.UseRhinoTolerance = useRhinoTolerance.Checked;

      Utility.SaveSettings();
    }

    private void SetSelectedDropdown(ComboBox comboBox, List<string> dataSource, string selected) {
      comboBox.SelectedIndex = dataSource.IndexOf(selected);
    }

    private void SetTempTolerance(LengthUnit unit) {
      decimal tolerance = (decimal)new Length((double)toleranceUpDown.Value, _tempTolerance.Unit).As(unit);

      // check if chosen tolerance is inside min/max values, and reset form to min/max values if outside
      if (tolerance >= toleranceUpDown.Minimum && tolerance <= toleranceUpDown.Maximum)
        toleranceUpDown.Value = tolerance;
      else if (tolerance < toleranceUpDown.Minimum)
        toleranceUpDown.Value = toleranceUpDown.Minimum;
      else
        toleranceUpDown.Value = toleranceUpDown.Maximum;

      _tempTolerance = new Length((double)toleranceUpDown.Value, unit);
    }

    private void SI_Click(object sender, EventArgs e) {
      UpdateSelectedFromUnitSystem(new UnitSystem(
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
        CurvatureUnit.PerMeter));
    }

    private void UpdateSelectedFromUnitSystem(UnitSystem unitSystem) {
      // Properties
      SetSelectedDropdown(lengthSectionComboBox, _lengthAbbr, Length.GetAbbreviation(unitSystem.SectionLengthUnit));
      SetSelectedDropdown(areaComboBox, _areaAbbr, Area.GetAbbreviation(unitSystem.SectionAreaUnit));
      SetSelectedDropdown(volumeComboBox, _volumeAbbr, Volume.GetAbbreviation(unitSystem.SectionVolumeUnit));
      SetSelectedDropdown(momentOfInertiaComboBox, _inertiaAbbr, AreaMomentOfInertia.GetAbbreviation(unitSystem.SectionAreaMomentOfInertiaUnit));
      SetSelectedDropdown(massComboBox, _massAbbr, Mass.GetAbbreviation(unitSystem.MassUnit));
      SetSelectedDropdown(densityComboBox, _densityAbbr, Density.GetAbbreviation(unitSystem.DensityUnit));
      SetSelectedDropdown(linearDensityComboBox, _linearDensityAbbr, LinearDensity.GetAbbreviation(unitSystem.LinearDensityUnit));
      SetSelectedDropdown(volumePerLengthComboBox, _volPerLengthAbbr, VolumePerLength.GetAbbreviation(unitSystem.VolumePerLengthUnit));
      SetSelectedDropdown(materialStrengthComboBox, _pressureAbbr, Pressure.GetAbbreviation(unitSystem.MaterialStrengthUnit));
      SetSelectedDropdown(materialStrainComboBox, _strainAbbr, Strain.GetAbbreviation(unitSystem.MaterialStrainUnit));
      SetSelectedDropdown(youngsModulusComboBox, _pressureAbbr, Pressure.GetAbbreviation(unitSystem.YoungsModulusUnit));

      // Geometry
      useRhinoLengthUnit.Checked = false;
      lengthComboBox.Enabled = true;
      SetSelectedDropdown(lengthComboBox, _lengthAbbr, Length.GetAbbreviation(unitSystem.LengthUnit));
      if (!useRhinoTolerance.Checked) {
        toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(unitSystem.LengthUnit) + "]";

        SetTempTolerance(unitSystem.LengthUnit);
      }

      // Loads
      SetSelectedDropdown(forceComboBox, _forceAbbr, Force.GetAbbreviation(unitSystem.ForceUnit));
      SetSelectedDropdown(forcePerLengthComboBox, _forcePerLengthAbbr, ForcePerLength.GetAbbreviation(unitSystem.ForcePerLengthUnit));
      SetSelectedDropdown(forcePerAreaComboBox, _forcePerAreaAbbr, Pressure.GetAbbreviation(unitSystem.ForcePerAreaUnit));
      SetSelectedDropdown(momentComboBox, _momentAbbr, Moment.GetAbbreviation(unitSystem.MomentUnit));
      SetSelectedDropdown(temperatureComboBox, _temperatureAbbr, Temperature.GetAbbreviation(unitSystem.TemperatureUnit));

      // Results
      SetSelectedDropdown(displacementComboBox, _lengthAbbr, Length.GetAbbreviation(unitSystem.LengthUnitResult));
      SetSelectedDropdown(stressComboBox, _pressureAbbr, Pressure.GetAbbreviation(unitSystem.StressUnitResult));
      SetSelectedDropdown(strainComboBox, _strainAbbr, Strain.GetAbbreviation(unitSystem.StrainUnitResult));
      SetSelectedDropdown(axialStiffnessComboBox, _axialStffAbbr, AxialStiffness.GetAbbreviation(unitSystem.AxialStiffnessUnit));
      SetSelectedDropdown(bendingStiffnessComboBox, _bendingStffAbbr, BendingStiffness.GetAbbreviation(unitSystem.BendingStiffnessUnit));
      SetSelectedDropdown(velocityComboBox, _velocityAbbr, Speed.GetAbbreviation(unitSystem.VelocityUnit));
      SetSelectedDropdown(accelerationComboBox, _accelerationAbbr, Acceleration.GetAbbreviation(unitSystem.AccelerationUnit));
      SetSelectedDropdown(energyComboBox, _energyAbbr, Energy.GetAbbreviation(unitSystem.EnergyUnit));
      SetSelectedDropdown(curvatureComboBox, _curvatureAbbr, Curvature.GetAbbreviation(unitSystem.CurvatureUnit));
    }

    private void useRhinoLengthUnit_CheckedChanged(object sender, EventArgs e) {
      lengthComboBox.Enabled = !useRhinoLengthUnit.Checked;
      LengthUnit unit = lengthComboBox.Enabled ? DefaultUnits.LengthUnitGeometry : RhinoUnit.GetRhinoLengthUnit();
      SetSelectedDropdown(lengthComboBox, _lengthAbbr, Length.GetAbbreviation(unit));
      toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(unit) + "]";
      if (!useRhinoTolerance.Checked) {
        toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(Length.ParseUnit(lengthComboBox.Text)) + "]";

        SetTempTolerance(unit);
      }
    }

    private void useRhinoTolerance_CheckedChanged(object sender, EventArgs e) {
      if (!useRhinoTolerance.Checked) {
        toleranceUpDown.Enabled = true;
        toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(Length.ParseUnit(lengthComboBox.Text)) + "]";
        SetTempTolerance(Length.ParseUnit(lengthComboBox.Text));
      }
      else {
        toleranceUpDown.Enabled = false;
        LengthUnit unit = RhinoUnit.GetRhinoLengthUnit();
        toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(unit) + "]";
        toleranceUpDown.Value = (decimal)RhinoUnit.GetRhinoTolerance().As(unit);
        _tempTolerance = new Length((double)toleranceUpDown.Value, unit);
      }
    }
  }
}
