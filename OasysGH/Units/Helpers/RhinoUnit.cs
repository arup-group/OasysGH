using System.Collections.Generic;
using OasysUnits;
using OasysUnits.Units;
using Rhino;

namespace OasysGH.Units.Helpers {
  public class RhinoUnit {

    public static LengthUnit GetRhinoLengthUnit(RhinoDoc doc = null) {
      if (doc == null) {
        doc = RhinoDoc.ActiveDoc;
      }

      if (doc == null) {
        return LengthUnit.Meter;
      }

      return GetRhinoLengthUnit(doc.ModelUnitSystem);
    }

    public static LengthUnit GetRhinoLengthUnit(Rhino.UnitSystem rhinoUnits) {
      var units = new Dictionary<int, LengthUnit>() {
        { 1, LengthUnit.Micrometer },
        { 2, LengthUnit.Millimeter },
        { 3, LengthUnit.Centimeter },
        { 4, LengthUnit.Meter },
        { 5, LengthUnit.Kilometer },
        { 6, LengthUnit.Microinch },
        { 7, LengthUnit.Mil },
        { 8, LengthUnit.Inch },
        { 9, LengthUnit.Foot },
        { 10, LengthUnit.Mile },
        { 13, LengthUnit.Nanometer },
        { 14, LengthUnit.Decimeter },
        { 16, LengthUnit.Hectometer },
        { 19, LengthUnit.Yard }
      };
      return units[rhinoUnits.GetHashCode()];
    }

    public static Length GetRhinoTolerance(RhinoDoc doc = null) {
      if (doc == null) {
        doc = RhinoDoc.ActiveDoc;
      }

      if (doc == null) {
        return new Length(0.01, LengthUnit.Meter);
      }

      LengthUnit lengthUnit = GetRhinoLengthUnit();
      double tolerance = doc.ModelAbsoluteTolerance;
      return new Length(tolerance, lengthUnit);
    }
  }
}
