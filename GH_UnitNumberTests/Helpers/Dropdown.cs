using OasysGH.Components;
using Xunit;

namespace GH_UnitNumberTests.Helpers {
  internal class Dropdown {
    internal static void ChangeDropDownDeserializeTest(
      GH_OasysDropDownComponent comp) {
      Assert.True(comp._isInitialised);
      Assert.Equal(comp._dropDownItems.Count, comp._spacerDescriptions.Count);

      Assert.Equal(comp._dropDownItems.Count, comp._selectedItems.Count);

      for (int i = 0; i < comp._dropDownItems.Count; i++) {
        comp.SetSelected(i, 0);

        for (int j = 0; j < comp._dropDownItems[i].Count; j++) {
          comp.SetSelected(i, j);
          Deserialize.TestDeserialize(comp);
          Assert.Equal(comp._selectedItems[i], comp._dropDownItems[i][j]);
        }
      }
    }
  }
}
