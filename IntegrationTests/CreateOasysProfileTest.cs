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
    [InlineData("Perimeter", "GEO P(m) M(0|0) L(-0.00402683193048891|0.00931923961056004) L(-0.0379672724874669|0.0108149200418845) L(-0.0559154376633603|-0.0078235591792356) L(-0.0546498619137781|-0.0541896525502936) L(-0.0235857298785779|-0.0731732887940271) L(0.00667303577052449|-0.0593670078894937) L(0.00241609915829334|-0.0352060163065602) L(0.0177180604941512|-0.034975911624818) L(0.0177180604941512|-0.0277276141499379) L(-0.0263469860594846|-0.0284179281951646) L(-0.0245061486055468|-0.0361264350335291) L(-0.00632787874791115|-0.034975911624818) L(-0.00184083745393779|-0.0563756470268448) L(-0.0230104681742223|-0.0660400436600181) L(-0.0460209363484447|-0.0502778729606758) L(-0.0482069308249958|-0.0100095536557867) L(-0.0322146554439113|0.00310641320352001) L(-0.0135761762227912|0.00230104681742223) L(-0.0069031404522667|-0.00448704129397337)")]
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
