﻿using System.Windows.Forms;
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

      Assert.True(comp.IsInitialised);
      Assert.Equal(4, comp.SpacerDescriptions.Count);
      Assert.Equal(comp.DropDownItems.Count, comp.SelectedItems.Count);

      for (int i = 0; i < comp.DropDownItems.Count; i++) {
        comp.SetSelected(i, 0);

        for (int j = 0; j < comp.DropDownItems[i].Count; j++) {
          comp.SetSelected(i, j);
          comp.ExpireSolution(true);
          comp.Params.Output[0].CollectData();
          Assert.Equal(comp.SelectedItems[i], comp.DropDownItems[i][j]);
        }
      }
    }

    [Fact]
    public void ChangeDropDownCatalogueTest() {
      var comp = new CreateProfile();
      comp.CreateAttributes();

      Assert.True(comp.IsInitialised);
      Assert.Equal(4, comp.SpacerDescriptions.Count);
      Assert.Equal(comp.DropDownItems.Count, comp.SelectedItems.Count);

      comp.SetSelected(0, 1);

      // looping over catalogues, skipping first "All"
      for (int i = 1; i < comp.DropDownItems[1].Count; i++) {
        comp.SetSelected(1, i);

        // if selected catalogue does not contain any sections then check warning and continue
        if (comp.DropDownItems.Count < 3) {
          Assert.Single(comp.RuntimeMessages(Grasshopper.Kernel.GH_RuntimeMessageLevel.Warning));
          continue;
        }

        // looping over sections
        for (int j = 1; j < comp.DropDownItems[2].Count; j++) {
          comp.SetSelected(2, j);
          comp.ExpireSolution(true);
          comp.Params.Output[0].CollectData();
          Assert.Equal(comp.SelectedItems[2], comp.DropDownItems[2][j]);

          for (int k = 1; k < comp.DropDownItems[3].Count; k++) {
            comp.SetSelected(3, k);
            comp.ExpireSolution(true);
            comp.Params.Output[0].CollectData();
            Assert.Equal(comp.SelectedItems[3], comp.DropDownItems[3][k]);
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
