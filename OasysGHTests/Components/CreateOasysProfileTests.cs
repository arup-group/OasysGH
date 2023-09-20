using System.Windows.Forms;
using OasysGH.Components.Tests;
using OasysGH.UI;
using OasysGHTests.TestHelpers;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class CreateOasysProfileTests {
    [Fact]
    public void ChangeDropDownTest() {
      var comp = new CreateProfile();
      comp.CreateAttributes();

      Assert.True(comp._isInitialised);
      Assert.Equal(4, comp._spacerDescriptions.Count);
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
    public void ChangeDropDownCatalogueTest() {
      var comp = new CreateProfile();
      comp.CreateAttributes();

      Assert.True(comp._isInitialised);
      Assert.Equal(4, comp._spacerDescriptions.Count);
      Assert.Equal(comp._dropDownItems.Count, comp._selectedItems.Count);

      comp.SetSelected(0, 1);

      // looping over catalogues, skipping first "All"
      for (int i = 1; i < comp._dropDownItems[1].Count; i++) {
        comp.SetSelected(1, i);

        // if selected catalogue does not contain any sections then check warning and continue
        if (comp._dropDownItems.Count < 3) {
          Assert.Single(comp.RuntimeMessages(Grasshopper.Kernel.GH_RuntimeMessageLevel.Warning));
          continue;
        }

        // looping over sections
        for (int j = 1; j < comp._dropDownItems[2].Count; j++) {
          comp.SetSelected(2, j);
          comp.ExpireSolution(true);
          comp.Params.Output[0].CollectData();
          Assert.Equal(comp._selectedItems[2], comp._dropDownItems[2][j]);

          // if selected section type does not contain any profiles then check warning and continue
          if (comp._dropDownItems.Count < 4) {
            Assert.Single(comp.RuntimeMessages(Grasshopper.Kernel.GH_RuntimeMessageLevel.Warning));
            continue;
          }

          for (int k = 1; k < comp._dropDownItems[3].Count; k++) {
            comp.SetSelected(3, k);
            comp.ExpireSolution(true);
            comp.Params.Output[0].CollectData();
            Assert.Equal(comp._selectedItems[3], comp._dropDownItems[3][k]);
          }
        }
      }
    }

    [Fact]
    public void TestAttributes() {
      var comp = new CreateProfile();
      Assert.True(Mouse.TestMouseMove(comp));
      Assert.True(Mouse.TestMouseClick(comp));
      var attributes = (DropDownComponentAttributes)Document.Attributes(comp);
      attributes.CustomRender(new PictureBox().CreateGraphics());
    }
  }
}
