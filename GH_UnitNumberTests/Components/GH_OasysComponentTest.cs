using System;
using GH_UnitNumber;
using GH_UnitNumber.Components;
using OasysGH.Components;
using Xunit;

namespace GH_UnitNumberTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class GH_OasysComponentTests {

    [Theory]
    [InlineData(typeof(ConvertUnitNumber), 65664)]
    [InlineData(typeof(CreateUnitNumber), 65664)]
    public void GH_OasysComponentTest(Type t, int expectedExposure) {
      var comp = (GH_OasysComponent)Activator.CreateInstance(t);
      Assert.NotNull(comp.Icon_24x24);
      Assert.Equal(expectedExposure, (int)comp.Exposure);
      Assert.NotEqual(new Guid(), comp.ComponentGuid);
      Assert.Equal(GH_UnitNumberPluginInfo.Instance, comp.PluginInfo);
    }
  }
}
