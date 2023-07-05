using Grasshopper.Kernel.Types;
using OasysGH.Components.Tests;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class DropDownSliderComponentTests {
    [Fact]
    public static void ChangeSlider() {
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
    public static void ChangeDropDownTest() {
      var comp = new DropDownSliderComponent();
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
