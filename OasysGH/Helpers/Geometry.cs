using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel.Geometry.Delaunay;
using Oasys.Taxonomy.Geometry;
using Oasys.Taxonomy.Profiles;
using OasysGH.Units;
using OasysUnits;
using OasysUnits.Units;
using Rhino.Geometry;

namespace OasysGH.Helpers {

  public readonly struct BrepPolylineResult {
    public Polyline Boundary { get; }
    public List<Polyline> Voids { get; }
    public Plane Plane { get; }

    public BrepPolylineResult(Polyline boundary, List<Polyline> voids, Plane plane) {
      Boundary = boundary;
      Voids = voids;
      Plane = plane;
    }
  }

  public static class Geometry {
    public static List<IPoint2d> PointsFromRhinoPolyline(Polyline polyline, LengthUnit lengthUnit, Plane local) {
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

    public static BrepPolylineResult PolyLineFromBrep(Brep brep) {

      BrepFace mainFace = brep.Faces.OrderByDescending(face => {
        BoundingBox bbox = face.GetBoundingBox(true);
        return bbox.Area;
      }).FirstOrDefault();

      if (!mainFace.OuterLoop.To3dCurve().TryGetPolyline(out Polyline polyline)) {
        throw new Exception("Cannot extract polyline from Brep surface.");
      }

      List<Polyline> voids = ExtractInnerVoids(mainFace);

      Plane plane = PlaneFromFace(mainFace);

      return new BrepPolylineResult(polyline, voids, plane);
    }

    private static Plane PlaneFromFace(BrepFace mainFace) {
      mainFace.TryGetPlane(out Plane plane);
      // planer normal should point upwards
      // for consistent profile creation
      if (plane.Normal.Z < 0) {
        plane = new Plane(plane.Origin, -plane.Normal);
      }
      return plane;
    }

    private static List<Polyline> ExtractInnerVoids(BrepFace mainFace) {
      var voids = new List<Polyline>();
      foreach (BrepLoop loop in mainFace.Loops) {
        if (loop.LoopType == BrepLoopType.Inner) {
          Curve voidCurve = loop.To3dCurve();
          if (voidCurve.TryGetPolyline(out Polyline voidPolyline)) {
            voids.Add(voidPolyline);
          }
          else {
            throw new Exception("Cannot extract polyline from Brep inner loop.");
          }
        }
      }
      return voids;
    }
  }
}
