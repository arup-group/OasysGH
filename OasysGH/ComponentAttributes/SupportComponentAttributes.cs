using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using OasysGH.UI.Helpers;

namespace OasysGH.UI {

  /// <summary>
  /// Class to create custom component UI with three buttons and 6 check box toggles underneath
  ///
  /// This class is made for the CreateSupport component
  ///
  /// To use this method override CreateAttributes() in component class and set m_attributes = new SupportComponentAttributes(...
  /// </summary>
  public class SupportComponentAttributes : GH_ComponentAttributes {
    private float MinWidth {
      get {
        return 90;
      }
      set {
        MinWidth = value;
      }
    }

    private readonly string _spacerTxt;

    // function that sends back the user input to component
    private readonly Action<bool, bool, bool, bool, bool, bool> _update;

    private bool _mouseOver;
    private bool _mouseOverFix;
    private bool _mouseOverFree;
    private bool _mouseOverPin;

    // text boxes bounds for pre-set restraints
    private RectangleF _spacerBounds;

    private RectangleF _txtFixBounds;
    private RectangleF _txtFreeBounds;
    private RectangleF _txtPinBounds;

    // restraints set by component
    private bool _x;

    // bounds for check boxes
    private RectangleF _xBounds;

    // annotation text bounds
    private RectangleF _xTxtBounds;

    private bool _xx;
    private RectangleF _xxBounds;
    private RectangleF _xxTxtBounds;
    private bool _y;
    private RectangleF _yBounds;
    private RectangleF _yTxtBounds;
    private bool _yy;
    private RectangleF _yyBounds;
    private RectangleF _yyTxtBounds;
    private bool _z;
    private RectangleF _zBounds;
    private RectangleF _zTxtBounds;
    private bool _zz;
    private RectangleF _zzBounds;
    private RectangleF _zzTxtBounds;
    public SupportComponentAttributes(GH_Component owner, Action<bool, bool, bool, bool, bool, bool> updateHandle, string spacerText, bool resx, bool resy, bool resz, bool resxx, bool resyy, bool reszz) : base(owner) {
      _x = resx;
      _y = resy;
      _z = resz;
      _xx = resxx;
      _yy = resyy;
      _zz = reszz;
      _update = updateHandle;
      _spacerTxt = spacerText;
    }

    #region Custom layout logic
    public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e) {
      if (e.Button == System.Windows.Forms.MouseButtons.Left) {
        var comp = Owner as GH_Component;

        if (_txtFreeBounds.Contains(e.CanvasLocation)) {
          if (_x == false & _y == false & _z == false & _xx == false & _yy == false & _zz == false) return GH_ObjectResponse.Handled;
          comp.RecordUndoEvent("Free");
          _x = false;
          _y = false;
          _z = false;
          _xx = false;
          _yy = false;
          _zz = false;
          _update(_x, _y, _z, _xx, _yy, _zz);
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Handled;
        }

        if (_txtPinBounds.Contains(e.CanvasLocation)) {
          if (_x == true & _y == true & _z == true & _xx == false & _yy == false & _zz == false) return GH_ObjectResponse.Handled;
          comp.RecordUndoEvent("Pin");
          _x = true;
          _y = true;
          _z = true;
          _xx = false;
          _yy = false;
          _zz = false;
          _update(_x, _y, _z, _xx, _yy, _zz);
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Handled;
        }

        if (_txtFixBounds.Contains(e.CanvasLocation)) {
          if (_x == true & _y == true & _z == true & _xx == true & _yy == true & _zz == true) return GH_ObjectResponse.Handled;
          comp.RecordUndoEvent("Fix");
          _x = true;
          _y = true;
          _z = true;
          _xx = true;
          _yy = true;
          _zz = true;
          _update(_x, _y, _z, _xx, _yy, _zz);
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Handled;
        }
        if (_xTxtBounds.Contains(e.CanvasLocation) | _xBounds.Contains(e.CanvasLocation)) {
          comp.RecordUndoEvent("Toggle X");
          _x = !_x;
          _update(_x, _y, _z, _xx, _yy, _zz);
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Handled;
        }
        if (_yTxtBounds.Contains(e.CanvasLocation) | _yBounds.Contains(e.CanvasLocation)) {
          comp.RecordUndoEvent("Toggle Y");
          _y = !_y;
          _update(_x, _y, _z, _xx, _yy, _zz);
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Handled;
        }
        if (_zTxtBounds.Contains(e.CanvasLocation) | _zBounds.Contains(e.CanvasLocation)) {
          comp.RecordUndoEvent("Toggle Z");
          _z = !_z;
          _update(_x, _y, _z, _xx, _yy, _zz);
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Handled;
        }
        if (_xxTxtBounds.Contains(e.CanvasLocation) | _xxBounds.Contains(e.CanvasLocation)) {
          comp.RecordUndoEvent("Toggle XX");
          _xx = !_xx;
          _update(_x, _y, _z, _xx, _yy, _zz);
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Handled;
        }
        if (_yyTxtBounds.Contains(e.CanvasLocation) | _yyBounds.Contains(e.CanvasLocation)) {
          comp.RecordUndoEvent("Toggle YY");
          _yy = !_yy;
          _update(_x, _y, _z, _xx, _yy, _zz);
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Handled;
        }
        if (_zzTxtBounds.Contains(e.CanvasLocation) | _zzBounds.Contains(e.CanvasLocation)) {
          comp.RecordUndoEvent("Toggle ZZ");
          _zz = !_zz;
          _update(_x, _y, _z, _xx, _yy, _zz);
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Handled;
        }
      }
      return base.RespondToMouseDown(sender, e);
    }

    public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e) {
      if (_xBounds.Contains(e.CanvasLocation) |
          _yBounds.Contains(e.CanvasLocation) |
          _zBounds.Contains(e.CanvasLocation) |
          _xxBounds.Contains(e.CanvasLocation) |
          _yyBounds.Contains(e.CanvasLocation) |
          _zzBounds.Contains(e.CanvasLocation)) {
        _mouseOver = true;
        sender.Cursor = System.Windows.Forms.Cursors.Hand;
        return GH_ObjectResponse.Capture;
      }

      if (_txtFreeBounds.Contains(e.CanvasLocation)) {
        if (_x == false & _y == false & _z == false & _xx == false & _yy == false & _zz == false) {
          Instances.CursorServer.ResetCursor(sender);
          return GH_ObjectResponse.Release;
        }
        _mouseOverFree = true;
        _mouseOverPin = false;
        _mouseOverFix = false;
        Owner.OnDisplayExpired(false);
        sender.Cursor = System.Windows.Forms.Cursors.Hand;
        return GH_ObjectResponse.Capture;
      }
      if (_txtPinBounds.Contains(e.CanvasLocation)) {
        if (_x == true & _y == true & _z == true & _xx == false & _yy == false & _zz == false) {
          Instances.CursorServer.ResetCursor(sender);
          return GH_ObjectResponse.Release;
        }
        _mouseOverPin = true;
        _mouseOverFree = false;
        _mouseOverFix = false;
        sender.Cursor = System.Windows.Forms.Cursors.Hand;
        Owner.OnDisplayExpired(false);
        return GH_ObjectResponse.Capture;
      }
      if (_txtFixBounds.Contains(e.CanvasLocation)) {
        if (_x == true & _y == true & _z == true & _xx == true & _yy == true & _zz == true) {
          Instances.CursorServer.ResetCursor(sender);
          return GH_ObjectResponse.Release;
        }

        _mouseOverFix = true;
        _mouseOverFree = false;
        _mouseOverPin = false;
        Owner.OnDisplayExpired(false);
        sender.Cursor = System.Windows.Forms.Cursors.Hand;
        return GH_ObjectResponse.Capture;
      }

      if (_mouseOver | _mouseOverFree | _mouseOverPin | _mouseOverFix) {
        _mouseOver = false;
        _mouseOverFree = false;
        _mouseOverPin = false;
        _mouseOverFix = false;
        Instances.CursorServer.ResetCursor(sender);
        Owner.OnDisplayExpired(false);
        return GH_ObjectResponse.Release;
      }

      return base.RespondToMouseMove(sender, e);
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

    protected override void Layout() {
      base.Layout();
      // Set the actual layout of the component here:

      // first change the width to suit; using max to determine component visualisation style
      FixLayout();

      int s = 3; //spacing to edges and internal between boxes

      //spacer and title
      int h0 = 0;
      if (_spacerTxt != "") {
        Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height - (CentralSettings.CanvasObjectIcons ? 5 : 0));
        h0 = 10;
        _spacerBounds = new RectangleF(Bounds.X, Bounds.Bottom + s / 2, Bounds.Width, h0);
      }

      int h1 = 15; // height
                   // create text box placeholders
      float w = (Bounds.Width - 3 * s) / 3; // width of heach text box
      _txtFreeBounds = new RectangleF(Bounds.X + s / 2 + 1, Bounds.Bottom + h0 + 1.5f * s, w, h1);
      _txtPinBounds = new RectangleF(Bounds.X + 1.5f * s + w, Bounds.Bottom + h0 + 1.5f * s, w, h1);
      _txtFixBounds = new RectangleF(Bounds.X + 2.5f * s - 1 + 2 * w, Bounds.Bottom + h0 + 1.5f * s, w, h1);

      // create annotation (x, y, z, xx, yy, zz) placeholders
      w = (Bounds.Width - 6 * s) / 6; // width of each check box
      int h2 = 15; // height
      _xTxtBounds = new RectangleF(Bounds.X + s, Bounds.Bottom + h0 + h1 * 2 / 3 + 3 * s, w, h2);
      _yTxtBounds = new RectangleF(Bounds.X + 1.5f * s + w, Bounds.Bottom + h0 + h1 * 2 / 3 + 3 * s, w, h2);
      _zTxtBounds = new RectangleF(Bounds.X + 2.5f * s + 2 * w, Bounds.Bottom + h0 + h1 * 2 / 3 + 3 * s, w, h2);
      _xxTxtBounds = new RectangleF(Bounds.X + 3f * s + 3 * w, Bounds.Bottom + h0 + h1 * 2 / 3 + 3 * s, w + s, h2);
      _yyTxtBounds = new RectangleF(Bounds.X + 4f * s + 4 * w, Bounds.Bottom + h0 + h1 * 2 / 3 + 3 * s, w + s, h2);
      _zzTxtBounds = new RectangleF(Bounds.X + 5f * s - 1 + 5 * w, Bounds.Bottom + h0 + h1 * 2 / 3 + 3 * s, w + s, h2);

      // create check box placeholders
      int h3 = 15;
      _xBounds = new RectangleF(Bounds.X + s / 2, Bounds.Bottom + h0 + h1 / 2 + h2 + 4 * s, w, h3);
      _yBounds = new RectangleF(Bounds.X + 1.5f * s + w, Bounds.Bottom + h0 + h1 / 2 + h2 + 4 * s, w, h3);
      _zBounds = new RectangleF(Bounds.X + 2.5f * s + 2 * w, Bounds.Bottom + h0 + h1 / 2 + h2 + 4 * s, w, h3);
      _xxBounds = new RectangleF(Bounds.X + 3.5f * s + 3 * w, Bounds.Bottom + h0 + h1 / 2 + h2 + 4 * s, w, h3);
      _yyBounds = new RectangleF(Bounds.X + 4.5f * s + 4 * w, Bounds.Bottom + h0 + h1 / 2 + h2 + 4 * s, w, h3);
      _zzBounds = new RectangleF(Bounds.X + 5.5f * s - 1 + 5 * w, Bounds.Bottom + h0 + h1 / 2 + h2 + 4 * s, w, h3);

      Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height + h0 + h1 / 2 + h2 + h3 + 5 * s);
    }

    #endregion Custom layout logic


    #region Custom Render logic
    protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel) {
      base.Render(canvas, graphics, channel);

      if (channel == GH_CanvasChannel.Objects) {
        //Text boxes
        Brush activeTextBrush = Brushes.White;
        Brush passiveTextBrush = new SolidBrush(Colour.OasysDarkBlue);
        Brush activeFillBrush = Colour.ButtonColour;
        Brush passiveFillBrush = new SolidBrush(Colour.OasysLightGrey);
        Color borderColour = Colour.OasysDarkBlue;
        Color passiveBorder = Color.DarkGray;
        Brush annoText = Brushes.Black;

        Font font = GH_FontServer.Standard;
        int s = 8;
        if (CentralSettings.CanvasFullNames) {
          s = 10;
          font = GH_FontServer.Standard;
        }

        // adjust fontsize to high resolution displays
        font = new Font(font.FontFamily, font.Size / GH_GraphicsUtil.UiScale, FontStyle.Regular);

        Font sml = GH_FontServer.Small;
        // adjust fontsize to high resolution displays
        sml = new Font(sml.FontFamily, sml.Size / GH_GraphicsUtil.UiScale, FontStyle.Regular);

        var pen = new Pen(borderColour);

        graphics.DrawString(_spacerTxt, sml, annoText, _spacerBounds, GH_TextRenderingConstants.CenterCenter);
        graphics.DrawLine(pen, _spacerBounds.X, _spacerBounds.Y + _spacerBounds.Height / 2, _spacerBounds.X + (_spacerBounds.Width - GH_FontServer.StringWidth(_spacerTxt, sml)) / 2 - 4, _spacerBounds.Y + _spacerBounds.Height / 2);
        graphics.DrawLine(pen, _spacerBounds.X + (_spacerBounds.Width - GH_FontServer.StringWidth(_spacerTxt, sml)) / 2 + GH_FontServer.StringWidth(_spacerTxt, sml) + 4, _spacerBounds.Y + _spacerBounds.Height / 2, _spacerBounds.X + _spacerBounds.Width, _spacerBounds.Y + _spacerBounds.Height / 2);

        graphics.DrawRectangle(pen, _txtFreeBounds.X, _txtFreeBounds.Y, _txtFreeBounds.Width, _txtFreeBounds.Height);
        graphics.DrawRectangle(pen, _txtPinBounds.X, _txtPinBounds.Y, _txtPinBounds.Width, _txtPinBounds.Height);
        graphics.DrawRectangle(pen, _txtFixBounds.X, _txtFixBounds.Y, _txtFixBounds.Width, _txtFixBounds.Height);

        Brush freeBrushPassive = (_mouseOverFree) ? Colour.HoverInactiveButtonColour : passiveFillBrush;
        graphics.FillRectangle((_x == false & _y == false & _z == false & _xx == false & _yy == false & _zz == false) ? activeFillBrush : freeBrushPassive, _txtFreeBounds);
        graphics.DrawString("Free", font, (_x == false & _y == false & _z == false & _xx == false & _yy == false & _zz == false) ? activeTextBrush : passiveTextBrush, _txtFreeBounds, GH_TextRenderingConstants.CenterCenter);

        Brush pinBrushPassive = (_mouseOverPin) ? Colour.HoverInactiveButtonColour : passiveFillBrush;
        graphics.FillRectangle((_x == true & _y == true & _z == true & _xx == false & _yy == false & _zz == false) ? activeFillBrush : pinBrushPassive, _txtPinBounds);
        graphics.DrawString("Pin", font, (_x == true & _y == true & _z == true & _xx == false & _yy == false & _zz == false) ? activeTextBrush : passiveTextBrush, _txtPinBounds, GH_TextRenderingConstants.CenterCenter);

        Brush fixBrushPassive = (_mouseOverFix) ? Colour.HoverInactiveButtonColour : passiveFillBrush;
        graphics.FillRectangle((_x == true & _y == true & _z == true & _xx == true & _yy == true & _zz == true) ? activeFillBrush : fixBrushPassive, _txtFixBounds);
        graphics.DrawString("Fix", font, (_x == true & _y == true & _z == true & _xx == true & _yy == true & _zz == true) ? activeTextBrush : passiveTextBrush, _txtFixBounds, GH_TextRenderingConstants.CenterCenter);

        graphics.DrawString("x", font, annoText, _xTxtBounds, GH_TextRenderingConstants.CenterCenter);
        CheckBox.DrawCheckButton(graphics, new PointF(_xBounds.X + _xBounds.Width / 2, _xBounds.Y + _xBounds.Height / 2), _x, activeFillBrush, borderColour, passiveFillBrush, passiveBorder, s);

        graphics.DrawString("y", font, annoText, _yTxtBounds, GH_TextRenderingConstants.CenterCenter);
        CheckBox.DrawCheckButton(graphics, new PointF(_yBounds.X + _yBounds.Width / 2, _yBounds.Y + _yBounds.Height / 2), _y, activeFillBrush, borderColour, passiveFillBrush, passiveBorder, s);

        graphics.DrawString("z", font, annoText, _zTxtBounds, GH_TextRenderingConstants.CenterCenter);
        CheckBox.DrawCheckButton(graphics, new PointF(_zBounds.X + _zBounds.Width / 2, _zBounds.Y + _zBounds.Height / 2), _z, activeFillBrush, borderColour, passiveFillBrush, passiveBorder, s);

        graphics.DrawString("xx", font, annoText, _xxTxtBounds, GH_TextRenderingConstants.CenterCenter);
        CheckBox.DrawCheckButton(graphics, new PointF(_xxBounds.X + _xxBounds.Width / 2, _xxBounds.Y + _xxBounds.Height / 2), _xx, activeFillBrush, borderColour, passiveFillBrush, passiveBorder, s);

        graphics.DrawString("yy", font, annoText, _yyTxtBounds, GH_TextRenderingConstants.CenterCenter);
        CheckBox.DrawCheckButton(graphics, new PointF(_yyBounds.X + _yyBounds.Width / 2, _yyBounds.Y + _yyBounds.Height / 2), _yy, activeFillBrush, borderColour, passiveFillBrush, passiveBorder, s);

        graphics.DrawString("zz", font, annoText, _zzTxtBounds, GH_TextRenderingConstants.CenterCenter);
        CheckBox.DrawCheckButton(graphics, new PointF(_zzBounds.X + _zzBounds.Width / 2, _zzBounds.Y + _zzBounds.Height / 2), _zz, activeFillBrush, borderColour, passiveFillBrush, passiveBorder, s);
      }
    }

    #endregion Custom Render logic
  }
}
