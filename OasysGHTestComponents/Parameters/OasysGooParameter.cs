using System;
using Grasshopper.Kernel;

namespace OasysGH.Parameters.Tests {
  public class OasysGooParameter : GH_OasysPersistentParam<OasysGoo> {
    public override Guid ComponentGuid => new Guid("c5dee968-20b0-4f12-834b-ed18259883f5");
    public override GH_Exposure Exposure => GH_Exposure.hidden;
    public override string InstanceDescription => m_data.DataCount == 0
      ? "Empty " + OasysGoo.Name + " parameter"
      : base.InstanceDescription;
    public override string TypeName => SourceCount == 0 ? OasysGoo.Name : base.TypeName;

    public OasysGooParameter() : base(new GH_InstanceDescription(OasysGoo.Name,
      OasysGoo.NickName, OasysGoo.Description + " parameter", "OasysGH", "Test")) { }
  }
}
