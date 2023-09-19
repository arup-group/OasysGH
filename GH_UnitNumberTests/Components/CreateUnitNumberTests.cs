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
    public void ChangeDropdownTest() {
      var comp = new CreateUnitNumber();
      comp.CreateAttributes();
      Dropdown.ChangeDropDownDeserializeTest(comp);
    }

    [Theory]
    [InlineData(0, "rad")]
    [InlineData(1, "m")]
    [InlineData(2, "cm²")]
    [InlineData(3, "cm³")]
    [InlineData(4, "cm⁴")]
    [InlineData(5, "kN")]
    [InlineData(6, "kN/m")]
    [InlineData(7, "kN/m²")]
    [InlineData(8, "kN·m")]
    [InlineData(9, "MPa")]
    [InlineData(10, "ε")]
    [InlineData(11, "kN")]
    [InlineData(12, "kN·m²")]
    [InlineData(13, "m⁻¹")]
    [InlineData(14, "t")]
    [InlineData(15, "kg/m³")]
    [InlineData(16, "kg/m")]
    [InlineData(17, "m³/m")]
    [InlineData(18, "°C")]
    [InlineData(19, "m/s")]
    [InlineData(20, "m/s²")]
    [InlineData(21, "MJ")]
    [InlineData(22, "")]
    [InlineData(23, "m")]
    [InlineData(24, "cm³")]
    public void CreateFromSelectedTest(int selectedItem, string abbreviation) {
      var comp = new CreateUnitNumber();
      comp.CreateAttributes();
      comp.SetSelected(0, selectedItem);
      ComponentTestHelper.SetInput(comp, "1.5");
      var output = (OasysGH.Parameters.GH_UnitNumber)ComponentTestHelper.GetOutput(comp);
      Assert.Equal($"1.5{abbreviation}", output.ToString());
    }
  }
}
