using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnitsNet;
using UnitsNet.Units;
using Oasys.Units;

namespace OasysGH.Units
{
  public enum EngineeringUnits
  {
    Angle,
    Length,
    Area,
    Volume,
    AreaMomentOfInertia,
    Force,
    ForcePerLength,
    ForcePerArea,
    Moment,
    Stress,
    Strain,
    AxialStiffness,
    BendingStiffness,
    Curvature,
    Mass,
    Density,
    Temperature,
    Velocity,
    Acceleration,
    Energy,
    Ratio,
    Time
  }

  public class FilteredUnits
  {
    public static ReadOnlyCollection<string> FilteredAngleUnits = new ReadOnlyCollection<string>(new[]
    {
      AngleUnit.Radian.ToString(),
      AngleUnit.Degree.ToString()
    });

    public static ReadOnlyCollection<string> FilteredLengthUnits = new ReadOnlyCollection<string>(new[]
    {
      LengthUnit.Millimeter.ToString(),
      LengthUnit.Centimeter.ToString(),
      LengthUnit.Meter.ToString(),
      LengthUnit.Inch.ToString(),
      LengthUnit.Foot.ToString()
    });

    public static ReadOnlyCollection<string> FilteredAreaUnits = new ReadOnlyCollection<string>(new[]
    {
      AreaUnit.SquareMillimeter.ToString(),
      AreaUnit.SquareCentimeter.ToString(),
      AreaUnit.SquareMeter.ToString(),
      AreaUnit.SquareInch.ToString(),
      AreaUnit.SquareFoot.ToString()
    });

    public static ReadOnlyCollection<string> FilteredVolumeUnits = new ReadOnlyCollection<string>(new[]
    {
      VolumeUnit.CubicMillimeter.ToString(),
      VolumeUnit.CubicCentimeter.ToString(),
      VolumeUnit.CubicMeter.ToString(),
      VolumeUnit.CubicInch.ToString(),
      VolumeUnit.CubicFoot.ToString()
    });

    public static ReadOnlyCollection<string> FilteredAreaMomentOfInertiaUnits = new ReadOnlyCollection<string>(new[]
    {
      AreaMomentOfInertiaUnit.MillimeterToTheFourth.ToString(),
      AreaMomentOfInertiaUnit.CentimeterToTheFourth.ToString(),
      AreaMomentOfInertiaUnit.MeterToTheFourth.ToString(),
      AreaMomentOfInertiaUnit.InchToTheFourth.ToString(),
      AreaMomentOfInertiaUnit.FootToTheFourth.ToString()
    });


    public static ReadOnlyCollection<string> FilteredCurvatureUnits = new ReadOnlyCollection<string>(Enum.GetNames(typeof(CurvatureUnit)).Skip(1).ToList());

    public static ReadOnlyCollection<string> FilteredForceUnits = new ReadOnlyCollection<string>(new[]
    {
      ForceUnit.Newton.ToString(),
      ForceUnit.Kilonewton.ToString(),
      ForceUnit.Meganewton.ToString(),
      ForceUnit.PoundForce.ToString(),
      ForceUnit.KilopoundForce.ToString(),
      ForceUnit.TonneForce.ToString()
    });

    public static ReadOnlyCollection<string> FilteredForcePerLengthUnits = new ReadOnlyCollection<string>(new[]
    {
      ForcePerLengthUnit.NewtonPerMillimeter.ToString(),
      ForcePerLengthUnit.NewtonPerCentimeter.ToString(),
      ForcePerLengthUnit.NewtonPerMeter.ToString(),
      ForcePerLengthUnit.KilonewtonPerMillimeter.ToString(),
      ForcePerLengthUnit.KilonewtonPerCentimeter.ToString(),
      ForcePerLengthUnit.KilonewtonPerMeter.ToString(),
      ForcePerLengthUnit.TonneForcePerCentimeter.ToString(),
      ForcePerLengthUnit.TonneForcePerMeter.ToString(),
      ForcePerLengthUnit.TonneForcePerMillimeter.ToString(),
      ForcePerLengthUnit.MeganewtonPerMeter.ToString(),
      ForcePerLengthUnit.PoundForcePerInch.ToString(),
      ForcePerLengthUnit.PoundForcePerFoot.ToString(),
      ForcePerLengthUnit.PoundForcePerYard.ToString(),
      ForcePerLengthUnit.KilopoundForcePerInch.ToString(),
      ForcePerLengthUnit.KilopoundForcePerFoot.ToString()
    });

    public static ReadOnlyCollection<string> FilteredForcePerAreaUnits = new ReadOnlyCollection<string>(new[]
    {
      PressureUnit.NewtonPerSquareMillimeter.ToString(),
      PressureUnit.NewtonPerSquareCentimeter.ToString(),
      PressureUnit.NewtonPerSquareMeter.ToString(),
      PressureUnit.KilonewtonPerSquareCentimeter.ToString(),
      PressureUnit.KilonewtonPerSquareMillimeter.ToString(),
      PressureUnit.KilonewtonPerSquareMeter.ToString(),
      PressureUnit.PoundForcePerSquareInch.ToString(),
      PressureUnit.PoundForcePerSquareFoot.ToString(),
      PressureUnit.KilopoundForcePerSquareInch.ToString(),
    });

    public static ReadOnlyCollection<string> FilteredAxialStiffnessUnits = new ReadOnlyCollection<string>(Enum.GetNames(typeof(AxialStiffnessUnit)).Skip(1).ToList());

    public static ReadOnlyCollection<string> FilteredBendingStiffnessUnits = new ReadOnlyCollection<string>(Enum.GetNames(typeof(BendingStiffnessUnit)).Skip(1).ToList());

    public static ReadOnlyCollection<string> FilteredMomentUnits = new ReadOnlyCollection<string>(Enum.GetNames(typeof(MomentUnit)).Skip(1).ToList());

    public static ReadOnlyCollection<string> FilteredStressUnits = new ReadOnlyCollection<string>(new[]
    {
      PressureUnit.Pascal.ToString(),
      PressureUnit.Kilopascal.ToString(),
      PressureUnit.Megapascal.ToString(),
      PressureUnit.Gigapascal.ToString(),
      PressureUnit.NewtonPerSquareMillimeter.ToString(),
      PressureUnit.NewtonPerSquareMeter.ToString(),
      PressureUnit.PoundForcePerSquareInch.ToString(),
      PressureUnit.PoundForcePerSquareFoot.ToString(),
      PressureUnit.KilopoundForcePerSquareInch.ToString()
    });

    public static ReadOnlyCollection<string> FilteredStrainUnits = new ReadOnlyCollection<string>(Enum.GetNames(typeof(StrainUnit)).Skip(1).ToList());

    public static ReadOnlyCollection<string> FilteredMassUnits = new ReadOnlyCollection<string>(new[]
    {
      MassUnit.Gram.ToString(),
      MassUnit.Kilogram.ToString(),
      MassUnit.Tonne.ToString(),
      MassUnit.Kilotonne.ToString(),
      MassUnit.Pound.ToString(),
      MassUnit.Kilopound.ToString(),
      MassUnit.LongTon.ToString(),
      MassUnit.Slug.ToString()
    });

    public static ReadOnlyCollection<string> FilteredDensityUnits = new ReadOnlyCollection<string>(new[]
    {
      DensityUnit.GramPerCubicMillimeter.ToString(),
      DensityUnit.GramPerCubicCentimeter.ToString(),
      DensityUnit.GramPerCubicMeter.ToString(),
      DensityUnit.KilogramPerCubicMillimeter.ToString(),
      DensityUnit.KilogramPerCubicCentimeter.ToString(),
      DensityUnit.KilogramPerCubicMeter.ToString(),
      DensityUnit.TonnePerCubicMillimeter.ToString(),
      DensityUnit.TonnePerCubicCentimeter.ToString(),
      DensityUnit.TonnePerCubicMeter.ToString(),
      DensityUnit.PoundPerCubicFoot.ToString(),
      DensityUnit.PoundPerCubicInch.ToString(),
      DensityUnit.KilopoundPerCubicFoot.ToString(),
      DensityUnit.KilopoundPerCubicInch.ToString(),
    });

    public static ReadOnlyCollection<string> FilteredTemperatureUnits = new ReadOnlyCollection<string>(new[]
    {
      TemperatureUnit.DegreeCelsius.ToString(),
      TemperatureUnit.Kelvin.ToString(),
      TemperatureUnit.DegreeFahrenheit.ToString(),
    });

    public static ReadOnlyCollection<string> FilteredVelocityUnits = new ReadOnlyCollection<string>(new[]
    {
      SpeedUnit.MillimeterPerSecond.ToString(),
      SpeedUnit.CentimeterPerSecond.ToString(),
      SpeedUnit.MeterPerSecond.ToString(),
      SpeedUnit.FootPerSecond.ToString(),
      SpeedUnit.InchPerSecond.ToString(),
      SpeedUnit.KilometerPerHour.ToString(),
      SpeedUnit.MilePerHour.ToString(),
    });

    public static ReadOnlyCollection<string> FilteredAccelerationUnits = new ReadOnlyCollection<string>(new[]
    {
      AccelerationUnit.MillimeterPerSecondSquared.ToString(),
      AccelerationUnit.CentimeterPerSecondSquared.ToString(),
      AccelerationUnit.MeterPerSecondSquared.ToString(),
      AccelerationUnit.KilometerPerSecondSquared.ToString(),
      AccelerationUnit.FootPerSecondSquared.ToString(),
      AccelerationUnit.InchPerSecondSquared.ToString(),
    });

    public static ReadOnlyCollection<string> FilteredEnergyUnits = new ReadOnlyCollection<string>(new[]
    {
      EnergyUnit.Joule.ToString(),
      EnergyUnit.Kilojoule.ToString(),
      EnergyUnit.Megajoule.ToString(),
      EnergyUnit.Gigajoule.ToString(),
      EnergyUnit.KilowattHour.ToString(),
      EnergyUnit.FootPound.ToString(),
      EnergyUnit.Calorie.ToString(),
      EnergyUnit.BritishThermalUnit.ToString(),
    });

    public static ReadOnlyCollection<string> FilteredTimeUnits = new ReadOnlyCollection<string>(new[]
    {
      DurationUnit.Millisecond.ToString(),
      DurationUnit.Second.ToString(),
      DurationUnit.Minute.ToString(),
      DurationUnit.Hour.ToString(),
      DurationUnit.Day.ToString(),
    });

    public static ReadOnlyCollection<string> FilteredRatioUnits = new ReadOnlyCollection<string>(Enum.GetNames(typeof(RatioUnit)).Skip(1).ToList());
  }
}
