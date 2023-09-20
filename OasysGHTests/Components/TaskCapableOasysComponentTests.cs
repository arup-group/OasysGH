using Grasshopper.Kernel;
using OasysGH.Components.Tests;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class TaskCapableOasysComponentTests {
    [Fact]
    public void AddRemoveComponentTest() {
      var comp = new TaskCapableComponent();
      comp.CreateAttributes();

      var doc = new GH_Document();
      doc.AddObject(comp, true);
      Assert.Single(doc.Objects);

      doc.RemoveObject(comp, true);
      Assert.Empty(doc.Objects);

      doc.Dispose();
    }
  }
}
