using Grasshopper.Kernel.Types;
using Oasys.Taxonomy.Profiles;

namespace OasysGH.Parameters {
  public class OasysProfileGoo : GH_OasysGoo<IProfile> {
    public static string Description => "GSA Profile";
    public static string Name => "Profile";
    public static string NickName => "Pf";
    public override OasysPluginInfo PluginInfo => OasysGH.PluginInfo.Instance;

    public OasysProfileGoo() : base() { }

    public OasysProfileGoo(IProfile item) : base(item) { }

    public override IGH_Goo Duplicate() => new OasysProfileGoo(Value);

    public override string ToString() => Value.ToString();
  }
}
