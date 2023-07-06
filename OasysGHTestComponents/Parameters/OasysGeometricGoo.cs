using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using static OasysGHTestComponents.OasysGHTestComponentsInfo;

namespace OasysGH.Parameters.Tests {
  public class OasysGeometricGoo : GH_OasysGeometricGoo<LineCurve> {
    public static string Description => "A LineCurve example";
    public static string Name => "LineCrv";
    public static string NickName => "LC";
    public override OasysPluginInfo PluginInfo => OasysGHTestComponentsPluginInfo.Instance;

    public OasysGeometricGoo(LineCurve item) : base(item) { }

    public override void DrawViewportMeshes(GH_PreviewMeshArgs args) { }
    public override void DrawViewportWires(GH_PreviewWireArgs args) { }
    public override IGH_GeometricGoo Duplicate() {
      return new OasysGeometricGoo(Value);
    }
    public override GeometryBase GetGeometry() {
      return Value == null ? null : (GeometryBase)Value;
    }
    public override IGH_GeometricGoo Morph(SpaceMorph xmorph) {
      return new OasysGeometricGoo(Value);
    }
    public override IGH_GeometricGoo Transform(Transform xform) {
      return new OasysGeometricGoo(Value);
    }
  }
}
