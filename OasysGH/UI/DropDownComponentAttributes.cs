using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using OasysGH.UI.Helpers;

namespace OasysGH.UI {
  /// <summary>
  /// Class to create custom component UI with multiple dropdowns
  ///
  /// Note that it is the component's responsibility to dynamically update lists, this class is only displaying what it gets.
  ///
  /// To use this class override CreateAttributes() in component class and set m_attributes to an instance of this class.
  /// </summary>
  public class DropDownComponentAttributes : GH_ComponentAttributes {
    private int MinWidth {
      get {
        int sp = WidthAttributes.MaxTextWidth(_spacerTxts, GH_FontServer.Standard);
        int bt = 0;
        for (int i = 0; i < _dropdownlists.Count; i++) {
          int tbt = WidthAttributes.MaxTextWidth(_dropdownlists[i], new Font(GH_FontServer.FamilyStandard, 7));
          if (tbt > bt)
            bt = tbt;
        }

        bt += 12; // another magic number

        int num = Math.Max(Math.Max(sp, bt), 90);
        return Math.Min(num, 170);
      }
    }

    private readonly Action<int, int> _action;
    private readonly List<string> _displayTexts;
    private readonly List<List<string>> _dropdownlists;

    // the selected item text
    private readonly List<string> _initialTxts;

    private readonly List<string> _spacerTxts;
    private List<RectangleF> _borderBound;
    private List<RectangleF> _buttonBound;
    private float _deltaY;
    private bool _drag;
    private float _dragMouseStartY;
    private List<RectangleF> _dropdownBound;
    private List<List<RectangleF>> _dropdownBounds;

    // location of mouse at drag start
    // moved Y-location of scroll element
    private int _maxNoRows = 10;

    private bool _mouseOver;
    private RectangleF _scrollBar;

    // surrounding bound for vertical scroll element
    private float _scrollStartY;

    // list of descriptive texts above each dropdown
    private List<RectangleF> _spacerBounds;

    // area where the selected item is displayed
    private List<RectangleF> _textBound;

    private List<bool> _unfolded;

    public DropDownComponentAttributes(GH_Component owner, Action<int, int> clickHandle, List<List<string>> dropdownContents, List<string> selections, List<string> spacerTexts = null, List<string> initialdescriptions = null) : base(owner) {
      _dropdownlists = dropdownContents;
      _spacerTxts = spacerTexts;
      _action = clickHandle;
      _initialTxts = initialdescriptions ?? null; // if no description is inputted then null initialTxt
      if (dropdownContents != null) {
        if (selections == null) {
          var tempDisplaytxt = new List<string>();
          for (int i = 0; i < _dropdownlists.Count; i++)
            tempDisplaytxt.Add((initialdescriptions == null) ? _dropdownlists[i][0] : initialdescriptions[i]);
          _displayTexts = tempDisplaytxt;
        } else
          _displayTexts = selections;
      }
    }

    // lefternmost part of the selected/displayed item
    // right side bit where we place the button to unfold the dropdown list

    // initial text to be able to display a hint

    // content lists of items for dropdown

    // list of bounds for each item in dropdown list
    // surrounding bound for the entire dropdown list

    //function sending back the selection to component (i = dropdowncontentlist, j = selected item in that list)

    // list of bools for unfolded or closed dropdown

    public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e) {
      for (int i = 0; i < _dropdownlists.Count; i++) {
        if (_unfolded[i]) {
          if (e.Button == MouseButtons.Left) {
            RectangleF rec = _scrollBar;
            var comp = Owner as GH_Component;
            if (rec.Contains(e.CanvasLocation)) {
              _dragMouseStartY = e.CanvasLocation.Y;
              _drag = true;
              comp.ExpireSolution(true);
              return GH_ObjectResponse.Capture;
            }
          }
        }
      }

      return base.RespondToMouseDown(sender, e);
    }

    public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e) {
      if (_drag) {
        var comp = Owner as GH_Component;

        _deltaY = e.CanvasLocation.Y - _dragMouseStartY;

        comp.ExpireSolution(true);
        return GH_ObjectResponse.Ignore;
      }

      for (int i = 0; i < _buttonBound.Count; i++) {
        if (_buttonBound[i].Contains(e.CanvasLocation)) {
          _mouseOver = true;
          sender.Cursor = Cursors.Hand;
          return GH_ObjectResponse.Capture;
        }
      }

      if (_mouseOver) {
        _mouseOver = false;
        Grasshopper.Instances.CursorServer.ResetCursor(sender);
        return GH_ObjectResponse.Release;
      }

      return base.RespondToMouseMove(sender, e);
    }

    public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e) {

      if (e.Button == MouseButtons.Left) {
        var comp = Owner as GH_Component;
        if (_drag) {
          // if drag was true then we release it here:
          _scrollStartY += _deltaY;
          _deltaY = 0;
          _drag = false;
          comp.ExpireSolution(true);
          return GH_ObjectResponse.Release;
        }

        for (int i = 0; i < _dropdownlists.Count; i++) {
          RectangleF rec = _borderBound[i];
          if (rec.Contains(e.CanvasLocation)) {
            bool selected = comp.Attributes.Selected;
            comp.Attributes.Selected = true;
            comp.OnPingDocument().BringSelectionToTop();
            comp.Attributes.Selected = selected;

            _unfolded[i] = !_unfolded[i];
            // close any other dropdowns that may be unfolded
            for (int j = 0; j < _unfolded.Count; j++) {
              if (j == i)
                continue;
              _unfolded[j] = false;
            }

            comp.ExpireSolution(true);
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

                    // recalculate component
                    comp.ExpireSolution(true);
                  } else {
                    _unfolded[i] = !_unfolded[i];
                    comp.ExpireSolution(true);
                  }

                  return GH_ObjectResponse.Handled;
                }
              }
            } else {
              _unfolded[i] = !_unfolded[i];
              comp.ExpireSolution(true);
              return GH_ObjectResponse.Handled;
            }
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

    // location of scroll element at drag start
    protected override void Layout() {
      base.Layout();

      // first change the width to suit; using max to determine component visualisation style
      FixLayout();

      if (_spacerBounds == null)
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

      int s = 2; // spacing to edges and internal between boxes

      int h0 = 0;

      bool removeScroll = true;
      for (int i = 0; i < _dropdownlists.Count; i++) {
        //spacer and title
        if (_spacerTxts[i] != "") {
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

        // update component bounds
        Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height + h0 + h1 + 4 * s);

        // create list of bounds for dropdown if dropdown is unfolded
        if (_unfolded.Count == i)
          _unfolded.Add(new bool()); // ensure we have a bool for every list

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

          // update component size if dropdown is unfolded to be able to capture mouseclicks
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
        // draw divider line
        if (_spacerTxts[i] != "") {
          graphics.DrawString(_spacerTxts[i], sml, Colour.AnnotationTextDark, _spacerBounds[i], GH_TextRenderingConstants.CenterCenter);
          graphics.DrawLine(spacer, _spacerBounds[i].X, _spacerBounds[i].Y + _spacerBounds[i].Height / 2, _spacerBounds[i].X + (_spacerBounds[i].Width - GH_FontServer.StringWidth(_spacerTxts[i], sml)) / 2 - 4, _spacerBounds[i].Y + _spacerBounds[i].Height / 2);
          graphics.DrawLine(spacer, _spacerBounds[i].X + (_spacerBounds[i].Width - GH_FontServer.StringWidth(_spacerTxts[i], sml)) / 2 + GH_FontServer.StringWidth(_spacerTxts[i], sml) + 4, _spacerBounds[i].Y + _spacerBounds[i].Height / 2, _spacerBounds[i].X + _spacerBounds[i].Width, _spacerBounds[i].Y + _spacerBounds[i].Height / 2);
        }

        // draw selected item
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
          Brush scrollbar = new SolidBrush(Color.FromArgb(_drag ? 160 : 120, Color.Black));
          var scrollPen = new Pen(scrollbar);
          scrollPen.Width = _scrollBar.Width - 2;
          scrollPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
          scrollPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
          graphics.DrawLine(scrollPen, _scrollBar.X + 4, _scrollBar.Y + 4, _scrollBar.X + 4, _scrollBar.Y + _scrollBar.Height - 4);
        }
      }
    }
  }
}
