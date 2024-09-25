using System.IO;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using IntegrationTests.Helpers;
using Xunit;

namespace IntegrationTests {
  [Collection("GrasshopperFixture collection")]
  public class CreateOasysProfileTest {
    private static GH_Document Document => document ?? (document = OpenDocument());
    private static GH_Document document = null;
    private static GH_Document OpenDocument() {
      string fileName = "CreateOasysProfile.gh";

      string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent
       .Parent.FullName;
      string path = Path.Combine(solutiondir, "ExampleFiles");

      return DocumentHelper.CreateDocument(Path.Combine(path, fileName));
    }

    [Theory]
    [InlineData("Angle", "STD A(cm) 500 100 10 20 [R(0)]")]
    [InlineData("Channel", "STD CH(cm) 50 10 1 2")]
    [InlineData("Circle Hollow", "STD CHS(cm) 5 0.1")]
    [InlineData("Circle", "STD C(cm) 10")]
    [InlineData("Cruciform Symmetrical", "STD X(cm) 500 100 10 20")]
    [InlineData("Ellipse Hollow", "STD OVAL(cm) 500 100 10")]
    [InlineData("Ellipse", "STD E(cm) 500 100 2")]
    [InlineData("General C", "STD GC(cm) 500 100 10 20")]
    [InlineData("General Z", "STD GZ(cm) 500 100 10 20 30 30")]
    [InlineData("I Beam Asysmmetrical", "STD GI(cm) 500 100 10 20 30 30")]
    [InlineData("I Beam Cellular", "STD CB(cm) 500 100 10 20 300 3000")]
    [InlineData("I Beam Symmetrical", "STD I(cm) 500 100 10 20")]
    [InlineData("Perimeter", "GEO P(cm)  M(1.66 cm|1.66 cm) L(2.35 cm|2.35 cm) L(1.94 cm|1.94 cm) L(-1.45 cm|-1.45 cm) L(-3.24 cm|-3.24 cm) L(-3.12 cm|-3.12 cm) L(-0.011 cm|-0.011 cm) L(3.01 cm|3.01 cm) L(2.59 cm|2.59 cm) L(4.12 cm|4.12 cm) L(4.12 cm|4.12 cm) L(-0.29 cm|-0.29 cm) L(-0.1 cm|-0.1 cm) L(1.71 cm|1.71 cm) L(2.16 cm|2.16 cm) L(0.046 cm|0.046 cm) L(-2.25 cm|-2.25 cm) L(-2.47 cm|-2.47 cm) L(-0.87 cm|-0.87 cm) L(0.99 cm|0.99 cm)")]
    [InlineData("Rectangle Hollow", "STD RHS(cm) 500 100 10 20")]
    [InlineData("Rectangle", "STD R(cm) 500 100")]
    [InlineData("Recto Ellipse", "STD RE(cm) 500 100 10 20")]
    [InlineData("Recto Circle", "STD RC(cm) 100 500")]
    [InlineData("Secant Pile", "STD SPW(cm) 500 100 10")]
    [InlineData("Sheet Pile", "STD SHT(cm) 500 100 10 20 30 30")]
    [InlineData("Trapezoid", "STD TR(cm) 500 100 10")]
    [InlineData("T Section", "STD T(cm) 500 100 10 20")]
    [InlineData("IPE100", "CAT BSI-IPE IPE100")]
    [InlineData("HE200AA", "CAT HE HE200.AA")]
    public void AssertOutput(string groupIdentifier, string expectedOutput) {
      IGH_Param param = DocumentHelper.FindParameter(Document, groupIdentifier);
      var output = (GH_String)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(expectedOutput, output.Value);
    }

    [Theory]
    [InlineData("Count", 13998)]
    [InlineData("CountSuperseeded", 18572)]
    public void AssertCount(string groupIdentifier, int expectedOutput) {
      IGH_Param param = DocumentHelper.FindParameter(Document, groupIdentifier);
      var output = (GH_Integer)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(expectedOutput, output.Value);
    }
  }
}
