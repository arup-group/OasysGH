using Grasshopper.Kernel;
using OasysGH.Components.Tests;
using OasysGH.Helpers;
using Xunit;
using static OasysGHTestComponents.OasysGHTestComponentsInfo;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class PostHogTests {
    [Fact]
    public void ModelIOTest() {
      PostHog.ModelIO(OasysGHTestComponentsPluginInfo.Instance, "Test", 99);
      Assert.True(true);
    }

    [Fact]
    public void PluginLoadedTest() {
      PostHog.PluginLoaded(OasysGHTestComponentsPluginInfo.Instance);
      Assert.True(true);
    }

    [Fact]
    public void RemovedFromDocTest() {
      var comp = new DropDownComponent();
      comp.Attributes.Selected = true;
      PostHog.RemovedFromDocument(comp);
      Assert.True(true);
    }

    [Fact]
    public void Sha256ConsistentHashTest() {
      string input = "test@example.com";

      string hash1 = User.Sha256(input);
      string hash2 = User.Sha256(input);

      Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Sha256ExpectedHashForKnownInputTest() {
      string input = "test@example.com";
      string expectedHash = "973dfe463ec85785f5f95af5ba3906eedb2d931c24e69824a89ea65dba4e813b";

      string actualHash = User.Sha256(input);

      Assert.Equal(expectedHash, actualHash);
    }

    [Fact]
    public void Sha256DifferentHashesForDifferentInputsTest() {
      string input1 = "user1@example.com";
      string input2 = "user2@example.com";

      string hash1 = User.Sha256(input1);
      string hash2 = User.Sha256(input2);

      Assert.NotEqual(hash1, hash2);
    }
  }
}
