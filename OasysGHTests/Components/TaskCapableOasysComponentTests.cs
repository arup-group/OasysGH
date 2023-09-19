using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Types;
using OasysGH.Components;
using OasysGH.Components.Tests;
using OasysGH.UI;
using OasysGHTests.TestHelpers;
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

      doc.Objects.Remove(comp);
      Assert.Empty(doc.Objects);
    }
  }
}
