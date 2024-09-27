using OasysGH.Components;
using Xunit;

namespace GH_UnitNumberTests.Helpers {
  internal class Dropdown {
    internal static void ChangeDropDownDeserializeTest(
      GH_OasysDropDownComponent comp) {
      Assert.True(comp._isInitialised);
      Assert.Equal(comp.DropDownItems.Count, comp._spacerDescriptions.Count);

      Assert.Equal(comp.DropDownItems.Count, comp._selectedItems.Count);

      for (int i = 0; i < comp.DropDownItems.Count; i++) {
        comp.SetSelected(i, 0);

        for (int j = 0; j < comp.DropDownItems[i].Count; j++) {
          comp.SetSelected(i, j);
          Deserialize.TestDeserialize(comp);
          Assert.Equal(comp._selectedItems[i], comp.DropDownItems[i][j]);
        }
      }
    }
  }
}
