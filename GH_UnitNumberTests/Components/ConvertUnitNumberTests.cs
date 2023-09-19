using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
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
  public class ConvertUnitNumberTests {

    [Fact]
    public void AppendAdditionalMenuItemsTest() {
      var comp = new ConvertUnitNumber();
      var form = new ContextMenuStrip();
      comp.AppendAdditionalMenuItems(form);
      Assert.Equal(3, form.Items.Count);
    }

    [Fact]
    public void CreateTextPanelTest() {
      var l = new Length(3.5, OasysUnits.Units.LengthUnit.Centimeter);
      var comp = new ConvertUnitNumber();
      comp.CreateAttributes();
      ComponentTestHelper.SetInput(comp, l);

      var output = (OasysGH.Parameters.GH_UnitNumber)ComponentTestHelper.GetOutput(comp);
      comp.CreateTextPanel(new Grasshopper.Kernel.GH_Document());
      Assert.True(true);
    }

    [Fact]
    public void CreateValueListTest() {
      var l = new Length(3.5, OasysUnits.Units.LengthUnit.Centimeter);
      var comp = new ConvertUnitNumber();
      comp.CreateAttributes();
      ComponentTestHelper.SetInput(comp, l);

      var output = (OasysGH.Parameters.GH_UnitNumber)ComponentTestHelper.GetOutput(comp);
      comp.CreateValueList(new Grasshopper.Kernel.GH_Document());
      Assert.True(true);
    }
  }
}
