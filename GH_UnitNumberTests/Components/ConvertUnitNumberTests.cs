using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using GH_UnitNumber.Components;
using GH_UnitNumber.Properties;
using GH_UnitNumberTests.Helpers;
using OasysUnits;
using Rhino.Geometry;
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

      var l = new Length(3.5, OasysUnits.Units.LengthUnit.Centimeter);
      ComponentTestHelper.SetInput(comp, l);
      var output = (OasysGH.Parameters.GH_UnitNumber)ComponentTestHelper.GetOutput(comp);

      form = new ContextMenuStrip();
      comp.AppendAdditionalMenuItems(form);
      Assert.Equal(3, form.Items.Count);
    }

    [Fact]
    public void DropDownTest() {
      var l = new Length(3.5, OasysUnits.Units.LengthUnit.Centimeter);
      var comp = new ConvertUnitNumber();
      comp.CreateAttributes();
      ComponentTestHelper.SetInput(comp, l);

      var output = (OasysGH.Parameters.GH_UnitNumber)ComponentTestHelper.GetOutput(comp);
      Dropdown.ChangeDropDownDeserializeTest(comp);
    }

    [Fact]
    public void TextUnitTest() {
      var l = new Length(3.5, OasysUnits.Units.LengthUnit.Centimeter);
      var comp = new ConvertUnitNumber();
      comp.CreateAttributes();
      ComponentTestHelper.SetInput(comp, l);
      ComponentTestHelper.SetInput(comp, "mm", 1);

      var output = (OasysGH.Parameters.GH_UnitNumber)ComponentTestHelper.GetOutput(comp);
      Assert.Equal(35, output.Value.Value);

      ComponentTestHelper.SetInput(comp, "horse", 1);
      output = (OasysGH.Parameters.GH_UnitNumber)ComponentTestHelper.GetOutput(comp);
      Assert.Single(comp.RuntimeMessages(Grasshopper.Kernel.GH_RuntimeMessageLevel.Error));
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

    [Fact]
    public void InputErrorTest() {
      var comp = new ConvertUnitNumber();
      comp.CreateAttributes();
      var output = (OasysGH.Parameters.GH_UnitNumber)ComponentTestHelper.GetOutput(comp);
      Assert.Single(comp.RuntimeMessages(Grasshopper.Kernel.GH_RuntimeMessageLevel.Warning));

      var pt = new Point3d(0, 0, 0);
      ComponentTestHelper.SetInput(comp, pt);

      output = (OasysGH.Parameters.GH_UnitNumber)ComponentTestHelper.GetOutput(comp);
      Assert.Equal(2, comp.RuntimeMessages(Grasshopper.Kernel.GH_RuntimeMessageLevel.Error).Count);
    }

    [Fact]
    public void IconTest() {
      var component = new ConvertUnitNumber();
      string className = "ConvertUnitNumber";

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
