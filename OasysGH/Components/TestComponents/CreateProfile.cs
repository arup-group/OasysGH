using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using OasysGH.Properties;

namespace OasysGH.Components.TestComponents {
  public class CreateProfile : CreateOasysProfile {
    public override Guid ComponentGuid => new Guid("01206123-4a6a-4694-8cb8-ad42d5530b9c");
    public override GH_Exposure Exposure => GH_Exposure.hidden;
    public override OasysPluginInfo PluginInfo => OasysGH.PluginInfo.Instance;

    public override string DataSource => Path.Combine(
      Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName,
      "lib", "sectlib.db3");

    public CreateProfile() : base("Create Profile", "Profile",
      "Create Profile text-string for a GSA Section", "OasysGH", "Test") { }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddGenericParameter("Profile", "Pf", "Profile for a GSA Section", GH_ParamAccess.tree);
    }
  }
}
