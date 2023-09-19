using System.Windows.Forms;
using Grasshopper.Kernel.Types;
using OasysGH.Components.Tests;
using OasysGH.UI;
using OasysGHTests.TestHelpers;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class DropDownSliderComponentTests {
    [Fact]
    public void ChangeSlider() {
      var comp = new DropDownSliderComponent();
      comp.CreateAttributes();
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      var initialValue = (GH_Number)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.Equal(500, initialValue.Value);
      var initialMax = (GH_Number)comp.Params.Output[1].VolatileData.get_Branch(0)[0];
      Assert.Equal(1000, initialMax.Value);
      var initialMin = (GH_Number)comp.Params.Output[2].VolatileData.get_Branch(0)[0];
      Assert.Equal(-250, initialMin.Value);

      double newValue = 750;
      comp.SetVal(newValue);
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      var newValueOut = (GH_Number)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.Equal(newValue, newValueOut.Value);

      double max = 1500;
      double min = 0;
      comp.SetMaxMin(max, min);
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      newValueOut = (GH_Number)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.Equal(newValue, newValueOut.Value);
      var newMax = (GH_Number)comp.Params.Output[1].VolatileData.get_Branch(0)[0];
      Assert.Equal(max, newMax.Value);
      var newMin = (GH_Number)comp.Params.Output[2].VolatileData.get_Branch(0)[0];
      Assert.Equal(min, newMin.Value);
    }

    [Fact]
    public void ChangeDropDownTest() {
      var comp = new DropDownSliderComponent();
      comp.CreateAttributes();
      DeserializeTests.ChangeDropDownTest(comp, true);
    }

    [Fact]
    public void TestAttributes() {
      var comp = new DropDownSliderComponent();
      Assert.True(Mouse.TestMouseMove(comp));
      Assert.True(Mouse.TestMouseClick(comp));
      var attributes = (DropDownSliderComponentAttributes)Document.Attributes(comp);
      attributes.CustomRender(new PictureBox().CreateGraphics());
    }
  }
}
