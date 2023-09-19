using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Grasshopper.Kernel.Types;
using OasysGH.Components.Tests;
using OasysGH.UI;
using OasysGHTests.TestHelpers;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class DropDownCheckBoxesComponentTests {
    [Fact]
    public void CheckBoxTest() {
      var comp = new DropDownCheckBoxesComponent();
      comp.CreateAttributes();
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      var initialCheckState = (GH_Boolean)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.False(initialCheckState.Value);

      comp.CheckBox(new List<bool>() { true });
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      var isChecked = (GH_Boolean)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.True(isChecked.Value);
    }

    [Fact]
    public void ChangeDropDownTest() {
      var comp = new DropDownCheckBoxesComponent();
      comp.CreateAttributes();
      DropDownComponentTests.ChangeDropDownTest(comp, true);
    }

    [Fact]
    public void TestAttributes() {
      var comp = new DropDownCheckBoxesComponent();
      Assert.True(Mouse.TestMouseMove(comp));
      Assert.True(Mouse.TestMouseClick(comp));
      var attributes = (DropDownCheckBoxesComponentAttributes)Document.Attributes(comp);
      attributes.CustomRender(new PictureBox().CreateGraphics());
    }
  }
}
