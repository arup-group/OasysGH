using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
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
  /// This class has been made for the open and save components.
  ///
  /// To use this class override CreateAttributes() in component class and set m_attributes to an instance of this class.
  /// </summary>
  public class ThreeButtonComponentAttributes : GH_ComponentAttributes {
    private float MinWidth {
      get {
        var spacers = new List<string> {
          _spacerTxt
        };
        float sp = WidthAttributes.MaxTextWidth(spacers, GH_FontServer.Small);
        var buttons = new List<string> {
          _button1Text,
          _button2Text,
          _button3Text
        };
        float bt = WidthAttributes.MaxTextWidth(buttons, GH_FontServer.Standard);

        float num = Math.Max(Math.Max(sp, bt), 90);
        return num;
      }
    }

    private readonly Action _action1;
    private readonly Action _action2;
    private readonly Action _action3;
    private readonly string _button1Text;

    // text to be displayed button 1
    private readonly string _button2Text;

    // text to be displayed button 2
    private readonly string _button3Text;

    private readonly bool _greyoutButton3;
    private readonly string _spacerTxt;

    // text to be displayed button 3
    private RectangleF _button1Bounds;

    // area for button1 to be displayed
    private RectangleF _button2Bounds;

    // area for button2 to be displayed
    private RectangleF _button3Bounds;

    // area for button3 to be displayed
    private bool _mouseDown1;

    private bool _mouseDown2;
    private bool _mouseDown3;
    private bool _mouseOver1;
    private bool _mouseOver2;
    private bool _mouseOver3;
    private RectangleF _spacerBounds;

    public ThreeButtonComponentAttributes(GH_Component owner, string display1Text, string display2Text, string display3Text, Action clickHandle1, Action clickHandle2, Action clickHandle3, bool canOpen, string spacerText = "") : base(owner) {
      _button1Text = display1Text;
      _button2Text = display2Text;
      _button3Text = display3Text;
      _spacerTxt = spacerText;
      _action1 = clickHandle1;
      _action2 = clickHandle2;
      _action3 = clickHandle3;
      _greyoutButton3 = !canOpen;
    }

    public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e) {
      if (e.Button == MouseButtons.Left) {
        RectangleF rec1 = _button1Bounds;
        if (rec1.Contains(e.CanvasLocation)) {
          _mouseDown1 = true;
          _mouseDown2 = false;
          _mouseDown3 = false;
          Owner.ExpireSolution(true);
          return GH_ObjectResponse.Capture;
        }
        RectangleF rec2 = _button2Bounds;
        if (rec2.Contains(e.CanvasLocation)) {
          _mouseDown1 = false;
          _mouseDown2 = true;
          _mouseDown3 = false;
          Owner.ExpireSolution(true);
          return GH_ObjectResponse.Capture;
        }
        RectangleF rec3 = _button3Bounds;
        if (rec3.Contains(e.CanvasLocation)) {
          _mouseDown1 = false;
          _mouseDown2 = false;
          _mouseDown3 = true;
          ;
          Owner.ExpireSolution(true);
          return GH_ObjectResponse.Capture;
        }
      }

      return base.RespondToMouseDown(sender, e);
    }

    public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e) {
      if (_button1Bounds.Contains(e.CanvasLocation)) {
        _mouseOver1 = true;
        _mouseOver2 = false;
        _mouseOver3 = false;
        Owner.OnDisplayExpired(false);
        sender.Cursor = Cursors.Hand;
        return GH_ObjectResponse.Capture;
      }

      if (_button2Bounds.Contains(e.CanvasLocation)) {
        _mouseOver2 = true;
        _mouseOver1 = false;
        _mouseOver3 = false;
        Owner.OnDisplayExpired(false);
        sender.Cursor = Cursors.Hand;
        return GH_ObjectResponse.Capture;
      }

      if (_button3Bounds.Contains(e.CanvasLocation)) {
        _mouseOver3 = true;
        _mouseOver1 = false;
        _mouseOver2 = false;
        Owner.OnDisplayExpired(false);
        sender.Cursor = Cursors.Hand;
        return GH_ObjectResponse.Capture;
      }

      if (_mouseOver1 | _mouseOver2 | _mouseOver3) {
        _mouseOver1 = false;
        _mouseOver2 = false;
        _mouseOver3 = false;
        Owner.OnDisplayExpired(false);
        Instances.CursorServer.ResetCursor(sender);
        return GH_ObjectResponse.Release;
      }

      return base.RespondToMouseMove(sender, e);
    }

    public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e) {
      if (e.Button == MouseButtons.Left) {
        RectangleF rec1 = _button1Bounds;
        if (rec1.Contains(e.CanvasLocation)) {
          if (_mouseDown1) {
            _mouseDown1 = false;
            _mouseDown2 = false;
            _mouseDown3 = false;
            _action1();
            Owner.ExpireSolution(true);
            return GH_ObjectResponse.Release;
          }
        }

        RectangleF rec2 = _button2Bounds;
        if (rec2.Contains(e.CanvasLocation)) {
          if (_mouseDown2) {
            _mouseDown1 = false;
            _mouseDown2 = false;
            _mouseDown3 = false;
            _action2();
            Owner.ExpireSolution(true);
            return GH_ObjectResponse.Release;
          }
        }

        RectangleF rec3 = _button3Bounds;
        if (rec3.Contains(e.CanvasLocation)) {
          if (_mouseDown3) {
            _mouseDown1 = false;
            _mouseDown2 = false;
            _mouseDown3 = false;
            _action3();
            Owner.ExpireSolution(true);
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
      _button1Bounds = new RectangleF(Bounds.X + 2 * s, Bounds.Bottom + h0 + 2 * s, Bounds.Width - 4 * s, h1);
      _button2Bounds = new RectangleF(Bounds.X + 2 * s, _button1Bounds.Bottom + 2 * s, Bounds.Width - 4 * s, h1);
      _button3Bounds = new RectangleF(Bounds.X + 2 * s, _button2Bounds.Bottom + 2 * s, Bounds.Width - 4 * s, h1);
      //update component bounds
      Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height + h0 + 3 * h1 + 8 * s);
    }

    protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel) {
      if (graphics != null) {
        base.Render(canvas, graphics, channel);
      }

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

      // Draw divider line
      if (_spacerTxt != "") {
        graphics.DrawString(_spacerTxt, sml, Colour.AnnotationTextDark, _spacerBounds, GH_TextRenderingConstants.CenterCenter);
        graphics.DrawLine(spacer, _spacerBounds.X, _spacerBounds.Y + _spacerBounds.Height / 2, _spacerBounds.X + (_spacerBounds.Width - GH_FontServer.StringWidth(_spacerTxt, sml)) / 2 - 4, _spacerBounds.Y + _spacerBounds.Height / 2);
        graphics.DrawLine(spacer, _spacerBounds.X + (_spacerBounds.Width - GH_FontServer.StringWidth(_spacerTxt, sml)) / 2 + GH_FontServer.StringWidth(_spacerTxt, sml) + 4, _spacerBounds.Y + _spacerBounds.Height / 2, _spacerBounds.X + _spacerBounds.Width, _spacerBounds.Y + _spacerBounds.Height / 2);
      }

      // ### button 1 ###
      // Draw button box
      System.Drawing.Drawing2D.GraphicsPath button1 = ButtonAttributes.DrawRoundedRect(_button1Bounds, 2);

      Brush normal_colour1 = Colour.ButtonColour;
      Brush hover_colour1 = Colour.HoverButtonColour;
      Brush clicked_colour1 = Colour.ClickedButtonColour;

      Brush butCol1 = (_mouseOver1) ? hover_colour1 : normal_colour1;
      graphics.FillPath(_mouseDown1 ? clicked_colour1 : butCol1, button1);

      // draw button edge
      Color edgeColor1 = Colour.ButtonBorderColour;
      Color edgeHover1 = Colour.HoverBorderColour;
      Color edgeClick1 = Colour.ClickedBorderColour;
      Color edgeCol1 = (_mouseOver1) ? edgeHover1 : edgeColor1;
      var pen1 = new Pen(_mouseDown1 ? edgeClick1 : edgeCol1) {
        Width = (_mouseDown1) ? 0.8f : 0.5f
      };
      graphics.DrawPath(pen1, button1);

      // draw button glow
      System.Drawing.Drawing2D.GraphicsPath overlay1 = ButtonAttributes.DrawRoundedRect(_button1Bounds, 2, true);
      graphics.FillPath(new SolidBrush(Color.FromArgb(_mouseDown1 ? 0 : _mouseOver1 ? 40 : 60, 255, 255, 255)), overlay1);

      // draw button text
      graphics.DrawString(_button1Text, font, Colour.AnnotationTextBright, _button1Bounds, GH_TextRenderingConstants.CenterCenter);

      // ### button 2 ###
      // Draw button box
      System.Drawing.Drawing2D.GraphicsPath button2 = ButtonAttributes.DrawRoundedRect(_button2Bounds, 2);

      Brush normal_colour2 = Colour.ButtonColour;
      Brush hover_colour2 = Colour.HoverButtonColour;
      Brush clicked_colour2 = Colour.ClickedButtonColour;

      Brush butCol2 = (_mouseOver2) ? hover_colour2 : normal_colour2;
      graphics.FillPath(_mouseDown2 ? clicked_colour2 : butCol2, button2);

      // draw button edge
      Color edgeColor2 = Colour.ButtonBorderColour;
      Color edgeHover2 = Colour.HoverBorderColour;
      Color edgeClick2 = Colour.ClickedBorderColour;
      Color edgeCol2 = (_mouseOver2) ? edgeHover2 : edgeColor2;
      var pen2 = new Pen(_mouseDown2 ? edgeClick2 : edgeCol2) {
        Width = (_mouseDown2) ? 0.8f : 0.5f
      };
      graphics.DrawPath(pen2, button2);

      // draw button glow
      System.Drawing.Drawing2D.GraphicsPath overlay2 = ButtonAttributes.DrawRoundedRect(_button2Bounds, 2, true);
      graphics.FillPath(new SolidBrush(Color.FromArgb(_mouseDown2 ? 0 : _mouseOver2 ? 40 : 60, 255, 255, 255)), overlay2);

      // draw button text
      graphics.DrawString(_button2Text, font, Colour.AnnotationTextBright, _button2Bounds, GH_TextRenderingConstants.CenterCenter);

      // ### button 3 ###
      // Draw button box
      System.Drawing.Drawing2D.GraphicsPath button3 = ButtonAttributes.DrawRoundedRect(_button3Bounds, 2);

      Brush normal_colour3 = Colour.ButtonColour;
      Brush hover_colour3 = Colour.HoverButtonColour;
      Brush clicked_colour3 = Colour.ClickedButtonColour;

      Brush butCol3 = (_mouseOver3) ? hover_colour3 : normal_colour3;
      graphics.FillPath(_mouseDown3 ? clicked_colour3 : butCol3, button3);

      // draw button edge
      Color edgeColor3 = Colour.ButtonBorderColour;
      Color edgeHover3 = Colour.HoverBorderColour;
      Color edgeClick3 = Colour.ClickedBorderColour;
      Color edgeCol3 = (_mouseOver3) ? edgeHover3 : edgeColor3;
      var pen3 = new Pen(_mouseDown3 ? edgeClick3 : edgeCol3) {
        Width = (_mouseDown3) ? 0.8f : 0.5f
      };
      graphics.DrawPath(pen3, button3);

      // draw button glow
      System.Drawing.Drawing2D.GraphicsPath overlay3 = ButtonAttributes.DrawRoundedRect(_button3Bounds, 2, true);
      graphics.FillPath(new SolidBrush(Color.FromArgb(_mouseDown3 ? 0 : _mouseOver3 ? 40 : 60, 255, 255, 255)), overlay3);

      // draw button text
      graphics.DrawString(_button3Text, font, Colour.AnnotationTextBright, _button3Bounds, GH_TextRenderingConstants.CenterCenter);
    }
  }
}
