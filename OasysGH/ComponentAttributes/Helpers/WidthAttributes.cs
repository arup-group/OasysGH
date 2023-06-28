using System.Collections.Generic;
using System.Drawing;
using Grasshopper.GUI;
using Grasshopper.Kernel;

namespace OasysGH.UI.Helpers {
  public static class WidthAttributes {

    public static int MaxTextWidth(List<string> strings, Font font) {
      int sp = 0; // width of spacer text

      // adjust fontsize to high resolution displays
      font = new Font(font.FontFamily, font.Size / GH_GraphicsUtil.UiScale, font.Style);

      for (int i = 0; i < strings.Count; i++) {
        if (GH_FontServer.StringWidth(strings[i], font) + 8 > sp)
          sp = GH_FontServer.StringWidth(strings[i], font) + 8;
      }

      return sp;
    }
  }
}
