using System;
using System.Collections.Generic;
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
  /// Component to create a new UnitNumber
  /// </summary>
  public class CreateUnitNumber : GH_OasysDropDownComponent {
    // This region handles how the component in displayed on the ribbon including name, exposure level and icon
    public override Guid ComponentGuid => new Guid("a6d79db6-844f-4228-b38f-9223762185fb");

    public override GH_Exposure Exposure => GH_Exposure.septenary | GH_Exposure.obscure;
    public override OasysPluginInfo PluginInfo => GH_UnitNumberPluginInfo.Instance;
    protected override System.Drawing.Bitmap Icon => Properties.Resources.CreateUnitNumber;
    private Dictionary<string, Enum> _measureDictionary;
    private IQuantity _quantity;
    private Enum _selectedMeasure;
    private double _val;

    public CreateUnitNumber() : base(
      "Create UnitNumber",
      "CreateUnit",
      "Create a unit number (quantity) from value, unit and measure",
      "Params",
      "Util") {
      Hidden = true; // sets the initial state of the component to hidden
    }

    public override void SetSelected(int i, int j) {
      _selectedItems[i] = _dropDownItems[i][j];

      // if change is made to first (unit type) list we have to update lists
      if (i == 0) {
        var unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), _selectedItems[0]);
        UpdateQuantityUnitTypeFromUnitString(unit);
        UpdateMeasureDictionary();
        _selectedItems[1] = _selectedMeasure.ToString();
      } else // if change is made to the measure of a unit
        {
        _selectedMeasure = _measureDictionary[_selectedItems.Last()];
        UpdateUnitMeasureAndAbbreviation();
      }
      base.UpdateUI();
    }

    public override void VariableParameterMaintenance() {
      var unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), _selectedItems[0]);
      string unitAbbreviation = "";
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

        default:
          throw new Exception("Unable to get abbreviations for unit type " + unit.ToString());
      }

      Params.Input[0].Name = "Number [" + unitAbbreviation + "]";
    }

    protected override void InitialiseDropdowns() {
      _spacerDescriptions = new List<string>(new string[] { "Unit type", "Measure" });

      _dropDownItems = new List<List<string>>();
      _selectedItems = new List<string>();

      _dropDownItems.Add(Enum.GetNames(typeof(EngineeringUnits)).ToList());
      _selectedItems.Add(_dropDownItems[0][1]);

      _dropDownItems.Add(Enum.GetNames(typeof(LengthUnit)).ToList());
      _selectedItems.Add(DefaultUnits.LengthUnitGeometry.ToString());

      _quantity = new Length(0, DefaultUnits.LengthUnitGeometry);
      _selectedMeasure = _quantity.Unit;

      _measureDictionary = new Dictionary<string, Enum>();
      foreach (UnitInfo unit in _quantity.QuantityInfo.UnitInfos)
        _measureDictionary.Add(unit.Name, unit.Value);

      _isInitialised = true;
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager) {
      pManager.AddNumberParameter("Number [unit]", "N", "Number representing the value of selected unit and measure", GH_ParamAccess.item);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddParameter(new GH_UnitNumberParameter());
    }

    protected override void SolveInternal(IGH_DataAccess DA) {
      if (DA.GetData(0, ref _val)) {
        var unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), _selectedItems[0]);

        switch (unit) {
          case EngineeringUnits.Angle:
            _quantity = new Angle(_val, (AngleUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Length:
            _quantity = new Length(_val, (LengthUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Area:
            _quantity = new Area(_val, (AreaUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Volume:
            _quantity = new Volume(_val, (VolumeUnit)_selectedMeasure);
            break;

          case EngineeringUnits.AreaMomentOfInertia:
            _quantity = new AreaMomentOfInertia(_val, (AreaMomentOfInertiaUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Force:
            _quantity = new Force(_val, (ForceUnit)_selectedMeasure);
            break;

          case EngineeringUnits.ForcePerLength:
            _quantity = new ForcePerLength(_val, (ForcePerLengthUnit)_selectedMeasure);
            break;

          case EngineeringUnits.ForcePerArea:
            _quantity = new Pressure(_val, (PressureUnit)_selectedMeasure);
            break;
            ;

          case EngineeringUnits.Moment:
            _quantity = new Moment(_val, (MomentUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Stress:
            _quantity = new Pressure(_val, (PressureUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Strain:
            _quantity = new Strain(_val, (StrainUnit)_selectedMeasure);
            break;

          case EngineeringUnits.AxialStiffness:
            _quantity = new AxialStiffness(_val, (AxialStiffnessUnit)_selectedMeasure);
            break;

          case EngineeringUnits.BendingStiffness:
            _quantity = new BendingStiffness(_val, (BendingStiffnessUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Curvature:
            _quantity = new Curvature(_val, (CurvatureUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Mass:
            _quantity = new Mass(_val, (MassUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Density:
            _quantity = new Density(_val, (DensityUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Temperature:
            _quantity = new Temperature(_val, (TemperatureUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Velocity:
            _quantity = new Speed(_val, (SpeedUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Acceleration:
            _quantity = new Acceleration(_val, (AccelerationUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Energy:
            _quantity = new Energy(_val, (EnergyUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Ratio:
            _quantity = new Ratio(_val, (RatioUnit)_selectedMeasure);
            break;

          case EngineeringUnits.Time:
            _quantity = new Duration(_val, (DurationUnit)_selectedMeasure);
            break;

          default:
            throw new Exception("Unable to get abbreviations for unit type " + unit.ToString());
        }

        var unitNumber = new OasysGH.Parameters.GH_UnitNumber(_quantity);

        DA.SetData(0, unitNumber);
      }
    }

    protected override void UpdateUIFromSelectedItems() {
      var unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), _selectedItems[0]);
      UpdateQuantityUnitTypeFromUnitString(unit);
      UpdateMeasureDictionary();
      UpdateUnitMeasureAndAbbreviation();
      _selectedMeasure = _measureDictionary[_selectedItems.Last()];
      base.UpdateUIFromSelectedItems();
    }

    private void UpdateMeasureDictionary() {
      _measureDictionary = new Dictionary<string, Enum>();
      foreach (UnitInfo unitype in _quantity.QuantityInfo.UnitInfos)
        _measureDictionary.Add(unitype.Name, unitype.Value);
      _dropDownItems[1] = _measureDictionary.Keys.ToList();
    }

    private void UpdateQuantityUnitTypeFromUnitString(EngineeringUnits unit) {
      switch (unit) {
        case EngineeringUnits.Angle:
          _quantity = new Angle(_val, DefaultUnits.AngleUnit);
          break;

        case EngineeringUnits.Length:
          _quantity = new Length(_val, DefaultUnits.LengthUnitGeometry);
          break;

        case EngineeringUnits.Area:
          _quantity = new Area(_val, DefaultUnits.SectionAreaUnit);
          break;

        case EngineeringUnits.Volume:
          _quantity = new Volume(_val, DefaultUnits.SectionVolumeUnit);
          break;

        case EngineeringUnits.AreaMomentOfInertia:
          _quantity = new AreaMomentOfInertia(_val, DefaultUnits.SectionAreaMomentOfInertiaUnit);
          break;

        case EngineeringUnits.Force:
          _quantity = new Force(_val, DefaultUnits.ForceUnit);
          break;

        case EngineeringUnits.ForcePerLength:
          _quantity = new ForcePerLength(_val, DefaultUnits.ForcePerLengthUnit);
          break;

        case EngineeringUnits.ForcePerArea:
          _quantity = new Pressure(_val, DefaultUnits.ForcePerAreaUnit);
          break;
          ;

        case EngineeringUnits.Moment:
          _quantity = new Moment(_val, DefaultUnits.MomentUnit);
          break;

        case EngineeringUnits.Stress:
          _quantity = new Pressure(_val, DefaultUnits.StressUnitResult);
          break;

        case EngineeringUnits.Strain:
          _quantity = new Strain(_val, DefaultUnits.StrainUnitResult);
          break;

        case EngineeringUnits.AxialStiffness:
          _quantity = new AxialStiffness(_val, DefaultUnits.AxialStiffnessUnit);
          break;

        case EngineeringUnits.BendingStiffness:
          _quantity = new BendingStiffness(_val, DefaultUnits.BendingStiffnessUnit);
          break;

        case EngineeringUnits.Curvature:
          _quantity = new Curvature(_val, DefaultUnits.CurvatureUnit);
          break;

        case EngineeringUnits.Mass:
          _quantity = new Mass(_val, DefaultUnits.MassUnit);
          break;

        case EngineeringUnits.Density:
          _quantity = new Density(_val, DefaultUnits.DensityUnit);
          break;

        case EngineeringUnits.Temperature:
          _quantity = new Temperature(_val, DefaultUnits.TemperatureUnit);
          break;

        case EngineeringUnits.Velocity:
          _quantity = new Speed(_val, DefaultUnits.VelocityUnit);
          break;

        case EngineeringUnits.Acceleration:
          _quantity = new Acceleration(_val, DefaultUnits.AccelerationUnit);
          break;

        case EngineeringUnits.Energy:
          _quantity = new Energy(_val, DefaultUnits.EnergyUnit);
          break;

        case EngineeringUnits.Ratio:
          _quantity = new Ratio(_val, DefaultUnits.RatioUnit);
          break;

        case EngineeringUnits.Time:
          _quantity = new Duration(_val, DefaultUnits.TimeMediumUnit);
          break;

        default:
          throw new Exception("Unable to get abbreviations for unit type " + unit.ToString());
      }
      _selectedMeasure = _quantity.Unit;
    }

    private void UpdateUnitMeasureAndAbbreviation() {
      var unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), _selectedItems[0]);

      switch (unit) {
        case EngineeringUnits.Angle:
          _quantity = new Angle(_val, (AngleUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Length:
          _quantity = new Length(_val, (LengthUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Area:
          _quantity = new Area(_val, (AreaUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Volume:
          _quantity = new Volume(_val, (VolumeUnit)_selectedMeasure);
          break;

        case EngineeringUnits.AreaMomentOfInertia:
          _quantity = new AreaMomentOfInertia(_val, (AreaMomentOfInertiaUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Force:
          _quantity = new Force(_val, (ForceUnit)_selectedMeasure);
          break;

        case EngineeringUnits.ForcePerLength:
          _quantity = new ForcePerLength(_val, (ForcePerLengthUnit)_selectedMeasure);
          break;

        case EngineeringUnits.ForcePerArea:
          _quantity = new Pressure(_val, (PressureUnit)_selectedMeasure);
          break;
          ;

        case EngineeringUnits.Moment:
          _quantity = new Moment(_val, (MomentUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Stress:
          _quantity = new Pressure(_val, (PressureUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Strain:
          _quantity = new Strain(_val, (StrainUnit)_selectedMeasure);
          break;

        case EngineeringUnits.AxialStiffness:
          _quantity = new AxialStiffness(_val, (AxialStiffnessUnit)_selectedMeasure);
          break;

        case EngineeringUnits.BendingStiffness:
          _quantity = new BendingStiffness(_val, (BendingStiffnessUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Curvature:
          _quantity = new Curvature(_val, (CurvatureUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Mass:
          _quantity = new Mass(_val, (MassUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Density:
          _quantity = new Density(_val, (DensityUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Temperature:
          _quantity = new Temperature(_val, (TemperatureUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Velocity:
          _quantity = new Speed(_val, (SpeedUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Acceleration:
          _quantity = new Acceleration(_val, (AccelerationUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Energy:
          _quantity = new Energy(_val, (EnergyUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Ratio:
          _quantity = new Ratio(_val, (RatioUnit)_selectedMeasure);
          break;

        case EngineeringUnits.Time:
          _quantity = new Duration(_val, (DurationUnit)_selectedMeasure);
          break;

        default:
          throw new Exception("Unable to get abbreviations for unit type " + unit.ToString());
      }
    }
  }
}
