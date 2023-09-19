using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Grasshopper.Kernel;
using OasysGH.Parameters.Tests;
using Xunit;

namespace OasysGHTests.Parameters {
  [Collection("GrasshopperFixture collection")]
  public class GH_OasysPersistentParamTest {
    [Fact]
    public void OasysGooParameterTest() {
      var param = new OasysGooParameter();
      Assert.NotNull(param.Icon_24x24);
      Assert.Equal(GH_Exposure.hidden, param.Exposure);
      Assert.True(param.Hidden);
      Assert.False(param.IsPreviewCapable);
      Assert.NotEqual(new Guid(), param.ComponentGuid);
      Assert.NotNull(param.InstanceDescription);
      Assert.NotNull(param.TypeName);
    }

    [Theory]
    [InlineData("Prompt_Plural")]
    [InlineData("Prompt_Singular")]
    public void OasysGooParameterInternalPromptTests(string methodName) {
      var param = new OasysGooParameter();
      MethodInfo methodInfo = param.GetType().GetMethod(methodName,
        BindingFlags.NonPublic | BindingFlags.Instance);
      object res = methodInfo.Invoke(param, new object[] { null });
      Assert.NotNull(res);
      Assert.Equal(GH_GetterResult.cancel, (GH_GetterResult)res);
    }

    [Theory]
    [InlineData("Menu_CustomSingleValueItem")]
    [InlineData("Menu_CustomMultiValueItem")]
    public void OasysGooParameterInternalMenuCustomTests(string methodName) {
      var param = new OasysGooParameter();
      MethodInfo methodInfo = param.GetType().GetMethod(methodName,
        BindingFlags.NonPublic | BindingFlags.Instance);
      object res = methodInfo.Invoke(param, null);
      Assert.NotNull(res);
      Assert.Equal("Not available", ((ToolStripMenuItem)res).Text);
      Assert.False(((ToolStripMenuItem)res).Visible);
    }
  }
}
