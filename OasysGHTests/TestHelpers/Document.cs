using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Xunit;

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
  }
}
