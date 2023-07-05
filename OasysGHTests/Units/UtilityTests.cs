using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class UtilityTests {
    [Fact]
    public static void ReadSettingsTest() {
      Assert.True(OasysGH.Units.Utility.ReadSettings());
    }

    [Fact]
    public static void SaveSettingsTest() {
      OasysGH.Units.Utility.SaveSettings();
      Assert.True(true);
    }
  }
}
