using Grasshopper.Kernel;
using OasysGH.Components.Tests;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class OasysComponentTests {
    [Fact]
    public void AddRemoveComponentTest() {
      var comp = new DropDownComponent();
      comp.CreateAttributes();

      var doc = new GH_Document();
      doc.AddObject(comp, true);
      Assert.Single(doc.Objects);

      doc.Objects.Remove(comp);
      Assert.Empty(doc.Objects);
    }
  }
}
