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
  }
}
