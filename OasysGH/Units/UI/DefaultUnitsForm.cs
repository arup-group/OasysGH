using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OasysUnits;
using OasysUnits.Units;
using OasysGH.Units.Helpers;


namespace OasysGH.Units.UI
{
  public partial class DefaultUnitsForm : Form
  {
    List<string> LenghtAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Length);
    List<string> AreaAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Area);
    List<string> VolumeAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Volume);
    List<string> InertiaAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.AreaMomentOfInertia);
    List<string> MassAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Mass);
    List<string> DensityAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Density);
    List<string> LinearDensityAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.LinearDensity);
    List<string> VolPerLengthAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.VolumePerLength);
    List<string> PressureAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Stress);
    List<string> StrainAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Strain);
    List<string> ForceAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Force);
    List<string> ForcePerLengthAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.ForcePerLength);
    List<string> ForcePerAreaAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.ForcePerArea);
    List<string> MomentAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Moment);
    List<string> TemperatureAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Temperature);
    List<string> AxialStffAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.AxialStiffness);
    List<string> BendingStffAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.BendingStiffness);
    List<string> VelocityAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Velocity);
    List<string> AccelerationAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Acceleration);
    List<string> EnergyAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Energy);
    List<string> CurvatureAbbr = UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Curvature);
    Length tempTolerance = DefaultUnits.Tolerance;

    public DefaultUnitsForm()
    {
      InitializeComponent();

      // Properties
      InitialiseDropdown(this.lengthSectionComboBox, LenghtAbbr, Length.GetAbbreviation(DefaultUnits.LengthUnitSection));
      InitialiseDropdown(this.areaComboBox, AreaAbbr, Area.GetAbbreviation(DefaultUnits.SectionAreaUnit));
      InitialiseDropdown(this.volumeComboBox, VolumeAbbr, Volume.GetAbbreviation(DefaultUnits.SectionVolumeUnit));
      InitialiseDropdown(this.momentOfInertiaComboBox, InertiaAbbr, AreaMomentOfInertia.GetAbbreviation(DefaultUnits.SectionAreaMomentOfInertiaUnit));
      InitialiseDropdown(this.massComboBox, MassAbbr, Mass.GetAbbreviation(DefaultUnits.MassUnit));
      InitialiseDropdown(this.densityComboBox, DensityAbbr, Density.GetAbbreviation(DefaultUnits.DensityUnit));
      InitialiseDropdown(this.linearDensityComboBox, LinearDensityAbbr, LinearDensity.GetAbbreviation(DefaultUnits.LinearDensityUnit));
      InitialiseDropdown(this.volumePerLengthComboBox, VolPerLengthAbbr, VolumePerLength.GetAbbreviation(DefaultUnits.VolumePerLengthUnit));
      InitialiseDropdown(this.materialStrengthComboBox, PressureAbbr, Pressure.GetAbbreviation(DefaultUnits.MaterialStrengthUnit));
      InitialiseDropdown(this.materialStrainComboBox, StrainAbbr, Strain.GetAbbreviation(DefaultUnits.MaterialStrainUnit));
      InitialiseDropdown(this.youngsModulusComboBox, PressureAbbr, Pressure.GetAbbreviation(DefaultUnits.YoungsModulusUnit));

      // Geometry
      InitialiseDropdown(this.lengthComboBox, LenghtAbbr, Length.GetAbbreviation(DefaultUnits.LengthUnitGeometry));
      this.lengthComboBox.Enabled = !DefaultUnits.UseRhinoLengthGeometryUnit;
      this.useRhinoLengthUnit.Checked = DefaultUnits.UseRhinoLengthGeometryUnit;

      this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(DefaultUnits.LengthUnitGeometry) + "]";

      this.toleranceUpDown.Value = (decimal)tempTolerance.As(Length.ParseUnit(lengthComboBox.Text));
      this.useRhinoTolerance.Checked = DefaultUnits.UseRhinoTolerance;
      if (this.useRhinoTolerance.Checked)
      {
        this.toleranceUpDown.Value = (decimal)RhinoUnit.GetRhinoTolerance().As(DefaultUnits.LengthUnitGeometry);
        this.toleranceUpDown.Enabled = false;
      }

      // Loads
      InitialiseDropdown(this.forceComboBox, ForceAbbr, Force.GetAbbreviation(DefaultUnits.ForceUnit));
      InitialiseDropdown(this.forcePerLengthComboBox, ForcePerLengthAbbr, ForcePerLength.GetAbbreviation(DefaultUnits.ForcePerLengthUnit));
      InitialiseDropdown(this.forcePerAreaComboBox, ForcePerAreaAbbr, Pressure.GetAbbreviation(DefaultUnits.ForcePerAreaUnit));
      InitialiseDropdown(this.momentComboBox, MomentAbbr, Moment.GetAbbreviation(DefaultUnits.MomentUnit));
      InitialiseDropdown(this.temperatureComboBox, TemperatureAbbr, Temperature.GetAbbreviation(DefaultUnits.TemperatureUnit));

      // Results
      InitialiseDropdown(this.displacementComboBox, LenghtAbbr, Length.GetAbbreviation(DefaultUnits.LengthUnitResult));
      InitialiseDropdown(this.stressComboBox, PressureAbbr, Pressure.GetAbbreviation(DefaultUnits.StressUnitResult));
      InitialiseDropdown(this.strainComboBox, StrainAbbr, Strain.GetAbbreviation(DefaultUnits.StrainUnitResult));
      InitialiseDropdown(this.axialStiffnessComboBox, AxialStffAbbr, AxialStiffness.GetAbbreviation(DefaultUnits.AxialStiffnessUnit));
      InitialiseDropdown(this.bendingStiffnessComboBox, BendingStffAbbr, BendingStiffness.GetAbbreviation(DefaultUnits.BendingStiffnessUnit));
      InitialiseDropdown(this.velocityComboBox, VelocityAbbr, Speed.GetAbbreviation(DefaultUnits.VelocityUnit));
      InitialiseDropdown(this.accelerationComboBox, AccelerationAbbr, Acceleration.GetAbbreviation(DefaultUnits.AccelerationUnit));
      InitialiseDropdown(this.energyComboBox, EnergyAbbr, Energy.GetAbbreviation(DefaultUnits.EnergyUnit));
      InitialiseDropdown(this.curvatureComboBox, CurvatureAbbr, Curvature.GetAbbreviation(DefaultUnits.CurvatureUnit));
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

    private void useRhinoTolerance_CheckedChanged(object sender, EventArgs e)
    {
      toleranceUpDown.Enabled = !useRhinoTolerance.Checked;
      LengthUnit unit = useRhinoTolerance.Checked ? RhinoUnit.GetRhinoLengthUnit() : Length.ParseUnit(lengthComboBox.Text);
      this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(unit) + "]";

      if (!useRhinoTolerance.Checked)
      {
        this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(Length.ParseUnit(lengthComboBox.Text)) + "]";
        tempTolerance = new Length((double)this.toleranceUpDown.Value, tempTolerance.Unit);
        decimal newTolerance = (decimal)tempTolerance.As(unit);
        if ((newTolerance >= this.toleranceUpDown.Minimum)
          && (newTolerance <= this.toleranceUpDown.Maximum))
          this.toleranceUpDown.Value = newTolerance;
        else if (newTolerance < this.toleranceUpDown.Minimum)
          this.toleranceUpDown.Value = this.toleranceUpDown.Minimum;
        else
          this.toleranceUpDown.Value = this.toleranceUpDown.Maximum;
        tempTolerance = new Length((double)this.toleranceUpDown.Value, unit);
      }
      else
      {
        this.toleranceUpDown.Value = (decimal)RhinoUnit.GetRhinoTolerance().As(unit);
        tempTolerance = new Length((double)this.toleranceUpDown.Value, unit);
      }
    }
    private void lengthComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!useRhinoTolerance.Checked)
      {
        this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(Length.ParseUnit(lengthComboBox.Text)) + "]";
        tempTolerance = new Length((double)this.toleranceUpDown.Value, tempTolerance.Unit);
        LengthUnit unit = Length.ParseUnit(lengthComboBox.Text);
        decimal newTolerance = (decimal)tempTolerance.As(unit);
        if ((newTolerance >= this.toleranceUpDown.Minimum)
          && (newTolerance <= this.toleranceUpDown.Maximum))
          this.toleranceUpDown.Value = newTolerance;
        else if (newTolerance < this.toleranceUpDown.Minimum)
          this.toleranceUpDown.Value = this.toleranceUpDown.Minimum;
        else
          this.toleranceUpDown.Value = this.toleranceUpDown.Maximum;
        tempTolerance = new Length((double)this.toleranceUpDown.Value, unit);
      }
    }

    private void useRhinoLengthUnit_CheckedChanged(object sender, EventArgs e)
    {
      lengthComboBox.Enabled = !useRhinoLengthUnit.Checked;
      LengthUnit unit = lengthComboBox.Enabled ? DefaultUnits.LengthUnitGeometry : RhinoUnit.GetRhinoLengthUnit();
      SetSelectedDropdown(this.lengthComboBox, LenghtAbbr, Length.GetAbbreviation(unit));
      this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(unit) + "]";
      if (!useRhinoTolerance.Checked)
      {
        this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(Length.ParseUnit(lengthComboBox.Text)) + "]";
        tempTolerance = new Length((double)this.toleranceUpDown.Value, tempTolerance.Unit);
        decimal newTolerance = (decimal)tempTolerance.As(unit);
        if ((newTolerance >= this.toleranceUpDown.Minimum)
          && (newTolerance <= this.toleranceUpDown.Maximum))
          this.toleranceUpDown.Value = newTolerance;
        else if (newTolerance < this.toleranceUpDown.Minimum)
          this.toleranceUpDown.Value = this.toleranceUpDown.Minimum;
        else
          this.toleranceUpDown.Value = this.toleranceUpDown.Maximum;
        tempTolerance = new Length((double)this.toleranceUpDown.Value, unit);
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

    private void UpdateSelectedFromUnitSystem(UnitSystem unit)
    {
      // Properties
      SetSelectedDropdown(this.lengthSectionComboBox, LenghtAbbr, Length.GetAbbreviation(unit.SectionLengthUnit));
      SetSelectedDropdown(this.areaComboBox, AreaAbbr, Area.GetAbbreviation(unit.SectionAreaUnit));
      SetSelectedDropdown(this.volumeComboBox, VolumeAbbr, Volume.GetAbbreviation(unit.SectionVolumeUnit));
      SetSelectedDropdown(this.momentOfInertiaComboBox, InertiaAbbr, AreaMomentOfInertia.GetAbbreviation(unit.SectionAreaMomentOfInertiaUnit));
      string abbMass = Mass.GetAbbreviation(unit.MassUnit);
      if (abbMass.StartsWith("lb"))
        abbMass = abbMass.Insert(2, "m");
      SetSelectedDropdown(this.massComboBox, MassAbbr, abbMass);
      string abbDensity = Density.GetAbbreviation(unit.DensityUnit);
      if (abbDensity.StartsWith("lb"))
        abbDensity = abbDensity.Insert(2, "m");
      SetSelectedDropdown(this.densityComboBox, DensityAbbr, abbDensity);
      string abbLinearDensity = LinearDensity.GetAbbreviation(unit.LinearDensityUnit);
      if (abbLinearDensity.StartsWith("lb"))
        abbLinearDensity = abbLinearDensity.Insert(2, "m");
      SetSelectedDropdown(this.linearDensityComboBox, LinearDensityAbbr, abbLinearDensity);
      SetSelectedDropdown(this.volumePerLengthComboBox, VolPerLengthAbbr, VolumePerLength.GetAbbreviation(unit.VolumePerLengthUnit));
      SetSelectedDropdown(this.materialStrengthComboBox, PressureAbbr, Pressure.GetAbbreviation(unit.MaterialStrengthUnit));
      SetSelectedDropdown(this.materialStrainComboBox, StrainAbbr, Strain.GetAbbreviation(unit.MaterialStrainUnit));
      SetSelectedDropdown(this.youngsModulusComboBox, PressureAbbr, Pressure.GetAbbreviation(unit.YoungsModulusUnit));

      // Geometry
      this.useRhinoLengthUnit.Checked = false;
      this.lengthComboBox.Enabled = true;
      SetSelectedDropdown(this.lengthComboBox, LenghtAbbr, Length.GetAbbreviation(unit.LengthUnit));
      if (!this.useRhinoTolerance.Checked)
      {
        this.toleranceTxt.Text = "Tolerance [" + Length.GetAbbreviation(unit.LengthUnit) + "]";
        tempTolerance = new Length((double)this.toleranceUpDown.Value, tempTolerance.Unit);
        decimal newTolerance = (decimal)tempTolerance.As(unit.LengthUnit);
        if ((newTolerance >= this.toleranceUpDown.Minimum)
          && (newTolerance <= this.toleranceUpDown.Maximum))
          this.toleranceUpDown.Value = newTolerance;
        else if (newTolerance < this.toleranceUpDown.Minimum)
          this.toleranceUpDown.Value = this.toleranceUpDown.Minimum;
        else
          this.toleranceUpDown.Value = this.toleranceUpDown.Maximum;
        tempTolerance = new Length((double)this.toleranceUpDown.Value, unit.LengthUnit);
      }

      // Loads
      SetSelectedDropdown(this.forceComboBox, ForceAbbr, Force.GetAbbreviation(unit.ForceUnit));
      SetSelectedDropdown(this.forcePerLengthComboBox, ForcePerLengthAbbr, ForcePerLength.GetAbbreviation(unit.ForcePerLengthUnit));
      SetSelectedDropdown(this.forcePerAreaComboBox, ForcePerAreaAbbr, Pressure.GetAbbreviation(unit.ForcePerAreaUnit));
      SetSelectedDropdown(this.momentComboBox, MomentAbbr, Moment.GetAbbreviation(unit.MomentUnit));
      SetSelectedDropdown(this.temperatureComboBox, TemperatureAbbr, Temperature.GetAbbreviation(unit.TemperatureUnit));

      // Results
      SetSelectedDropdown(this.displacementComboBox, LenghtAbbr, Length.GetAbbreviation(unit.LengthUnitResult));
      SetSelectedDropdown(this.stressComboBox, PressureAbbr, Pressure.GetAbbreviation(unit.StressUnitResult));
      SetSelectedDropdown(this.strainComboBox, StrainAbbr, Strain.GetAbbreviation(unit.StrainUnitResult));
      SetSelectedDropdown(this.axialStiffnessComboBox, AxialStffAbbr, AxialStiffness.GetAbbreviation(unit.AxialStiffnessUnit));
      SetSelectedDropdown(this.bendingStiffnessComboBox, BendingStffAbbr, BendingStiffness.GetAbbreviation(unit.BendingStiffnessUnit));
      SetSelectedDropdown(this.velocityComboBox, VelocityAbbr, Speed.GetAbbreviation(unit.VelocityUnit));
      SetSelectedDropdown(this.accelerationComboBox, AccelerationAbbr, Acceleration.GetAbbreviation(unit.AccelerationUnit));
      SetSelectedDropdown(this.energyComboBox, EnergyAbbr, Energy.GetAbbreviation(unit.EnergyUnit));
      SetSelectedDropdown(this.curvatureComboBox, CurvatureAbbr, Curvature.GetAbbreviation(unit.CurvatureUnit));
    }
    private void OK_Click(object sender, EventArgs e)
    {
      DefaultUnits.LengthUnitSection = Length.ParseUnit(lengthSectionComboBox.Text);
      DefaultUnits.SectionAreaUnit = Area.ParseUnit(areaComboBox.Text);
      DefaultUnits.SectionVolumeUnit = Volume.ParseUnit(volumeComboBox.Text);
      DefaultUnits.SectionAreaMomentOfInertiaUnit = AreaMomentOfInertia.ParseUnit(momentOfInertiaComboBox.Text);
      DefaultUnits.MassUnit = Mass.ParseUnit(massComboBox.Text);
      string abbDensity = densityComboBox.Text;
      if (abbDensity.StartsWith("lbm"))
        abbDensity = abbDensity.Remove(2, 1);
      DefaultUnits.DensityUnit = Density.ParseUnit(abbDensity);
      string abbLinearDensity = linearDensityComboBox.Text;
      if (abbLinearDensity.StartsWith("lbm"))
        abbLinearDensity = abbLinearDensity.Remove(2, 1);
      DefaultUnits.LinearDensityUnit = LinearDensity.ParseUnit(abbLinearDensity);
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
  internal class UnitSystem
  {
    internal LengthUnit SectionLengthUnit { get; }
    internal AreaUnit SectionAreaUnit { get; }
    internal VolumeUnit SectionVolumeUnit { get; }
    internal AreaMomentOfInertiaUnit SectionAreaMomentOfInertiaUnit { get; }
    internal MassUnit MassUnit { get; }
    internal DensityUnit DensityUnit { get; }
    internal LinearDensityUnit LinearDensityUnit { get; }
    internal VolumePerLengthUnit VolumePerLengthUnit { get; }
    internal PressureUnit MaterialStrengthUnit { get; }
    internal StrainUnit MaterialStrainUnit { get; }
    internal PressureUnit YoungsModulusUnit { get; }

    internal LengthUnit LengthUnit { get; }

    internal ForceUnit ForceUnit { get; }
    internal ForcePerLengthUnit ForcePerLengthUnit { get; }
    internal PressureUnit ForcePerAreaUnit { get; }
    internal MomentUnit MomentUnit { get; }
    internal TemperatureUnit TemperatureUnit { get; }

    internal LengthUnit LengthUnitResult { get; }
    internal PressureUnit StressUnitResult { get; }
    internal StrainUnit StrainUnitResult { get; }
    internal AxialStiffnessUnit AxialStiffnessUnit { get; }
    internal BendingStiffnessUnit BendingStiffnessUnit { get; }
    internal SpeedUnit VelocityUnit { get; }
    internal AccelerationUnit AccelerationUnit { get; }
    internal EnergyUnit EnergyUnit { get; }
    internal CurvatureUnit CurvatureUnit { get; }

    internal UnitSystem(LengthUnit sectionLengthUnit, AreaUnit areaUnit, VolumeUnit volumeUnit, AreaMomentOfInertiaUnit areaMomentOfInertiaUnit, MassUnit massUnit, DensityUnit densityUnit, LinearDensityUnit linearDensityUnit, VolumePerLengthUnit volumePerLengthUnit, PressureUnit materialStrengthUnit, StrainUnit materialStrainUnit, PressureUnit youngsModulusUnit, LengthUnit lengthUnit, ForceUnit forceUnit, ForcePerLengthUnit forcePerLengthUnit, PressureUnit forcePerAreaUnit, MomentUnit momentUnit, TemperatureUnit temperatureUnit, LengthUnit displacementUuit, PressureUnit stressUnit, StrainUnit strainUnit, AxialStiffnessUnit axialStiffnessUnit, BendingStiffnessUnit bendingStiffnessUnit, SpeedUnit velocityUnit, AccelerationUnit accelerationUnit, EnergyUnit energyUnit, CurvatureUnit curvatureUnit)
    {
      SectionLengthUnit = sectionLengthUnit;
      SectionAreaUnit = areaUnit;
      SectionVolumeUnit = volumeUnit;
      SectionAreaMomentOfInertiaUnit = areaMomentOfInertiaUnit;
      MassUnit = massUnit;
      DensityUnit = densityUnit;
      LinearDensityUnit = linearDensityUnit;
      VolumePerLengthUnit = volumePerLengthUnit;
      MaterialStrengthUnit = materialStrengthUnit;
      MaterialStrainUnit = materialStrainUnit;
      YoungsModulusUnit = youngsModulusUnit;
      LengthUnit = lengthUnit;
      ForceUnit = forceUnit;
      ForcePerLengthUnit = forcePerLengthUnit;
      ForcePerAreaUnit = forcePerAreaUnit;
      MomentUnit = momentUnit;
      TemperatureUnit = temperatureUnit;
      LengthUnitResult = displacementUuit;
      StressUnitResult = stressUnit;
      StrainUnitResult = strainUnit;
      AxialStiffnessUnit = axialStiffnessUnit;
      BendingStiffnessUnit = bendingStiffnessUnit;
      VelocityUnit = velocityUnit;
      AccelerationUnit = accelerationUnit;
      EnergyUnit = energyUnit;
      CurvatureUnit = curvatureUnit;
    }
  }
}
