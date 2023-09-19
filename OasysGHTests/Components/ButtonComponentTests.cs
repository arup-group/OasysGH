using System.Drawing;
using System.Windows.Forms;
using Grasshopper.Kernel.Types;
using OasysGH.Components.Tests;
using OasysGH.UI;
using OasysGHTests.TestHelpers;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class ButtonComponentTests {
    [Fact]
    public void ClickButtonTest() {
      var comp = new ButtonComponent();
      comp.CreateAttributes();
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      var wasClickedInitial = (GH_Boolean)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.False(wasClickedInitial.Value);

      comp.Clicked();
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      var wasClicked = (GH_Boolean)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.True(wasClicked.Value);
    }


    [Fact]
    public void TestAttributes() {
      var comp = new ButtonComponent();
      Assert.True(Mouse.TestMouseMove(comp));
      Assert.True(Mouse.TestMouseClick(comp));
      var attributes = (ButtonComponentAttributes)Document.Attributes(comp);
      Graphics g = new PictureBox().CreateGraphics();
      attributes.CustomRender(g);
    }
  }
}
