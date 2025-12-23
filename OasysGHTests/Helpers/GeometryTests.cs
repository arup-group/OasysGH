using System;
using System.Collections.Generic;
using GsaGHTests.Helpers;
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

    [Fact]
    public void TestPolyLineFromPlanarRectangularBrep() {
      var rectangle = new Rectangle3d(Plane.WorldXY, 10.0, 5.0);
      Brep brep = ComponentTestHelper.CreatePlanarBrep(rectangle.ToNurbsCurve());
      BrepPolylineResult result = Geometry.PolyLineFromBrep(brep);
      Assert.NotNull(result.Boundary);
      Assert.Equal(5, result.Boundary.Count);
      Assert.Empty(result.Voids);
      Assert.True(result.Plane.ZAxis.Z > 0);
    }

    [Fact]
    public void TestPolyLineFromBrepWithVoid() {
      var outerRectangle = new Rectangle3d(Plane.WorldXY, 10.0, 10.0);
      var innerRectangle = new Rectangle3d(new Plane(new Point3d(5, 5, 0), Vector3d.ZAxis), 2.5, 2.5);

      Brep outerBrep = ComponentTestHelper.CreatePlanarBrep(outerRectangle.ToNurbsCurve());
      Brep innerBrep = ComponentTestHelper.CreatePlanarBrep(innerRectangle.ToNurbsCurve());
      Brep[] difference = ComponentTestHelper.CreateBooleanDifference(outerBrep, innerBrep);

      Assert.NotNull(difference);
      Assert.Single(difference);

      Brep brep = difference[0];
      BrepPolylineResult result = Geometry.PolyLineFromBrep(brep);
      Assert.NotNull(result.Boundary);
      Assert.True(result.Boundary.Count == 5);
      Assert.NotNull(result.Voids);
      Assert.Single(result.Voids);
    }

    [Fact]
    public void TestPolyLineFromBrepWithMultipleVoids() {
      Plane plane = Plane.WorldXY;
      var outerRectangle = new Rectangle3d(plane, 20.0, 20.0);

      // Create two inner rectangles (voids)
      var innerPlane1 = new Plane(new Point3d(5, 5, 0), Vector3d.ZAxis);
      var innerRectangle1 = new Rectangle3d(innerPlane1, 2.0, 2.0);

      var innerPlane2 = new Plane(new Point3d(10, 10, 0), Vector3d.ZAxis);
      var innerRectangle2 = new Rectangle3d(innerPlane2, 2.0, 2.0);

      Brep outerBrep = ComponentTestHelper.CreatePlanarBrep(outerRectangle.ToNurbsCurve());
      Brep innerBrep1 = ComponentTestHelper.CreatePlanarBrep(innerRectangle1.ToNurbsCurve());
      Brep innerBrep2 = ComponentTestHelper.CreatePlanarBrep(innerRectangle2.ToNurbsCurve());

      Brep[] difference1 = ComponentTestHelper.CreateBooleanDifference(outerBrep, innerBrep1);
      Assert.NotNull(difference1);
      Assert.Single(difference1);

      Brep[] difference2 = ComponentTestHelper.CreateBooleanDifference(difference1[0], innerBrep2);
      Assert.NotNull(difference2);
      Assert.Single(difference2);

      Brep brep = difference2[0];
      BrepPolylineResult result = Geometry.PolyLineFromBrep(brep);
      Assert.NotNull(result.Boundary);
      Assert.NotNull(result.Voids);
      Assert.True(result.Voids.Count == 2);
    }

    [Fact]
    public void PolyLineFromBrepFlippedNormalTest() {
      var plane = new Plane(Point3d.Origin, -Vector3d.ZAxis);
      var rectangle = new Rectangle3d(plane, 10.0, 5.0);
      Brep brep = ComponentTestHelper.CreatePlanarBrep(rectangle.ToNurbsCurve());
      BrepPolylineResult result = Geometry.PolyLineFromBrep(brep);
      Assert.NotNull(result.Boundary);
      Assert.True(result.Plane.ZAxis.Z > 0);
    }

    [Fact]
    public void PolyLineFromBrepNonPlanarBrepTest() {
      var sphere = new Sphere(Point3d.Origin, 5.0);
      var brep = sphere.ToBrep();
      Exception ex = Assert.Throws<Exception>(() => Geometry.PolyLineFromBrep(brep));
      Assert.Contains("Cannot extract polyline from Brep", ex.Message);
    }

    [Fact]
    public void PolyLineFromBrepInvalidCurvedEdgeTest() {
      // A circle is a continuous curved shape without straight segments and hence no polyline can be extracted
      var circle = new Circle(Plane.WorldXY, 5.0);
      Brep brep = ComponentTestHelper.CreatePlanarBrep(circle.ToNurbsCurve());
      Exception ex = Assert.Throws<Exception>(() => Geometry.PolyLineFromBrep(brep));
      Assert.Contains("Cannot extract polyline from Brep", ex.Message);
    }

    [Fact]
    public void PolyLineFromBrepNullBrepTest() {
      Brep brep = null;
      Assert.Throws<NullReferenceException>(() => Geometry.PolyLineFromBrep(brep));
    }

    [Fact]
    public void PolyLineFromBrepMultiFaceBrepTest() {
      var box = new Box(Plane.WorldXY, new Interval(-5, 5), new Interval(-5, 5), new Interval(0, 10));
      var brep = box.ToBrep();
      BrepPolylineResult result = Geometry.PolyLineFromBrep(brep);
      Assert.NotNull(result.Boundary);
      Assert.Equal(5, result.Boundary.Count);
    }
  }
}
