using OasysGH.Components;
using Xunit;

namespace GH_UnitNumberTests.Helpers {
  internal class Dropdown {
    internal static void ChangeDropDownDeserializeTest(
      GH_OasysDropDownComponent comp) {
      Assert.True(comp.IsInitialised);
      Assert.Equal(comp.DropDownItems.Count, comp.SpacerDescriptions.Count);

      Assert.Equal(comp.DropDownItems.Count, comp.SelectedItems.Count);

      for (int i = 0; i < comp.DropDownItems.Count; i++) {
        comp.SetSelected(i, 0);

        for (int j = 0; j < comp.DropDownItems[i].Count; j++) {
          comp.SetSelected(i, j);
          Deserialize.TestDeserialize(comp);
          Assert.Equal(comp.SelectedItems[i], comp.DropDownItems[i][j]);
        }
      }
    }
  }
}
