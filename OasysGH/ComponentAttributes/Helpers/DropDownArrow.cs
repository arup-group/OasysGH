using System.Drawing;

namespace OasysGH.UI.Helpers {
  /// <summary>
  /// Method to draw a dropdown arrow
  ///
  /// Call this method when overriding Render method
  /// </summary>
  public static class DropDownArrow {
    public static void DrawDropDownButton(Graphics graphics, PointF center, Color colour, int rectanglesize) {
      var pen = new Pen(new SolidBrush(colour)) {
        Width = rectanglesize / 8
      };

      graphics.DrawLines(
        pen, new PointF[]
        {
          new PointF(center.X - rectanglesize / 4, center.Y - rectanglesize / 8),
          new PointF(center.X, center.Y + rectanglesize / 6),
          new PointF(center.X + rectanglesize / 4, center.Y - rectanglesize / 8)
        });
    }
  }
}
