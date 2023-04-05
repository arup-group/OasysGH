using System.Drawing;
using System.Drawing.Drawing2D;

namespace OasysGH.UI.Helpers
{
  /// <summary>
  /// Method to draw a rounded rectangle
  /// 
  /// Call this method when overriding Render method
  /// </summary>
  public static class ButtonAttributes
  {
    public static GraphicsPath RoundedRect(RectangleF bounds, int radius, bool overlay = false)
    {
      RectangleF b = new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height);
      int diameter = radius * 2;
      Size size = new Size(diameter, diameter);
      RectangleF arc = new RectangleF(b.Location, size);
      GraphicsPath path = new GraphicsPath();

      if (overlay)
        b.Height = diameter;

      if (radius == 0)
      {
        path.AddRectangle(b);
        return path;
      }

      // top left arc  
      path.AddArc(arc, 180, 90);

      // top right arc  
      arc.X = b.Right - diameter;
      path.AddArc(arc, 270, 90);

      if (!overlay)
      {
        // bottom right arc  
        arc.Y = b.Bottom - diameter;
        path.AddArc(arc, 0, 90);

        // bottom left arc 
        arc.X = b.Left;
        path.AddArc(arc, 90, 90);
      }
      else
      {
        path.AddLine(new PointF(b.X + b.Width, b.Y + b.Height), new PointF(b.X, b.Y + b.Height));
      }

      path.CloseFigure();
      return path;
    }
  }
}
