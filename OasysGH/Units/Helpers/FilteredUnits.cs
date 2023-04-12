using System;
using System.Collections.Generic;
using System.Linq;
using OasysUnits.Units;

namespace OasysGH.Units.Helpers {
  public class FilteredUnits {

    public static List<string> FilteredAccelerationUnits = new List<string>(new[]
    {
      AccelerationUnit.MillimeterPerSecondSquared.ToString(),
      AccelerationUnit.CentimeterPerSecondSquared.ToString(),
      AccelerationUnit.MeterPerSecondSquared.ToString(),
      AccelerationUnit.KilometerPerSecondSquared.ToString(),
      AccelerationUnit.FootPerSecondSquared.ToString(),
      AccelerationUnit.InchPerSecondSquared.ToString(),
    });

    public static List<string> FilteredAngleUnits = new List<string>(new[]
        {
      AngleUnit.Radian.ToString(),
      AngleUnit.Degree.ToString()
    });

    public static List<string> FilteredAreaMomentOfInertiaUnits = new List<string>(new[]
    {
      AreaMomentOfInertiaUnit.MillimeterToTheFourth.ToString(),
      AreaMomentOfInertiaUnit.CentimeterToTheFourth.ToString(),
      AreaMomentOfInertiaUnit.MeterToTheFourth.ToString(),
      AreaMomentOfInertiaUnit.InchToTheFourth.ToString(),
      AreaMomentOfInertiaUnit.FootToTheFourth.ToString()
    });

    public static List<string> FilteredAreaUnits = new List<string>(new[]
    {
      AreaUnit.SquareMillimeter.ToString(),
      AreaUnit.SquareCentimeter.ToString(),
      AreaUnit.SquareMeter.ToString(),
      AreaUnit.SquareInch.ToString(),
      AreaUnit.SquareFoot.ToString()
    });

    public static List<string> FilteredAxialStiffnessUnits = new List<string>(Enum.GetNames(typeof(AxialStiffnessUnit)).Skip(1).ToList());

    public static List<string> FilteredBendingStiffnessUnits = new List<string>(Enum.GetNames(typeof(BendingStiffnessUnit)).Skip(1).ToList());

    public static List<string> FilteredCurvatureUnits = new List<string>(Enum.GetNames(typeof(CurvatureUnit)).Skip(1).ToList());

    public static List<string> FilteredDensityUnits = new List<string>(new[]
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
      DensityUnit.SlugPerCubicFoot.ToString(),
    });

    public static List<string> FilteredEnergyUnits = new List<string>(new[]
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

    public static List<string> FilteredForcePerAreaUnits = new List<string>(new[]
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
      PressureUnit.KilopoundForcePerSquareFoot.ToString(),
    });

    public static List<string> FilteredForcePerLengthUnits = new List<string>(new[]
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

    public static List<string> FilteredForceUnits = new List<string>(new[]
    {
      ForceUnit.Newton.ToString(),
      ForceUnit.Kilonewton.ToString(),
      ForceUnit.Meganewton.ToString(),
      ForceUnit.PoundForce.ToString(),
      ForceUnit.KilopoundForce.ToString(),
      ForceUnit.TonneForce.ToString()
    });

    public static List<string> FilteredLengthUnits = new List<string>(new[]
                                            {
      LengthUnit.Millimeter.ToString(),
      LengthUnit.Centimeter.ToString(),
      LengthUnit.Meter.ToString(),
      LengthUnit.Inch.ToString(),
      LengthUnit.Foot.ToString()
    });

    public static List<string> FilteredLinearDensityUnits = new List<string>(new[]
    {
      LinearDensityUnit.GramPerMillimeter.ToString(),
      LinearDensityUnit.GramPerCentimeter.ToString(),
      LinearDensityUnit.GramPerMeter.ToString(),
      LinearDensityUnit.KilogramPerMillimeter.ToString(),
      LinearDensityUnit.KilogramPerCentimeter.ToString(),
      LinearDensityUnit.KilogramPerMeter.ToString(),
      LinearDensityUnit.PoundPerInch.ToString(),
      LinearDensityUnit.PoundPerFoot.ToString(),
    });

    public static List<string> FilteredMassUnits = new List<string>(new[]
    {
      MassUnit.Gram.ToString(),
      MassUnit.Kilogram.ToString(),
      MassUnit.Tonne.ToString(),
      MassUnit.Kilotonne.ToString(),
      MassUnit.Pound.ToString(),
      MassUnit.Kilopound.ToString(),
      MassUnit.Slug.ToString()
    });

    public static List<string> FilteredMomentUnits = new List<string>(Enum.GetNames(typeof(MomentUnit)).Skip(1).ToList());

    public static List<string> FilteredRatioUnits = new List<string>(Enum.GetNames(typeof(RatioUnit)).Skip(1).ToList());

    public static List<string> FilteredStrainUnits = new List<string>(Enum.GetNames(typeof(StrainUnit)).Skip(1).ToList());

    public static List<string> FilteredStressUnits = new List<string>(new[]
    {
      PressureUnit.Pascal.ToString(),
      PressureUnit.Kilopascal.ToString(),
      PressureUnit.Megapascal.ToString(),
      PressureUnit.Gigapascal.ToString(),
      PressureUnit.NewtonPerSquareMillimeter.ToString(),
      PressureUnit.NewtonPerSquareMeter.ToString(),
      PressureUnit.PoundForcePerSquareInch.ToString(),
      PressureUnit.PoundForcePerSquareFoot.ToString(),
      PressureUnit.KilopoundForcePerSquareInch.ToString(),
      PressureUnit.KilopoundForcePerSquareFoot.ToString()
    });

    public static List<string> FilteredTemperatureUnits = new List<string>(new[]
    {
      TemperatureUnit.DegreeCelsius.ToString(),
      TemperatureUnit.Kelvin.ToString(),
      TemperatureUnit.DegreeFahrenheit.ToString(),
    });

    public static List<string> FilteredTimeUnits = new List<string>(new[]
    {
      DurationUnit.Millisecond.ToString(),
      DurationUnit.Second.ToString(),
      DurationUnit.Minute.ToString(),
      DurationUnit.Hour.ToString(),
      DurationUnit.Day.ToString(),
    });

    public static List<string> FilteredVelocityUnits = new List<string>(new[]
    {
      SpeedUnit.MillimeterPerSecond.ToString(),
      SpeedUnit.CentimeterPerSecond.ToString(),
      SpeedUnit.MeterPerSecond.ToString(),
      SpeedUnit.FootPerSecond.ToString(),
      SpeedUnit.InchPerSecond.ToString(),
      SpeedUnit.KilometerPerHour.ToString(),
      SpeedUnit.MilePerHour.ToString(),
    });

    public static List<string> FilteredVolumePerLengthUnits = new List<string>(new[]
    {
      VolumePerLengthUnit.CubicMeterPerMeter.ToString(),
      VolumePerLengthUnit.CubicYardPerFoot.ToString()
    });

    public static List<string> FilteredVolumeUnits = new List<string>(new[]
                                            {
      VolumeUnit.CubicMillimeter.ToString(),
      VolumeUnit.CubicCentimeter.ToString(),
      VolumeUnit.CubicMeter.ToString(),
      VolumeUnit.CubicInch.ToString(),
      VolumeUnit.CubicFoot.ToString()
    });
  }

  public enum EngineeringUnits {
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
    LinearDensity,
    VolumePerLength,
    Temperature,
    Velocity,
    Acceleration,
    Energy,
    Ratio,
    Time
  }
}
