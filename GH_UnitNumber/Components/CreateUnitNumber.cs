using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel;
using UnitsNet;
using OasysGH.Components;
using OasysGH.Units;
using UnitsNet.Units;
using Oasys.Units;
using System.IO;

namespace GH_UnitNumber.Components
{
  /// <summary>
  /// Component to create a new UnitNumber
  /// </summary>
  public class CreateUnitNumber : GH_OasysDropDownComponent
  {
    #region Name and Ribbon Layout
    // This region handles how the component in displayed on the ribbon
    // including name, exposure level and icon
    public override Guid ComponentGuid => new Guid("a6d79db6-844f-4228-b38f-9223762185fb");
    public CreateUnitNumber()
      : base("Create UnitNumber", "CreateUnit", "Create a unit number (quantity) from value, unit and measure",
            "Params",
            "Util")
    { this.Hidden = true; } // sets the initial state of the component to hidden
    public override GH_Exposure Exposure => GH_Exposure.septenary | GH_Exposure.obscure;

    protected override System.Drawing.Bitmap Icon => Properties.Resources.CreateUnitNumber;
    #endregion

    #region Input and output
    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddNumberParameter("Number [unit]", "N", "Number representing the value of selected unit and measure", GH_ParamAccess.item);
    }
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddGenericParameter("UnitNumber", "UN", "Number converted to selected unit", GH_ParamAccess.item);
    }
    #endregion

    protected override void SolveInstance(IGH_DataAccess DA)
    {
      if (DA.GetData(0, ref Val))
      {
        EngineeringUnits unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), this.SelectedItems[0]);

        switch (unit)
        {
          case EngineeringUnits.Angle:
            this.Quantity = new Angle(Val, (AngleUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Length:
            this.Quantity = new Length(Val, (LengthUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Area:
            this.Quantity = new Area(Val, (AreaUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Volume:
            this.Quantity = new Volume(Val, (VolumeUnit)SelectedMeasure);
            break;

          case EngineeringUnits.AreaMomentOfInertia:
            this.Quantity = new AreaMomentOfInertia(Val, (AreaMomentOfInertiaUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Force:
            this.Quantity = new Force(Val, (ForceUnit)SelectedMeasure);
            break;

          case EngineeringUnits.ForcePerLength:
            this.Quantity = new ForcePerLength(Val, (ForcePerLengthUnit)SelectedMeasure);
            break;

          case EngineeringUnits.ForcePerArea:
            this.Quantity = new Pressure(Val, (PressureUnit)SelectedMeasure);
            break; ;

          case EngineeringUnits.Moment:
            this.Quantity = new Moment(Val, (MomentUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Stress:
            this.Quantity = new Pressure(Val, (PressureUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Strain:
            this.Quantity = new Strain(Val, (StrainUnit)SelectedMeasure);
            break;

          case EngineeringUnits.AxialStiffness:
            this.Quantity = new AxialStiffness(Val, (AxialStiffnessUnit)SelectedMeasure);
            break;

          case EngineeringUnits.BendingStiffness:
            this.Quantity = new BendingStiffness(Val, (BendingStiffnessUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Curvature:
            this.Quantity = new Curvature(Val, (CurvatureUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Mass:
            this.Quantity = new Mass(Val, (MassUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Density:
            this.Quantity = new Density(Val, (DensityUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Temperature:
            this.Quantity = new Temperature(Val, (TemperatureUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Velocity:
            this.Quantity = new Speed(Val, (SpeedUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Acceleration:
            this.Quantity = new Acceleration(Val, (AccelerationUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Energy:
            this.Quantity = new Energy(Val, (EnergyUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Ratio:
            this.Quantity = new Ratio(Val, (RatioUnit)SelectedMeasure);
            break;

          case EngineeringUnits.Time:
            this.Quantity = new Duration(Val, (DurationUnit)SelectedMeasure);
            break;

          default:
            throw new Exception("Unable to get abbreviations for unit type " + unit.ToString());
        }

        OasysGH.Units.GH_UnitNumber unitNumber = new OasysGH.Units.GH_UnitNumber(this.Quantity);

        OasysGH.Helpers.Output.SetItem(this, DA, 0, unitNumber);
      }
    }

    #region Custom UI
    Dictionary<string, Enum> MeasureDictionary;
    Enum SelectedMeasure;
    IQuantity Quantity;
    double Val;

    public override void InitialiseDropdowns()
    {
      this.SpacerDescriptions = new List<string>(new string[] { "Unit type", "Measure" });

      this.DropDownItems = new List<List<string>>();
      this.SelectedItems = new List<string>();

      this.DropDownItems.Add(Enum.GetNames(typeof(EngineeringUnits)).ToList());
      this.SelectedItems.Add(this.DropDownItems[0][1]);

      this.DropDownItems.Add(Enum.GetNames(typeof(LengthUnit)).ToList());
      this.SelectedItems.Add(DefaultUnits.LengthUnitGeometry.ToString());

      this.Quantity = new Length(0, DefaultUnits.LengthUnitGeometry);
      this.SelectedMeasure = this.Quantity.Unit;

      this.MeasureDictionary = new Dictionary<string, Enum>();
      foreach (UnitInfo unit in Quantity.QuantityInfo.UnitInfos)
        this.MeasureDictionary.Add(unit.Name, unit.Value);

      this.IsInitialised = true;
    }

    public override void SetSelected(int i, int j)
    {
      this.SelectedItems[i] = this.DropDownItems[i][j];

      // if change is made to first (unit type) list we have to update lists
      if (i == 0)
      {
        EngineeringUnits unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), this.SelectedItems[0]);
        UpdateQuantityUnitTypeFromUnitString(unit);
        UpdateMeasureDictionary();
        this.SelectedItems[1] = this.SelectedMeasure.ToString();
      }
      else // if change is made to the measure of a unit
      {
        this.SelectedMeasure = this.MeasureDictionary[this.SelectedItems.Last()];
        UpdateUnitMeasureAndAbbreviation();
      }
      base.UpdateUI();
    }

    private void UpdateUnitMeasureAndAbbreviation()
    {
      EngineeringUnits unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), this.SelectedItems[0]);

      switch (unit)
      {
        case EngineeringUnits.Angle:
          this.Quantity = new Angle(Val, (AngleUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Length:
          this.Quantity = new Length(Val, (LengthUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Area:
          this.Quantity = new Area(Val, (AreaUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Volume:
          this.Quantity = new Volume(Val, (VolumeUnit)SelectedMeasure);
          break;

        case EngineeringUnits.AreaMomentOfInertia:
          this.Quantity = new AreaMomentOfInertia(Val, (AreaMomentOfInertiaUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Force:
          this.Quantity = new Force(Val, (ForceUnit)SelectedMeasure);
          break;

        case EngineeringUnits.ForcePerLength:
          this.Quantity = new ForcePerLength(Val, (ForcePerLengthUnit)SelectedMeasure);
          break;

        case EngineeringUnits.ForcePerArea:
          this.Quantity = new Pressure(Val, (PressureUnit)SelectedMeasure);
          break; ;

        case EngineeringUnits.Moment:
          this.Quantity = new Moment(Val, (MomentUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Stress:
          this.Quantity = new Pressure(Val, (PressureUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Strain:
          this.Quantity = new Strain(Val, (StrainUnit)SelectedMeasure);
          break;

        case EngineeringUnits.AxialStiffness:
          this.Quantity = new AxialStiffness(Val, (AxialStiffnessUnit)SelectedMeasure);
          break;

        case EngineeringUnits.BendingStiffness:
          this.Quantity = new BendingStiffness(Val, (BendingStiffnessUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Curvature:
          this.Quantity = new Curvature(Val, (CurvatureUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Mass:
          this.Quantity = new Mass(Val, (MassUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Density:
          this.Quantity = new Density(Val, (DensityUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Temperature:
          this.Quantity = new Temperature(Val, (TemperatureUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Velocity:
          this.Quantity = new Speed(Val, (SpeedUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Acceleration:
          this.Quantity = new Acceleration(Val, (AccelerationUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Energy:
          this.Quantity = new Energy(Val, (EnergyUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Ratio:
          this.Quantity = new Ratio(Val, (RatioUnit)SelectedMeasure);
          break;

        case EngineeringUnits.Time:
          this.Quantity = new Duration(Val, (DurationUnit)SelectedMeasure);
          break;

        default:
          throw new Exception("Unable to get abbreviations for unit type " + unit.ToString());
      }
    }

    private void UpdateQuantityUnitTypeFromUnitString(EngineeringUnits unit)
    {
      switch (unit)
      {
        case EngineeringUnits.Angle:
          this.Quantity = new Angle(Val, DefaultUnits.AngleUnit);
          break;

        case EngineeringUnits.Length:
          this.Quantity = new Length(Val, DefaultUnits.LengthUnitGeometry);
          break;

        case EngineeringUnits.Area:
          this.Quantity = new Area(Val, DefaultUnits.SectionAreaUnit);
          break;

        case EngineeringUnits.Volume:
          this.Quantity = new Volume(Val, DefaultUnits.VolumeUnit);
          break;

        case EngineeringUnits.AreaMomentOfInertia:
          this.Quantity = new AreaMomentOfInertia(Val, DefaultUnits.SectionAreaMomentOfInertiaUnit);
          break;

        case EngineeringUnits.Force:
          this.Quantity = new Force(Val, DefaultUnits.ForceUnit);
          break;

        case EngineeringUnits.ForcePerLength:
          this.Quantity = new ForcePerLength(Val, DefaultUnits.ForcePerLengthUnit);
          break;

        case EngineeringUnits.ForcePerArea:
          this.Quantity = new Pressure(Val, DefaultUnits.ForcePerAreaUnit);
          break; ;

        case EngineeringUnits.Moment:
          this.Quantity = new Moment(Val, DefaultUnits.MomentUnit);
          break;

        case EngineeringUnits.Stress:
          this.Quantity = new Pressure(Val, DefaultUnits.StressUnit);
          break;

        case EngineeringUnits.Strain:
          this.Quantity = new Strain(Val, DefaultUnits.StrainUnit);
          break;

        case EngineeringUnits.AxialStiffness:
          this.Quantity = new AxialStiffness(Val, DefaultUnits.AxialStiffnessUnit);
          break;

        case EngineeringUnits.BendingStiffness:
          this.Quantity = new BendingStiffness(Val, DefaultUnits.BendingStiffnessUnit);
          break;

        case EngineeringUnits.Curvature:
          this.Quantity = new Curvature(Val, DefaultUnits.CurvatureUnit);
          break;

        case EngineeringUnits.Mass:
          this.Quantity = new Mass(Val, DefaultUnits.MassUnit);
          break;

        case EngineeringUnits.Density:
          this.Quantity = new Density(Val, DefaultUnits.DensityUnit);
          break;

        case EngineeringUnits.Temperature:
          this.Quantity = new Temperature(Val, DefaultUnits.TemperatureUnit);
          break;

        case EngineeringUnits.Velocity:
          this.Quantity = new Speed(Val, DefaultUnits.VelocityUnit);
          break;

        case EngineeringUnits.Acceleration:
          this.Quantity = new Acceleration(Val, DefaultUnits.AccelerationUnit);
          break;

        case EngineeringUnits.Energy:
          this.Quantity = new Energy(Val, DefaultUnits.EnergyUnit);
          break;

        case EngineeringUnits.Ratio:
          this.Quantity = new Ratio(Val, DefaultUnits.RatioUnit);
          break;

        case EngineeringUnits.Time:
          this.Quantity = new Duration(Val, DefaultUnits.TimeMediumUnit);
          break;

        default:
          throw new Exception("Unable to get abbreviations for unit type " + unit.ToString());
      }
      SelectedMeasure = Quantity.Unit;
    }

    private void UpdateMeasureDictionary()
    {
      this.MeasureDictionary = new Dictionary<string, Enum>();
      foreach (UnitInfo unitype in Quantity.QuantityInfo.UnitInfos)
        this.MeasureDictionary.Add(unitype.Name, unitype.Value);
      this.DropDownItems[1] = this.MeasureDictionary.Keys.ToList();
    }

    public override void UpdateUIFromSelectedItems()
    {
      EngineeringUnits unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), this.SelectedItems[0]);
      UpdateQuantityUnitTypeFromUnitString(unit);
      UpdateMeasureDictionary();
      UpdateUnitMeasureAndAbbreviation();
      SelectedMeasure = MeasureDictionary[this.SelectedItems.Last()];
      base.UpdateUIFromSelectedItems();
    }

    public override void VariableParameterMaintenance()
    {
      EngineeringUnits unit = (EngineeringUnits)Enum.Parse(typeof(EngineeringUnits), this.SelectedItems[0]);
      string unitAbbreviation = "";
      switch (unit)
      {
        case EngineeringUnits.Angle:
          unitAbbreviation = Angle.GetAbbreviation((AngleUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Length:
          unitAbbreviation = Length.GetAbbreviation((LengthUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Area:
          unitAbbreviation = Area.GetAbbreviation((AreaUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Volume:
          unitAbbreviation = Volume.GetAbbreviation((VolumeUnit)Quantity.Unit);
          break;

        case EngineeringUnits.AreaMomentOfInertia:
          unitAbbreviation = AreaMomentOfInertia.GetAbbreviation((AreaMomentOfInertiaUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Force:
          unitAbbreviation = Force.GetAbbreviation((ForceUnit)Quantity.Unit);
          break;

        case EngineeringUnits.ForcePerLength:
          unitAbbreviation = ForcePerLength.GetAbbreviation((ForcePerLengthUnit)Quantity.Unit);
          break;

        case EngineeringUnits.ForcePerArea:
          unitAbbreviation = Pressure.GetAbbreviation((PressureUnit)Quantity.Unit);
          break; ;

        case EngineeringUnits.Moment:
          unitAbbreviation = Moment.GetAbbreviation((MomentUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Stress:
          unitAbbreviation = Pressure.GetAbbreviation((PressureUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Strain:
          unitAbbreviation = Strain.GetAbbreviation((StrainUnit)Quantity.Unit);
          break;

        case EngineeringUnits.AxialStiffness:
          unitAbbreviation = AxialStiffness.GetAbbreviation((AxialStiffnessUnit)Quantity.Unit);
          break;

        case EngineeringUnits.BendingStiffness:
          unitAbbreviation = BendingStiffness.GetAbbreviation((BendingStiffnessUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Curvature:
          unitAbbreviation = Curvature.GetAbbreviation((CurvatureUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Mass:
          unitAbbreviation = Mass.GetAbbreviation((MassUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Density:
          unitAbbreviation = Density.GetAbbreviation((DensityUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Temperature:
          unitAbbreviation = Temperature.GetAbbreviation((TemperatureUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Velocity:
          unitAbbreviation = Speed.GetAbbreviation((SpeedUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Acceleration:
          unitAbbreviation = Acceleration.GetAbbreviation((AccelerationUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Energy:
          unitAbbreviation = Energy.GetAbbreviation((EnergyUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Ratio:
          unitAbbreviation = Ratio.GetAbbreviation((RatioUnit)Quantity.Unit);
          break;

        case EngineeringUnits.Time:
          unitAbbreviation = Duration.GetAbbreviation((DurationUnit)Quantity.Unit);
          break;

        default:
          throw new Exception("Unable to get abbreviations for unit type " + unit.ToString());
      }
      
      Params.Input[0].Name = "Number [" + unitAbbreviation + "]";
    }
    #endregion
  }
}
