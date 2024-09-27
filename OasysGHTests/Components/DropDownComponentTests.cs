using System;
using System.Windows.Forms;
using Grasshopper.Kernel;
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
      DeserializeTests.ChangeDropDownTest(comp);
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

    [Fact]
    public void NoSelectionsTest() {
      var comp = new DropDownComponent();
      comp._selectedItems = null;
      comp._isInitialised = true;
      comp.CreateAttributes();
      Assert.NotNull(comp.Attributes);
    }

    [Fact]
    public void IGH_VariableParameterComponentTest() {
      var comp = (IGH_VariableParameterComponent)new DropDownComponent();
      Assert.False(comp.CanRemoveParameter(GH_ParameterSide.Input, 0));
      Assert.False(comp.CanRemoveParameter(GH_ParameterSide.Output, 0));
      Assert.False(comp.CanInsertParameter(GH_ParameterSide.Input, 0));
      Assert.False(comp.CanInsertParameter(GH_ParameterSide.Output, 0));
    }
  }
}
