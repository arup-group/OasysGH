﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet.Units;
using UnitsNet;
using OasysGH.Units;
using Oasys.Units;

namespace OasysGH.Units.Helpers
{
  public class UnitsHelper
  {
    private static BaseUnits SI = UnitSystem.SI.BaseUnits;
    public static int SignificantDigits
    {
      get
      {
        return BitConverter.GetBytes(
          decimal.GetBits(
            (decimal)DefaultUnits.Tolerance.As(DefaultUnits.LengthUnitGeometry))[3])[2];
      }
    }

    public static AreaUnit GetAreaUnit(LengthUnit unit)
    {
      switch (unit)
      {
        case LengthUnit.Millimeter:
          return AreaUnit.SquareMillimeter;
        case LengthUnit.Centimeter:
          return AreaUnit.SquareCentimeter;
        case LengthUnit.Meter:
          return AreaUnit.SquareMeter;
        case LengthUnit.Foot:
          return AreaUnit.SquareFoot;
        case LengthUnit.Inch:
          return AreaUnit.SquareInch;
      }
      // fallback:
      BaseUnits baseUnits = new BaseUnits(unit, SI.Mass, SI.Time, SI.Current, SI.Temperature, SI.Amount, SI.LuminousIntensity);
      UnitSystem unitSystem = new UnitSystem(baseUnits);
      return new Area(1, unitSystem).Unit;
    }

    public static VolumeUnit GetVolumeUnit(LengthUnit unit)
    {
      switch (unit)
      {
        case LengthUnit.Millimeter:
          return VolumeUnit.CubicMillimeter;
        case LengthUnit.Centimeter:
          return VolumeUnit.CubicCentimeter;
        case LengthUnit.Meter:
          return VolumeUnit.CubicMeter;
        case LengthUnit.Foot:
          return VolumeUnit.CubicFoot;
        case LengthUnit.Inch:
          return VolumeUnit.CubicInch;
      }
      // fallback:
      BaseUnits baseUnits = new BaseUnits(unit, SI.Mass, SI.Time, SI.Current, SI.Temperature, SI.Amount, SI.LuminousIntensity);
      UnitSystem unitSystem = new UnitSystem(baseUnits);
      return new Volume(1, unitSystem).Unit;
    }

    public static AreaMomentOfInertiaUnit GetAreaMomentOfInertiaUnit(LengthUnit unit)
    {
      switch (unit)
      {
        case LengthUnit.Millimeter:
          return AreaMomentOfInertiaUnit.MillimeterToTheFourth;
        case LengthUnit.Centimeter:
          return AreaMomentOfInertiaUnit.CentimeterToTheFourth;
        case LengthUnit.Meter:
          return AreaMomentOfInertiaUnit.MeterToTheFourth;
        case LengthUnit.Foot:
          return AreaMomentOfInertiaUnit.FootToTheFourth;
        case LengthUnit.Inch:
          return AreaMomentOfInertiaUnit.InchToTheFourth;
      }
      // fallback:
      BaseUnits baseUnits = new BaseUnits(unit, SI.Mass, SI.Time, SI.Current, SI.Temperature, SI.Amount, SI.LuminousIntensity);
      UnitsNet.UnitSystem unitSystem = new UnitsNet.UnitSystem(baseUnits);
      return new AreaMomentOfInertia(1, unitSystem).Unit;
    }

    public static VolumePerLengthUnit GetVolumePerLengthUnit(LengthUnit unit)
    {
      switch (unit)
      {
        case LengthUnit.Millimeter:
        case LengthUnit.Centimeter:
        case LengthUnit.Meter:
          return VolumePerLengthUnit.CubicMeterPerMeter;
        case LengthUnit.Foot:
        case LengthUnit.Inch:
          return VolumePerLengthUnit.CubicYardPerFoot;
      }
      // fallback:
      BaseUnits baseUnits = new BaseUnits(unit, SI.Mass, SI.Time, SI.Current, SI.Temperature, SI.Amount, SI.LuminousIntensity);
      UnitsNet.UnitSystem unitSystem = new UnitsNet.UnitSystem(baseUnits);
      return new VolumePerLength(1, unitSystem).Unit;
    }

    public static ForcePerLengthUnit GetForcePerLengthUnit(ForceUnit forceUnit, LengthUnit lengthUnit)
    {
      switch (forceUnit)
      {
        case ForceUnit.Newton:
          switch (lengthUnit)
          {
            case LengthUnit.Millimeter:
              return ForcePerLengthUnit.NewtonPerMillimeter;
            case LengthUnit.Centimeter:
              return ForcePerLengthUnit.NewtonPerCentimeter;
            case LengthUnit.Meter:
              return ForcePerLengthUnit.NewtonPerMeter;
          }
          break;
        case ForceUnit.Kilonewton:
          switch (lengthUnit)
          {
            case LengthUnit.Millimeter:
              return ForcePerLengthUnit.KilonewtonPerMillimeter;
            case LengthUnit.Centimeter:
              return ForcePerLengthUnit.KilonewtonPerCentimeter;
            case LengthUnit.Meter:
              return ForcePerLengthUnit.KilonewtonPerMeter;
          }
          break;
        case ForceUnit.Meganewton:
          switch (lengthUnit)
          {
            case LengthUnit.Millimeter:
              return ForcePerLengthUnit.MeganewtonPerMillimeter;
            case LengthUnit.Centimeter:
              return ForcePerLengthUnit.MeganewtonPerCentimeter;
            case LengthUnit.Meter:
              return ForcePerLengthUnit.MeganewtonPerMeter;
          }
          break;
        case ForceUnit.KilopoundForce:
          switch (lengthUnit)
          {
            case LengthUnit.Inch:
              return ForcePerLengthUnit.KilopoundForcePerInch;
            case LengthUnit.Foot:
              return ForcePerLengthUnit.KilopoundForcePerFoot;
          }
          break;
        case ForceUnit.PoundForce:
          switch (lengthUnit)
          {
            case LengthUnit.Inch:
              return ForcePerLengthUnit.PoundForcePerInch;
            case LengthUnit.Foot:
              return ForcePerLengthUnit.PoundForcePerFoot;
          }
          break;
      }

      // fallback
      Force force = Force.From(1, forceUnit);
      Length length = Length.From(1, lengthUnit);
      ForcePerLength kNperM = force / length;
      return kNperM.Unit;
    }

    public static PressureUnit GetForcePerAreaUnit(ForceUnit forceUnit, LengthUnit lengthUnit)
    {
      switch (forceUnit)
      {
        case ForceUnit.Newton:
          switch (lengthUnit)
          {
            case LengthUnit.Millimeter:
              return PressureUnit.NewtonPerSquareMillimeter;
            case LengthUnit.Centimeter:
              return PressureUnit.NewtonPerSquareCentimeter;
            case LengthUnit.Meter:
              return PressureUnit.NewtonPerSquareMeter;
          }
          break;
        case ForceUnit.Kilonewton:
          switch (lengthUnit)
          {
            case LengthUnit.Millimeter:
              return PressureUnit.KilonewtonPerSquareMillimeter;
            case LengthUnit.Centimeter:
              return PressureUnit.KilonewtonPerSquareCentimeter;
            case LengthUnit.Meter:
              return PressureUnit.KilonewtonPerSquareMeter;
          }
          break;
        case ForceUnit.Meganewton:
          switch (lengthUnit)
          {
            case LengthUnit.Meter:
              return PressureUnit.MeganewtonPerSquareMeter;
          }
          break;
        case ForceUnit.KilopoundForce:
          switch (lengthUnit)
          {
            case LengthUnit.Inch:
              return PressureUnit.KilopoundForcePerSquareInch;
            case LengthUnit.Foot:
              return PressureUnit.KilopoundForcePerSquareFoot;
          }
          break;
        case ForceUnit.PoundForce:
          switch (lengthUnit)
          {
            case LengthUnit.Inch:
              return PressureUnit.PoundForcePerSquareInch;
            case LengthUnit.Foot:
              return PressureUnit.PoundForcePerSquareFoot;
          }
          break;
      }
      throw new Exception("Unable to convert " + forceUnit.ToString() + " combined with " + lengthUnit.ToString() + " to force per area");
    }

    public static DensityUnit GetDensityUnit(MassUnit massUnit, LengthUnit lengthUnit)
    {
      Mass mass = Mass.From(1, massUnit);
      Length len = Length.From(1, lengthUnit);
      Volume vol = len * len * len;

      Density density = mass / vol;
      return density.Unit;
    }

    public static LinearDensityUnit GetLinearDensityUnit(MassUnit massUnit, LengthUnit lengthUnit)
    {
      switch (massUnit)
      {
        case MassUnit.Kilogram:
          switch (lengthUnit)
          {
            case LengthUnit.Millimeter:
              return LinearDensityUnit.KilogramPerMillimeter;
            case LengthUnit.Centimeter:
              return LinearDensityUnit.KilogramPerCentimeter;
            case LengthUnit.Meter:
              return LinearDensityUnit.KilogramPerMeter;
          }
          break;
        case MassUnit.Pound:
          switch (lengthUnit)
          {
            case LengthUnit.Foot:
              return LinearDensityUnit.PoundPerFoot;
            case LengthUnit.Inch:
              return LinearDensityUnit.PoundPerInch;
          }
          break;
      }
      return LinearDensityUnit.KilogramPerMeter;
    }


    public static List<string> GetFilteredAbbreviations(EngineeringUnits unit)
    {
      List<string> abbreviations = new List<string>();
      switch (unit)
      {
        case EngineeringUnits.Angle:
          foreach (string unitstring in FilteredUnits.FilteredAngleUnits)
            abbreviations.Add(Angle.GetAbbreviation((AngleUnit)Enum.Parse(typeof(AngleUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Length:
          foreach (string unitstring in FilteredUnits.FilteredLengthUnits)
            abbreviations.Add(Length.GetAbbreviation((LengthUnit)Enum.Parse(typeof(LengthUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Area:
          foreach (string unitstring in FilteredUnits.FilteredAreaUnits)
            abbreviations.Add(Area.GetAbbreviation((AreaUnit)Enum.Parse(typeof(AreaUnit), unitstring)));
          return abbreviations;
        
        case EngineeringUnits.Volume:
          foreach (string unitstring in FilteredUnits.FilteredVolumeUnits)
            abbreviations.Add(Volume.GetAbbreviation((VolumeUnit)Enum.Parse(typeof(VolumeUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.AreaMomentOfInertia:
          foreach (string unitstring in FilteredUnits.FilteredAreaMomentOfInertiaUnits)
            abbreviations.Add(AreaMomentOfInertia.GetAbbreviation((AreaMomentOfInertiaUnit)Enum.Parse(typeof(AreaMomentOfInertiaUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Force:
          foreach (string unitstring in FilteredUnits.FilteredForceUnits)
            abbreviations.Add(Force.GetAbbreviation((ForceUnit)Enum.Parse(typeof(ForceUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.ForcePerLength:
          foreach (string unitstring in FilteredUnits.FilteredForcePerLengthUnits)
            abbreviations.Add(ForcePerLength.GetAbbreviation((ForcePerLengthUnit)Enum.Parse(typeof(ForcePerLengthUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.ForcePerArea:
          foreach (string unitstring in FilteredUnits.FilteredForcePerAreaUnits)
            abbreviations.Add(Pressure.GetAbbreviation((PressureUnit)Enum.Parse(typeof(PressureUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Moment:
          foreach (string unitstring in FilteredUnits.FilteredMomentUnits)
            abbreviations.Add(Moment.GetAbbreviation((MomentUnit)Enum.Parse(typeof(MomentUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Stress:
          foreach (string unitstring in FilteredUnits.FilteredStressUnits)
            abbreviations.Add(Pressure.GetAbbreviation((PressureUnit)Enum.Parse(typeof(PressureUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Strain:
          foreach (string unitstring in FilteredUnits.FilteredStrainUnits)
            abbreviations.Add(Strain.GetAbbreviation((StrainUnit)Enum.Parse(typeof(StrainUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.AxialStiffness:
          foreach (string unitstring in FilteredUnits.FilteredAxialStiffnessUnits)
            abbreviations.Add(AxialStiffness.GetAbbreviation((AxialStiffnessUnit)Enum.Parse(typeof(AxialStiffnessUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.BendingStiffness:
          foreach (string unitstring in FilteredUnits.FilteredBendingStiffnessUnits)
            abbreviations.Add(BendingStiffness.GetAbbreviation((BendingStiffnessUnit)Enum.Parse(typeof(BendingStiffnessUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Curvature:
          foreach (string unitstring in FilteredUnits.FilteredCurvatureUnits)
            abbreviations.Add(Curvature.GetAbbreviation((CurvatureUnit)Enum.Parse(typeof(CurvatureUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Mass:
          foreach (string unitstring in FilteredUnits.FilteredMassUnits)
            abbreviations.Add(Mass.GetAbbreviation((MassUnit)Enum.Parse(typeof(MassUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Density:
          foreach (string unitstring in FilteredUnits.FilteredDensityUnits)
            abbreviations.Add(Density.GetAbbreviation((DensityUnit)Enum.Parse(typeof(DensityUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Temperature:
          foreach (string unitstring in FilteredUnits.FilteredTemperatureUnits)
            abbreviations.Add(Temperature.GetAbbreviation((TemperatureUnit)Enum.Parse(typeof(TemperatureUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Velocity:
          foreach (string unitstring in FilteredUnits.FilteredVelocityUnits)
            abbreviations.Add(Speed.GetAbbreviation((SpeedUnit)Enum.Parse(typeof(SpeedUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Acceleration:
          foreach (string unitstring in FilteredUnits.FilteredAccelerationUnits)
            abbreviations.Add(Acceleration.GetAbbreviation((AccelerationUnit)Enum.Parse(typeof(AccelerationUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Energy:
          foreach (string unitstring in FilteredUnits.FilteredEnergyUnits)
            abbreviations.Add(Energy.GetAbbreviation((EnergyUnit)Enum.Parse(typeof(EnergyUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Ratio:
          foreach (string unitstring in FilteredUnits.FilteredRatioUnits)
            abbreviations.Add(Ratio.GetAbbreviation((RatioUnit)Enum.Parse(typeof(RatioUnit), unitstring)));
          return abbreviations;

        case EngineeringUnits.Time:
          foreach (string unitstring in FilteredUnits.FilteredTimeUnits)
            abbreviations.Add(Duration.GetAbbreviation((DurationUnit)Enum.Parse(typeof(DurationUnit), unitstring)));
          return abbreviations;

        default:
          throw new Exception("Unable to get abbreviations for unit type " + unit.ToString());
      }
    }
  }
}
