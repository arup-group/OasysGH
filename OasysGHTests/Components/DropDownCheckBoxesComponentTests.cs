using System.Collections.Generic;
using Grasshopper.Kernel.Types;
using OasysGH.Components.TestComponents;
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
  }
}
