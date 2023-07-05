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
    public static void CheckBoxTest() {
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
    public static void ChangeDropDownTest() {
      var comp = new DropDownCheckBoxesComponent();
      comp.CreateAttributes();

      Assert.True(comp._isInitialised);
      Assert.Equal(2, comp._spacerDescriptions.Count);
      Assert.Equal(comp._dropDownItems.Count, comp._selectedItems.Count);

      for (int i = 0; i < comp._dropDownItems.Count; i++) {
        comp.SetSelected(i, 0);

        for (int j = 0; j < comp._dropDownItems[i].Count; j++) {
          comp.SetSelected(i, j);
          comp.ExpireSolution(true);
          comp.Params.Output[0].CollectData();
          Assert.Equal(comp._selectedItems[i], comp._dropDownItems[i][j]);
        }
      }
    }

    [Fact]
    public static void TestAttributes() {
      var comp = new DropDownCheckBoxesComponent();
      Assert.True(Mouse.TestMouseMove(comp));
      Assert.True(Mouse.TestMouseClick(comp));
      var attributes = (DropDownCheckBoxesComponentAttributes)Document.Attributes(comp);
      attributes.CustomRender(new PictureBox().CreateGraphics());
    }
  }
}
