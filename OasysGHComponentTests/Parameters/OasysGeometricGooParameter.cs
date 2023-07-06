using System;
using Grasshopper.Kernel;

namespace OasysGH.Parameters.Tests {
  public class OasysGeometricGooParameter : GH_OasysPersistentGeometryParam<OasysGeometricGoo> {
    public override Guid ComponentGuid => new Guid("186a4d18-8069-467b-83b6-636a894bc3a1");
    public override GH_Exposure Exposure => GH_Exposure.hidden;
    public override string InstanceDescription => m_data.DataCount == 0
      ? "Empty " + OasysGeometricGoo.Name + " parameter"
      : base.InstanceDescription;
    public override string TypeName => SourceCount == 0 ? OasysGeometricGoo.Name : base.TypeName;

    public OasysGeometricGooParameter() : base(new GH_InstanceDescription(OasysGeometricGoo.Name,
      OasysGeometricGoo.NickName, OasysGeometricGoo.Description + " parameter", "OasysGH", "Test")) { }
  }
}
