using System.IO;
using System.Reflection;
using GH_UnitNumberTests.Helpers;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using OasysUnits;
using OasysUnits.Units;
using Xunit;

namespace GH_UnitNumberTests {

  [Collection("GrasshopperFixture collection")]
  public class Example01Test {
    public static GH_Document Document() {
      string fileName = MethodBase.GetCurrentMethod().DeclaringType.ToString().Replace(".", "_") + ".gh";
      fileName = fileName.Replace("Tests", string.Empty).Replace("Test", string.Empty);

      string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;
      string path = Path.Combine(solutiondir, "ExampleFiles");
      var io = new GH_DocumentIO();
      Assert.True(File.Exists(Path.Combine(path, fileName)));
      Assert.True(io.Open(Path.Combine(path, fileName)));
      io.Document.NewSolution(true);
      return io.Document;
    }

    [Fact]
    public void Check1() {
      GH_Document doc = Document();
      IGH_Param param = Helper.FindParameter(doc, "Check1");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Length(15, LengthUnit.Millimeter), output.Value);
    }

    [Fact]
    public void Check2() {
      GH_Document doc = Document();
      IGH_Param param = Helper.FindParameter(doc, "Check2");
      Assert.NotNull(param);
      param.CollectData();
      var output = (GH_Number)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(1.5, output.Value);
    }

    [Fact]
    public void Check3() {
      GH_Document doc = Document();
      IGH_Param param = Helper.FindParameter(doc, "Check3");
      Assert.NotNull(param);
      param.CollectData();
      var output = (GH_Number)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(0.015, output.Value);
    }

    [Fact]
    public void Check4() {
      GH_Document doc = Document();
      IGH_Param param = Helper.FindParameter(doc, "Check4");
      Assert.NotNull(param);
      param.CollectData();
      var output0 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      var output1 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[1];
      var output2 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[2];
      var output3 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[3];

      Assert.Equal(new Length(2.3, LengthUnit.Meter), output0.Value);
      Assert.Equal(new Length(4.5, LengthUnit.Centimeter), output1.Value);
      Assert.Equal(new Force(1.2, ForceUnit.Kilonewton), output2.Value);
      Assert.Equal(new Force(3.4, ForceUnit.Meganewton), output3.Value);
    }

    [Fact]
    public void NoRuntimeErrorsTest() {
      Helper.TestNoRuntimeMessagesInDocument(Document(), GH_RuntimeMessageLevel.Error);
    }
  }
}
