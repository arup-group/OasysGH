using Grasshopper.Kernel.Types;
using Oasys.Taxonomy.Profiles;

namespace OasysGH.Parameters {

  public class OasysProfileGoo : GH_OasysGoo<IProfile> {
    public static string Description => "Profile Parameter";
    public static string Name => "Profile";
    public static string NickName => "Pf";
    public override OasysPluginInfo PluginInfo => OasysGH.PluginInfo.Instance;

    public OasysProfileGoo(IProfile item) : base(item) {
    }

    public override IGH_Goo Duplicate() => new OasysProfileGoo(Value);
  }
}
