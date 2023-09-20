using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using Eto.Forms;
using GH_UnitNumber;
using GH_UnitNumber.Components;
using GH_UnitNumber.Properties;
using GH_UnitNumberTests.Helpers;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using OasysGH.Components;
using OasysUnits;
using Rhino.Geometry;
using Rhino.NodeInCode;
using Xunit;

namespace GH_UnitNumberTests.Parameters {
  [Collection("GrasshopperFixture collection")]
  public class GH_UnitNumberParameterTests {

    [Fact]
    public void GH_UnitNumberParameterTest() {
      var param = new GH_UnitNumberParameter();
      param.CreateAttributes();
      
      Assert.True(param.Hidden);
      Assert.False(param.IsPreviewCapable);

      var form = new ContextMenuStrip();
      param.AppendAdditionalMenuItems(form);
      Assert.Equal(13, form.Items.Count);

      var path = new GH_Path(0);
      var l = new Length(5, OasysUnits.Units.LengthUnit.Meter);
      var unitnumber = new OasysGH.Parameters.GH_UnitNumber(l);
      param.AddVolatileData(path, 0, unitnumber);
      Assert.Equal("One locally defined value…\r\n5m", param.InstanceDescription);
    }

    [Fact]
    public void IconTest() {
      var param = new GH_UnitNumberParameter();
      string className = "UnitParam";

      // Test component icon is equal to class name
      ResourceManager rm = Resources.ResourceManager;
      // Find icon with expected name in resources
      var iconExpected = (Bitmap)rm.GetObject(className);
      Assert.True(iconExpected != null, $"{className} not found in resources");
      PropertyInfo pInfo = param.GetType().GetProperty("Icon",
        BindingFlags.NonPublic | BindingFlags.Instance);
      var icon = (Bitmap)pInfo.GetValue(param, null);
      Assert.Equal(iconExpected.RawFormat.Guid, icon.RawFormat.Guid);
    }

    [Fact]
    public void PreferredCastFromQuantityTest() {
      var param = new GH_UnitNumberParameter();
      var path = new GH_Path(0);
      var l = new Length(5, OasysUnits.Units.LengthUnit.Meter);
      param.AddVolatileData(path, 0, l);
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(5, output.Value.Value);
    }

    [Fact]
    public void PreferredCastFromObjectWrapperQuantityTest() {
      var param = new GH_UnitNumberParameter();
      var path = new GH_Path(0);
      var l = new Length(5, OasysUnits.Units.LengthUnit.Meter);
      var wrapper = new GH_ObjectWrapper(l);
      param.AddVolatileData(path, 0, wrapper);
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(5, output.Value.Value);
    }

    [Fact]
    public void PreferredCastFromObjectWrappeStringTest() {
      var param = new GH_UnitNumberParameter();
      var path = new GH_Path(0);
      string l = "5 yr";
      var wrapper = new GH_ObjectWrapper(l);
      param.AddVolatileData(path, 0, wrapper);
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(5, output.Value.Value);
    }
  }
}
