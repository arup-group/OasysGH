using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel.Types;
using static OasysGHComponentTests.OasysGHComponentTestsInfo;

namespace OasysGH.Parameters.Tests {
  public class OasysGoo : GH_OasysGoo<GH_Boolean> {
    public static string Description => "A boolean example";
    public static string Name => "Boolean";
    public static string NickName => "B";
    public override OasysPluginInfo PluginInfo => OasysGHComponentTestsPluginInfo.Instance;

    public OasysGoo(GH_Boolean item) : base(item) { }

    public override IGH_Goo Duplicate() {
      return new OasysGoo(Value);
    } 
  }
}
