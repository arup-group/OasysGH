using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace OasysGH.Parameters {
  public abstract class GH_OasysGeometricGoo<T> : GH_GeometricGoo<T>, IGH_PreviewData {
    public override BoundingBox Boundingbox => GetGeometry().GetBoundingBox(true);

    public virtual BoundingBox ClippingBox => GetGeometry().GetBoundingBox(false);

    public override bool IsValid => GetGeometry() != null && GetGeometry().IsValid;
    public override string IsValidWhyNot {
      get {
        if (IsValid) {
          return string.Empty;
        }
        else {
          GetGeometry().IsValidWithLog(out string whyNot);
          return whyNot;
        }
      }
    }

    public abstract OasysPluginInfo PluginInfo { get; }

    public override string TypeDescription => PluginInfo.ProductName + " " + TypeName + " Parameter";
    public override string TypeName => typeof(T).Name.TrimStart('I').Replace("Gsa", string.Empty).Replace("AdSec", string.Empty);

    public GH_OasysGeometricGoo(T item) {
      Value = item;
    }

    public override bool CastFrom(object source) {
      if (source == null) {
        return false;
      }

      if (typeof(T).IsAssignableFrom(source.GetType())) {
        Value = (T)source;
        return true;
      }

      return false;
    }

    public override bool CastTo<Q>(ref Q target) {
      if (typeof(Q).IsAssignableFrom(typeof(T))) {
        if (Value == null) {
          target = default;
        }
        else {
          target = (Q)(object)Value;
        }

        return true;
      }

      target = default;
      return false;
    }

    public abstract void DrawViewportMeshes(GH_PreviewMeshArgs args);

    public abstract void DrawViewportWires(GH_PreviewWireArgs args);

    public new abstract IGH_GeometricGoo Duplicate();

    public override IGH_GeometricGoo DuplicateGeometry() => Duplicate();

    public override BoundingBox GetBoundingBox(Transform xform) {
      GeometryBase geom = GetGeometry();
      if (geom == null) {
        return BoundingBox.Empty;
      }
      BoundingBox bbox = geom.GetBoundingBox(true);
      bbox.Transform(xform);
      return bbox;
    }

    public abstract GeometryBase GetGeometry();

    public abstract override IGH_GeometricGoo Morph(SpaceMorph xmorph);

    public override string ToString() {
      if (Value == null) {
        return "Null";
      } else {
        return PluginInfo.ProductName + " " + TypeName + " (" + Value.ToString() + ")";
      }
    }

    public abstract override IGH_GeometricGoo Transform(Transform xform);
  }
}
