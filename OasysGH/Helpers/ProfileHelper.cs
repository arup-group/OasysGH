using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Oasys.Taxonomy.Profiles;
using OasysUnits;
using OasysUnits.Units;

namespace OasysGH.Helpers {
  /// <summary>
  /// Helper methods for working with IProfile strings.
  /// </summary>
  public static class ProfileHelper {
    /// <summary>
    /// Creates an IProfile from a profile description string.
    /// Accepts the standard IProfile.ToString() format ("STD R(m) 0.3 0.3"),
    /// catalogue format ("CAT HE HE200.B"), and strings that contain an
    /// embedded profile description (e.g. "GSA Section (PB1 STD R(m) 0.3 0.3 Concrete)").
    /// Perimeter (GEO) profiles are not supported.
    /// </summary>
    /// <param name="description">Profile description string.</param>
    /// <returns>An IProfile corresponding to the description.</returns>
    /// <exception cref="ArgumentException">Thrown when description is null or empty.</exception>
    /// <exception cref="FormatException">Thrown when the string cannot be parsed.</exception>
    /// <exception cref="NotSupportedException">Thrown for GEO (perimeter) profiles.</exception>
    public static IProfile ProfileFromString(string description) {
      if (string.IsNullOrWhiteSpace(description))
        throw new ArgumentException("Profile description cannot be null or empty.", nameof(description));

      string profileStr = ExtractProfileString(description);

      if (profileStr.StartsWith("CAT ", StringComparison.OrdinalIgnoreCase))
        return ParseCatalogueProfile(profileStr);

      if (profileStr.StartsWith("GEO ", StringComparison.OrdinalIgnoreCase))
        throw new NotSupportedException("Perimeter (GEO) profiles cannot be reconstructed from a string.");

      if (profileStr.StartsWith("STD ", StringComparison.OrdinalIgnoreCase))
        return ParseStdProfile(profileStr);

      throw new FormatException($"Unrecognised profile description: \"{description}\"");
    }

    private static string ExtractProfileString(string input) {
      foreach (string prefix in new[] { "STD ", "CAT ", "GEO " }) {
        int idx = input.IndexOf(prefix, StringComparison.OrdinalIgnoreCase);
        if (idx >= 0)
          return input.Substring(idx).TrimEnd(' ', ')');
      }
      return input.Trim();
    }

    private static LengthUnit ParseUnit(string abbrev) {
      switch (abbrev.ToLowerInvariant()) {
        case "m": return LengthUnit.Meter;
        case "cm": return LengthUnit.Centimeter;
        case "mm": return LengthUnit.Millimeter;
        case "in": return LengthUnit.Inch;
        case "ft": return LengthUnit.Foot;
        default: throw new FormatException($"Unknown length unit abbreviation: '{abbrev}'");
      }
    }

    private static IProfile ParseStdProfile(string profileStr) {
      // Format: STD {type}({unit}) {val1} {val2} ...
      var match = Regex.Match(profileStr,
        @"^STD\s+(\w+)\((\w+)\)\s+(.+)$",
        RegexOptions.IgnoreCase);

      if (!match.Success)
        throw new FormatException($"Could not parse STD profile string: \"{profileStr}\"");

      string typeAbbrev = match.Groups[1].Value.ToUpperInvariant();
      LengthUnit unit = ParseUnit(match.Groups[2].Value);
      string[] parts = match.Groups[3].Value
        .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

      Length L(int i) => new Length(
        double.Parse(parts[i], CultureInfo.InvariantCulture), unit);

      // String token order determined from IProfile.ToString() outputs:
      //
      // A    (Angle):              depth  width  webThk  flangeThk  [R(rootRadius)]
      // CH   (Channel):            depth  width  webThk  flangeThk
      // CHS  (CircleHollow):       diameter  thickness
      // C    (Circle):             diameter
      // X    (CruciformSymm):      depth  width  webThk  flangeThk
      // OVAL (EllipseHollow):      depth  width  thickness
      // E    (Ellipse):            depth  width  n       ← n is always 2, ignored
      // GC   (GeneralC):           depth  width  lip  thickness
      // GZ   (GeneralZ):           depth  topWidth  botWidth  topLip  botLip  thickness
      // GI   (IBeamAsymm):         depth  topFlangeWidth  botFlangeWidth  webThk  topFlangeThk  botFlangeThk
      // CB   (IBeamCellular):      depth  flangeWidth  webThk  flangeThk  openingDiam  pitch
      // I    (IBeam):              depth  flangeWidth  webThk  flangeThk
      // RHS  (RectangleHollow):    depth  width  webThk  flangeThk
      // R    (Rectangle):          depth  width
      // RE   (RectoEllipse):       depth  depthFlat  width  widthFlat
      // RC   (RectoCircle):        width  depth      ← NOTE: reversed vs constructor order
      // SP   (SecantPile, isWall=true):   diameter  pileCentres  pileCount
      // SPW  (SecantPile, isWall=false):  diameter  pileCentres  pileCount
      // SHT  (SheetPile):          p0  p1  p2  p3  p4  p5
      // TR   (Trapezoid):          depth  topWidth  botWidth
      // T    (TSection):           depth  flangeWidth  webThk  flangeThk

      switch (typeAbbrev) {
        case "A":
          return new AngleProfile(L(0), new Flange(L(3), L(1)), new WebConstant(L(2)));

        case "CH":
          return new ChannelProfile(L(0), new Flange(L(3), L(1)), new WebConstant(L(2)));

        case "CHS":
          return new CircleHollowProfile(L(0), L(1));

        case "C":
          return new CircleProfile(L(0));

        case "X":
          return new CruciformSymmetricalProfile(L(0), new Flange(L(3), L(1)), new WebConstant(L(2)));

        case "OVAL":
          return new EllipseHollowProfile(L(0), L(1), L(2));

        case "E":
          return new EllipseProfile(L(0), L(1));

        case "GC":
          return new GeneralCProfile(L(0), L(1), L(2), L(3));

        case "GZ":
          return new GeneralZProfile(L(0), L(1), L(2), L(3), L(4), L(5));

        case "GI":
          return new IBeamAsymmetricalProfile(
            L(0),
            new Flange(L(4), L(1)),
            new Flange(L(5), L(2)),
            new WebConstant(L(3)));

        case "CB":
          return new IBeamCellularProfile(
            L(0),
            new Flange(L(3), L(1)),
            new WebConstant(L(2)),
            IBeamOpeningType.Cellular,
            L(4),
            L(5));

        case "I":
          return new IBeamProfile(L(0), new Flange(L(3), L(1)), new WebConstant(L(2)));

        case "RHS":
          return new RectangleHollowProfile(L(0), new Flange(L(3), L(1)), new WebConstant(L(2)));

        case "R":
          return new RectangleProfile(L(0), L(1));

        case "RE":
          return new RectoEllipseProfile(L(0), L(1), L(2), L(3));

        case "RC":
          // ToString() outputs "width depth" (reversed vs constructor); constructor is (depth, width)
          return new RectoCircleProfile(L(1), L(0));

        case "SP":
          return new SecantPileProfile(L(0), L(1), int.Parse(parts[2]), true);

        case "SPW":
          return new SecantPileProfile(L(0), L(1), int.Parse(parts[2]), false);

        case "SHT":
          return new SheetPileProfile(L(0), L(1), L(2), L(3), L(4), L(5));

        case "TR":
          return new TrapezoidProfile(L(0), L(1), L(2));

        case "T":
          return new TSectionProfile(L(0), new Flange(L(3), L(1)), new WebConstant(L(2)));

        default:
          throw new FormatException(
            $"Unknown STD profile type abbreviation '{typeAbbrev}' in: \"{profileStr}\"");
      }
    }

    private static IProfile ParseCatalogueProfile(string profileStr) {
      // CatalogueProfile expects the full description e.g. "CAT HE HE200.B"
      return new CatalogueProfile(profileStr.Trim());
    }
  }
}
