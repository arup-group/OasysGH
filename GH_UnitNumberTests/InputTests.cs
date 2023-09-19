using System.IO;
using GH_UnitNumberTests.Helpers;
using Grasshopper.Kernel;
using OasysUnits;
using OasysUnits.Units;
using Xunit;

namespace GH_UnitNumberTests.Components.Helpers {
  [Collection("GrasshopperFixture collection")]
  public class InputTests {

    public static GH_Document Document() {
      string fileName = "OasysGH_InputHelperTests.gh";

      string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;
      string path = Path.Combine(solutiondir, "ExampleFiles");
      var io = new GH_DocumentIO();
      Assert.True(File.Exists(Path.Combine(path, fileName)));
      Assert.True(io.Open(Path.Combine(path, fileName)));
      io.Document.NewSolution(true);
      return io.Document;
    }

    [Fact]
    public void GenericGooInputFromUnitNumberTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "GenericGooInputFromUnitNumberTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Moment(2.5, MomentUnit.KilonewtonMeter), output.Value);
    }

    [Fact]
    public void GenericGooListInputFromUnitNumbersTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "GenericGooListInputFromUnitNumbersTest");
      Assert.NotNull(param);
      param.CollectData();
      var output1 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Force(4.0, ForceUnit.Kilonewton), output1.Value);
      var output2 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[1];
      Assert.Equal(new Force(5.2, ForceUnit.Kilonewton), output2.Value);
    }

    [Fact]
    public void LengthOrRatioInputFromNegativeNumberTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "LengthOrRatioInputFromNegativeNumberTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(25, RatioUnit.Percent), output.Value);
    }

    [Fact]
    public void LengthOrRatioInputFromNumberTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "LengthOrRatioInputFromNumberTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Length(2.5, LengthUnit.Meter), output.Value);
    }

    [Fact]
    public void LengthOrRatioInputFromTextRatioTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "LengthOrRatioInputFromTextRatioTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(25, RatioUnit.Percent), output.Value);
    }

    [Fact]
    public void LengthOrRatioInputFromTextTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "LengthOrRatioInputFromTextTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Length(2.5, LengthUnit.Meter), output.Value);
    }

    [Fact]
    public void LengthOrRatioInputFromUnitNumberRatioTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "LengthOrRatioInputFromUnitNumberRatioTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(2.5, RatioUnit.Percent), output.Value);
    }

    [Fact]
    public void LengthOrRatioInputFromUnitNumberTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "LengthsOrRatioInputFromUnitNumberTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Length(2.5, LengthUnit.Meter), output.Value);
    }

    [Fact]
    public void LengthOrRatioListInputFromNegativeNumbersTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "LengthOrRatioListInputFromNegativeNumbersTest");
      Assert.NotNull(param);
      param.CollectData();
      var output1 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(40, RatioUnit.Percent), output1.Value);
      var output2 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[1];
      Assert.Equal(new Ratio(52, RatioUnit.Percent), output2.Value);
    }

    [Fact]
    public void LengthOrRatioListInputFromNumbersTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "LengthOrRatioListInputFromNumbersTest");
      Assert.NotNull(param);
      param.CollectData();
      var output1 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Length(4.0, LengthUnit.Meter), output1.Value);
      var output2 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[1];
      Assert.Equal(new Length(5.2, LengthUnit.Meter), output2.Value);
    }

    [Fact]
    public void LengthOrRatioListInputFromTextRatiosTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "LengthOrRatioListInputFromTextRatiosTest");
      Assert.NotNull(param);
      param.CollectData();
      var output1 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(4, RatioUnit.Percent), output1.Value);
      var output2 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[1];
      Assert.Equal(new Ratio(5.2, RatioUnit.Percent), output2.Value);
    }

    [Fact]
    public void LengthOrRatioListInputFromTextsTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "LengthOrRatioListInputFromTextsTest");
      Assert.NotNull(param);
      param.CollectData();
      var output1 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Length(4.0, LengthUnit.Meter), output1.Value);
      var output2 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[1];
      Assert.Equal(new Length(5.2, LengthUnit.Meter), output2.Value);
    }

    [Fact]
    public void LengthOrRatioListInputFromUnitNumberRatiosTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "LengthOrRatioListInputFromUnitNumberRatiosTest");
      Assert.NotNull(param);
      param.CollectData();
      var output1 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(4, RatioUnit.DecimalFraction), output1.Value);
      var output2 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[1];
      Assert.Equal(new Ratio(5.2, RatioUnit.Percent), output2.Value);
    }

    [Fact]
    public void LengthOrRatioListInputFromUnitNumbersTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "LengthsOrRatioListInputFromUnitNumbersTest");
      Assert.NotNull(param);
      param.CollectData();
      var output1 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Length(4.0, LengthUnit.Meter), output1.Value);
      var output2 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[1];
      Assert.Equal(new Length(5.2, LengthUnit.Meter), output2.Value);
    }

    [Fact]
    public void RatioInDecimalFractionToDecimalFractionFromNumberTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "RatioInDecimalFractionToDecimalFractionFromNumberTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(0.5, RatioUnit.DecimalFraction), output.Value);
    }

    [Fact]
    public void RatioInDecimalFractionToDecimalFractionFromTextTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "RatioInDecimalFractionToDecimalFractionFromTextTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(50, RatioUnit.Percent), output.Value);
    }

    [Fact]
    public void RatioInDecimalFractionToDecimalFractionFromUnitNumberTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "RatioInDecimalFractionToDecimalFractionFromUnitNumberTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(2.5, RatioUnit.Percent), output.Value);
    }

    [Fact]
    public void RatioInDecimalFractionToPercentageFromNumberTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "RatioInDecimalFractionToPercentageFromNumberTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(50, RatioUnit.Percent), output.Value);
    }

    [Fact]
    public void RatioInDecimalFractionToPercentageFromTextTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "RatioInDecimalFractionToPercentageFromTextTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(50, RatioUnit.Percent), output.Value);
    }

    [Fact]
    public void RatioInDecimalFractionToPercentageFromUnitNumberTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "RatioInDecimalFractionToPercentageFromUnitNumberTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(2.5, RatioUnit.DecimalFraction), output.Value);
    }

    [Fact]
    public void UnitNumberInputFromNumberTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "UnitNumberInputFromNumberTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Moment(2.5, MomentUnit.KilonewtonMeter), output.Value);
    }

    [Fact]
    public void UnitNumberInputFromTextTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "UnitNumberInputFromTextTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Moment(2.5, MomentUnit.KilonewtonMeter), output.Value);
    }

    [Fact]
    public void UnitNumberInputFromUnitNumberTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "UnitNumberInputFromUnitNumberTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Moment(2.5, MomentUnit.KilonewtonMeter), output.Value);
    }

    [Fact]
    public void UnitNumberListInputFromNumbersTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "UnitNumberListInputFromNumbersTest");
      Assert.NotNull(param);
      param.CollectData();
      var output1 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Force(4.0, ForceUnit.Kilonewton), output1.Value);
      var output2 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[1];
      Assert.Equal(new Force(5.2, ForceUnit.Kilonewton), output2.Value);
    }

    [Fact]
    public void UnitNumberListInputFromTextsTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "UnitNumberListInputFromTextsTest");
      Assert.NotNull(param);
      param.CollectData();
      var output1 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Force(4.0, ForceUnit.Kilonewton), output1.Value);
      var output2 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[1];
      Assert.Equal(new Force(5.2, ForceUnit.Kilonewton), output2.Value);
    }

    [Fact]
    public void UnitNumberListInputFromUnitNumbersTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "UnitNumberListInputFromUnitNumbersTest");
      Assert.NotNull(param);
      param.CollectData();
      var output1 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Force(4.0, ForceUnit.Kilonewton), output1.Value);
      var output2 = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[1];
      Assert.Equal(new Force(5.2, ForceUnit.Kilonewton), output2.Value);
    }

    [Fact]
    public void UnitNumberOrDoubleAsRatioToPercentageFromNumberTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "UnitNumberOrDoubleAsRatioToPercentageFromNumberTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(50, RatioUnit.Percent), output.Value);
    }

    [Fact]
    public void UnitNumberOrDoubleAsRatioToPercentageFromTextTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "UnitNumberOrDoubleAsRatioToPercentageFromTextTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(50, RatioUnit.Percent), output.Value);
    }

    [Fact]
    public void UnitNumberOrDoubleAsRatioToPercentageFromUnitNumberTest() {
      GH_Document doc = Document();
      IGH_Param param = DocumentHelper.FindParameter(doc, "UnitNumberOrDoubleAsRatioToPercentageFromUnitNumberTest");
      Assert.NotNull(param);
      param.CollectData();
      var output = (OasysGH.Parameters.GH_UnitNumber)param.VolatileData.get_Branch(0)[0];
      Assert.Equal(new Ratio(2.5, RatioUnit.DecimalFraction), output.Value);
    }
  }
}
