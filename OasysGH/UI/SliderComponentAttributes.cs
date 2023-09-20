using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Base;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Special;
using OasysGH.UI.Helpers;

namespace OasysGH.UI {
  /// <summary>
  /// Class to create custom component UI with a single dropdown menu
  ///
  /// To use this class override CreateAttributes() in component class and set m_attributes to an instance of this class.
  /// </summary>
  public class SliderComponentAttributes : GH_ComponentAttributes {
    private float MinWidth {
      get {
        var spacers = new List<string> {
          _spacerTxt
        };
        float sp = WidthAttributes.MaxTextWidth(spacers, GH_FontServer.Small);
        float num = Math.Max(sp, 90);
        return num;
      }
    }

    private readonly Action<double, double> _changeMaxMin;
    private readonly Action<double> _returnSliderValue;
    private readonly string _spacerTxt;
    private double _currentValue;
    private float _deltaX;
    private float _dragMouseStartX;

    // location of mouse at drag start
    // moved Y-location of scroll element
    private bool _dragX;

    private bool _first;
    private RectangleF _grabBound;
    private double _maxValue;

    // bound around the value grab
    private double _minValue;

    private bool _mouseOver;
    private int _noDigits;
    private float _scrollStartX;
    private RectangleF _sliderBound;

    // area where the slider is displayed
    private RectangleF _sliderValTextBound;

    // bound where text is displayed
    // location of scroll element at drag start
    private RectangleF _spacerBounds;

    public SliderComponentAttributes(GH_Component owner, Action<double> sliderValue, Action<double, double> setMaxMinVals, double initValue, double minVal, double maxVal, int digits, string spacerText = "") : base(owner) {
      _spacerTxt = spacerText;
      _returnSliderValue = sliderValue;
      _changeMaxMin = setMaxMinVals;
      _minValue = minVal;
      _maxValue = maxVal;
      _currentValue = initValue;
      _noDigits = digits;
      _first = true;
    }

    public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e) {
      RectangleF rec = _grabBound;
      if (rec.Contains(e.CanvasLocation)) {
        var hiddenSlider = new GH_NumberSlider();
        hiddenSlider.Slider.Maximum = (decimal)_maxValue;
        hiddenSlider.Slider.Minimum = (decimal)_minValue;
        hiddenSlider.Slider.DecimalPlaces = _noDigits;
        hiddenSlider.Slider.Type = _noDigits == 0 ? GH_SliderAccuracy.Integer : GH_SliderAccuracy.Float;
        hiddenSlider.Name = Owner.Name + " Slider";
        hiddenSlider.Slider.Value = (decimal)_currentValue;
        var gH_MenuSliderForm = new GH_NumberSliderPopup();
        GH_WindowsFormUtil.CenterFormOnCursor(gH_MenuSliderForm, true);
        gH_MenuSliderForm.Setup(hiddenSlider);
        //hiddenSlider.PopupEditor();
        DialogResult res = gH_MenuSliderForm.ShowDialog();
        if (res == DialogResult.OK) {
          _first = true;
          _maxValue = (double)hiddenSlider.Slider.Maximum;
          _minValue = (double)hiddenSlider.Slider.Minimum;
          _currentValue = (double)hiddenSlider.Slider.Value;
          _noDigits = hiddenSlider.Slider.Type == GH_SliderAccuracy.Integer ? 0 : hiddenSlider.Slider.DecimalPlaces;
          _changeMaxMin(_maxValue, _minValue);
          Owner.OnDisplayExpired(false);
          Owner.ExpireSolution(true);
          return GH_ObjectResponse.Handled;
        }
      }

      return GH_ObjectResponse.Ignore;
    }

    public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e) {
      if (e.Button == MouseButtons.Left) {
        RectangleF rec = _grabBound;
        var comp = Owner as GH_Component;
        if (rec.Contains(e.CanvasLocation)) {
          _dragMouseStartX = e.CanvasLocation.X;
          _dragX = true;
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Capture;
        }
      }

      return base.RespondToMouseDown(sender, e);
    }

    public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e) {
      if (_dragX) {
        var comp = Owner as GH_Component;

        _deltaX = e.CanvasLocation.X - _dragMouseStartX;

        Instances.CursorServer.AttachCursor(sender, "GH_NumericSlider");

        comp.ExpireSolution(true);
        return GH_ObjectResponse.Ignore;
      }

      RectangleF rec = _grabBound;
      if (rec.Contains(e.CanvasLocation)) {
        _mouseOver = true;
        Instances.CursorServer.AttachCursor(sender, "GH_NumericSlider");
        return GH_ObjectResponse.Capture;
      }

      if (_mouseOver) {
        _mouseOver = false;
        Instances.CursorServer.ResetCursor(sender);
        return GH_ObjectResponse.Release;
      }

      return base.RespondToMouseMove(sender, e);
    }

    public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e) {
      if (e.Button == MouseButtons.Left) {
        var comp = Owner as GH_Component;
        if (_dragX) {
          // if drag was true then we release it here:
          _scrollStartX += _deltaX;
          _deltaX = 0;
          _dragX = false;
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Release;
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

    protected override void Layout() {
      base.Layout();

      // first change the width to suit; using max to determine component visualisation style
      FixLayout();

      int s = 2; // spacing to edges and internal between boxes

      // create bound for spacer and title
      int h0 = 0; // height of spacer bound
      if (_spacerTxt != "") {
        Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height - (CentralSettings.CanvasObjectIcons ? 5 : 0));
        h0 = 10;
        _spacerBounds = new RectangleF(Bounds.X, Bounds.Bottom + s / 2, Bounds.Width, h0);
      }

      int hslider = 15; // height of bound for slider
      int bhslider = 10; // height and width of grab

      // create overall bound for slider
      _sliderBound = new RectangleF(Bounds.X + 2 * s, Bounds.Bottom + h0 + 2 * s, Bounds.Width - 2 - 4 * s, hslider);

      // slider grab
      _grabBound = new RectangleF(_sliderBound.X + _deltaX + _scrollStartX, _sliderBound.Y + _sliderBound.Height / 2 - bhslider / 2, bhslider, bhslider);

      // round current value for snapping
      _currentValue = Math.Round(_currentValue, _noDigits);
      double dragPercentage;

      // when component is initiated or value range is changed
      // calculate position of grab
      if (_first) {
        dragPercentage = (_currentValue - _minValue) / (_maxValue - _minValue);
        _scrollStartX = (float)(dragPercentage * (_sliderBound.Width - _grabBound.Width));
        _first = false;
      }

      // horizontal position (.X)
      if (_deltaX + _scrollStartX >= 0) // handle if user drags left of starting point
      {
        // dragging right-wards:
        if (_sliderBound.Width - _grabBound.Width >= _deltaX + _scrollStartX) // handles if user drags below bottom point
        {
          // update scroll bar position for normal scroll event within bounds
          dragPercentage = (_deltaX + _scrollStartX) / (_sliderBound.Width - _grabBound.Width);
          _currentValue = Math.Round(_minValue + dragPercentage * (_maxValue - _minValue), _noDigits);
          dragPercentage = (_currentValue - _minValue) / (_maxValue - _minValue);
        } else {
          // scroll reached end
          dragPercentage = 1;
          _scrollStartX = _sliderBound.Width - _grabBound.Width;
          _deltaX = 0;
          _currentValue = _maxValue;
        }
      } else {
        // scroll reached start
        dragPercentage = 0;
        _scrollStartX = 0;
        _deltaX = 0;
        _currentValue = _minValue;
      }

      // set grab item position with snap
      _grabBound.X = _sliderBound.X + (float)(dragPercentage * (_sliderBound.Width - _grabBound.Width));

      // return new current value to component
      _returnSliderValue(_currentValue);

      // text box for value display

      if (_currentValue < (_maxValue - _minValue) / 2) // we are in the left half of the range
        _sliderValTextBound = new RectangleF(_grabBound.X + _grabBound.Width, _sliderBound.Y, _sliderBound.X + _sliderBound.Width - _grabBound.X + _grabBound.Width, _sliderBound.Height);
      else // we are in the right half of the range
        _sliderValTextBound = new RectangleF(_sliderBound.X, _sliderBound.Y, _grabBound.X - _sliderBound.X, _sliderBound.Height);

      //update component bounds
      Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height + h0 + hslider + 4 * s);
    }

    protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel) {
      base.Render(canvas, graphics, channel);
      if (channel == GH_CanvasChannel.Objects) {
        CustomRender(graphics);
      }
    }
    internal void CustomRender(Graphics graphics) {
      //Draw divider line
      if (_spacerTxt != "") {
        var spacer = new Pen(Colour.SpacerColour);
        Font sml = GH_FontServer.Small;
        // adjust fontsize to high resolution displays
        sml = new Font(sml.FontFamily, sml.Size / GH_GraphicsUtil.UiScale, FontStyle.Regular);

        graphics.DrawString(_spacerTxt, sml, Colour.AnnotationTextDark, _spacerBounds, GH_TextRenderingConstants.CenterCenter);
        graphics.DrawLine(spacer, _spacerBounds.X, _spacerBounds.Y + _spacerBounds.Height / 2, _spacerBounds.X + (_spacerBounds.Width - GH_FontServer.StringWidth(_spacerTxt, sml)) / 2 - 4, _spacerBounds.Y + _spacerBounds.Height / 2);
        graphics.DrawLine(spacer, _spacerBounds.X + (_spacerBounds.Width - GH_FontServer.StringWidth(_spacerTxt, sml)) / 2 + GH_FontServer.StringWidth(_spacerTxt, sml) + 4, _spacerBounds.Y + _spacerBounds.Height / 2, _spacerBounds.X + _spacerBounds.Width, _spacerBounds.Y + _spacerBounds.Height / 2);
      }

      // draw drag line and intervals
      var line = new Pen(Colour.OasysDarkGrey);
      graphics.DrawLine(line, new PointF(_sliderBound.X + _grabBound.Width / 2, _sliderBound.Y + _sliderBound.Height / 2), new PointF(_sliderBound.X + _sliderBound.Width - _grabBound.Width / 2, _sliderBound.Y + _sliderBound.Height / 2));
      //graphics.DrawLine(line, new PointF(BorderBound.X + GrabBound.Width / 2, BorderBound.Y + BorderBound.Height / 3), new PointF(BorderBound.X + GrabBound.Width / 2, BorderBound.Y + BorderBound.Height * 2 / 3));
      //graphics.DrawLine(line, new PointF(BorderBound.X + BorderBound.Width - GrabBound.Width / 2, BorderBound.Y + BorderBound.Height / 3), new PointF(BorderBound.X + BorderBound.Width - GrabBound.Width / 2, BorderBound.Y + BorderBound.Height * 2 / 3));

      // draw grab item
      var pen = new Pen(Colour.OasysDarkBlue);
      pen.Width = 2f;
      var button = new RectangleF(_grabBound.X, _grabBound.Y, _grabBound.Width, _grabBound.Height);
      button.Inflate(-2, -2);
      Brush fill = new SolidBrush(Colour.OasysLightGrey);
      graphics.FillEllipse(fill, button);
      graphics.DrawEllipse(pen, button);

      // Draw display value text
      var font = new Font(GH_FontServer.FamilyStandard, 7);
      // adjust fontsize to high resolution displays
      font = new Font(font.FontFamily, font.Size / GH_GraphicsUtil.UiScale, FontStyle.Regular);
      string val = string.Format(new System.Globalization.NumberFormatInfo() { NumberDecimalDigits = _noDigits }, "{0:F}", new decimal(_currentValue));

      graphics.DrawString(val, font, Colour.AnnotationTextDark, _sliderValTextBound, ((_currentValue - _minValue) / (_maxValue - _minValue) < 0.5) ? GH_TextRenderingConstants.NearCenter : GH_TextRenderingConstants.FarCenter);
    }
  }
}
