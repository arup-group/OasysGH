using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using OasysGH.Components;
using OasysGH.Components.Tests;
using OasysGH.UI;
using OasysGHTests.TestHelpers;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class DropDownComponentTests {
    [Fact]
    public void ChangeDropDownComponentTest() {
      var comp = new DropDownComponent();
      comp.CreateAttributes();
      ChangeDropDownTest(comp);
    }

    [Fact]
    public void CanInsertRemoveParamsTest() {
      IGH_VariableParameterComponent comp = new DropDownComponent();
      Assert.False(comp.CanInsertParameter(GH_ParameterSide.Input, 0));
      Assert.False(comp.CanInsertParameter(GH_ParameterSide.Output, 0));
      Assert.False(comp.CanRemoveParameter(GH_ParameterSide.Input, 0));
      Assert.False(comp.CanRemoveParameter(GH_ParameterSide.Output, 0));
      Assert.Null(comp.CreateParameter(GH_ParameterSide.Input, 0));
      Assert.False(comp.DestroyParameter(GH_ParameterSide.Input, 0));
      Assert.False(comp.DestroyParameter(GH_ParameterSide.Output, 0));
    }

    [Fact]
    public void TestAttributes() {
      var comp = new DropDownComponent();
      Assert.True(Mouse.TestMouseMove(comp));
      Assert.True(Mouse.TestMouseClick(comp));
      var attributes = (DropDownComponentAttributes)Document.Attributes(comp);
      attributes.CustomRender(new PictureBox().CreateGraphics());
    }

    internal static void ChangeDropDownTest(
      GH_OasysDropDownComponent comp, bool ignoreSpacerDescriptionsCount = false) {
      Assert.True(comp._isInitialised);
      if (!ignoreSpacerDescriptionsCount) {
        Assert.Equal(comp._dropDownItems.Count, comp._spacerDescriptions.Count);
      }

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

    internal static void TestDeserialize(GH_OasysComponent comp, string customIdentifier = "") {
      comp.CreateAttributes();

      var doc = new GH_Document();
      doc.AddObject(comp, true);

      var serialize = new GH_DocumentIO {
        Document = doc,
      };
      var originalComponent = (GH_Component)serialize.Document.Objects[0];
      originalComponent.Attributes.PerformLayout();
      originalComponent.ExpireSolution(true);
      originalComponent.Params.Output[0].CollectData();

      string path = Path.Combine(Environment.CurrentDirectory, "GH-Test-Files");
      Directory.CreateDirectory(path);
      Type myType = comp.GetType();
      string pathFileName = Path.Combine(path, myType.Name) + customIdentifier + ".gh";
      Assert.True(serialize.SaveQuiet(pathFileName));

      var deserialize = new GH_DocumentIO();
      Assert.True(deserialize.Open(pathFileName));

      var deserializedComponent = (GH_Component)deserialize.Document.Objects[0];
      deserializedComponent.Attributes.PerformLayout();
      deserializedComponent.ExpireSolution(true);
      deserializedComponent.Params.Output[0].CollectData();

      Duplicates.AreEqual(originalComponent, deserializedComponent, true);
      doc.Dispose();
    }
  }
}
