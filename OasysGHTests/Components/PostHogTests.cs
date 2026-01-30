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
    public void Sha256_ShouldReturnConsistentHash() {
      // Arrange
      string input = "test@example.com";

      // Act
      string hash1 = User.Sha256(input);
      string hash2 = User.Sha256(input);

      // Assert
      Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Sha256_ShouldReturnExpectedHashForKnownInput() {
      // Arrange
      string input = "test@example.com";
      // Expected SHA256 hash for "test@example.com" in lowercase hex
      string expectedHash = "973dfe463ec85785f5f95af5ba3906eedb2d931c24e69824a89ea65dba4e813b";

      // Act
      string actualHash = User.Sha256(input);

      // Assert
      Assert.Equal(expectedHash, actualHash);
    }

    [Fact]
    public void Sha256_ShouldReturnDifferentHashesForDifferentInputs() {
      // Arrange
      string input1 = "user1@example.com";
      string input2 = "user2@example.com";

      // Act
      string hash1 = User.Sha256(input1);
      string hash2 = User.Sha256(input2);

      // Assert
      Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Sha256_ShouldReturnLowercaseHexString() {
      // Arrange
      string input = "TEST@EXAMPLE.COM";

      // Act
      string hash = User.Sha256(input);

      // Assert
      Assert.Equal(hash, hash.ToLower());
      Assert.Matches("^[0-9a-f]{64}$", hash); // SHA256 produces 64 hex characters
    }

    [Fact]
    public void Sha256_ShouldReturn64CharacterHash() {
      // Arrange
      string input = "anystring";

      // Act
      string hash = User.Sha256(input);

      // Assert
      Assert.Equal(64, hash.Length);
    }

    [Fact]
    public void Sha256_ShouldHandleEmptyString() {
      // Arrange
      string input = "";
      // Expected SHA256 hash for empty string
      string expectedHash = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

      // Act
      string hash = User.Sha256(input);

      // Assert
      Assert.Equal(expectedHash, hash);
    }
  }
}
