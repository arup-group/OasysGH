using System.Runtime.CompilerServices;

using OasysUnits.Units;

#if RELEASEFORTESTING || DEBUG
[assembly: InternalsVisibleTo("GH_UnitNumberTests")]
[assembly: InternalsVisibleTo("OasysGHTests")]
#endif
namespace OasysGH.Units {
  internal class UnitSystem {
    internal AccelerationUnit AccelerationUnit { get; }
    internal AxialStiffnessUnit AxialStiffnessUnit { get; }
    internal BendingStiffnessUnit BendingStiffnessUnit { get; }
    internal CurvatureUnit CurvatureUnit { get; }
    internal DensityUnit DensityUnit { get; }
    internal EnergyUnit EnergyUnit { get; }
    internal PressureUnit ForcePerAreaUnit { get; }
    internal ForcePerLengthUnit ForcePerLengthUnit { get; }
    internal ForceUnit ForceUnit { get; }
    internal LengthUnit LengthUnit { get; }
    internal LengthUnit LengthUnitResult { get; }
    internal LinearDensityUnit LinearDensityUnit { get; }
    internal MassUnit MassUnit { get; }
    internal StrainUnit MaterialStrainUnit { get; }
    internal PressureUnit MaterialStrengthUnit { get; }
    internal MomentUnit MomentUnit { get; }
    internal AreaMomentOfInertiaUnit SectionAreaMomentOfInertiaUnit { get; }
    internal AreaUnit SectionAreaUnit { get; }
    internal LengthUnit SectionLengthUnit { get; }
    internal SectionModulusUnit SectionModulusUnit { get; }
    internal VolumeUnit SectionVolumeUnit { get; }
    internal StrainUnit StrainUnitResult { get; }
    internal PressureUnit StressUnitResult { get; }
    internal TemperatureUnit TemperatureUnit { get; }
    internal SpeedUnit VelocityUnit { get; }
    internal VolumePerLengthUnit VolumePerLengthUnit { get; }
    internal PressureUnit YoungsModulusUnit { get; }

    internal UnitSystem(LengthUnit sectionLengthUnit, AreaUnit areaUnit, VolumeUnit volumeUnit, AreaMomentOfInertiaUnit areaMomentOfInertiaUnit, MassUnit massUnit, DensityUnit densityUnit, LinearDensityUnit linearDensityUnit, VolumePerLengthUnit volumePerLengthUnit, PressureUnit materialStrengthUnit, StrainUnit materialStrainUnit, PressureUnit youngsModulusUnit, LengthUnit lengthUnit, ForceUnit forceUnit, ForcePerLengthUnit forcePerLengthUnit, PressureUnit forcePerAreaUnit, MomentUnit momentUnit, TemperatureUnit temperatureUnit, LengthUnit displacementUuit, PressureUnit stressUnit, StrainUnit strainUnit, AxialStiffnessUnit axialStiffnessUnit, BendingStiffnessUnit bendingStiffnessUnit, SpeedUnit velocityUnit, AccelerationUnit accelerationUnit, EnergyUnit energyUnit, CurvatureUnit curvatureUnit, SectionModulusUnit sectionModulusUnit) {
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
      SectionModulusUnit = sectionModulusUnit;
    }
  }
}
