using System;
using System.Drawing;
using Grasshopper.Kernel;
using OasysGH;

namespace OasysGHTestComponents {
  public class OasysGHTestComponentsInfo : GH_AssemblyInfo {
    public override string Name => "OasysGHTestComponents";

    //Return a 24x24 pixel bitmap to represent this GHA library.
    public override Bitmap Icon => null;

    //Return a short string describing the purpose of this GHA library.
    public override string Description => "";

    public override Guid Id => new Guid("ebee4f06-0137-4c97-a457-3ecb1e1940fc");

    //Return a string identifying you or your company.
    public override string AuthorName => "";

    //Return a string representing your preferred contact details.
    public override string AuthorContact => "";

    internal sealed class OasysGHTestComponentsPluginInfo {
      public static OasysPluginInfo Instance { get { return lazy.Value; } }

      private static readonly Lazy<OasysPluginInfo> lazy =
              new Lazy<OasysPluginInfo>(() => new OasysPluginInfo(
            "OasysGH",
            "OasysGHTestComponents",
            "0.0.1",
            true,
            "phc_alOp3OccDM3D18xJTWDoW44Y1cJvbEScm5LJSX8qnhs"
            ));

      private OasysGHTestComponentsPluginInfo() {
      }
    }
  }
}
