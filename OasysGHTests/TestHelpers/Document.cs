using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using OasysGH.Components;

namespace OasysGHTests.TestHelpers {
  internal class Document {
    public static GH_Document CreateDocument() {
      var doc = new GH_Document();
      var io = new GH_DocumentIO(doc);
      return io.Document;
    }

    public static GH_Canvas CreateCanvas(GH_Document doc = null) {
      var canvas = new GH_Canvas {
        Document = doc ?? CreateDocument()
      };
      return canvas;
    }

    public static IGH_Attributes Attributes(GH_OasysDropDownComponent comp) {
      GH_Canvas canvas = Document.CreateCanvas();
      canvas.Document.AddObject(comp, true);
      comp.CreateAttributes();
      comp.Attributes.PerformLayout();
      return comp.Attributes;
    }
  }
}
