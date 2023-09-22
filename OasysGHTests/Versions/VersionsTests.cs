using System;
using System.Drawing;
using OasysGH.Versions.UI;
using OasysGH.Versions;
using OasysGH.Properties;
using Xunit;
using OasysGH;

namespace OasysGHTests.Versions {
  [Collection("GrasshopperFixture collection")]
  public class VersionsTests {

    [Fact]
    public void CheckTest() {
      UpdateDependentPlugins.CheckAndShowDialogue();
      Assert.True(true);
    }

    [Fact]
    public void IsPluginOutdatedTest() {
      var unitNumberGuid = new Guid("6080a841-4f35-4182-9922-f40a66977a69");
      Assert.False(OasysGH.Versions.Versions.IsPluginOutdated(unitNumberGuid));
    }

    [Theory]
    [InlineData("0.0.1", true)]
    [InlineData("0.2.2147483647", true)]
    [InlineData("2.0", false)]
    public void IsVersionOutdatedTest(string version, bool expected) {
      var v = new Version(version);
      Assert.Equal(expected, OasysGH.Versions.Versions.IsVersionOutdated(v));
    }

    [Fact]
    public void CheckAdSecIsOutdatedTest() {
      UpdatePluginsBox box = UpdateDependentPlugins.CreatePluginUpdateDialogue(true, false, false);

      string text = "An update is available for AdSec.\n\nClick OK to update now.";
      string header = "Update AdSec";
      Bitmap expectedIcon = Resources.AdSecGHUpdate;

      TestUpdatePluginsBox(box, header, text, expectedIcon);
    }

    [Fact]
    public void CheckComposIsOutdatedTest() {
      UpdatePluginsBox box = UpdateDependentPlugins.CreatePluginUpdateDialogue(false, true, false);

      string text = "An update is available for Compos.\n\nClick OK to update now.";
      string header = "Update Compos";
      Bitmap expectedIcon = Resources.ComposGHUpdate;

      TestUpdatePluginsBox(box, header, text, expectedIcon);
    }

    [Fact]
    public void CheckGsaIsOutdatedTest() {
      UpdatePluginsBox box = UpdateDependentPlugins.CreatePluginUpdateDialogue(false, false, true);

      string text = "An update is available for GSA.\n\nClick OK to update now.";
      string header = "Update GSA";
      Bitmap expectedIcon = Resources.GsaGHUpdate;

      TestUpdatePluginsBox(box, header, text, expectedIcon);
    }

    [Fact]
    public void CheckAllAreOutdatedTest() {
      UpdatePluginsBox box = UpdateDependentPlugins.CreatePluginUpdateDialogue(true, true, true);

      string text = "Updates are available for AdSec, Compos and GSA.\n\nClick OK to update now.";
      string header = "Update Oasys Plugins";
      Bitmap expectedIcon = Resources.OasysGHUpdate3;

      TestUpdatePluginsBox(box, header, text, expectedIcon);
    }

    [Fact]
    public void CheckAdSecAndComposAreOutdatedTest() {
      UpdatePluginsBox box = UpdateDependentPlugins.CreatePluginUpdateDialogue(true, true, false);

      string text = "Updates are available for AdSec and Compos.\n\nClick OK to update now.";
      string header = "Update Oasys Plugins";
      Bitmap expectedIcon = Resources.OasysGHUpdate2;

      TestUpdatePluginsBox(box, header, text, expectedIcon);
    }

    [Fact]
    public void CheckAdSecAndGsaAreOutdatedTest() {
      UpdatePluginsBox box = UpdateDependentPlugins.CreatePluginUpdateDialogue(true, false, true);

      string text = "Updates are available for AdSec and GSA.\n\nClick OK to update now.";
      string header = "Update Oasys Plugins";
      Bitmap expectedIcon = Resources.OasysGHUpdate2;

      TestUpdatePluginsBox(box, header, text, expectedIcon);
    }

    [Fact]
    public void CheckComposAndGsaAreOutdatedTest() {
      UpdatePluginsBox box = UpdateDependentPlugins.CreatePluginUpdateDialogue(false, true, true);

      string text = "Updates are available for Compos and GSA.\n\nClick OK to update now.";
      string header = "Update Oasys Plugins";
      Bitmap expectedIcon = Resources.OasysGHUpdate2;

      TestUpdatePluginsBox(box, header, text, expectedIcon);
    }

    [Theory]
    [InlineData("0.6.13-beta", 0, 6, 13, 0)]
    [InlineData("1.0.0", 1, 0, 0, -1)]
    public void CreateVersionTest(string versionString, int major, int minor, int build, int revision) {
      Version versionFromString = OasysGH.Versions.Versions.CreateVersion(versionString);
      Assert.Equal(major, versionFromString.Major);
      Assert.Equal(minor, versionFromString.Minor);
      Assert.Equal(build, versionFromString.Build);
      Assert.Equal(revision, versionFromString.Revision);
    }

    [Fact]
    public void GetOasysGhVersion() {
      Version oasyGhVersion = OasysGH.Versions.Versions.GetOasysGhVersion();
      Assert.Equal(OasysGHVersion.Version, oasyGhVersion.ToString());
    }

    private static void TestUpdatePluginsBox(
      UpdatePluginsBox box, string header, string text, Bitmap expectedIcon) {
      Assert.Equal(header, box.Text);
      Assert.Equal(text, box.textBox.Text);
      var icon = (Bitmap)box.pictureBox.Image;
      Assert.Equal(expectedIcon.RawFormat.Guid, icon.RawFormat.Guid);
    }
  }
}
