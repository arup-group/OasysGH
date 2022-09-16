using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System.IO;
using System.Reflection;
using Xunit;
using GH_UnitNumberTests.Helpers;
using OasysUnitsNet;
using OasysUnitsNet.Units;
using static GH_IO.VersionNumber;

namespace GH_UnitNumberTests
{
  [Collection("GrasshopperFixture collection")]
  public class Example01Test
  {
    public static GH_Document Document()
    {
      string fileName = MethodBase.GetCurrentMethod().DeclaringType.ToString().Replace(".", "_") + ".gh";
      fileName = fileName.Replace("Tests", string.Empty).Replace("Test", string.Empty);

      string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;
      string path = Path.Combine(solutiondir, "ExampleFiles");
      GH_DocumentIO io = new GH_DocumentIO();
      Assert.True(File.Exists(Path.Combine(path, fileName)));
      Assert.True(io.Open(Path.Combine(path, fileName)));
      io.Document.NewSolution(true);
      return io.Document;
    }

    [Fact]
    public void Check1()
    {
      GH_Document doc = Document();
      GH_Param<OasysGH.Units.GH_UnitNumber> param = Helper.FindComponentInDocumentByGroup<OasysGH.Units.GH_UnitNumber>(doc, "Check1");
      Assert.NotNull(param);
      param.CollectData();
      OasysGH.Units.GH_UnitNumber output = (OasysGH.Units.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Length(15, LengthUnit.Millimeter), output.Value);
    }

    [Fact]
    public void Check2()
    {
      GH_Document doc = Document();
      GH_Param<GH_Number> param = Helper.FindComponentInDocumentByGroup<GH_Number>(doc, "Check2");
      Assert.NotNull(param);
      param.CollectData();
      GH_Number output = (GH_Number)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(1.5, output.Value);
    }

    [Fact]
    public void NoRuntimeErrorsTest()
    {
      Helper.TestNoRuntimeMessagesInDocument(Document(), GH_RuntimeMessageLevel.Error);
    }
  }
}
