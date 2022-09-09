using Rhino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet.Units;
using UnitsNet;

namespace OasysGH.Units.Helpers
{
  public class RhinoUnit
  {
    public static Length GetRhinoTolerance()
    {
      LengthUnit lengthUnit = GetRhinoLengthUnit();
      double tolerance = RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
      return new Length(tolerance, lengthUnit);
    }

    public static LengthUnit GetRhinoLengthUnit()
    {
      return GetRhinoLengthUnit(RhinoDoc.ActiveDoc.ModelUnitSystem);
    }

    public static LengthUnit GetRhinoLengthUnit(Rhino.UnitSystem rhinoUnits)
    {
      List<LengthUnit> units = new List<LengthUnit>(new LengthUnit[] {
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
  }
}
