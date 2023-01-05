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
      InitialiseDropdown(this.lengthSectionComboBox, this.LengthAbbr, Length.GetAbbreviation(DefaultUnits.LengthUnitSection));
      InitialiseDropdown(this.areaComboBox, this.AreaAbbr, Area.GetAbbreviation(DefaultUnits.SectionAreaUnit));
      InitialiseDropdown(this.volumeComboBox, this.VolumeAbbr, Volume.GetAbbreviation(DefaultUnits.SectionVolumeUnit));
      InitialiseDropdown(this.momentOfInertiaComboBox, this.InertiaAbbr, AreaMomentOfInertia.GetAbbreviation(DefaultUnits.SectionAreaMomentOfInertiaUnit));
      InitialiseDropdown(this.massComboBox, this.MassAbbr, Mass.GetAbbreviation(DefaultUnits.MassUnit));
      InitialiseDropdown(this.densityComboBox, this.DensityAbbr, Density.GetAbbreviation(DefaultUnits.DensityUnit));
      InitialiseDropdown(this.linearDensityComboBox, this.LinearDensityAbbr, LinearDensity.GetAbbreviation(DefaultUnits.LinearDensityUnit));
      InitialiseDropdown(this.volumePerLengthComboBox, this.VolPerLengthAbbr, VolumePerLength.GetAbbreviation(DefaultUnits.VolumePerLengthUnit));
      InitialiseDropdown(this.materialStrengthComboBox, this.PressureAbbr, Pressure.GetAbbreviation(DefaultUnits.MaterialStrengthUnit));
      InitialiseDropdown(this.materialStrainComboBox, this.StrainAbbr, Strain.GetAbbreviation(DefaultUnits.MaterialStrainUnit));
      InitialiseDropdown(this.youngsModulusComboBox, this.PressureAbbr, Pressure.GetAbbreviation(DefaultUnits.YoungsModulusUnit));

      // Geometry
      InitialiseDropdown(this.lengthComboBox, this.LengthAbbr, Length.GetAbbreviation(DefaultUnits.LengthUnitGeometry));
      this.lengthComboBox.Enabled = !DefaultUnits.UseRhinoLengthGeometryUnit;
      this.useRhinoLengthUnit.Checked = DefaultUnits.UseRhinoLengthGeometryUnit;

      this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(DefaultUnits.LengthUnitGeometry) + "]";

      this.toleranceUpDown.Value = (decimal)TempTolerance.As(Length.ParseUnit(lengthComboBox.Text));
      this.useRhinoTolerance.Checked = DefaultUnits.UseRhinoTolerance;
      if (this.useRhinoTolerance.Checked)
      {
        this.toleranceUpDown.Value = (decimal)RhinoUnit.GetRhinoTolerance().As(DefaultUnits.LengthUnitGeometry);
        this.toleranceUpDown.Enabled = false;
      }

      // Loads
      InitialiseDropdown(this.forceComboBox, this.ForceAbbr, Force.GetAbbreviation(DefaultUnits.ForceUnit));
      InitialiseDropdown(this.forcePerLengthComboBox, this.ForcePerLengthAbbr, ForcePerLength.GetAbbreviation(DefaultUnits.ForcePerLengthUnit));
      InitialiseDropdown(this.forcePerAreaComboBox, this.ForcePerAreaAbbr, Pressure.GetAbbreviation(DefaultUnits.ForcePerAreaUnit));
      InitialiseDropdown(this.momentComboBox, this.MomentAbbr, Moment.GetAbbreviation(DefaultUnits.MomentUnit));
      InitialiseDropdown(this.temperatureComboBox, this.TemperatureAbbr, Temperature.GetAbbreviation(DefaultUnits.TemperatureUnit));

      // Results
      InitialiseDropdown(this.displacementComboBox, this.LengthAbbr, Length.GetAbbreviation(DefaultUnits.LengthUnitResult));
      InitialiseDropdown(this.stressComboBox, this.PressureAbbr, Pressure.GetAbbreviation(DefaultUnits.StressUnitResult));
      InitialiseDropdown(this.strainComboBox, this.StrainAbbr, Strain.GetAbbreviation(DefaultUnits.StrainUnitResult));
      InitialiseDropdown(this.axialStiffnessComboBox, this.AxialStffAbbr, AxialStiffness.GetAbbreviation(DefaultUnits.AxialStiffnessUnit));
      InitialiseDropdown(this.bendingStiffnessComboBox, this.BendingStffAbbr, BendingStiffness.GetAbbreviation(DefaultUnits.BendingStiffnessUnit));
      InitialiseDropdown(this.velocityComboBox, this.VelocityAbbr, Speed.GetAbbreviation(DefaultUnits.VelocityUnit));
      InitialiseDropdown(this.accelerationComboBox, this.AccelerationAbbr, Acceleration.GetAbbreviation(DefaultUnits.AccelerationUnit));
      InitialiseDropdown(this.energyComboBox, this.EnergyAbbr, Energy.GetAbbreviation(DefaultUnits.EnergyUnit));
      InitialiseDropdown(this.curvatureComboBox, this.CurvatureAbbr, Curvature.GetAbbreviation(DefaultUnits.CurvatureUnit));
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
      decimal tolerance = (decimal)new Length((double)this.toleranceUpDown.Value, this.TempTolerance.Unit).As(unit);

      // check if chosen tolerance is inside min/max values, and reset form to min/max values if outside
      if (tolerance >= this.toleranceUpDown.Minimum && tolerance <= this.toleranceUpDown.Maximum)
        this.toleranceUpDown.Value = tolerance;
      else if (tolerance < this.toleranceUpDown.Minimum)
        this.toleranceUpDown.Value = this.toleranceUpDown.Minimum;
      else
        this.toleranceUpDown.Value = this.toleranceUpDown.Maximum;

      this.TempTolerance = new Length((double)this.toleranceUpDown.Value, unit);
    }

    private void useRhinoTolerance_CheckedChanged(object sender, EventArgs e)
    {
      toleranceUpDown.Enabled = !useRhinoTolerance.Checked;
      LengthUnit unit = useRhinoTolerance.Checked ? RhinoUnit.GetRhinoLengthUnit() : Length.ParseUnit(lengthComboBox.Text);
      this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(unit) + "]";

      if (!useRhinoTolerance.Checked)
      {
        this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(Length.ParseUnit(lengthComboBox.Text)) + "]";

        this.SetTempTolerance(unit);
      }
      else
      {
        this.toleranceUpDown.Value = (decimal)RhinoUnit.GetRhinoTolerance().As(unit);
        this.TempTolerance = new Length((double)this.toleranceUpDown.Value, unit);
      }
    }

    private void lengthComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!useRhinoTolerance.Checked)
      {
        this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(Length.ParseUnit(lengthComboBox.Text)) + "]";

        LengthUnit unit = Length.ParseUnit(lengthComboBox.Text);

        this.SetTempTolerance(unit);
      }
    }

    private void useRhinoLengthUnit_CheckedChanged(object sender, EventArgs e)
    {
      lengthComboBox.Enabled = !useRhinoLengthUnit.Checked;
      LengthUnit unit = lengthComboBox.Enabled ? DefaultUnits.LengthUnitGeometry : RhinoUnit.GetRhinoLengthUnit();
      SetSelectedDropdown(this.lengthComboBox, LengthAbbr, Length.GetAbbreviation(unit));
      this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(unit) + "]";
      if (!useRhinoTolerance.Checked)
      {
        this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(Length.ParseUnit(lengthComboBox.Text)) + "]";

        this.SetTempTolerance(unit);
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
      this.SetSelectedDropdown(this.lengthSectionComboBox, LengthAbbr, Length.GetAbbreviation(unitSystem.SectionLengthUnit));
      this.SetSelectedDropdown(this.areaComboBox, AreaAbbr, Area.GetAbbreviation(unitSystem.SectionAreaUnit));
      this.SetSelectedDropdown(this.volumeComboBox, VolumeAbbr, Volume.GetAbbreviation(unitSystem.SectionVolumeUnit));
      this.SetSelectedDropdown(this.momentOfInertiaComboBox, InertiaAbbr, AreaMomentOfInertia.GetAbbreviation(unitSystem.SectionAreaMomentOfInertiaUnit));
      this.SetSelectedDropdown(this.massComboBox, MassAbbr, Mass.GetAbbreviation(unitSystem.MassUnit));
      this.SetSelectedDropdown(this.densityComboBox, DensityAbbr, Density.GetAbbreviation(unitSystem.DensityUnit));
      this.SetSelectedDropdown(this.linearDensityComboBox, LinearDensityAbbr, LinearDensity.GetAbbreviation(unitSystem.LinearDensityUnit));
      this.SetSelectedDropdown(this.volumePerLengthComboBox, VolPerLengthAbbr, VolumePerLength.GetAbbreviation(unitSystem.VolumePerLengthUnit));
      this.SetSelectedDropdown(this.materialStrengthComboBox, PressureAbbr, Pressure.GetAbbreviation(unitSystem.MaterialStrengthUnit));
      this.SetSelectedDropdown(this.materialStrainComboBox, StrainAbbr, Strain.GetAbbreviation(unitSystem.MaterialStrainUnit));
      this.SetSelectedDropdown(this.youngsModulusComboBox, PressureAbbr, Pressure.GetAbbreviation(unitSystem.YoungsModulusUnit));

      // Geometry
      this.useRhinoLengthUnit.Checked = false;
      this.lengthComboBox.Enabled = true;
      this.SetSelectedDropdown(this.lengthComboBox, LengthAbbr, Length.GetAbbreviation(unitSystem.LengthUnit));
      if (!this.useRhinoTolerance.Checked)
      {
        this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(unitSystem.LengthUnit) + "]";

        this.SetTempTolerance(unitSystem.LengthUnit);
      }

      // Loads
      this.SetSelectedDropdown(this.forceComboBox, ForceAbbr, Force.GetAbbreviation(unitSystem.ForceUnit));
      this.SetSelectedDropdown(this.forcePerLengthComboBox, ForcePerLengthAbbr, ForcePerLength.GetAbbreviation(unitSystem.ForcePerLengthUnit));
      this.SetSelectedDropdown(this.forcePerAreaComboBox, ForcePerAreaAbbr, Pressure.GetAbbreviation(unitSystem.ForcePerAreaUnit));
      this.SetSelectedDropdown(this.momentComboBox, MomentAbbr, Moment.GetAbbreviation(unitSystem.MomentUnit));
      this.SetSelectedDropdown(this.temperatureComboBox, TemperatureAbbr, Temperature.GetAbbreviation(unitSystem.TemperatureUnit));

      // Results
      this.SetSelectedDropdown(this.displacementComboBox, LengthAbbr, Length.GetAbbreviation(unitSystem.LengthUnitResult));
      this.SetSelectedDropdown(this.stressComboBox, PressureAbbr, Pressure.GetAbbreviation(unitSystem.StressUnitResult));
      this.SetSelectedDropdown(this.strainComboBox, StrainAbbr, Strain.GetAbbreviation(unitSystem.StrainUnitResult));
      this.SetSelectedDropdown(this.axialStiffnessComboBox, AxialStffAbbr, AxialStiffness.GetAbbreviation(unitSystem.AxialStiffnessUnit));
      this.SetSelectedDropdown(this.bendingStiffnessComboBox, BendingStffAbbr, BendingStiffness.GetAbbreviation(unitSystem.BendingStiffnessUnit));
      this.SetSelectedDropdown(this.velocityComboBox, VelocityAbbr, Speed.GetAbbreviation(unitSystem.VelocityUnit));
      this.SetSelectedDropdown(this.accelerationComboBox, AccelerationAbbr, Acceleration.GetAbbreviation(unitSystem.AccelerationUnit));
      this.SetSelectedDropdown(this.energyComboBox, EnergyAbbr, Energy.GetAbbreviation(unitSystem.EnergyUnit));
      this.SetSelectedDropdown(this.curvatureComboBox, CurvatureAbbr, Curvature.GetAbbreviation(unitSystem.CurvatureUnit));
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
      DefaultUnits.Tolerance = new Length((double)this.toleranceUpDown.Value, useRhinoTolerance.Checked ? RhinoUnit.GetRhinoLengthUnit() : DefaultUnits.LengthUnitGeometry);
      DefaultUnits.UseRhinoTolerance = useRhinoTolerance.Checked;

      Utility.SaveSettings();
    }

    private void DefaultUnitsForm_Load(object sender, EventArgs e)
    {

    }
  }
}
