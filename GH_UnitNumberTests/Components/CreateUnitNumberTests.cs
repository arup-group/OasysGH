using System.Drawing;
using System.Reflection;
using System.Resources;
using GH_UnitNumber.Components;
using GH_UnitNumber.Properties;
using GH_UnitNumberTests.Helpers;
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
    [InlineData(4, "cm³")]
    [InlineData(5, "cm⁴")]
    [InlineData(6, "kN")]
    [InlineData(7, "kN/m")]
    [InlineData(8, "kN/m²")]
    [InlineData(9, "kN·m")]
    [InlineData(10, "MPa")]
    [InlineData(11, "ε")]
    [InlineData(12, "kN")]
    [InlineData(13, "N·m/rad")]
    [InlineData(14, "kN·m²")]
    [InlineData(15, "m⁻¹")]
    [InlineData(16, "t")]
    [InlineData(17, "kg/m³")]
    [InlineData(18, "kg/m")]
    [InlineData(19, "m³/m")]
    [InlineData(20, "°C")]
    [InlineData(21, "m/s")]
    [InlineData(22, "m/s²")]
    [InlineData(23, "MJ")]
    [InlineData(24, "")]
    [InlineData(25, "m")]
    public void CreateFromSelectedTest(int selectedItem, string abbreviation) {
      var comp = new CreateUnitNumber();
      comp.CreateAttributes();
      comp.SetSelected(0, selectedItem);
      ComponentTestHelper.SetInput(comp, "1.5");
      var output = (OasysGH.Parameters.GH_UnitNumber)ComponentTestHelper.GetOutput(comp);
      Assert.Equal($"1.5{abbreviation}", output.ToString());
    }

    [Fact]
    public void IconTest() {
      var component = new CreateUnitNumber();
      string className = "CreateUnitNumber";

      // Test component icon is equal to class name
      ResourceManager rm = Resources.ResourceManager;
      // Find icon with expected name in resources
      var iconExpected = (Bitmap)rm.GetObject(className);
      Assert.True(iconExpected != null, $"{className} not found in resources");
      PropertyInfo pInfo = component.GetType().GetProperty("Icon",
        BindingFlags.NonPublic | BindingFlags.Instance);
      var icon = (Bitmap)pInfo.GetValue(component, null);
      Assert.Equal(iconExpected.RawFormat.Guid, icon.RawFormat.Guid);
    }
  }
}
