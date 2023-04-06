using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace OasysGH.Parameters {

  public abstract class GH_OasysPersistentGeometryParam<T> : GH_OasysPersistentParam<T>, IGH_PreviewObject where T : class, IGH_GeometricGoo {
    public virtual BoundingBox ClippingBox => Preview_ComputeClippingBox();

    public override bool Hidden { get; set; }

    public override bool IsPreviewCapable {
      get { return true; }
    }

    private bool _hidden = false;

    protected GH_OasysPersistentGeometryParam(GH_InstanceDescription nTag) : base(nTag) {
    }

    public virtual void DrawViewportMeshes(IGH_PreviewArgs args) {
      Preview_DrawMeshes(args);
    }

    public virtual void DrawViewportWires(IGH_PreviewArgs args) {
      Preview_DrawWires(args);
    }
  }
}
