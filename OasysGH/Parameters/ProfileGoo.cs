using Arup.Profiles;
using Grasshopper.Kernel.Types;

namespace OasysGH.Parameters
{
  public class ProfileGoo : GH_OasysGoo<IProfile>
  {
    public static string Name => "Profile";
    public static string NickName => "Pf";
    public static string Description => "Profile Parameter";
    public override OasysPluginInfo PluginInfo => OasysGH.PluginInfo.Instance;

    public ProfileGoo(IProfile item) : base(item)
    {
    }

    public override IGH_Goo Duplicate() => new ProfileGoo(this.Value);

    public override bool CastTo<Q>(ref Q target)
    {
      if (base.CastTo<Q>(ref target))
        return true;

      if (typeof(Q).IsAssignableFrom(typeof(string)) && this.Value is IGsaProfile profile)
      {
        if (this.Value == null)
          target = default;
        else
          target = (Q)(object)profile.GetGsaProfileString();
        return true;
      }

      target = default;
      return false;
    }
  }
}
