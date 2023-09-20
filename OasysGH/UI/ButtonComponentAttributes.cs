using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using OasysGH.UI.Helpers;

namespace OasysGH.UI {
  /// <summary>
  /// Class to create custom component UI with a button
  ///
  /// This class is made for the open and save components
  ///
  /// To use this class override CreateAttributes() in component class and set m_attributes to an instance of this class.
  /// </summary>
  public class ButtonComponentAttributes : GH_ComponentAttributes {
    private float MinWidth {
      get {
        var spacers = new List<string> {
          _spacerTxt
        };
        float sp = WidthAttributes.MaxTextWidth(spacers, GH_FontServer.Small);
        var buttons = new List<string> {
          _buttonText
        };
        float bt = WidthAttributes.MaxTextWidth(buttons, GH_FontServer.Standard);

        float num = Math.Max(Math.Max(sp, bt), 90);
        return num;
      }
      set => MinWidth = value;
    }

    private readonly Action _action;
    private readonly string _buttonText;
    private readonly string _spacerTxt;

    // text to be displayed
    private RectangleF _buttonBounds;

    private bool _mouseDown;
    private bool _mouseOver;

    // area for button to be displayed
    private RectangleF _spacerBounds;

    public ButtonComponentAttributes(GH_Component owner, string displayText, Action clickHandle, string spacerText = "") : base(owner) {
      _buttonText = displayText;
      _spacerTxt = spacerText;
      _action = clickHandle;
    }

    public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e) {
      if (e.Button == System.Windows.Forms.MouseButtons.Left) {
        RectangleF rec = _buttonBounds;
        if (rec.Contains(e.CanvasLocation)) {
          _mouseDown = true;
          Owner.OnDisplayExpired(false);
          return GH_ObjectResponse.Capture;
        }
      }
      return base.RespondToMouseDown(sender, e);
    }

    public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e) {
      if (_buttonBounds.Contains(e.CanvasLocation)) {
        _mouseOver = true;
        Owner.OnDisplayExpired(false);
        sender.Cursor = System.Windows.Forms.Cursors.Hand;
        return GH_ObjectResponse.Capture;
      }

      if (_mouseOver) {
        _mouseOver = false;
        Owner.OnDisplayExpired(false);
        Instances.CursorServer.ResetCursor(sender);
        return GH_ObjectResponse.Release;
      }

      return base.RespondToMouseMove(sender, e);
    }

    public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e) {
      if (e.Button == System.Windows.Forms.MouseButtons.Left) {
        RectangleF rec = _buttonBounds;
        if (rec.Contains(e.CanvasLocation)) {
          if (_mouseDown) {
            _mouseDown = false;
            _mouseOver = false;
            Owner.OnDisplayExpired(false);
            _action();
            //                        Owner.ExpireSolution(true);
            return GH_ObjectResponse.Release;
          }
        }
      }
      return base.RespondToMouseUp(sender, e);
    }

    protected void FixLayout() {
      float width = Bounds.Width; // initial component width before UI overrides
      float num = Math.Max(width, MinWidth); // number for new width
      float num2 = 0f; // value for increased width (if any)

      // first check if original component must be widened
      if (num > width) {
        num2 = num - width; // change in width
                            // update component bounds to new width
        Bounds = new RectangleF(
            Bounds.X - num2 / 2f,
            Bounds.Y,
            num,
            Bounds.Height);
      }

      // secondly update position of input and output parameter text
      // first find the maximum text width of parameters

      foreach (IGH_Param item in Owner.Params.Output) {
        PointF pivot = item.Attributes.Pivot; // original anchor location of output
        RectangleF bounds = item.Attributes.Bounds; // text box itself
        item.Attributes.Pivot = new PointF(
            pivot.X + num2 / 2f, // move anchor to the right
            pivot.Y);
        item.Attributes.Bounds = new RectangleF(
            bounds.Location.X + num2 / 2f,  // move text box to the right
            bounds.Location.Y,
            bounds.Width,
            bounds.Height);
      }
      // for input params first find the widest input text box as these are right-aligned
      float inputwidth = 0f;
      foreach (IGH_Param item in Owner.Params.Input) {
        if (inputwidth < item.Attributes.Bounds.Width)
          inputwidth = item.Attributes.Bounds.Width;
      }
      foreach (IGH_Param item2 in Owner.Params.Input) {
        PointF pivot2 = item2.Attributes.Pivot; // original anchor location of input
        RectangleF bounds2 = item2.Attributes.Bounds;
        item2.Attributes.Pivot = new PointF(
            pivot2.X - num2 / 2f + inputwidth, // move to the left, move back by max input width
            pivot2.Y);
        item2.Attributes.Bounds = new RectangleF(
             bounds2.Location.X - num2 / 2f,
             bounds2.Location.Y,
             bounds2.Width,
             bounds2.Height);
      }
    }

    // spacer between standard component and button
    // text to be displayed on spacer
    protected override void Layout() {
      base.Layout();

      // first change the width to suit; using max to determine component visualisation style
      FixLayout();

      int s = 2; //spacing to edges and internal between boxes

      int h0 = 0;
      //spacer and title
      if (_spacerTxt != "") {
        Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height - (CentralSettings.CanvasObjectIcons ? 5 : 0));
        h0 = 10;
        _spacerBounds = new RectangleF(Bounds.X, Bounds.Bottom + s / 2, Bounds.Width, h0);
      }

      int h1 = 20; // height of button
                   // create text box placeholders
      _buttonBounds = new RectangleF(Bounds.X + 2 * s, Bounds.Bottom + h0 + 2 * s, Bounds.Width - 4 * s, h1);

      //update component bounds
      Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height + h0 + h1 + 4 * s);
    }

    protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel) {
      base.Render(canvas, graphics, channel);

      if (channel == GH_CanvasChannel.Objects) {
        CustomRender(graphics);
      }
    }

    internal void CustomRender(Graphics graphics) {
      var spacer = new Pen(Colour.SpacerColour);

      Font font = GH_FontServer.Standard;
      // adjust fontsize to high resolution displays
      font = new Font(font.FontFamily, font.Size / GH_GraphicsUtil.UiScale, FontStyle.Regular);

      Font sml = GH_FontServer.Small;
      // adjust fontsize to high resolution displays
      sml = new Font(sml.FontFamily, sml.Size / GH_GraphicsUtil.UiScale, FontStyle.Regular);

      //Draw divider line
      if (_spacerTxt != "") {
        graphics.DrawString(_spacerTxt, sml, Colour.AnnotationTextDark, _spacerBounds, GH_TextRenderingConstants.CenterCenter);
        graphics.DrawLine(spacer, _spacerBounds.X, _spacerBounds.Y + _spacerBounds.Height / 2, _spacerBounds.X + (_spacerBounds.Width - GH_FontServer.StringWidth(_spacerTxt, sml)) / 2 - 4, _spacerBounds.Y + _spacerBounds.Height / 2);
        graphics.DrawLine(spacer, _spacerBounds.X + (_spacerBounds.Width - GH_FontServer.StringWidth(_spacerTxt, sml)) / 2 + GH_FontServer.StringWidth(_spacerTxt, sml) + 4, _spacerBounds.Y + _spacerBounds.Height / 2, _spacerBounds.X + _spacerBounds.Width, _spacerBounds.Y + _spacerBounds.Height / 2);
      }

      // Draw button box
      System.Drawing.Drawing2D.GraphicsPath button = ButtonAttributes.DrawRoundedRect(_buttonBounds, 2);

      Brush normal_colour = Colour.ButtonColour;
      Brush hover_colour = Colour.HoverButtonColour;
      Brush clicked_colour = Colour.ClickedButtonColour;

      Brush butCol = (_mouseOver) ? hover_colour : normal_colour;
      graphics.FillPath(_mouseDown ? clicked_colour : butCol, button);

      // draw button edge
      Color edgeColor = Colour.ButtonBorderColour;
      Color edgeHover = Colour.HoverBorderColour;
      Color edgeClick = Colour.ClickedBorderColour;
      Color edgeCol = (_mouseOver) ? edgeHover : edgeColor;
      var pen = new Pen(_mouseDown ? edgeClick : edgeCol) {
        Width = (_mouseDown) ? 0.8f : 0.5f
      };
      graphics.DrawPath(pen, button);

      // draw button glow
      System.Drawing.Drawing2D.GraphicsPath overlay = ButtonAttributes.DrawRoundedRect(_buttonBounds, 2, true);
      graphics.FillPath(new SolidBrush(Color.FromArgb(_mouseDown ? 0 : _mouseOver ? 40 : 60, 255, 255, 255)), overlay);

      // draw button text
      graphics.DrawString(_buttonText, font, Colour.AnnotationTextBright, _buttonBounds, GH_TextRenderingConstants.CenterCenter);
    }
  }
}
