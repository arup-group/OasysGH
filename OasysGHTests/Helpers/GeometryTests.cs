using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Oasys.Taxonomy.Geometry;
using OasysGH.Helpers;
using OasysUnits.Units;
using Rhino.Geometry;
using Xunit;

namespace OasysGHTests.Helpers {
  [Collection("GrasshopperFixture collection")]
  public class GeometryTests {
    [Fact]
    public void PointsFromRhinoPolylineTest() {
      var polyline = new Polyline {
        new Point3d(0, 0, 0),
        new Point3d(1, 0, 0),
        new Point3d(1, 1, 0)
      };

      List<IPoint2d> pts = Geometry.PointsFromRhinoPolyline(polyline, LengthUnit.Meter, Plane.WorldXY);
      Assert.Equal(3, pts.Count);
    }
  }
}
