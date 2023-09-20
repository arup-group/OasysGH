using System.Collections.Generic;
using System.Drawing;
using Grasshopper.GUI.Gradient;
using Rhino.Display;

namespace OasysGH.UI {
  /// <summary>
  /// Colour class holding the main colours used in colour scheme.
  /// Make calls to this class to be able to easy update colours.
  /// </summary>
  public static class Colour {
    public static Brush ActiveBrush {
      get { return new SolidBrush(ActiveColour); }
    }

    public static Color ActiveColour {
      get { return OasysDarkBlue; }
    }

    public static Brush AnnotationTextBright {
      get { return Brushes.White; }
    }

    public static Brush AnnotationTextDark {
      get { return Brushes.Black; }
    }

    public static Brush AnnotationTextDarkGrey {
      get { return new SolidBrush(OasysDarkGrey); }
    }

    public static Color BorderColour {
      get { return OasysLightGrey; }
    }

    public static Color ButtonBorderColour {
      get { return OasysLightGrey; }
    }

    //Set colours for Component UI
    public static Brush ButtonColour {
      get { return new SolidBrush(OasysDarkBlue); }
    }

    public static Color ClickedBorderColour {
      get { return Color.White; }
    }

    public static Brush ClickedButtonColour {
      get { return new SolidBrush(WhiteOverlay(OasysDarkBlue, 0.32)); }
    }

    public static Color Dummy1D {
      get { return Color.FromArgb(255, 143, 143, 143); }
    }

    public static DisplayMaterial Dummy2D {
      get {
        var material = new DisplayMaterial {
          Diffuse = Color.FromArgb(1, 143, 143, 143),
          Emission = Color.White, //Color.FromArgb(1, 45, 45, 45),
          Transparency = 0.9
        };
        return material;
      }
    }

    public static Color Element1d {
      get { return Color.FromArgb(255, 95, 190, 180); }
    }

    public static Color Element1dNode {
      get { return OasysDarkGreen; }
    }

    public static Color Element1dNodeSelected {
      get { return OasysDarkGreen; }
    }

    public static Color Element1dSelected {
      get { return OasysDarkPurple; }
    }

    public static Color Element2dEdge {
      get { return OasysBlue; }
    }

    public static Color Element2dEdgeSelected {
      get { return OasysDarkPurple; }
    }

    public static DisplayMaterial Element2dFace {
      get {
        var material = new DisplayMaterial {
          Diffuse = Color.FromArgb(50, 150, 150, 150),
          Emission = Color.FromArgb(50, 190, 190, 190),
          Transparency = 0.1
        };
        return material;
      }
    }

    public static DisplayMaterial Element2dFaceSelected {
      get {
        var material = new DisplayMaterial {
          Diffuse = Color.FromArgb(5, 150, 150, 150),
          Emission = Color.FromArgb(5, 150, 150, 150),
          Transparency = 0.2
        };
        return material;
      }
    }

    public static DisplayMaterial Element3dFace {
      get {
        var material = new DisplayMaterial {
          Diffuse = Color.FromArgb(50, 150, 150, 150),
          Emission = Color.FromArgb(50, 190, 190, 190),
          Transparency = 0.3
        };
        return material;
      }
    }

    public static Color HoverBorderColour {
      get { return Color.White; }
    }

    public static Brush HoverButtonColour {
      get { return new SolidBrush(WhiteOverlay(OasysDarkBlue, 0.16)); }
    }

    public static Brush HoverInactiveButtonColour {
      get { return new SolidBrush(Color.FromArgb(255, 216, 216, 216)); }
    }

    public static Brush InactiveBorderColor {
      get { return new SolidBrush(Color.FromArgb(255, 216, 216, 216)); }
    }

    public static Brush InactiveButtonColour {
      get { return new SolidBrush(OasysLightGrey); }
    }

    public static Color Member1d {
      get { return OasysGreen; }
    }

    public static Color Member1dNode {
      get { return OasysDarkGreen; }
    }

    public static Color Member1dNodeSelected {
      get { return OasysGold; }
    }

    public static Color Member1dSelected {
      get { return OasysDarkPurple; }
    }

    public static Color Member2dEdge {
      get { return OasysBlue; }
    }

    public static Color Member2dEdgeSelected {
      get { return OasysDarkPurple; }
    }

    public static DisplayMaterial Member2dFace {
      get {
        var material = new DisplayMaterial {
          Diffuse = Color.FromArgb(50, 150, 150, 150),
          Emission = Color.FromArgb(50, 45, 45, 45),
          Transparency = 0.1
        };
        return material;
      }
    }

    public static DisplayMaterial Member2dFaceSelected {
      get {
        var material = new DisplayMaterial {
          Diffuse = Color.FromArgb(5, 150, 150, 150),
          Emission = Color.FromArgb(5, 5, 5, 5),
          Transparency = 0.2
        };
        return material;
      }
    }

    public static Color Member2dInclLn {
      get { return OasysGold; }
    }

    public static Color Member2dInclPt {
      get { return OasysGold; }
    }

    public static DisplayMaterial Member2dVoidCutterFace {
      get {
        var material = new DisplayMaterial {
          Diffuse = Color.FromArgb(50, 200, 0, 0),
          Emission = Color.FromArgb(50, 45, 45, 45),
          Transparency = 0.6
        };
        return material;
      }
    }

    //Set colours for custom geometry
    public static Color Node {
      get { return OasysGreen; }
    }

    public static Color NodeSelected {
      get { return OasysDarkPurple; }
    }

    public static Color OasysBlue {
      get { return Color.FromArgb(255, 99, 148, 237); }
    }

    public static Color OasysDarkBlue {
      get { return Color.FromArgb(255, 0, 92, 175); }
    }

    public static Color OasysDarkGreen {
      get { return Color.FromArgb(255, 27, 141, 133); }
    }

    public static Color OasysDarkGrey {
      get { return Color.FromArgb(255, 164, 164, 164); }
    }

    public static Color OasysDarkPurple {
      get { return Color.FromArgb(255, 136, 0, 136); }
    }

    public static Color OasysGold {
      get { return Color.FromArgb(255, 255, 183, 0); }
    }

    // General colour scheme
    public static Color OasysGreen {
      get { return Color.FromArgb(255, 48, 170, 159); }
    }

    public static Color OasysLightBlue {
      get { return Color.FromArgb(255, 130, 169, 241); }
    }

    public static Color OasysLightGrey {
      get { return Color.FromArgb(255, 244, 244, 244); }
    }

    public static Color Release {
      get { return Color.FromArgb(255, 153, 32, 32); }
    }

    public static Color SpacerColour {
      get { return OasysDarkBlue; }
    }

    public static Color Support {
      get { return Color.FromArgb(255, 0, 100, 0); }
    }

    public static DisplayMaterial SupportSymbol {
      get {
        var material = new DisplayMaterial() {
          Diffuse = Color.FromArgb(255, Support.R, Support.G, Support.B),
          Emission = Color.FromArgb(255, 50, 50, 50),
          Transparency = 0.2
        };
        return material;
      }
    }

    public static DisplayMaterial SupportSymbolSelected {
      get {
        var material = new DisplayMaterial() {
          Diffuse = Color.FromArgb(255, NodeSelected.R, NodeSelected.G, NodeSelected.B),
          Emission = Color.FromArgb(255, 50, 50, 50),
          Transparency = 0.2
        };
        return material;
      }
    }

    public static Color VoidCutter {
      get { return Color.FromArgb(255, 200, 0, 0); }
    }

    public static Color ElementType(int elementType) // GsaAPI.ElementType
    {
      switch (elementType) {
        case 1:
          return Color.FromArgb(255, 72, 99, 254);

        case 2:
          return Color.FromArgb(255, 95, 190, 180);

        case 23:
          return Color.FromArgb(255, 39, 52, 147);

        case 3:
          return Color.FromArgb(255, 73, 101, 101);

        case 21:
          return Color.FromArgb(255, 200, 81, 45);

        case 20:
          return Color.FromArgb(255, 192, 67, 255);

        case 9:
          return Color.FromArgb(255, 178, 178, 178);

        case 10:
          return Color.FromArgb(255, 32, 32, 32);
          ;
        case 24:
          return Color.FromArgb(255, 51, 82, 82);

        case 19:
          return Color.FromArgb(255, 155, 18, 214);

        default:
          return Color.FromArgb(255, 95, 190, 180);
      }
    }

    public static DisplayMaterial FaceCustom(Color colour) {
      var material = new DisplayMaterial() {
        Diffuse = Color.FromArgb(50, colour.R, colour.G, colour.B),
        Emission = Color.White, // Color.FromArgb(50, 190, 190, 190),
        Transparency = 0.1
      };
      return material;
    }

    public static Color Overlay(Color original, Color overlay, double ratio) {
      return Color.FromArgb(255,
          (int)(ratio * overlay.R + (1 - ratio) * original.R),
          (int)(ratio * overlay.G + (1 - ratio) * original.G),
          (int)(ratio * overlay.B + (1 - ratio) * original.B));
    }

    public static GH_Gradient Stress_Gradient(List<Color> colours = null) {
      var gH_Gradient = new GH_Gradient();

      if (colours.Count < 2 || colours == null) {
        gH_Gradient.AddGrip(-1, Color.FromArgb(0, 0, 206));
        gH_Gradient.AddGrip(-0.666, Color.FromArgb(0, 127, 229));
        gH_Gradient.AddGrip(-0.333, Color.FromArgb(90, 220, 186));
        gH_Gradient.AddGrip(0, Color.FromArgb(205, 254, 114));
        gH_Gradient.AddGrip(0.333, Color.FromArgb(255, 220, 71));
        gH_Gradient.AddGrip(0.666, Color.FromArgb(255, 127, 71));
        gH_Gradient.AddGrip(1, Color.FromArgb(205, 0, 71));
      } else {
        for (int i = 0; i < colours.Count; i++) {
          double t = 1.0 - 2.0 / ((double)colours.Count - 1.0) * (double)i;
          gH_Gradient.AddGrip(t, colours[i]);
        }
      }

      return gH_Gradient;
    }

    public static Color WhiteOverlay(Color original, double ratio) {
      Color white = Color.White;
      return Color.FromArgb(255,
          (int)(ratio * white.R + (1 - ratio) * original.R),
          (int)(ratio * white.G + (1 - ratio) * original.G),
          (int)(ratio * white.B + (1 - ratio) * original.B));
    }
  }
}
