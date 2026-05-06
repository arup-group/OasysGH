using Oasys.Taxonomy.Profiles;
using OasysGH.Helpers;
using OasysUnits;
using OasysUnits.Units;
using Xunit;
using Xunit.Abstractions;

namespace OasysGHTests.Helpers {
  /// <summary>
  /// Diagnostic tests to capture the exact ToString() output for every IProfile type.
  /// These outputs drive the implementation of ProfileHelper.ProfileFromString.
  /// </summary>
  public class ProfileToStringTests {
    private readonly ITestOutputHelper _output;

    public ProfileToStringTests(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void AllProfileToStringOutputs() {
      var m = LengthUnit.Meter;
      var d = new Length(0.5, m);
      var w = new Length(0.1, m);
      var tw = new Length(0.01, m);
      var tf = new Length(0.02, m);
      var d2 = new Length(0.3, m);
      var w2 = new Length(0.06, m);

      _output.WriteLine($"Angle:              {new AngleProfile(d, new Flange(tf, w), new WebConstant(tw))}");
      _output.WriteLine($"Channel:            {new ChannelProfile(d, new Flange(tf, w), new WebConstant(tw))}");
      _output.WriteLine($"CircleHollow:       {new CircleHollowProfile(d, tw)}");
      _output.WriteLine($"Circle:             {new CircleProfile(d)}");
      _output.WriteLine($"CruciformSymm:      {new CruciformSymmetricalProfile(d, new Flange(tf, w), new WebConstant(tw))}");
      _output.WriteLine($"EllipseHollow:      {new EllipseHollowProfile(d, w, tw)}");
      _output.WriteLine($"Ellipse:            {new EllipseProfile(d, w)}");
      _output.WriteLine($"GeneralC:           {new GeneralCProfile(d, w, tw, tf)}");
      _output.WriteLine($"GeneralZ:           {new GeneralZProfile(d, w, w2, tw, tf, tw)}");
      _output.WriteLine($"IBeamAsymm:         {new IBeamAsymmetricalProfile(d, new Flange(tf, w), new Flange(tw, w2), new WebConstant(tw))}");
      _output.WriteLine($"IBeamCellular:      {new IBeamCellularProfile(d, new Flange(tf, w), new WebConstant(tw), IBeamOpeningType.Cellular, d2, w)}");
      _output.WriteLine($"IBeam:              {new IBeamProfile(d, new Flange(tf, w), new WebConstant(tw))}");
      _output.WriteLine($"RectangleHollow:    {new RectangleHollowProfile(d, new Flange(tf, w), new WebConstant(tw))}");
      _output.WriteLine($"Rectangle:          {new RectangleProfile(d, w)}");
      _output.WriteLine($"RectoEllipse:       {new RectoEllipseProfile(d, d2, w, w2)}");
      _output.WriteLine($"RectoCircle:        {new RectoCircleProfile(d, w)}");
      _output.WriteLine($"SecantPileWall:     {new SecantPileProfile(d, w, 3, true)}");
      _output.WriteLine($"SecantPileSection:  {new SecantPileProfile(d, w, 3, false)}");
      _output.WriteLine($"SheetPile:          {new SheetPileProfile(d, w, tw, tf, d2, w2)}");
      _output.WriteLine($"Trapezoid:          {new TrapezoidProfile(d, w, w2)}");
      _output.WriteLine($"TSection:           {new TSectionProfile(d, new Flange(tf, w), new WebConstant(tw))}");

      Assert.True(true);
    }
  }

  /// <summary>
  /// Round-trip tests: create a profile → ToString() → ProfileFromString → verify ToString() matches.
  /// </summary>
  public class ProfileFromStringTests {
    [Theory]
    [InlineData("STD A(m) 0.5 0.1 0.01 0.02 [R(0)]")]
    [InlineData("STD CH(m) 0.5 0.1 0.01 0.02")]
    [InlineData("STD CHS(m) 0.5 0.01")]
    [InlineData("STD C(m) 0.5")]
    [InlineData("STD X(m) 0.5 0.1 0.01 0.02")]
    [InlineData("STD OVAL(m) 0.5 0.1 0.01")]
    [InlineData("STD E(m) 0.5 0.1 2")]
    [InlineData("STD GC(m) 0.5 0.1 0.01 0.02")]
    [InlineData("STD GZ(m) 0.5 0.1 0.06 0.01 0.02 0.01")]
    [InlineData("STD GI(m) 0.5 0.1 0.06 0.01 0.02 0.01")]
    [InlineData("STD CB(m) 0.5 0.1 0.01 0.02 0.3 0.1")]
    [InlineData("STD I(m) 0.5 0.1 0.01 0.02")]
    [InlineData("STD RHS(m) 0.5 0.1 0.01 0.02")]
    [InlineData("STD R(m) 0.5 0.1")]
    [InlineData("STD RE(m) 0.5 0.3 0.1 0.06")]
    [InlineData("STD RC(m) 0.1 0.5")]
    [InlineData("STD SP(m) 0.5 0.1 3")]
    [InlineData("STD SPW(m) 0.5 0.1 3")]
    [InlineData("STD SHT(m) 0.5 0.1 0.01 0.02 0.3 0.06")]
    [InlineData("STD TR(m) 0.5 0.1 0.06")]
    [InlineData("STD T(m) 0.5 0.1 0.01 0.02")]
    public void RoundTripProfileString(string profileString) {
      IProfile profile = ProfileHelper.ProfileFromString(profileString);
      Assert.NotNull(profile);
      Assert.Equal(profileString, profile.ToString());
    }

    [Theory]
    [InlineData("STD R(m) 0.3 0.3")]
    [InlineData("STD R(m) 0.2 0.2")]
    [InlineData("STD C(m) 0.2")]
    public void UserExampleStrings(string profileString) {
      IProfile profile = ProfileHelper.ProfileFromString(profileString);
      Assert.NotNull(profile);
      Assert.Equal(profileString, profile.ToString());
    }

    [Theory]
    [InlineData("GSA Section (PB1 STD R(m) 0.3 0.3 Concrete)", "STD R(m) 0.3 0.3")]
    [InlineData("GSA Section (PB4 STD C(m) 0.2 Concrete)", "STD C(m) 0.2")]
    public void EmbeddedProfileStringIsExtracted(string fullDescription, string expectedProfileString) {
      IProfile profile = ProfileHelper.ProfileFromString(fullDescription);
      Assert.NotNull(profile);
      Assert.Equal(expectedProfileString, profile.ToString());
    }

    [Theory]
    [InlineData("CAT HE HE200.B")]
    [InlineData("CAT BSI-IPE IPE100")]
    public void CatalogueProfileRoundTrip(string profileString) {
      IProfile profile = ProfileHelper.ProfileFromString(profileString);
      Assert.NotNull(profile);
    }
  }
}
