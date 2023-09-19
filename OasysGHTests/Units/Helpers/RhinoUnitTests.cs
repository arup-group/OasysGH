using OasysGH.Units.Helpers;
using OasysUnits;
using OasysUnits.Units;
using Xunit;

namespace OasysGHTests.Units {
  [Collection("GrasshopperFixture collection")]
  public class RhinoUnitTests {
    [Fact]
    public static void GetRhinoTolerance() {
      var doc = Rhino.RhinoDoc.CreateHeadless(null);
      Length tolerance = RhinoUnit.GetRhinoTolerance(doc);
      Assert.Equal(new Length(0.001, LengthUnit.Meter), tolerance);
      doc.Dispose();
    }

    [Fact]
    public static void GetRhinoLength() {
      var doc = Rhino.RhinoDoc.CreateHeadless(null);
      LengthUnit unit = RhinoUnit.GetRhinoLengthUnit(doc);
      Assert.Equal(LengthUnit.Millimeter, unit);
      doc.Dispose();
    }
  }
}
