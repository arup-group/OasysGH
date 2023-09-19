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
    public static List<IPoint2d> PointsFromRhinoPolyline(Polyline polyline, LengthUnit lengthUnit, Plane local) {
      if (polyline.First() != polyline.Last()) {
        polyline.Add(polyline.First());
      }

      var points = new List<IPoint2d>();

      // map points to XY plane so we can create local points from x and y coordinates
      var xform = Rhino.Geometry.Transform.PlaneToPlane(local, Plane.WorldXY);

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
  }
}
