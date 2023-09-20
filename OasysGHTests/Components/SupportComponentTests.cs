using System.Windows.Forms;
using Grasshopper.Kernel.Types;
using OasysGH.Components.Tests;
using OasysGH.UI;
using OasysGHTests.TestHelpers;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class SupportComponentTests {
    [Theory]
    [InlineData(true, true, true, true, true, true)]
    [InlineData(false, false, false, false, false, false)]
    [InlineData(true, false, true, false, true, false)]
    [InlineData(false, true, false, true, false, true)]
    public void ChangeCheckBoxesTest(bool x, bool y, bool z, bool xx, bool yy, bool zz) {
      var comp = new SupportComponent();
      comp.CreateAttributes();

      comp.SetRestraints(x, y, z, xx, yy, zz);

      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      var outx = (GH_Boolean)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      var outy = (GH_Boolean)comp.Params.Output[1].VolatileData.get_Branch(0)[0];
      var outz = (GH_Boolean)comp.Params.Output[2].VolatileData.get_Branch(0)[0];
      var outxx = (GH_Boolean)comp.Params.Output[3].VolatileData.get_Branch(0)[0];
      var outyy = (GH_Boolean)comp.Params.Output[4].VolatileData.get_Branch(0)[0];
      var outzz = (GH_Boolean)comp.Params.Output[5].VolatileData.get_Branch(0)[0];

      Assert.Equal(x, outx.Value);
      Assert.Equal(y, outy.Value);
      Assert.Equal(z, outz.Value);
      Assert.Equal(xx, outxx.Value);
      Assert.Equal(yy, outyy.Value);
      Assert.Equal(zz, outzz.Value);
    }

    [Fact]
    public void TestAttributes() {
      var comp = new SupportComponent();
      Assert.True(Mouse.TestMouseMove(comp));
      Assert.True(Mouse.TestMouseClick(comp));
      var attributes = (SupportComponentAttributes)Document.Attributes(comp);
      attributes.CustomRender(new PictureBox().CreateGraphics());
    }
  }
}
