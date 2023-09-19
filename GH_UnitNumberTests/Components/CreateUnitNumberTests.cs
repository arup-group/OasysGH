using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using GH_IO.Serialization;
using GH_UnitNumber;
using GH_UnitNumber.Components;
using GH_UnitNumberTests.Helpers;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using OasysGH.Components;
using OasysUnits;
using Rhino.NodeInCode;
using Xunit;

namespace GH_UnitNumberTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class CreateUnitNumberTests {

    [Fact]
    public void AppendAdditionalMenuItemsTest() {
      var comp = new CreateUnitNumber();
      comp.CreateAttributes();
      ChangeDropDownTest(comp);
    }

    internal static void ChangeDropDownTest(
      GH_OasysDropDownComponent comp) {
      Assert.True(comp._isInitialised);
      Assert.Equal(comp._dropDownItems.Count, comp._spacerDescriptions.Count);

      Assert.Equal(comp._dropDownItems.Count, comp._selectedItems.Count);

      for (int i = 0; i < comp._dropDownItems.Count; i++) {
        comp.SetSelected(i, 0);

        for (int j = 0; j < comp._dropDownItems[i].Count; j++) {
          comp.SetSelected(i, j);
          TestDeserialize(comp);
          Assert.Equal(comp._selectedItems[i], comp._dropDownItems[i][j]);
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
