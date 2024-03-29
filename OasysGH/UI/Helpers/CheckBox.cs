﻿using System.Drawing;

namespace OasysGH.UI.Helpers {
  /// <summary>
  /// Class holding custom UI graphical buttons/boxes
  /// </summary>
  public static class CheckBox {
    /// <summary>
    /// Method to draw a check box with GSA-styling
    /// </summary>
    /// <param name="graphics"></param>
    /// <param name="center"></param>
    /// <param name="check"></param>
    /// <param name="activeFill"></param>
    /// <param name="activeEdge"></param>
    /// <param name="passiveFill"></param>
    /// <param name="passiveEdge"></param>
    /// <param name="size"></param>
    public static void DrawCheckButton(Graphics graphics, PointF center, bool check, Brush activeFill, Color activeEdge, Brush passiveFill, Color passiveEdge, int size) {
      // draws the check-button GSA styled
      //add scaler?

      if (check) {
        graphics.FillRectangle(activeFill, center.X - size / 2, center.Y - size / 2, size, size);
        var pen = new Pen(activeEdge);
        graphics.DrawRectangle(pen, center.X - size / 2, center.Y - size / 2, size, size);
        pen.Color = Color.White;
        pen.Width = size / 8;
        graphics.DrawLines(pen, new PointF[] { new PointF(center.X - size / 2 + pen.Width, center.Y), new PointF(center.X - pen.Width, center.Y + size / 2 - pen.Width), new PointF(center.X + size / 2 - pen.Width, center.Y - size / 2 + pen.Width) });
      } else {
        graphics.FillRectangle(passiveFill, center.X - size / 2, center.Y - size / 2, size, size);
        var pen = new Pen(passiveEdge) {
          Width = size / 8
        };
        graphics.DrawRectangle(pen, center.X - size / 2, center.Y - size / 2, size, size);
      }
    }
  }
}
