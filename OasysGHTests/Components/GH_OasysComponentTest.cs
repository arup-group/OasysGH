using System;
using OasysGH.Components;
using OasysGHTests.Components;
using Xunit;

namespace GsaGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class GH_OasysComponentTests {

    [Theory]
    [InlineData(typeof(TestButtonComponent))]
    //[InlineData(typeof(TestCreateOasysProfile))]
    public void GH_OasysComponentTest(Type t) {
      var comp = (GH_OasysComponent)Activator.CreateInstance(t);
      Assert.NotNull(comp.Icon_24x24);
      Assert.NotEqual(Grasshopper.Kernel.GH_Exposure.hidden, comp.Exposure);
      Assert.NotEqual(new Guid(), comp.ComponentGuid);
      Assert.Equal(OasysGH.PluginInfo.Instance, comp.PluginInfo);
    }

    [Fact]
    public void GH_OasysTaskCapableComponent() {
      var comp = new TestOasysTaskCapableComponent();
      Assert.NotNull(comp.Icon_24x24);
      Assert.NotEqual(Grasshopper.Kernel.GH_Exposure.hidden, comp.Exposure);
      Assert.NotEqual(new Guid(), comp.ComponentGuid);
      Assert.Equal(OasysGH.PluginInfo.Instance, comp.PluginInfo);
    }
  }
}
