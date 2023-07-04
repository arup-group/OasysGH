using Grasshopper.Kernel.Types;
using OasysGH.Components.TestComponents;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class ButtonComponentTests {
    [Fact]
    public static void ClickButtonTest() {
      var comp = new ButtonComponent();
      comp.CreateAttributes();
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      var wasClickedInitial = (GH_Boolean)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.False(wasClickedInitial.Value);

      comp.Clicked();
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      var wasClicked = (GH_Boolean)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.True(wasClicked.Value);
    }
  }
}
