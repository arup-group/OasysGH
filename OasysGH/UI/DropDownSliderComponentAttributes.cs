﻿using System;
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
using OasysGH.Components;
using OasysGH.UI.Helpers;

namespace OasysGH.UI {
  /// <summary>
  /// Class to create custom component UI with multiple dropdowns
  ///
  /// Note that it is the component's responsibility to dynamically update lists, this class is only displaying what it gets.
  ///
  /// To use this class override CreateAttributes() in component class and set m_attributes to an instance of this class.
  /// </summary>
  public class DropDownSliderComponentAttributes : GH_ComponentAttributes {
    private float MinWidth {
      get {
        float sp = WidthAttributes.MaxTextWidth(_spacerTxts, GH_FontServer.Small);
        float bt = 0;
        for (int i = 0; i < _dropdownlists.Count; i++) {
          float tbt = WidthAttributes.MaxTextWidth(_dropdownlists[i], new Font(GH_FontServer.FamilyStandard, 7));
          if (tbt > bt)
            bt = tbt;
        }
        float num = Math.Max(Math.Max(sp, bt), 90);
        return num;
      }
    }

    private readonly Action<int, int> _action;
    private readonly Action<double, double> _changeMaxMin;
    private readonly List<string> _displayTexts;
    private readonly List<List<string>> _dropdownlists;

    // the selected item text
    private readonly List<string> _initialTxts;

    private readonly Action<double> _returnSliderValue;
    private readonly List<string> _spacerTxts;
    private List<RectangleF> _borderBound;
    private List<RectangleF> _buttonBound;
    private double _currentValue;
    private float _deltaX;
    private float _deltaY;
    private float _dragMouseStartX;
    private float _dragMouseStartY;
    private bool _dragX;
    private bool _dragY;
    private bool _drawSlider;
    private List<RectangleF> _dropdownBound;
    private List<List<RectangleF>> _dropdownBounds;
    private bool _first;
    private RectangleF _grabBound;

    // location of mouse at drag start
    // moved Y-location of scroll element
    private int _maxNoRows = 10;

    private double _maxValue;

    // bound around the value grab
    private double _minValue;

    private bool _mouseOver;
    private int _noDigits;
    private RectangleF _scrollBar;
    private float _scrollStartX;

    // surrounding bound for vertical scroll element
    private float _scrollStartY;

    // location of scroll element at drag start
    private RectangleF _sliderBound;

    // area where the slider is displayed
    private RectangleF _sliderValTextBound;

    // list of descriptive texts above each dropdown
    private List<RectangleF> _spacerBounds;

    // area where the selected item is displayed
    private List<RectangleF> _textBound;

    private List<bool> _unfolded;

    public DropDownSliderComponentAttributes(GH_Component owner, Action<int, int> clickHandle, List<List<string>> dropdownContents, List<string> selections,
                                                                                                                                                    bool Slider, Action<double> sliderValue, Action<double, double> setMaxMinVals, double initValue, double maxVal, double minVal, int digits,
        List<string> spacerTexts = null, List<string> initialdescriptions = null) : base(owner) {
      _dropdownlists = dropdownContents;
      _spacerTxts = spacerTexts;
      _action = clickHandle;
      _initialTxts = initialdescriptions ?? null; // if no description is inputted then null initialTxt
      if (selections == null) {
        var tempDisplaytxt = new List<string>();
        for (int i = 0; i < _dropdownlists.Count; i++)
          tempDisplaytxt.Add((initialdescriptions == null) ? _dropdownlists[i][0] : initialdescriptions[i]);
        _displayTexts = tempDisplaytxt;
      } else
        _displayTexts = selections;

      _drawSlider = Slider;
      _returnSliderValue = sliderValue;
      _changeMaxMin = setMaxMinVals;
      _minValue = minVal;
      _maxValue = maxVal;
      _currentValue = initValue;
      _noDigits = digits;
      _first = true;
    }

    // lefternmost part of the selected/displayed item
    // right side bit where we place the button to unfold the dropdown list

    // initial text to be able to display a hint

    // content lists of items for dropdown

    // list of bounds for each item in dropdown list
    // surrounding bound for the entire dropdown list

    //function sending back the selection to component (i = dropdowncontentlist, j = selected item in that list)

    // list of bools for unfolded or closed dropdown

    public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e) {
      var comp = Owner as IExpirableComponent;
      comp.Expire = false;

      RectangleF rec = _grabBound;
      if (rec.Contains(e.CanvasLocation)) {
        var hiddenSlider = new GH_NumberSlider();
        hiddenSlider.Slider.Maximum = (decimal)_maxValue;
        hiddenSlider.Slider.Minimum = (decimal)_minValue;
        hiddenSlider.Slider.DecimalPlaces = _noDigits;
        hiddenSlider.Slider.Type = _noDigits == 0 ? GH_SliderAccuracy.Integer : GH_SliderAccuracy.Float;
        hiddenSlider.Name = comp.Name + " Slider";
        hiddenSlider.Slider.Value = (decimal)_currentValue;
        var gH_MenuSliderForm = new GH_NumberSliderPopup();
        GH_WindowsFormUtil.CenterFormOnCursor(gH_MenuSliderForm, true);
        gH_MenuSliderForm.Setup(hiddenSlider);
        DialogResult res = gH_MenuSliderForm.ShowDialog();
        if (res == DialogResult.OK) {
          _first = true;
          _maxValue = (double)hiddenSlider.Slider.Maximum;
          _minValue = (double)hiddenSlider.Slider.Minimum;
          _currentValue = (double)hiddenSlider.Slider.Value;
          _noDigits = hiddenSlider.Slider.Type == GH_SliderAccuracy.Integer ? 0 : hiddenSlider.Slider.DecimalPlaces;
          _changeMaxMin(_maxValue, _minValue);
          comp.OnDisplayExpired(false);

          comp.Expire = true;
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Handled;
        }
      }

      comp.Expire = true;
      return GH_ObjectResponse.Ignore;
    }

    public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e) {
      var comp = Owner as IExpirableComponent;
      comp.Expire = false;

      if (e.Button == MouseButtons.Left) {
        RectangleF rec = _grabBound;
        if (rec.Contains(e.CanvasLocation)) {
          _dragMouseStartX = e.CanvasLocation.X;
          _dragX = true;

          comp.ExpireSolution(true);
          comp.Expire = true;
          return GH_ObjectResponse.Capture;
        }

        for (int i = 0; i < _dropdownlists.Count; i++) {
          if (_unfolded[i]) {
            if (e.Button == MouseButtons.Left) {
              rec = _scrollBar;
              if (rec.Contains(e.CanvasLocation)) {
                _dragMouseStartY = e.CanvasLocation.Y;
                _dragY = true;

                comp.ExpireSolution(true);
                comp.Expire = true;
                return GH_ObjectResponse.Capture;
              }
            }
          }
        }
      }

      comp.Expire = true;
      return base.RespondToMouseDown(sender, e);
    }

    public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e) {
      var comp = Owner as IExpirableComponent;
      comp.Expire = false;

      if (_dragY) {
        _deltaY = e.CanvasLocation.Y - _dragMouseStartY;

        comp.ExpireSolution(true);
        comp.Expire = true;
        return GH_ObjectResponse.Ignore;
      }

      if (_dragX) {
        _deltaX = e.CanvasLocation.X - _dragMouseStartX;

        Instances.CursorServer.AttachCursor(sender, "GH_NumericSlider");

        comp.Expire = true;
        comp.ExpireSolution(true);
        return GH_ObjectResponse.Ignore;
      }

      RectangleF rec = _grabBound;
      if (rec.Contains(e.CanvasLocation)) {
        _mouseOver = true;
        Instances.CursorServer.AttachCursor(sender, "GH_NumericSlider");

        comp.Expire = true;
        return GH_ObjectResponse.Capture;
      }

      for (int i = 0; i < _buttonBound.Count; i++) {
        if (_buttonBound[i].Contains(e.CanvasLocation)) {
          _mouseOver = true;
          sender.Cursor = Cursors.Hand;

          comp.Expire = true;
          return GH_ObjectResponse.Capture;
        }
      }

      if (_mouseOver) {
        _mouseOver = false;
        Instances.CursorServer.ResetCursor(sender);

        comp.Expire = true;
        return GH_ObjectResponse.Release;
      }

      comp.Expire = true;
      return base.RespondToMouseMove(sender, e);
    }

    public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e) {
      var comp = Owner as IExpirableComponent;
      comp.Expire = false;

      if (e.Button == MouseButtons.Left) {
        if (_dragY) {
          // if drag was true then we release it here:
          _scrollStartY += _deltaY;
          _deltaY = 0;
          _dragY = false;

          comp.Expire = true;
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Release;
        }

        if (_dragX) {
          // if drag was true then we release it here:
          _scrollStartX += _deltaX;
          _deltaX = 0;
          _dragX = false;

          comp.Expire = true;
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Release;
        }

        for (int i = 0; i < _dropdownlists.Count; i++) {
          RectangleF rec = _borderBound[i];
          if (rec.Contains(e.CanvasLocation)) {
            _unfolded[i] = !_unfolded[i];
            // close any other dropdowns that may be unfolded
            for (int j = 0; j < _unfolded.Count; j++) {
              if (j == i)
                continue;
              _unfolded[j] = false;
            }

            comp.ExpireSolution(true);
            comp.Expire = true;
            return GH_ObjectResponse.Handled;
          }

          if (_unfolded[i]) {
            RectangleF rec2 = _dropdownBound[i];
            if (rec2.Contains(e.CanvasLocation)) {
              for (int j = 0; j < _dropdownBounds[i].Count; j++) {
                RectangleF rec3 = _dropdownBounds[i][j];
                if (rec3.Contains(e.CanvasLocation)) {
                  if (_displayTexts[i] != _dropdownlists[i][j]) {
                    // record an undo event so that user can ctrl + z
                    comp.RecordUndoEvent("Selected " + _dropdownlists[i][j]);

                    // change the displayed text on canvas
                    _displayTexts[i] = _dropdownlists[i][j];

                    // if initial texts exists then change all dropdowns below this one to the initial description
                    if (_initialTxts != null) {
                      for (int k = i + 1; k < _dropdownlists.Count; k++)
                        _displayTexts[k] = _initialTxts[k];
                    }

                    // send the selected item back to component (i = dropdownlist index, j = selected item in that list)
                    _action(i, j);

                    // close the dropdown
                    _unfolded[i] = !_unfolded[i];

                    comp.Expire = true;

                    // recalculate component
                    comp.ExpireSolution(true);
                  } else {
                    _unfolded[i] = !_unfolded[i];
                    comp.ExpireSolution(true);
                    comp.Expire = true;
                  }

                  return GH_ObjectResponse.Handled;
                }
              }
            } else {
              _unfolded[i] = !_unfolded[i];

              comp.ExpireSolution(true);
              comp.Expire = true;
              return GH_ObjectResponse.Handled;
            }
          }
        }
      }

      comp.Expire = true;
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

    // bound where text is displayed
    // location of scroll element at drag start
    // location of mouse at drag start
    // moved Y-location of scroll element
    protected override void Layout() {
      base.Layout();

      // first change the width to suit; using max to determine component visualisation style
      FixLayout();

      _spacerBounds = new List<RectangleF>();
      if (_borderBound == null)
        _borderBound = new List<RectangleF>();
      if (_textBound == null)
        _textBound = new List<RectangleF>();
      if (_buttonBound == null)
        _buttonBound = new List<RectangleF>();
      if (_dropdownBound == null)
        _dropdownBound = new List<RectangleF>();
      if (_dropdownBounds == null)
        _dropdownBounds = new List<List<RectangleF>>();
      if (_unfolded == null)
        _unfolded = new List<bool>();

      int s = 2; //spacing to edges and internal between boxes

      int h0 = 0;

      bool removeScroll = true;

      for (int i = 0; i < _dropdownlists.Count; i++) {
        //spacer and title
        if (_spacerTxts[i] != "") {
          if (i < 1)
            Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height - (CentralSettings.CanvasObjectIcons ? 5 : 0));

          h0 = 10;
          var tempSpacer = new RectangleF(Bounds.X, Bounds.Bottom + s / 2, Bounds.Width, h0);
          if (_spacerBounds.Count == i || _spacerBounds[i] == null)
            _spacerBounds.Add(tempSpacer);
          else
            _spacerBounds[i] = tempSpacer;
        }

        int h1 = 15; // height border
        int bw = h1; // button width

        // create text box border
        var tempBorder = new RectangleF(Bounds.X + 2 * s, Bounds.Bottom + h0 + 2 * s, Bounds.Width - 2 - 4 * s, h1);
        if (_borderBound.Count == i || _borderBound[i] == null)
          _borderBound.Add(tempBorder);
        else
          _borderBound[i] = tempBorder;

        // text box inside border
        var tempText = new RectangleF(_borderBound[i].X, _borderBound[i].Y, _borderBound[i].Width, _borderBound[i].Height);
        if (_textBound.Count == i || _textBound[i] == null)
          _textBound.Add(tempText);
        else
          _textBound[i] = tempText;

        // button area inside border
        var tempButton = new RectangleF(_borderBound[i].X + _borderBound[i].Width - bw, _borderBound[i].Y, bw, _borderBound[i].Height);
        if (_buttonBound.Count == i || _buttonBound[i] == null)
          _buttonBound.Add(tempButton);
        else
          _buttonBound[i] = tempButton;

        //update component bounds
        Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height + h0 + h1 + 4 * s);

        // create list of bounds for dropdown if dropdown is unfolded
        if (_unfolded.Count == i)
          _unfolded.Add(new bool()); //ensure we have a bool for every list

        if (_unfolded[i]) // if unfolded checked create dropdown list
        {
          removeScroll = false;

          if (_dropdownBounds[i] == null)
            _dropdownBounds[i] = new List<RectangleF>(); // if first time clicked create new list
          else
            _dropdownBounds[i].Clear(); // if previously created make sure to clear existing if content has changed
          for (int j = 0; j < _dropdownlists[i].Count; j++) {
            _dropdownBounds[i].Add(new RectangleF(_borderBound[i].X, _borderBound[i].Y + (j + 1) * h1 + s, _borderBound[i].Width, _borderBound[i].Height));
          }
          _dropdownBound[i] = new RectangleF(_borderBound[i].X, _borderBound[i].Y + h1 + s, _borderBound[i].Width, Math.Min(_dropdownlists[i].Count, _maxNoRows) * _borderBound[i].Height);

          //update component size if dropdown is unfolded to be able to capture mouseclicks
          Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height + _dropdownBound[i].Height + s);

          // additional move for the content (moves more than the scroll bar)
          float contentScroll = 0;

          // vertical scroll bar if number of items in dropdown list is bigger than max rows allowed
          if (_dropdownlists[i].Count > _maxNoRows) {
            if (_scrollBar == null)
              _scrollBar = new RectangleF();

            // setup size of scroll bar
            _scrollBar.X = _dropdownBound[i].X + _dropdownBound[i].Width - 8; // locate from right-side of dropdown area
                                                                              // compute height based on number of items in list, but with a minimum size of 2 rows
            _scrollBar.Height = (float)Math.Max(2 * h1, _dropdownBound[i].Height * ((double)_maxNoRows / ((double)_dropdownlists[i].Count)));
            _scrollBar.Width = 8; // width of mouse-grab area (actual scroll bar drawn later)

            // vertical position (.Y)
            if (_deltaY + _scrollStartY >= 0) // handle if user drags above starting point
            {
              // dragging downwards:
              if (_dropdownBound[i].Height - _scrollBar.Height >= _deltaY + _scrollStartY) // handles if user drags below bottom point
              {
                // update scroll bar position for normal scroll event within bounds
                _scrollBar.Y = _dropdownBound[i].Y + _deltaY + _scrollStartY;
              } else {
                // scroll reached bottom
                _scrollStartY = _dropdownBound[i].Height - _scrollBar.Height;
                _deltaY = 0;
              }
            } else {
              // scroll reached top
              _scrollStartY = 0;
              _deltaY = 0;
            }

            // calculate moved position of content
            float scrollBarMovedPercentage = (_dropdownBound[i].Y - _scrollBar.Y) / (_dropdownBound[i].Height - _scrollBar.Height);
            float scrollContentHeight = _dropdownlists[i].Count * h1 - _dropdownBound[i].Height;
            contentScroll = scrollBarMovedPercentage * scrollContentHeight;
          }

          // create list of text boxes (we will only draw the visible ones later)
          _dropdownBounds[i] = new List<RectangleF>();
          for (int j = 0; j < _dropdownlists[i].Count; j++) {
            _dropdownBounds[i].Add(new RectangleF(_borderBound[i].X, _borderBound[i].Y + (j + 1) * h1 + s + contentScroll, _borderBound[i].Width, h1));
          }
        } else {
          if (_dropdownBounds != null) {
            if (_dropdownBounds.Count == i)
              _dropdownBounds.Add(new List<RectangleF>());
            if (_dropdownBounds[i] != null)
              _dropdownBounds[i].Clear();
            if (_dropdownBound.Count == i)
              _dropdownBound.Add(new RectangleF());
            else
              _dropdownBound[i] = new RectangleF();
          }
        }
      }

      if (removeScroll) {
        _scrollBar = new RectangleF();
        _scrollStartY = 0;
      }

      // ### slider ###
      if (_drawSlider) {
        // create bound for spacer and title
        h0 = 0; // height of spacer bound
        if (_spacerTxts[_spacerTxts.Count - 1] != "") {
          h0 = 10;
          _spacerBounds.Add(new RectangleF(Bounds.X, Bounds.Bottom + s / 2, Bounds.Width, h0));
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
    }

    protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel) {
      base.Render(canvas, graphics, channel);
      if (channel == GH_CanvasChannel.Objects) {
        CustomRender(graphics);
      }
    }
    internal void CustomRender(Graphics graphics) {
      var spacer = new Pen(Colour.SpacerColour);
      var pen = new Pen(Colour.OasysDarkBlue) {
        Width = 0.5f
      };

      Font sml = GH_FontServer.Small;
      // adjust fontsize to high resolution displays
      sml = new Font(sml.FontFamily, sml.Size / GH_GraphicsUtil.UiScale, FontStyle.Regular);

      for (int i = 0; i < _dropdownlists.Count; i++) {
        //Draw divider line
        if (_spacerTxts[i] != "") {
          graphics.DrawString(_spacerTxts[i], sml, Colour.AnnotationTextDark, _spacerBounds[i], GH_TextRenderingConstants.CenterCenter);
          graphics.DrawLine(spacer, _spacerBounds[i].X, _spacerBounds[i].Y + _spacerBounds[i].Height / 2, _spacerBounds[i].X + (_spacerBounds[i].Width - GH_FontServer.StringWidth(_spacerTxts[i], sml)) / 2 - 4, _spacerBounds[i].Y + _spacerBounds[i].Height / 2);
          graphics.DrawLine(spacer, _spacerBounds[i].X + (_spacerBounds[i].Width - GH_FontServer.StringWidth(_spacerTxts[i], sml)) / 2 + GH_FontServer.StringWidth(_spacerTxts[i], sml) + 4, _spacerBounds[i].Y + _spacerBounds[i].Height / 2, _spacerBounds[i].X + _spacerBounds[i].Width, _spacerBounds[i].Y + _spacerBounds[i].Height / 2);
        }

        // Draw selected item
        // set font and colour depending on inital or selected text
        var font = new Font(GH_FontServer.FamilyStandard, 7);
        // adjust fontsize to high resolution displays
        font = new Font(font.FontFamily, font.Size / GH_GraphicsUtil.UiScale, FontStyle.Regular);
        Brush fontColour = Colour.AnnotationTextDark;
        if (_initialTxts != null) {
          if (_displayTexts[i] == _initialTxts[i]) {
            pen = new Pen(Colour.BorderColour);
            font = sml;
            fontColour = Brushes.Gray;
          }
        }

        // background
        Brush background = new SolidBrush(Colour.OasysLightGrey);
        // background
        graphics.FillRectangle(background, _borderBound[i]);
        // border
        graphics.DrawRectangle(pen, _borderBound[i].X, _borderBound[i].Y, _borderBound[i].Width, _borderBound[i].Height);
        // text
        graphics.DrawString(_displayTexts[i], font, fontColour, _textBound[i], GH_TextRenderingConstants.NearCenter);
        // draw dropdown arrow
        DropDownArrow.DrawDropDownButton(graphics, new PointF(_buttonBound[i].X + _buttonBound[i].Width / 2, _buttonBound[i].Y + _buttonBound[i].Height / 2), Colour.OasysDarkBlue, 15);

        // draw dropdown list
        font = new Font(GH_FontServer.FamilyStandard, 7);
        // adjust fontsize to high resolution displays
        font = new Font(font.FontFamily, font.Size / GH_GraphicsUtil.UiScale, FontStyle.Regular);
        fontColour = Colour.AnnotationTextDark;
        if (_unfolded[i]) {
          var penborder = new Pen(Brushes.Gray);
          Brush dropdownbackground = new SolidBrush(Colour.OasysLightGrey);
          penborder.Width = 0.3f;
          for (int j = 0; j < _dropdownBounds[i].Count; j++) {
            RectangleF listItem = _dropdownBounds[i][j];
            if (listItem.Y < _dropdownBound[i].Y) {
              if (listItem.Y + listItem.Height < _dropdownBound[i].Y) {
                _dropdownBounds[i][j] = new RectangleF();
                continue;
              } else {
                listItem.Height = listItem.Height - (_dropdownBound[i].Y - listItem.Y);
                listItem.Y = _dropdownBound[i].Y;
                _dropdownBounds[i][j] = listItem;
              }
            } else if (listItem.Y + listItem.Height > _dropdownBound[i].Y + _dropdownBound[i].Height) {
              if (listItem.Y > _dropdownBound[i].Y + _dropdownBound[i].Height) {
                _dropdownBounds[i][j] = new RectangleF();
                continue;
              } else {
                listItem.Height = _dropdownBound[i].Y + _dropdownBound[i].Height - listItem.Y;
                _dropdownBounds[i][j] = listItem;
              }
            }

            // background
            graphics.FillRectangle(dropdownbackground, _dropdownBounds[i][j]);
            // border
            graphics.DrawRectangle(penborder, _dropdownBounds[i][j].X, _dropdownBounds[i][j].Y, _dropdownBounds[i][j].Width, _dropdownBounds[i][j].Height);
            // text
            if (_dropdownBounds[i][j].Height > 2)
              graphics.DrawString(_dropdownlists[i][j], font, fontColour, _dropdownBounds[i][j], GH_TextRenderingConstants.NearCenter);
          }
          // border
          graphics.DrawRectangle(pen, _dropdownBound[i].X, _dropdownBound[i].Y, _dropdownBound[i].Width, _dropdownBound[i].Height);

          // draw vertical scroll bar
          Brush scrollbar = new SolidBrush(Color.FromArgb(_dragY ? 160 : 120, Color.Black));
          var scrollPen = new Pen(scrollbar);
          scrollPen.Width = _scrollBar.Width - 2;
          scrollPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
          scrollPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
          graphics.DrawLine(scrollPen, _scrollBar.X + 4, _scrollBar.Y + 4, _scrollBar.X + 4, _scrollBar.Y + _scrollBar.Height - 4);
        }
      }

      if (_drawSlider) {
        //Draw divider line
        int i = _spacerTxts.Count - 1;
        if (_spacerTxts[i] != "") {
          graphics.DrawString(_spacerTxts[i], sml, Colour.AnnotationTextDark, _spacerBounds[i], GH_TextRenderingConstants.CenterCenter);
          graphics.DrawLine(spacer, _spacerBounds[i].X, _spacerBounds[i].Y + _spacerBounds[i].Height / 2, _spacerBounds[i].X + (_spacerBounds[i].Width - GH_FontServer.StringWidth(_spacerTxts[i], sml)) / 2 - 4, _spacerBounds[i].Y + _spacerBounds[i].Height / 2);
          graphics.DrawLine(spacer, _spacerBounds[i].X + (_spacerBounds[i].Width - GH_FontServer.StringWidth(_spacerTxts[i], sml)) / 2 + GH_FontServer.StringWidth(_spacerTxts[i], sml) + 4, _spacerBounds[i].Y + _spacerBounds[i].Height / 2, _spacerBounds[i].X + _spacerBounds[i].Width, _spacerBounds[i].Y + _spacerBounds[i].Height / 2);
        }

        // draw drag line and intervals
        var line = new Pen(Colour.OasysDarkGrey);
        graphics.DrawLine(line, new PointF(_sliderBound.X + _grabBound.Width / 2, _sliderBound.Y + _sliderBound.Height / 2), new PointF(_sliderBound.X + _sliderBound.Width - _grabBound.Width / 2, _sliderBound.Y + _sliderBound.Height / 2));

        // draw grab item
        var penS = new Pen(Colour.OasysDarkBlue);
        penS.Width = 2f;
        var button = new RectangleF(_grabBound.X, _grabBound.Y, _grabBound.Width, _grabBound.Height);
        button.Inflate(-2, -2);
        Brush fill = new SolidBrush(Colour.OasysLightGrey);
        graphics.FillEllipse(fill, button);
        graphics.DrawEllipse(penS, button);

        // Draw display value text
        var font = new Font(GH_FontServer.FamilyStandard, 7);
        // adjust fontsize to high resolution displays
        font = new Font(font.FontFamily, font.Size / GH_GraphicsUtil.UiScale, FontStyle.Regular);
        string val = string.Format(new System.Globalization.NumberFormatInfo() { NumberDecimalDigits = _noDigits }, "{0:F}", new decimal(_currentValue));

        graphics.DrawString(val, font, Colour.AnnotationTextDark, _sliderValTextBound, ((_currentValue - _minValue) / (_maxValue - _minValue) < 0.5) ? GH_TextRenderingConstants.NearCenter : GH_TextRenderingConstants.FarCenter);
      }
    }
  }
}
