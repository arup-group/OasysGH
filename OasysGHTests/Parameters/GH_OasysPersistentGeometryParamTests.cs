using System;
using Grasshopper.Kernel;
using OasysGH.Parameters.Tests;
using Xunit;

namespace OasysGHTests.Parameters {
  [Collection("GrasshopperFixture collection")]
  public class GH_OasysPersistentGeometryParamTests {
    [Fact]
    public void OasysGeometricGooParameterTest() {
      var param = new OasysGeometricGooParameter();
      Assert.NotNull(param.Icon_24x24);
      Assert.Equal(GH_Exposure.hidden, param.Exposure);
      Assert.True(param.Hidden);
      Assert.NotEqual(new Guid(), param.ComponentGuid);
      Assert.NotNull(param.InstanceDescription);
      Assert.NotNull(param.TypeName);
      Assert.True(param.IsPreviewCapable);
      Assert.False(param.ClippingBox.IsValid);
    }

    [Fact]
    public void OasysGeometricGooParameterDrawViewportWiresTest() {
      var param = new OasysGeometricGooParameter();
      param.DrawViewportWires(null);
      Assert.True(true);
    }

    [Fact]
    public void OasysGeometricGooParameterDrawViewportMeshesTest() {
      var param = new OasysGeometricGooParameter();
      param.DrawViewportMeshes(null);
      Assert.True(true);
    }
  }
}
