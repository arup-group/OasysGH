using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OasysGH.Units.Helpers;
using OasysUnits;
using OasysUnits.Units;

namespace OasysGH.Units.UI
{
  public partial class DefaultUnitsForm : Form
  {
    readonly List<string> LengthAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Length);
    readonly List<string> AreaAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Area);
    readonly List<string> VolumeAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Volume);
    readonly List<string> InertiaAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.AreaMomentOfInertia);
    readonly List<string> MassAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Mass);
    readonly List<string> DensityAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Density);
    readonly List<string> LinearDensityAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.LinearDensity);
    readonly List<string> VolPerLengthAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.VolumePerLength);
    readonly List<string> PressureAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Stress);
    readonly List<string> StrainAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Strain);
    readonly List<string> ForceAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Force);
    readonly List<string> ForcePerLengthAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.ForcePerLength);
    readonly List<string> ForcePerAreaAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.ForcePerArea);
    readonly List<string> MomentAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Moment);
    readonly List<string> TemperatureAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Temperature);
    readonly List<string> AxialStffAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.AxialStiffness);
    readonly List<string> BendingStffAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.BendingStiffness);
    readonly List<string> VelocityAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Velocity);
    readonly List<string> AccelerationAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Acceleration);
    readonly List<string> EnergyAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Energy);
    readonly List<string> CurvatureAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Curvature);
    Length TempTolerance = DefaultUnits.Tolerance;

    public DefaultUnitsForm()
    {
      InitializeComponent();

      // Properties
      InitialiseDropdown(lengthSectionComboBox, LengthAbbr, Length.GetAbbreviation(DefaultUnits.LengthUnitSection));
      InitialiseDropdown(areaComboBox, AreaAbbr, Area.GetAbbreviation(DefaultUnits.SectionAreaUnit));
      InitialiseDropdown(volumeComboBox, VolumeAbbr, Volume.GetAbbreviation(DefaultUnits.SectionVolumeUnit));
      InitialiseDropdown(momentOfInertiaComboBox, InertiaAbbr, AreaMomentOfInertia.GetAbbreviation(DefaultUnits.SectionAreaMomentOfInertiaUnit));
      InitialiseDropdown(massComboBox, MassAbbr, Mass.GetAbbreviation(DefaultUnits.MassUnit));
      InitialiseDropdown(densityComboBox, DensityAbbr, Density.GetAbbreviation(DefaultUnits.DensityUnit));
      InitialiseDropdown(linearDensityComboBox, LinearDensityAbbr, LinearDensity.GetAbbreviation(DefaultUnits.LinearDensityUnit));
      InitialiseDropdown(volumePerLengthComboBox, VolPerLengthAbbr, VolumePerLength.GetAbbreviation(DefaultUnits.VolumePerLengthUnit));
      InitialiseDropdown(materialStrengthComboBox, PressureAbbr, Pressure.GetAbbreviation(DefaultUnits.MaterialStrengthUnit));
      InitialiseDropdown(materialStrainComboBox, StrainAbbr, Strain.GetAbbreviation(DefaultUnits.MaterialStrainUnit));
      InitialiseDropdown(youngsModulusComboBox, PressureAbbr, Pressure.GetAbbreviation(DefaultUnits.YoungsModulusUnit));

      // Geometry
      toleranceUpDown.Value = (decimal)TempTolerance.As(Length.ParseUnit(Length.GetAbbreviation(DefaultUnits.LengthUnitGeometry)));
      useRhinoTolerance.Checked = DefaultUnits.UseRhinoTolerance;
      toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(DefaultUnits.LengthUnitGeometry) + "]";

      if (useRhinoTolerance.Checked)
      {
        toleranceUpDown.Value = (decimal)RhinoUnit.GetRhinoTolerance().As(DefaultUnits.LengthUnitGeometry);
        toleranceUpDown.Enabled = false;
        toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(RhinoUnit.GetRhinoLengthUnit()) + "]";
      }
      
      InitialiseDropdown(lengthComboBox, LengthAbbr, Length.GetAbbreviation(DefaultUnits.LengthUnitGeometry));
      lengthComboBox.Enabled = !DefaultUnits.UseRhinoLengthGeometryUnit;
      useRhinoLengthUnit.Checked = DefaultUnits.UseRhinoLengthGeometryUnit;

      // Loads
      InitialiseDropdown(forceComboBox, ForceAbbr, Force.GetAbbreviation(DefaultUnits.ForceUnit));
      InitialiseDropdown(forcePerLengthComboBox, ForcePerLengthAbbr, ForcePerLength.GetAbbreviation(DefaultUnits.ForcePerLengthUnit));
      InitialiseDropdown(forcePerAreaComboBox, ForcePerAreaAbbr, Pressure.GetAbbreviation(DefaultUnits.ForcePerAreaUnit));
      InitialiseDropdown(momentComboBox, MomentAbbr, Moment.GetAbbreviation(DefaultUnits.MomentUnit));
      InitialiseDropdown(temperatureComboBox, TemperatureAbbr, Temperature.GetAbbreviation(DefaultUnits.TemperatureUnit));

      // Results
      InitialiseDropdown(displacementComboBox, LengthAbbr, Length.GetAbbreviation(DefaultUnits.LengthUnitResult));
      InitialiseDropdown(stressComboBox, PressureAbbr, Pressure.GetAbbreviation(DefaultUnits.StressUnitResult));
      InitialiseDropdown(strainComboBox, StrainAbbr, Strain.GetAbbreviation(DefaultUnits.StrainUnitResult));
      InitialiseDropdown(axialStiffnessComboBox, AxialStffAbbr, AxialStiffness.GetAbbreviation(DefaultUnits.AxialStiffnessUnit));
      InitialiseDropdown(bendingStiffnessComboBox, BendingStffAbbr, BendingStiffness.GetAbbreviation(DefaultUnits.BendingStiffnessUnit));
      InitialiseDropdown(velocityComboBox, VelocityAbbr, Speed.GetAbbreviation(DefaultUnits.VelocityUnit));
      InitialiseDropdown(accelerationComboBox, AccelerationAbbr, Acceleration.GetAbbreviation(DefaultUnits.AccelerationUnit));
      InitialiseDropdown(energyComboBox, EnergyAbbr, Energy.GetAbbreviation(DefaultUnits.EnergyUnit));
      InitialiseDropdown(curvatureComboBox, CurvatureAbbr, Curvature.GetAbbreviation(DefaultUnits.CurvatureUnit));
    }

    private void InitialiseDropdown(ComboBox comboBox, List<string> dataSource, string selected)
    {
      comboBox.DataSource = dataSource.ToList();
      comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBox.SelectedIndex = dataSource.IndexOf(selected);
    }


    private void SetSelectedDropdown(ComboBox comboBox, List<string> dataSource, string selected)
    {
      comboBox.SelectedIndex = dataSource.IndexOf(selected);
    }

    private void SetTempTolerance(LengthUnit unit)
    {
      decimal tolerance = (decimal)new Length((double)toleranceUpDown.Value, TempTolerance.Unit).As(unit);

      // check if chosen tolerance is inside min/max values, and reset form to min/max values if outside
      if (tolerance >= toleranceUpDown.Minimum && tolerance <= toleranceUpDown.Maximum)
        toleranceUpDown.Value = tolerance;
      else if (tolerance < toleranceUpDown.Minimum)
        toleranceUpDown.Value = toleranceUpDown.Minimum;
      else
        toleranceUpDown.Value = toleranceUpDown.Maximum;

      TempTolerance = new Length((double)toleranceUpDown.Value, unit);
    }

    private void useRhinoTolerance_CheckedChanged(object sender, EventArgs e)
    {
      if (!useRhinoTolerance.Checked)
      {
        toleranceUpDown.Enabled = true;
        toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(Length.ParseUnit(lengthComboBox.Text)) + "]";
        SetTempTolerance(Length.ParseUnit(lengthComboBox.Text));
      }
      else
      {
        toleranceUpDown.Enabled = false;
        LengthUnit unit = RhinoUnit.GetRhinoLengthUnit();
        toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(unit) + "]";
        toleranceUpDown.Value = (decimal)RhinoUnit.GetRhinoTolerance().As(unit);
        TempTolerance = new Length((double)toleranceUpDown.Value, unit);
      }
    }

    private void lengthComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!useRhinoTolerance.Checked)
      {
        toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(Length.ParseUnit(lengthComboBox.Text)) + "]";

        LengthUnit unit = Length.ParseUnit(lengthComboBox.Text);

        SetTempTolerance(unit);
      }
    }

    private void useRhinoLengthUnit_CheckedChanged(object sender, EventArgs e)
    {
      lengthComboBox.Enabled = !useRhinoLengthUnit.Checked;
      LengthUnit unit = lengthComboBox.Enabled ? DefaultUnits.LengthUnitGeometry : RhinoUnit.GetRhinoLengthUnit();
      SetSelectedDropdown(lengthComboBox, LengthAbbr, Length.GetAbbreviation(unit));
      toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(unit) + "]";
      if (!useRhinoTolerance.Checked)
      {
        toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(Length.ParseUnit(lengthComboBox.Text)) + "]";

        SetTempTolerance(unit);
      }
    }

    private void SI_Click(object sender, EventArgs e)
    {
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

    private void kNm_Click(object sender, EventArgs e)
    {
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

    private void kipft_Click(object sender, EventArgs e)
    {
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

    private void kipin_Click(object sender, EventArgs e)
    {
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

    private void UpdateSelectedFromUnitSystem(UnitSystem unitSystem)
    {
      // Properties
      SetSelectedDropdown(lengthSectionComboBox, LengthAbbr, Length.GetAbbreviation(unitSystem.SectionLengthUnit));
      SetSelectedDropdown(areaComboBox, AreaAbbr, Area.GetAbbreviation(unitSystem.SectionAreaUnit));
      SetSelectedDropdown(volumeComboBox, VolumeAbbr, Volume.GetAbbreviation(unitSystem.SectionVolumeUnit));
      SetSelectedDropdown(momentOfInertiaComboBox, InertiaAbbr, AreaMomentOfInertia.GetAbbreviation(unitSystem.SectionAreaMomentOfInertiaUnit));
      SetSelectedDropdown(massComboBox, MassAbbr, Mass.GetAbbreviation(unitSystem.MassUnit));
      SetSelectedDropdown(densityComboBox, DensityAbbr, Density.GetAbbreviation(unitSystem.DensityUnit));
      SetSelectedDropdown(linearDensityComboBox, LinearDensityAbbr, LinearDensity.GetAbbreviation(unitSystem.LinearDensityUnit));
      SetSelectedDropdown(volumePerLengthComboBox, VolPerLengthAbbr, VolumePerLength.GetAbbreviation(unitSystem.VolumePerLengthUnit));
      SetSelectedDropdown(materialStrengthComboBox, PressureAbbr, Pressure.GetAbbreviation(unitSystem.MaterialStrengthUnit));
      SetSelectedDropdown(materialStrainComboBox, StrainAbbr, Strain.GetAbbreviation(unitSystem.MaterialStrainUnit));
      SetSelectedDropdown(youngsModulusComboBox, PressureAbbr, Pressure.GetAbbreviation(unitSystem.YoungsModulusUnit));

      // Geometry
      useRhinoLengthUnit.Checked = false;
      lengthComboBox.Enabled = true;
      SetSelectedDropdown(lengthComboBox, LengthAbbr, Length.GetAbbreviation(unitSystem.LengthUnit));
      if (!useRhinoTolerance.Checked)
      {
        toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(unitSystem.LengthUnit) + "]";

        SetTempTolerance(unitSystem.LengthUnit);
      }

      // Loads
      SetSelectedDropdown(forceComboBox, ForceAbbr, Force.GetAbbreviation(unitSystem.ForceUnit));
      SetSelectedDropdown(forcePerLengthComboBox, ForcePerLengthAbbr, ForcePerLength.GetAbbreviation(unitSystem.ForcePerLengthUnit));
      SetSelectedDropdown(forcePerAreaComboBox, ForcePerAreaAbbr, Pressure.GetAbbreviation(unitSystem.ForcePerAreaUnit));
      SetSelectedDropdown(momentComboBox, MomentAbbr, Moment.GetAbbreviation(unitSystem.MomentUnit));
      SetSelectedDropdown(temperatureComboBox, TemperatureAbbr, Temperature.GetAbbreviation(unitSystem.TemperatureUnit));

      // Results
      SetSelectedDropdown(displacementComboBox, LengthAbbr, Length.GetAbbreviation(unitSystem.LengthUnitResult));
      SetSelectedDropdown(stressComboBox, PressureAbbr, Pressure.GetAbbreviation(unitSystem.StressUnitResult));
      SetSelectedDropdown(strainComboBox, StrainAbbr, Strain.GetAbbreviation(unitSystem.StrainUnitResult));
      SetSelectedDropdown(axialStiffnessComboBox, AxialStffAbbr, AxialStiffness.GetAbbreviation(unitSystem.AxialStiffnessUnit));
      SetSelectedDropdown(bendingStiffnessComboBox, BendingStffAbbr, BendingStiffness.GetAbbreviation(unitSystem.BendingStiffnessUnit));
      SetSelectedDropdown(velocityComboBox, VelocityAbbr, Speed.GetAbbreviation(unitSystem.VelocityUnit));
      SetSelectedDropdown(accelerationComboBox, AccelerationAbbr, Acceleration.GetAbbreviation(unitSystem.AccelerationUnit));
      SetSelectedDropdown(energyComboBox, EnergyAbbr, Energy.GetAbbreviation(unitSystem.EnergyUnit));
      SetSelectedDropdown(curvatureComboBox, CurvatureAbbr, Curvature.GetAbbreviation(unitSystem.CurvatureUnit));
    }

    private void OK_Click(object sender, EventArgs e)
    {
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

    private void DefaultUnitsForm_Load(object sender, EventArgs e)
    {

    }
  }
}
