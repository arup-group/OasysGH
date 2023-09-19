using System;
using System.Collections.Generic;
using System.Linq;
using Oasys.Taxonomy.Geometry;
using Oasys.Taxonomy.Profiles;
using OasysGH.Units;
using OasysUnits;
using OasysUnits.Units;
using Rhino.Geometry;

namespace OasysGH.Helpers {
  public static class Geometry {

    public static Tuple<List<Point3d>, List<List<Point3d>>> PointsFromPerimeterProfile(IPerimeterProfile profile, Plane local) {
      if (profile == null) {
        return null;
      }

      IPolygon solid = profile.Perimeter;
      List<Point3d> rhEdgePts = PointsFromPolygon(solid, local);

      var rhVoidPts = new List<List<Point3d>>();
      foreach (IPolygon vpol in profile.VoidPolygons) {
        rhVoidPts.Add(PointsFromPolygon(vpol, local));
      }

      return new Tuple<List<Point3d>, List<List<Point3d>>>(rhEdgePts, rhVoidPts);
    }

    public static List<Point3d> PointsFromPolygon(IPolygon polygon, Plane local) {
      if (polygon == null) {
        return null;
      }

      // transform to local plane
      var maptToLocal = Transform.PlaneToPlane(Plane.WorldYZ, local);

      var rhPts = new List<Point3d>();

      foreach (IPoint2d apt in polygon.Points) {
        var pt = new Point3d(0,
          apt.Y.As(DefaultUnits.LengthUnitGeometry),
          apt.Z.As(DefaultUnits.LengthUnitGeometry));
        pt.Transform(maptToLocal);
        rhPts.Add(pt);
      }

      // add first point to end of list for closed polyline
      rhPts.Add(rhPts[0]);

      return rhPts;
    }

    public static List<IPoint2d> PointsFromRhinoPolyline(Polyline polyline, LengthUnit lengthUnit, Plane local) {
      if (polyline == null) {
        return null;
      }
      if (polyline.First() != polyline.Last()) {
        polyline.Add(polyline.First());
      }

      var points = new List<IPoint2d>();

      // map points to XY plane so we can create local points from x and y coordinates
      var xform = Transform.PlaneToPlane(local, Plane.WorldXY);

      for (int i = 0; i < polyline.Count - 1; i++)
      // -1 on count because the profile is always closed and thus doesn´t
      // need the last point being equal to first as a rhino polyline needs
      {
        Point3d point3d = polyline[i];
        point3d.Transform(xform);
        IPoint2d point2d = new Oasys.Taxonomy.Geometry.Point2d(
          new Length(point3d.X, lengthUnit),
          new Length(point3d.Y, lengthUnit));
        points.Add(point2d);
      }

      return points;
    }

    public static IPolygon PolygonFromRhinoPolyline(Polyline polyline, LengthUnit lengthUnit, Plane local) {
      var polygon = new Polygon() {
        Points = PointsFromRhinoPolyline(polyline, lengthUnit, local)
      };
      return polygon;
    }

    public static Tuple<Polyline, List<Polyline>> PolylinesFromProfile(IProfile profile, Plane local) {
      IPerimeterProfile perimeter = new PerimeterProfile();

      Tuple<List<Point3d>, List<List<Point3d>>> pts = PointsFromPerimeterProfile(perimeter, local);

      var solid = new Polyline(pts.Item1);
      var voids = new List<Polyline>();
      foreach (List<Point3d> plvoid in pts.Item2) {
        voids.Add(new Polyline(plvoid));
      }

      return new Tuple<Polyline, List<Polyline>>(solid, voids);
    }
  }
}
