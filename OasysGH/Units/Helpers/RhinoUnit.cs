using System.Collections.Generic;
using OasysUnits;
using OasysUnits.Units;
using Rhino;

namespace OasysGH.Units.Helpers {
  public class RhinoUnit {

    public static LengthUnit GetRhinoLengthUnit() {
      if (RhinoDoc.ActiveDoc == null)
        return LengthUnit.Meter;
      return GetRhinoLengthUnit(RhinoDoc.ActiveDoc.ModelUnitSystem);
    }

    public static LengthUnit GetRhinoLengthUnit(Rhino.UnitSystem rhinoUnits) {
      var units = new List<LengthUnit>(new LengthUnit[] {
        LengthUnit.Undefined,
        LengthUnit.Micrometer,
        LengthUnit.Millimeter,
        LengthUnit.Centimeter,
        LengthUnit.Meter,
        LengthUnit.Kilometer,
        LengthUnit.Microinch,
        LengthUnit.Mil,
        LengthUnit.Inch,
        LengthUnit.Foot,
        LengthUnit.Mile,
        LengthUnit.Undefined,
        LengthUnit.Undefined,
        LengthUnit.Nanometer,
        LengthUnit.Decimeter,
        LengthUnit.Undefined,
        LengthUnit.Hectometer,
        LengthUnit.Undefined,
        LengthUnit.Undefined,
        LengthUnit.Yard
      });
      return units[rhinoUnits.GetHashCode()];
    }

    public static Length GetRhinoTolerance() {
      if (RhinoDoc.ActiveDoc == null)
        return new Length(0.01, LengthUnit.Meter);
      LengthUnit lengthUnit = GetRhinoLengthUnit();
      double tolerance = RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
      return new Length(tolerance, lengthUnit);
    }
  }
}
