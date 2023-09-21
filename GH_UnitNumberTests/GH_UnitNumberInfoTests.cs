using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using GH_UnitNumber;
using GH_UnitNumber.Components;
using GH_UnitNumberTests.Helpers;
using Grasshopper.Kernel;
using Grasshopper;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using OasysGH.Components;
using OasysUnits;
using Rhino.Geometry;
using Rhino.NodeInCode;
using Xunit;
using System.Linq;
using OasysGH;

namespace GH_UnitNumberTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class GH_UnitNumberInfoTests {

    [Fact]
    public void GH_UnitNumberInfoTest() {
      GH_UnitNumberInfo info = GetPlugin();
      Assert.Equal("https://www.oasys-software.com/", info.AuthorContact);
      Assert.Equal("Oasys", info.AuthorName);
      Assert.Equal(
        "A small plugin enabling use of units in Grasshopper through UnitsNet and OasysUnits",
        info.Description);
      Assert.Equal(new Guid("6080a841-4f35-4182-9922-f40a66977a69"), info.Id);
      Assert.Equal("UnitNumber", info.Name);
      Assert.NotNull(info.Version);
    }

    private GH_UnitNumberInfo GetPlugin() {
      ReadOnlyCollection<GH_AssemblyInfo> plugins = Instances.ComponentServer.Libraries;
      return (GH_UnitNumberInfo)plugins.Where(plugin => plugin.Name.StartsWith("UnitNumber")).FirstOrDefault();
    }
  }
}
