using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using OasysUnits;
using OasysUnits.Units;

namespace OasysGH.Units.Helpers {
  public class UnitsHelper {
    public static int SignificantDigits => BitConverter.GetBytes(decimal.GetBits((decimal)DefaultUnits.Tolerance.As(DefaultUnits.LengthUnitGeometry))[3])[2];

    private static readonly BaseUnits sI = OasysUnits.UnitSystem.SI.BaseUnits;

    public static AreaMomentOfInertiaUnit GetAreaMomentOfInertiaUnit(LengthUnit unit) {
      switch (unit) {
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

      throw new OasysUnitsException("Unable to convert " + unit + " to a known type of AreaMomentOfInertia");
    }

    public static AreaUnit GetAreaUnit(LengthUnit unit) {
      switch (unit) {
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
      var baseUnits = new BaseUnits(unit, sI.Mass, sI.Time, sI.Current, sI.Temperature, sI.Amount, sI.LuminousIntensity);
      var unitSystem = new OasysUnits.UnitSystem(baseUnits);
      return new Area(1, unitSystem).Unit;
    }

    public static AxialStiffnessUnit GetAxialStiffnessUnit(ForceUnit forceUnit) {
      try {
        return AxialStiffness.ParseUnit(Force.GetAbbreviation(forceUnit));
      } catch (Exception) {
        throw new OasysUnitsException("Unable to convert " + forceUnit.ToString() + " to Axial Stiffness");
      }
    }

    public static BendingStiffnessUnit GetBendingStiffnessUnit(ForceUnit forceUnit, LengthUnit lengthUnit) {
      switch (forceUnit) {
        case ForceUnit.Newton:
          switch (lengthUnit) {
            case LengthUnit.Millimeter:
              return BendingStiffnessUnit.NewtonSquareMillimeter;

            case LengthUnit.Meter:
              return BendingStiffnessUnit.NewtonSquareMeter;
          }

          break;

        case ForceUnit.Kilonewton:
          switch (lengthUnit) {
            case LengthUnit.Millimeter:
              return BendingStiffnessUnit.KilonewtonSquareMillimeter;

            case LengthUnit.Meter:
              return BendingStiffnessUnit.KilonewtonSquareMeter;
          }

          break;

        case ForceUnit.PoundForce:
          switch (lengthUnit) {
            case LengthUnit.Inch:
              return BendingStiffnessUnit.PoundForceSquareInch;

            case LengthUnit.Foot:
              return BendingStiffnessUnit.PoundForceSquareFoot;
          }

          break;
      }

      throw new OasysUnitsException("Unable to convert " + forceUnit.ToString() + " combined with " + lengthUnit.ToString() + " to BendingStiffness");
    }

    public static CoefficientOfThermalExpansionUnit GetCoefficientOfThermalExpansionUnit(TemperatureUnit temperatureUnit) {
      switch (temperatureUnit) {
        case TemperatureUnit.Kelvin:
          return CoefficientOfThermalExpansionUnit.InverseKelvin;

        case TemperatureUnit.DegreeFahrenheit:
          return CoefficientOfThermalExpansionUnit.InverseDegreeFahrenheit;

        case TemperatureUnit.DegreeCelsius:
        default:
          return CoefficientOfThermalExpansionUnit.InverseDegreeCelsius;
      }
    }

    public static DensityUnit GetDensityUnit(MassUnit massUnit, LengthUnit lengthUnit) {
      string mass = massUnit.ToString();
      string length = lengthUnit.ToString();
      return (DensityUnit)Enum.Parse(typeof(DensityUnit), mass + "PerCubic" + length);
    }

    public static List<string> GetFilteredAbbreviations(EngineeringUnits unit) {
      var abbreviations = new List<string>();
      switch (unit) {
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

        case EngineeringUnits.VolumePerLength:
          foreach (string unitstring in FilteredUnits.FilteredVolumePerLengthUnits)
            abbreviations.Add(VolumePerLength.GetAbbreviation((VolumePerLengthUnit)Enum.Parse(typeof(VolumePerLengthUnit), unitstring)));
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

        case EngineeringUnits.LinearDensity:
          foreach (string unitstring in FilteredUnits.FilteredLinearDensityUnits)
            abbreviations.Add(LinearDensity.GetAbbreviation((LinearDensityUnit)Enum.Parse(typeof(LinearDensityUnit), unitstring)));
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

        case EngineeringUnits.SectionModulus:
          foreach (string unitstring in FilteredUnits.FilteredSectionModulusUnits)
            abbreviations.Add(SectionModulus.GetAbbreviation((SectionModulusUnit)Enum.Parse(typeof(SectionModulusUnit), unitstring)));
          return abbreviations;

        default:
          throw new OasysUnitsException("Unable to get abbreviations for unit type " + unit.ToString());
      }
    }

    public static PressureUnit GetForcePerAreaUnit(ForceUnit forceUnit, LengthUnit lengthUnit) {
      switch (forceUnit) {
        case ForceUnit.Newton:
          switch (lengthUnit) {
            case LengthUnit.Millimeter:
              return PressureUnit.NewtonPerSquareMillimeter;

            case LengthUnit.Centimeter:
              return PressureUnit.NewtonPerSquareCentimeter;

            case LengthUnit.Meter:
              return PressureUnit.NewtonPerSquareMeter;
          }

          break;

        case ForceUnit.Kilonewton:
          switch (lengthUnit) {
            case LengthUnit.Millimeter:
              return PressureUnit.KilonewtonPerSquareMillimeter;

            case LengthUnit.Centimeter:
              return PressureUnit.KilonewtonPerSquareCentimeter;

            case LengthUnit.Meter:
              return PressureUnit.KilonewtonPerSquareMeter;
          }

          break;

        case ForceUnit.Meganewton:
          switch (lengthUnit) {
            case LengthUnit.Meter:
              return PressureUnit.MeganewtonPerSquareMeter;
          }

          break;

        case ForceUnit.KilopoundForce:
          switch (lengthUnit) {
            case LengthUnit.Inch:
              return PressureUnit.KilopoundForcePerSquareInch;

            case LengthUnit.Foot:
              return PressureUnit.KilopoundForcePerSquareFoot;
          }

          break;

        case ForceUnit.PoundForce:
          switch (lengthUnit) {
            case LengthUnit.Inch:
              return PressureUnit.PoundForcePerSquareInch;

            case LengthUnit.Foot:
              return PressureUnit.PoundForcePerSquareFoot;
          }

          break;
      }

      throw new OasysUnitsException("Unable to convert " + forceUnit.ToString() + " combined with " + lengthUnit.ToString() + " to force per area");
    }

    public static ForcePerLengthUnit GetForcePerLengthUnit(ForceUnit forceUnit, LengthUnit lengthUnit) {
      switch (forceUnit) {
        case ForceUnit.Newton:
          switch (lengthUnit) {
            case LengthUnit.Millimeter:
              return ForcePerLengthUnit.NewtonPerMillimeter;

            case LengthUnit.Centimeter:
              return ForcePerLengthUnit.NewtonPerCentimeter;

            case LengthUnit.Meter:
              return ForcePerLengthUnit.NewtonPerMeter;
          }

          break;

        case ForceUnit.Kilonewton:
          switch (lengthUnit) {
            case LengthUnit.Millimeter:
              return ForcePerLengthUnit.KilonewtonPerMillimeter;

            case LengthUnit.Centimeter:
              return ForcePerLengthUnit.KilonewtonPerCentimeter;

            case LengthUnit.Meter:
              return ForcePerLengthUnit.KilonewtonPerMeter;
          }

          break;

        case ForceUnit.Meganewton:
          switch (lengthUnit) {
            case LengthUnit.Millimeter:
              return ForcePerLengthUnit.MeganewtonPerMillimeter;

            case LengthUnit.Centimeter:
              return ForcePerLengthUnit.MeganewtonPerCentimeter;

            case LengthUnit.Meter:
              return ForcePerLengthUnit.MeganewtonPerMeter;
          }

          break;

        case ForceUnit.KilopoundForce:
          switch (lengthUnit) {
            case LengthUnit.Inch:
              return ForcePerLengthUnit.KilopoundForcePerInch;

            case LengthUnit.Foot:
              return ForcePerLengthUnit.KilopoundForcePerFoot;
          }

          break;

        case ForceUnit.PoundForce:
          switch (lengthUnit) {
            case LengthUnit.Inch:
              return ForcePerLengthUnit.PoundForcePerInch;

            case LengthUnit.Foot:
              return ForcePerLengthUnit.PoundForcePerFoot;
          }

          break;
      }

      throw new OasysUnitsException("Unable to convert " + forceUnit + " x " + lengthUnit + " to a known type of VolumePerLengthUnit");
    }

    public static LinearDensityUnit GetLinearDensityUnit(MassUnit massUnit, LengthUnit lengthUnit) {
      switch (massUnit) {
        case MassUnit.Kilogram:
          switch (lengthUnit) {
            case LengthUnit.Millimeter:
              return LinearDensityUnit.KilogramPerMillimeter;

            case LengthUnit.Centimeter:
              return LinearDensityUnit.KilogramPerCentimeter;

            case LengthUnit.Meter:
              return LinearDensityUnit.KilogramPerMeter;
          }

          break;

        case MassUnit.Pound:
          switch (lengthUnit) {
            case LengthUnit.Foot:
              return LinearDensityUnit.PoundPerFoot;

            case LengthUnit.Inch:
              return LinearDensityUnit.PoundPerInch;
          }

          break;
      }

      throw new OasysUnitsException("Unable to convert " + massUnit.ToString() + " combined with " + lengthUnit.ToString() + " to Linear Density");
    }

    public static MomentUnit GetMomentUnit(ForceUnit forceUnit, LengthUnit lengthUnit) {
      switch (forceUnit) {
        case ForceUnit.Newton:
          switch (lengthUnit) {
            case LengthUnit.Millimeter:
              return MomentUnit.NewtonMillimeter;

            case LengthUnit.Centimeter:
              return MomentUnit.NewtonCentimeter;

            case LengthUnit.Meter:
              return MomentUnit.NewtonMeter;
          }

          break;

        case ForceUnit.Kilonewton:
          switch (lengthUnit) {
            case LengthUnit.Millimeter:
              return MomentUnit.KilonewtonMillimeter;

            case LengthUnit.Centimeter:
              return MomentUnit.KilonewtonCentimeter;

            case LengthUnit.Meter:
              return MomentUnit.KilonewtonMeter;
          }

          break;

        case ForceUnit.Meganewton:
          switch (lengthUnit) {
            case LengthUnit.Millimeter:
              return MomentUnit.MeganewtonMillimeter;

            case LengthUnit.Centimeter:
              return MomentUnit.MeganewtonCentimeter;

            case LengthUnit.Meter:
              return MomentUnit.MeganewtonMeter;
          }

          break;

        case ForceUnit.KilopoundForce:
          switch (lengthUnit) {
            case LengthUnit.Inch:
              return MomentUnit.KilopoundForceInch;

            case LengthUnit.Foot:
              return MomentUnit.KilopoundForceFoot;
          }

          break;

        case ForceUnit.PoundForce:
          switch (lengthUnit) {
            case LengthUnit.Inch:
              return MomentUnit.PoundForceInch;

            case LengthUnit.Foot:
              return MomentUnit.PoundForceFoot;
          }

          break;
      }

      throw new OasysUnitsException("Unable to convert " + forceUnit.ToString() + " combined with " + lengthUnit.ToString() + " to moment");
    }

    public static SectionModulusUnit GetSectionModulusUnit(LengthUnit unit) {
      switch (unit) {
        case LengthUnit.Millimeter:
          return SectionModulusUnit.CubicMillimeter;

        case LengthUnit.Centimeter:
          return SectionModulusUnit.CubicCentimeter;

        case LengthUnit.Meter:
          return SectionModulusUnit.CubicMeter;

        case LengthUnit.Foot:
          return SectionModulusUnit.CubicFoot;

        case LengthUnit.Inch:
          return SectionModulusUnit.CubicInch;
      }
      // fallback:
      var baseUnits = new BaseUnits(unit, sI.Mass, sI.Time, sI.Current, sI.Temperature, sI.Amount, sI.LuminousIntensity);
      var unitSystem = new OasysUnits.UnitSystem(baseUnits);
      return new SectionModulus(1, unitSystem).Unit;
    }

    public static VolumePerLengthUnit GetVolumePerLengthUnit(LengthUnit unit) {
      switch (unit) {
        case LengthUnit.Foot:
        case LengthUnit.Inch:
          return VolumePerLengthUnit.CubicYardPerFoot;

        case LengthUnit.Millimeter:
        case LengthUnit.Centimeter:
        case LengthUnit.Meter:
          return VolumePerLengthUnit.CubicMeterPerMeter;

        default:
          throw new OasysUnitsException("Unable to convert " + unit + " to a known type of VolumePerLengthUnit");
      }
    }

    public static VolumeUnit GetVolumeUnit(LengthUnit unit) {
      switch (unit) {
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
      var baseUnits = new BaseUnits(unit, sI.Mass, sI.Time, sI.Current, sI.Temperature, sI.Amount, sI.LuminousIntensity);
      var unitSystem = new OasysUnits.UnitSystem(baseUnits);
      return new Volume(1, unitSystem).Unit;
    }

    /// <summary>
    /// Tries to parse a units abbreviation or string representation.
    /// </summary>
    /// <param name="unitType"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Enum Parse(Type unitType, string value) {
      CultureInfo culture = Thread.CurrentThread.CurrentUICulture;
      return UnitsHelper.Parse(unitType, value, culture);
    }

    /// <summary>
    /// Tries to parse a unit´s abbreviation or string representation.
    /// </summary>
    /// <param name="unitType"></param>
    /// <param name="value"></param>
    /// <param name="currentUICulture"></param>
    /// <returns></returns>
    public static Enum Parse(Type unitType, string value, CultureInfo currentUICulture) {
      if (UnitParser.Default.TryParse(value, unitType, out Enum unit))
        return unit;
      try {
        return (Enum)Enum.Parse(unitType, value, true);
      } catch (ArgumentException) {
        // try to use current culture to parse unit abbreviation
        switch (unitType) {
          case Type _ when unitType == typeof(AccelerationUnit):
            return Acceleration.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(AngleUnit):
            return Angle.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(AreaMomentOfInertiaUnit):
            return AreaMomentOfInertia.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(AreaUnit):
            return Area.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(AxialStiffnessUnit):
            return AxialStiffness.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(BendingStiffnessUnit):
            return BendingStiffness.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(CoefficientOfThermalExpansionUnit):
            return CoefficientOfThermalExpansion.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(CurvatureUnit):
            return Curvature.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(DensityUnit):
            return Density.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(DurationUnit):
            return Duration.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(EnergyUnit):
            return Energy.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(ForcePerLengthUnit):
            return ForcePerLength.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(ForceUnit):
            return Force.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(MassUnit):
            return Mass.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(MomentUnit):
            return Moment.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(LengthUnit):
            return Length.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(LinearDensityUnit):
            return LinearDensity.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(PressureUnit):
            return Pressure.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(RatioUnit):
            return Ratio.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(SpeedUnit):
            return Speed.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(StrainUnit):
            return Strain.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(TemperatureUnit):
            return Temperature.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(VolumePerLengthUnit):
            return VolumePerLength.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(VolumeUnit):
            return Volume.ParseUnit(value, currentUICulture);

          case Type _ when unitType == typeof(SectionModulus):
            return SectionModulus.ParseUnit(value, currentUICulture);

          default:
            throw new ArgumentException();
        }
      }
    }
  }
}
