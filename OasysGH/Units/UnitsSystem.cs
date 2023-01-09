using OasysUnits.Units;

namespace OasysGH.Units
{
  // that should be called something else
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
