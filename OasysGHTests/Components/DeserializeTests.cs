using GH_IO.Serialization;
using OasysGH.Components;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class DeserializeTests {
    internal static void ChangeDropDownTest(
      GH_OasysDropDownComponent comp, bool ignoreSpacerDescriptionsCount = false) {
      Assert.True(comp.IsInitialised);
      if (!ignoreSpacerDescriptionsCount) {
        Assert.Equal(comp.DropDownItems.Count, comp.SpacerDescriptions.Count);
      }

      Assert.Equal(comp.DropDownItems.Count, comp.SelectedItems.Count);

      for (int i = 0; i < comp.DropDownItems.Count; i++) {
        comp.SetSelected(i, 0);

        for (int j = 0; j < comp.DropDownItems[i].Count; j++) {
          comp.SetSelected(i, j);
          TestDeserialize(comp);
          Assert.Equal(comp.SelectedItems[i], comp.DropDownItems[i][j]);
        }
      }
    }

    internal static void TestDeserialize(GH_OasysComponent comp) {
      var write = new GH_Archive();
      Assert.True(write.AppendObject(comp, "Component"));
      string xml = write.Serialize_Xml();

      var read = new GH_Archive();
      Assert.True(read.Deserialize_Xml(xml));

      read.ExtractObject(comp, "Component");
    }
  }
}
