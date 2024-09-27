using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Grasshopper.Kernel;
using OasysGH;
using OasysGH.Components;
using OasysGH.Units;
using OasysGH.Units.Helpers;
using OasysUnits;
using OasysUnits.Units;

namespace GH_UnitNumber.Components {
  /// <summary>
  /// Component to create a new <see cref="OasysGH.Parameters.GH_UnitNumber"/>
  /// </summary>
  public class CreateUnitNumber : GH_OasysDropDownComponent {
    public override Guid ComponentGuid => new Guid("a6d79db6-844f-4228-b38f-9223762185fb");
    public override GH_Exposure Exposure => GH_Exposure.septenary | GH_Exposure.obscure;
    public override OasysPluginInfo PluginInfo => GH_UnitNumberPluginInfo.Instance;
    protected override Bitmap Icon => Properties.Resources.CreateUnitNumber;
    private Dictionary<string, Enum> _measureDictionary;
    private IQuantity _quantity;
    private Enum _selectedMeasure;
    private double _value;

    public CreateUnitNumber() : base("Create UnitNumber", "CreateUnit",
      "Create a unit number (quantity) from value, unit and measure", "Params", "Util") {
      Hidden = true;
    }

    public override void SetSelected(int i, int j) {
      SelectedItems[i] = DropDownItems[i][j];

      // if change is made to first (unit type) list we have to update lists
      if (i == 0) {
        var unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), SelectedItems[0]);
        UpdateQuantityUnitTypeFromUnitString(unit);
        UpdateMeasureDictionary();
        SelectedItems[1] = _selectedMeasure.ToString();
      } else // if change is made to the measure of a unit
        {
        _selectedMeasure = _measureDictionary[SelectedItems.Last()];
        UpdateUnitMeasureAndAbbreviation();
      }

      base.UpdateUI();
    }

    public override void VariableParameterMaintenance() {
      var unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), SelectedItems[0]);
      string unitAbbreviation;
      switch (unit) {
        case EngineeringUnits.Angle:
          unitAbbreviation = Angle.GetAbbreviation((AngleUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Length:
          unitAbbreviation = Length.GetAbbreviation((LengthUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Area:
          unitAbbreviation = Area.GetAbbreviation((AreaUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Volume:
          unitAbbreviation = Volume.GetAbbreviation((VolumeUnit)_selectedMeasure);
          break;

        case EngineeringUnits.AreaMomentOfInertia:
          unitAbbreviation = AreaMomentOfInertia.GetAbbreviation((AreaMomentOfInertiaUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Force:
          unitAbbreviation = Force.GetAbbreviation((ForceUnit)_selectedMeasure);
          break;

        case EngineeringUnits.ForcePerLength:
          unitAbbreviation = ForcePerLength.GetAbbreviation((ForcePerLengthUnit)_selectedMeasure);
          break;

        case EngineeringUnits.ForcePerArea:
          unitAbbreviation = Pressure.GetAbbreviation((PressureUnit)_selectedMeasure);
          break;
          ;

        case EngineeringUnits.Moment:
          unitAbbreviation = Moment.GetAbbreviation((MomentUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Stress:
          unitAbbreviation = Pressure.GetAbbreviation((PressureUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Strain:
          unitAbbreviation = Strain.GetAbbreviation((StrainUnit)_selectedMeasure);
          break;

        case EngineeringUnits.AxialStiffness:
          unitAbbreviation = AxialStiffness.GetAbbreviation((AxialStiffnessUnit)_selectedMeasure);
          break;

        case EngineeringUnits.BendingStiffness:
          unitAbbreviation = BendingStiffness.GetAbbreviation((BendingStiffnessUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Curvature:
          unitAbbreviation = Curvature.GetAbbreviation((CurvatureUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Mass:
          unitAbbreviation = Mass.GetAbbreviation((MassUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Density:
          unitAbbreviation = Density.GetAbbreviation((DensityUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Temperature:
          unitAbbreviation = Temperature.GetAbbreviation((TemperatureUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Velocity:
          unitAbbreviation = Speed.GetAbbreviation((SpeedUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Acceleration:
          unitAbbreviation = Acceleration.GetAbbreviation((AccelerationUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Energy:
          unitAbbreviation = Energy.GetAbbreviation((EnergyUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Ratio:
          unitAbbreviation = Ratio.GetAbbreviation((RatioUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Time:
          unitAbbreviation = Duration.GetAbbreviation((DurationUnit)_selectedMeasure);
          break;

        case EngineeringUnits.LinearDensity:
          unitAbbreviation = LinearDensity.GetAbbreviation((LinearDensityUnit)_selectedMeasure);
          break;

        case EngineeringUnits.VolumePerLength:
          unitAbbreviation = VolumePerLength.GetAbbreviation((VolumePerLengthUnit)_selectedMeasure);
          break;

        case EngineeringUnits.SectionModulus:
          unitAbbreviation = SectionModulus.GetAbbreviation((SectionModulusUnit)_selectedMeasure);
          break;

        case EngineeringUnits.RotationalStiffness:
          unitAbbreviation = RotationalStiffness.GetAbbreviation((RotationalStiffnessUnit)_selectedMeasure);
          break;

        default:
          throw new Exception("Unable to get abbreviations for unit type " + unit.ToString());
      }

      Params.Input[0].Name = "Number [" + unitAbbreviation + "]";
    }

    protected override void InitialiseDropdowns() {
      SpacerDescriptions = new List<string>(new string[] { "Unit type", "Measure" });

      DropDownItems = new List<List<string>>();
      SelectedItems = new List<string>();

      DropDownItems.Add(Enum.GetNames(typeof(EngineeringUnits)).ToList());
      SelectedItems.Add(DropDownItems[0][1]);

      DropDownItems.Add(Enum.GetNames(typeof(LengthUnit)).ToList());
      SelectedItems.Add(DefaultUnits.LengthUnitGeometry.ToString());

      _quantity = new Length(0, DefaultUnits.LengthUnitGeometry);
      _selectedMeasure = _quantity.Unit;

      _measureDictionary = new Dictionary<string, Enum>();
      foreach (UnitInfo unit in _quantity.QuantityInfo.UnitInfos)
        _measureDictionary.Add(unit.Name, unit.Value);

      IsInitialised = true;
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager) {
      pManager.AddNumberParameter("Number [unit]", "N", "Number representing the value of selected unit and measure", GH_ParamAccess.item);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddParameter(new GH_UnitNumberParameter());
    }

    protected override void SolveInternal(IGH_DataAccess DA) {
      if (DA.GetData(0, ref _value)) {
        var unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), SelectedItems[0]);

        switch (unit) {
          case EngineeringUnits.Angle:
            _quantity = new Angle(_value, (AngleUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Length:
            _quantity = new Length(_value, (LengthUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Area:
            _quantity = new Area(_value, (AreaUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Volume:
            _quantity = new Volume(_value, (VolumeUnit)_selectedMeasure);
            break;

          case EngineeringUnits.AreaMomentOfInertia:
            _quantity = new AreaMomentOfInertia(_value, (AreaMomentOfInertiaUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Force:
            _quantity = new Force(_value, (ForceUnit)_selectedMeasure);
            break;

          case EngineeringUnits.ForcePerLength:
            _quantity = new ForcePerLength(_value, (ForcePerLengthUnit)_selectedMeasure);
            break;

          case EngineeringUnits.ForcePerArea:
            _quantity = new Pressure(_value, (PressureUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Moment:
            _quantity = new Moment(_value, (MomentUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Stress:
            _quantity = new Pressure(_value, (PressureUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Strain:
            _quantity = new Strain(_value, (StrainUnit)_selectedMeasure);
            break;

          case EngineeringUnits.AxialStiffness:
            _quantity = new AxialStiffness(_value, (AxialStiffnessUnit)_selectedMeasure);
            break;

          case EngineeringUnits.BendingStiffness:
            _quantity = new BendingStiffness(_value, (BendingStiffnessUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Curvature:
            _quantity = new Curvature(_value, (CurvatureUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Mass:
            _quantity = new Mass(_value, (MassUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Density:
            _quantity = new Density(_value, (DensityUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Temperature:
            _quantity = new Temperature(_value, (TemperatureUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Velocity:
            _quantity = new Speed(_value, (SpeedUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Acceleration:
            _quantity = new Acceleration(_value, (AccelerationUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Energy:
            _quantity = new Energy(_value, (EnergyUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Ratio:
            _quantity = new Ratio(_value, (RatioUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Time:
            _quantity = new Duration(_value, (DurationUnit)_selectedMeasure);
            break;

          case EngineeringUnits.LinearDensity:
            _quantity = new LinearDensity(_value, (LinearDensityUnit)_selectedMeasure);
            break;

          case EngineeringUnits.VolumePerLength:
            _quantity = new VolumePerLength(_value, (VolumePerLengthUnit)_selectedMeasure);
            break;

          case EngineeringUnits.SectionModulus:
            _quantity = new SectionModulus(_value, (SectionModulusUnit)_selectedMeasure);
            break;

          case EngineeringUnits.RotationalStiffness:
            _quantity = new RotationalStiffness(_value, (RotationalStiffnessUnit)_selectedMeasure);
            break;

          default:
            throw new Exception("Unable to get abbreviations for unit type " + unit.ToString());
        }


        var unitNumber = new OasysGH.Parameters.GH_UnitNumber(_quantity);

        DA.SetData(0, unitNumber);
      }
    }

    protected override void UpdateUIFromSelectedItems() {
      var unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), SelectedItems[0]);
      UpdateQuantityUnitTypeFromUnitString(unit);
      UpdateMeasureDictionary();
      UpdateUnitMeasureAndAbbreviation();
      _selectedMeasure = _measureDictionary[SelectedItems.Last()];
      base.UpdateUIFromSelectedItems();
    }

    private void UpdateMeasureDictionary() {
      _measureDictionary = new Dictionary<string, Enum>();
      foreach (UnitInfo unitype in _quantity.QuantityInfo.UnitInfos)
        _measureDictionary.Add(unitype.Name, unitype.Value);
      DropDownItems[1] = _measureDictionary.Keys.ToList();
    }

    private void UpdateQuantityUnitTypeFromUnitString(EngineeringUnits unit) {
      switch (unit) {
        case EngineeringUnits.Angle:
          _quantity = new Angle(_value, DefaultUnits.AngleUnit);
          break;

        case EngineeringUnits.Length:
          _quantity = new Length(_value, DefaultUnits.LengthUnitGeometry);
          break;

        case EngineeringUnits.Area:
          _quantity = new Area(_value, DefaultUnits.SectionAreaUnit);
          break;

        case EngineeringUnits.Volume:
          _quantity = new Volume(_value, DefaultUnits.SectionVolumeUnit);
          break;

        case EngineeringUnits.AreaMomentOfInertia:
          _quantity = new AreaMomentOfInertia(_value, DefaultUnits.SectionAreaMomentOfInertiaUnit);
          break;

        case EngineeringUnits.Force:
          _quantity = new Force(_value, DefaultUnits.ForceUnit);
          break;

        case EngineeringUnits.ForcePerLength:
          _quantity = new ForcePerLength(_value, DefaultUnits.ForcePerLengthUnit);
          break;

        case EngineeringUnits.ForcePerArea:
          _quantity = new Pressure(_value, DefaultUnits.ForcePerAreaUnit);
          break;
          ;

        case EngineeringUnits.Moment:
          _quantity = new Moment(_value, DefaultUnits.MomentUnit);
          break;

        case EngineeringUnits.Stress:
          _quantity = new Pressure(_value, DefaultUnits.StressUnitResult);
          break;

        case EngineeringUnits.Strain:
          _quantity = new Strain(_value, DefaultUnits.StrainUnitResult);
          break;

        case EngineeringUnits.AxialStiffness:
          _quantity = new AxialStiffness(_value, DefaultUnits.AxialStiffnessUnit);
          break;

        case EngineeringUnits.BendingStiffness:
          _quantity = new BendingStiffness(_value, DefaultUnits.BendingStiffnessUnit);
          break;

        case EngineeringUnits.Curvature:
          _quantity = new Curvature(_value, DefaultUnits.CurvatureUnit);
          break;

        case EngineeringUnits.Mass:
          _quantity = new Mass(_value, DefaultUnits.MassUnit);
          break;

        case EngineeringUnits.Density:
          _quantity = new Density(_value, DefaultUnits.DensityUnit);
          break;

        case EngineeringUnits.Temperature:
          _quantity = new Temperature(_value, DefaultUnits.TemperatureUnit);
          break;

        case EngineeringUnits.Velocity:
          _quantity = new Speed(_value, DefaultUnits.VelocityUnit);
          break;

        case EngineeringUnits.Acceleration:
          _quantity = new Acceleration(_value, DefaultUnits.AccelerationUnit);
          break;

        case EngineeringUnits.Energy:
          _quantity = new Energy(_value, DefaultUnits.EnergyUnit);
          break;

        case EngineeringUnits.Ratio:
          _quantity = new Ratio(_value, DefaultUnits.RatioUnit);
          break;

        case EngineeringUnits.Time:
          _quantity = new Duration(_value, DefaultUnits.TimeMediumUnit);
          break;

        case EngineeringUnits.LinearDensity:
          _quantity = new LinearDensity(_value, DefaultUnits.LinearDensityUnit);
          break;

        case EngineeringUnits.VolumePerLength:
          _quantity = new VolumePerLength(_value, DefaultUnits.VolumePerLengthUnit);
          break;

        case EngineeringUnits.SectionModulus:
          _quantity = new SectionModulus(_value, DefaultUnits.SectionModulusUnit);
          break;

        case EngineeringUnits.RotationalStiffness:
          _quantity = new RotationalStiffness(_value, DefaultUnits.RotationalStiffnessUnit);
          break;

        default:
          throw new Exception("Unable to get abbreviations for unit type " + unit.ToString());
      }
      _selectedMeasure = _quantity.Unit;
    }

    private void UpdateUnitMeasureAndAbbreviation() {
      var unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), SelectedItems[0]);

      switch (unit) {
        case EngineeringUnits.Angle:
          _quantity = new Angle(_value, (AngleUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Length:
          _quantity = new Length(_value, (LengthUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Area:
          _quantity = new Area(_value, (AreaUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Volume:
          _quantity = new Volume(_value, (VolumeUnit)_selectedMeasure);
          break;

        case EngineeringUnits.AreaMomentOfInertia:
          _quantity = new AreaMomentOfInertia(_value, (AreaMomentOfInertiaUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Force:
          _quantity = new Force(_value, (ForceUnit)_selectedMeasure);
          break;

        case EngineeringUnits.ForcePerLength:
          _quantity = new ForcePerLength(_value, (ForcePerLengthUnit)_selectedMeasure);
          break;

        case EngineeringUnits.ForcePerArea:
          _quantity = new Pressure(_value, (PressureUnit)_selectedMeasure);
          break;
          ;

        case EngineeringUnits.Moment:
          _quantity = new Moment(_value, (MomentUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Stress:
          _quantity = new Pressure(_value, (PressureUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Strain:
          _quantity = new Strain(_value, (StrainUnit)_selectedMeasure);
          break;

        case EngineeringUnits.AxialStiffness:
          _quantity = new AxialStiffness(_value, (AxialStiffnessUnit)_selectedMeasure);
          break;

        case EngineeringUnits.BendingStiffness:
          _quantity = new BendingStiffness(_value, (BendingStiffnessUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Curvature:
          _quantity = new Curvature(_value, (CurvatureUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Mass:
          _quantity = new Mass(_value, (MassUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Density:
          _quantity = new Density(_value, (DensityUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Temperature:
          _quantity = new Temperature(_value, (TemperatureUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Velocity:
          _quantity = new Speed(_value, (SpeedUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Acceleration:
          _quantity = new Acceleration(_value, (AccelerationUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Energy:
          _quantity = new Energy(_value, (EnergyUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Ratio:
          _quantity = new Ratio(_value, (RatioUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Time:
          _quantity = new Duration(_value, (DurationUnit)_selectedMeasure);
          break;

        case EngineeringUnits.LinearDensity:
          _quantity = new LinearDensity(_value, (LinearDensityUnit)_selectedMeasure);
          break;

        case EngineeringUnits.VolumePerLength:
          _quantity = new VolumePerLength(_value, (VolumePerLengthUnit)_selectedMeasure);
          break;

        case EngineeringUnits.SectionModulus:
          _quantity = new SectionModulus(_value, (SectionModulusUnit)_selectedMeasure);
          break;

        case EngineeringUnits.RotationalStiffness:
          _quantity = new RotationalStiffness(_value, (RotationalStiffnessUnit)_selectedMeasure);
          break;

        default:
          throw new Exception("Unable to get abbreviations for unit type " + unit.ToString());
      }
    }
  }
}
