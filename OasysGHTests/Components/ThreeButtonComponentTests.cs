using System.Drawing;
using System.Windows.Forms;
using Grasshopper.Kernel.Types;
using OasysGH.Components.Tests;
using OasysGH.UI;
using OasysGHTests.TestHelpers;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class ThreeButtonComponentTests {
    [Fact]
    public void ClickFirstButtonTest() {
      var comp = new ThreeButtonComponent();
      comp.CreateAttributes();
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      var wasClickedInitial = (GH_Boolean)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.False(wasClickedInitial.Value);

      comp.ClickedFirst();
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      var wasClicked = (GH_Boolean)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.True(wasClicked.Value);
    }

    [Fact]
    public void ClickSecondButtonTest() {
      var comp = new ThreeButtonComponent();
      comp.CreateAttributes();
      comp.ExpireSolution(true);
      comp.Params.Output[1].CollectData();
      var wasClickedInitial = (GH_Boolean)comp.Params.Output[1].VolatileData.get_Branch(0)[0];
      Assert.False(wasClickedInitial.Value);

      comp.ClickedSecond();
      comp.ExpireSolution(true);
      comp.Params.Output[1].CollectData();
      var wasClicked = (GH_Boolean)comp.Params.Output[1].VolatileData.get_Branch(0)[0];
      Assert.True(wasClicked.Value);
    }

    [Fact]
    public void ClickThirdButtonTest() {
      var comp = new ThreeButtonComponent();
      comp.CreateAttributes();
      comp.ExpireSolution(true);
      comp.Params.Output[2].CollectData();
      var wasClickedInitial = (GH_Boolean)comp.Params.Output[2].VolatileData.get_Branch(0)[0];
      Assert.False(wasClickedInitial.Value);

      comp.ClickedThird();
      comp.ExpireSolution(true);
      comp.Params.Output[2].CollectData();
      var wasClicked = (GH_Boolean)comp.Params.Output[2].VolatileData.get_Branch(0)[0];
      Assert.True(wasClicked.Value);
    }

    [Fact]
    public void TestAttributes() {
      var comp = new ThreeButtonComponent();
      Assert.True(Mouse.TestMouseMove(comp));
      Assert.True(Mouse.TestMouseClick(comp));
      var attributes = (ThreeButtonComponentAttributes)Document.Attributes(comp);
      attributes.CustomRender(new PictureBox().CreateGraphics());
    }
  }
}
