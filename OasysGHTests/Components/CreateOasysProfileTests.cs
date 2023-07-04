using OasysGH.Components.TestComponents;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class CreateOasysProfileTests {
    [Fact]
    public static void ChangeDropDownTest() {
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
  }
}
