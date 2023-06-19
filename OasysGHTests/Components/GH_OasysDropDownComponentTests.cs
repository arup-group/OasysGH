using System;
using GsaGHTests.Helpers;
using OasysGH.Components;
using OasysGHTests.Components;
using Xunit;

namespace GsaGHTests.CustomComponent {
  [Collection("GrasshopperFixture collection")]
  public class GH_OasysDropDownComponentTests {

    [Theory]
    [InlineData(typeof(TestButtonComponent))]
    [InlineData(typeof(TestCheckBoxComponentComponent))]
    public void DropDownComponentTest(Type t, bool ignoreSpacerDescriptionCount = false) {
      var comp = (GH_OasysDropDownComponent)Activator.CreateInstance(t);
      OasysDropDownComponentTestHelper.ChangeDropDownTest(comp, ignoreSpacerDescriptionCount);
    }
  }
}
