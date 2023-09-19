using System.IO;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using IntegrationTests.Helpers;
using Xunit;

namespace IntegrationTests {
  [Collection("GrasshopperFixture collection")]
  public class CreateOasysProfileTest {
    public static GH_Document Document() {
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
    [InlineData("Perimeter", "GEO P(m) M(0.0165719507531976|0.026978684623114) L(0.0234750912054643|0.0314657259170873) L(0.0194482592749754|0.0407849655276474) L(-0.0144921812820025|0.0422806459589718) L(-0.0324403464578959|0.0236421667378517) L(-0.0311747707083137|-0.0227239266332063) L(-0.000110638673113561|-0.0417075628769397) L(0.0301481269759888|-0.0279012819724063) L(0.0258911903637577|-0.00374029038947287) L(0.0411931516996155|-0.00351018570773064) L(0.0411931516996155|0.0037381117671494) L(-0.00287189485402024|0.00304779772192272) L(-0.00103105740008245|-0.00466070911644176) L(0.0171472124575532|-0.00351018570773064) L(0.0216342537515266|-0.0249099211097574) L(0.000464623031242004|-0.0345743177429308) L(-0.0225458451429803|-0.0188121470435885) L(-0.0247318396195315|0.0214561722613006) L(-0.00873956423844693|0.0345721391206073) L(0.00989891498267316|0.0337667727345096)")]
    [InlineData("Rectangle Hollow", "STD RHS(cm) 500 100 10 20")]
    [InlineData("Rectangle", "STD R(cm) 500 100")]
    [InlineData("Recto Ellipse", "STD RE(cm) 500 100 10 20")]
    [InlineData("Recto Circle", "STD RC(cm) 100 500")]
    [InlineData("Secant Pile", "STD SPW(cm) 500 100 10")]
    [InlineData("Sheet Pile", "STD SHT(cm) 500 100 10 20 30 30")]
    [InlineData("Trapezoid", "STD TR(cm) 500 100 10")]
    [InlineData("T Section", "STD T(cm) 500 100 10 20")]
    public void AssertOutput(string groupIdentifier, string expectedOutput) {
      IGH_Param param = DocumentHelper.FindParameter(Document(), groupIdentifier);
      var output = (GH_String)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(expectedOutput, output.Value);
    }
  }
}
