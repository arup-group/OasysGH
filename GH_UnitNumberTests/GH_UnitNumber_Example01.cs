using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System.IO;
using System.Reflection;
using Xunit;
using UnitsNet;
using GH_UnitNumberTests.Helpers;

namespace GH_UnitNumberTests
{
  [Collection("GrasshopperFixture collection")]
  public class Example01Test
  {
    public static GH_Document Document()
    {
      string fileName = "GH_UnitNumber_" + MethodBase.GetCurrentMethod().DeclaringType + ".gh";
      fileName = fileName.Replace("IntegrationTests.", string.Empty).Replace("Test", string.Empty);

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
      GH_Component comp = Helper.FindComponentInDocumentByGroup(doc, "Check1");
      Assert.NotNull(comp);
      OasysGH.Units.GH_UnitNumber output = (OasysGH.Units.GH_UnitNumber)ComponentTestHelper.GetOutput(comp);
      Assert.Equal(new Length(15, UnitsNet.Units.LengthUnit.Millimeter), output.Value);
    }

    [Fact]
    public void Check2()
    {
      GH_Document doc = Document();
      GH_Component comp = Helper.FindComponentInDocumentByGroup(doc, "Check2");
      Assert.NotNull(comp);
      GH_Number output = (GH_Number)ComponentTestHelper.GetOutput(comp);
      Assert.Equal(0.15, output.Value);
    }

    [Fact]
    public void NoRuntimeErrorsTest()
    {
      Helper.TestNoRuntimeMessagesInDocument(Document(), GH_RuntimeMessageLevel.Error);
    }
  }
}
