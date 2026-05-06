using Grasshopper.Kernel.Types;
using Oasys.Taxonomy.Profiles;
using OasysGH.Helpers;

namespace OasysGH.Parameters {
  public class OasysProfileGoo : GH_OasysGoo<IProfile> {
    public static string Description => "GSA Profile";
    public static string Name => "Profile";
    public static string NickName => "Pf";
    public override OasysPluginInfo PluginInfo => OasysGH.PluginInfo.Instance;

    public OasysProfileGoo() : base() { }

    public OasysProfileGoo(IProfile item) : base(item) { }

    public override bool CastFrom(object source) {
      if (base.CastFrom(source))
        return true;

      string str = null;
      if (source is string s)
        str = s;
      else if (source is GH_String gh)
        str = gh.Value;

      if (str != null) {
        try {
          Value = ProfileHelper.ProfileFromString(str);
          return true;
        } catch {
          return false;
        }
      }

      return false;
    }

    public override IGH_Goo Duplicate() => new OasysProfileGoo(Value);

    public override string ToString() => Value.ToString();
  }
}

