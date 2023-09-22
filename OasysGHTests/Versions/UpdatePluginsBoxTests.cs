using OasysGH.Properties;
using OasysGH.Versions.UI;
using System.Drawing;
using System.Resources;
using Xunit;

namespace OasysGHTests.UI {
  [Collection("GrasshopperFixture collection")]
  public class UpdatePluginsBoxTests {
    [Fact]
    public void UpdatePluginsBoxTest() {
      string process = @"rhino://package/search?name=oasys";
      string text = "Updates are avaiable for AdSec and Compos";
      string header = "Update Oasys Plugins";
      Bitmap expectedIcon = Resources.OasysGHUpdate2;

      var box = new UpdatePluginsBox(header, text, process, expectedIcon);
      Assert.Equal(header, box.Text);
      Assert.Equal(text, box.textBox.Text);

      // Test component icon is equal to class name
      ResourceManager rm = Resources.ResourceManager;
      // Find icon with expected name in resources
      string className = "OasysGHUpdate2";
      var iconExpected = (Bitmap)rm.GetObject(className);
      Assert.True(iconExpected != null, $"{className} not found in resources");
      var icon = (Bitmap)box.pictureBox.Image;
      Assert.Equal(iconExpected.RawFormat.Guid, icon.RawFormat.Guid);
    }
  }
}
