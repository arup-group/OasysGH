using Grasshopper.GUI.Canvas;
using Grasshopper.GUI;
using Grasshopper.Kernel.Attributes;
using OasysGH.Components;
using System.Drawing;
using System.Windows.Forms;

namespace OasysGHTests.TestHelpers {
  internal class Mouse {
    internal static bool TestMouseMove(GH_OasysDropDownComponent comp) {
      GH_Canvas canvas = Document.CreateCanvas();
      canvas.Document.AddObject(comp, true);
      comp.CreateAttributes();
      var attributes = (GH_ComponentAttributes)comp.Attributes;
      attributes.PerformLayout();

      RectangleF bounds = comp.Attributes.Bounds;
      for (int i = 0; i < bounds.Width; i++) {
        for (int j = 0; j < bounds.Height; j++) {
          var ptControl = new Point(i, j);
          var ptcanvas = new PointF(i, j);
          var mouse = new GH_CanvasMouseEvent(ptControl, ptcanvas, MouseButtons.None);

          comp.Attributes.RespondToMouseMove(canvas, mouse);
        }
      }

      return true;
    }

    internal static bool TestMouseClick(GH_OasysDropDownComponent comp) {
      GH_Canvas canvas = Document.CreateCanvas();
      canvas.Document.AddObject(comp, true);
      comp.CreateAttributes();
      var attributes = (GH_ComponentAttributes)comp.Attributes;
      attributes.PerformLayout();

      RectangleF bounds = comp.Attributes.Bounds;
      for (int i = 0; i < bounds.Width; i++) {
        for (int j = 0; j < bounds.Height; j++) {
          var ptControl = new Point(i, j);
          var ptcanvas = new PointF(i, j);
          var mouse = new GH_CanvasMouseEvent(ptControl, ptcanvas, MouseButtons.Left);
          comp.Attributes.RespondToMouseDown(canvas, mouse);
          mouse = new GH_CanvasMouseEvent(ptControl, ptcanvas, MouseButtons.None);
          comp.Attributes.RespondToMouseUp(canvas, mouse);
        }
      }

      return true;
    }
  }
}
