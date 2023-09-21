using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel.Attributes;
using OasysGH.Components;

namespace OasysGHTests.TestHelpers {
  internal class Render {
    internal static bool TestRender(GH_OasysDropDownComponent comp) {
      GH_Canvas canvas = Document.CreateCanvas();
      canvas.Document.AddObject(comp, true);
      comp.CreateAttributes();
      var attributes = (GH_ComponentAttributes)comp.Attributes;
      attributes.PerformLayout();
      attributes.RenderToCanvas(canvas, GH_CanvasChannel.Objects);
      return true;
    }
  }
}
