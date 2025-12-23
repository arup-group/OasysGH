using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using GsaGHTests.Helpers;
using Oasys.Taxonomy.Geometry;
using Oasys.Taxonomy.Profiles;
using OasysGH.Components.Tests;
using OasysGH.Helpers;
using OasysGH.Parameters;
using OasysGH.UI;
using OasysGHTests.TestHelpers;
using OasysUnits.Units;
using Rhino.Geometry;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class CreateOasysProfileTests {
    [Fact]
    public void ChangeDropDownTest() {
      var comp = new CreateProfile();
      comp.CreateAttributes();

      Assert.True(comp._isInitialised);
      Assert.Equal(4, comp._spacerDescriptions.Count);
      Assert.Equal(comp.DropDownItems.Count, comp._selectedItems.Count);

      for (int i = 0; i < comp.DropDownItems.Count; i++) {
        comp.SetSelected(i, 0);

        for (int j = 0; j < comp.DropDownItems[i].Count; j++) {
          comp.SetSelected(i, j);
          comp.ExpireSolution(true);
          comp.Params.Output[0].CollectData();
          Assert.Equal(comp._selectedItems[i], comp.DropDownItems[i][j]);
        }
      }
    }

    [Fact]
    public void ChangeDropDownCatalogueTest() {
      var comp = new CreateProfile();
      comp.CreateAttributes();

      Assert.True(comp._isInitialised);
      Assert.Equal(4, comp._spacerDescriptions.Count);
      Assert.Equal(comp.DropDownItems.Count, comp._selectedItems.Count);

      comp.SetSelected(0, 1);

      // looping over catalogues, skipping first "All"
      for (int i = 1; i < comp.DropDownItems[1].Count; i++) {
        comp.SetSelected(1, i);

        // if selected catalogue does not contain any sections then check warning and continue
        if (comp.DropDownItems.Count < 3) {
          Assert.Single(comp.RuntimeMessages(Grasshopper.Kernel.GH_RuntimeMessageLevel.Warning));
          continue;
        }

        // looping over sections
        for (int j = 1; j < comp.DropDownItems[2].Count; j++) {
          comp.SetSelected(2, j);
          comp.ExpireSolution(true);
          comp.Params.Output[0].CollectData();
          Assert.Equal(comp._selectedItems[2], comp.DropDownItems[2][j]);

          for (int k = 1; k < comp.DropDownItems[3].Count; k++) {
            comp.SetSelected(3, k);
            comp.ExpireSolution(true);
            comp.Params.Output[0].CollectData();
            Assert.Equal(comp._selectedItems[3], comp.DropDownItems[3][k]);
          }
        }
      }
    }

    [Fact]
    public void TestAttributes() {
      var comp = new CreateProfile();
      Assert.True(Mouse.TestMouseMove(comp));
      Assert.True(Mouse.TestMouseClick(comp));
      var attributes = (DropDownComponentAttributes)Document.Attributes(comp);
      attributes.CustomRender(new PictureBox().CreateGraphics());
    }

    [Fact]
    public void PerimeterProfileExpectedUsingCurve() {
      var comp = new CreateProfile();
      comp.SetSelected(0, 13);
      comp.SetSelected(1, 2);
      ComponentTestHelper.SetInput(comp, new LineCurve(new Point3d(0, 0, 0), new Point3d(2, 1, 0)));
      var output = (OasysProfileGoo)ComponentTestHelper.GetOutput(comp);
      Assert.NotNull(output);
      Assert.Contains("GEO P(m) M(0|0) L(2|1)", output.ToString());
    }

    [Fact]
    public void PerimeterProfileExpectedInXYPlane() {
      var comp = new CreateProfile();
      comp.SetSelected(0, 13);
      comp.SetSelected(1, 2);

      var rectangle = new Rectangle3d(Plane.WorldXY, 2.0, 1.0);
      rectangle.ToPolyline().ToPolylineCurve();
      Brep brep = ComponentTestHelper.CreatePlanarBrep(rectangle.ToNurbsCurve());

      ComponentTestHelper.SetInput(comp, brep);

      var output = (OasysProfileGoo)ComponentTestHelper.GetOutput(comp);
      Assert.NotNull(output);
      Assert.Contains("GEO P(m) M(0|0) L(2|0) L(2|1) L(0|1)", output.ToString());
    }

    [Fact]
    public void PerimeterProfileExpectedInInclinedPlane() {
      var comp = new CreateProfile();
      comp.SetSelected(0, 13);
      comp.SetSelected(1, 2);

      var inclinedPlane = new Plane(Point3d.Origin, Vector3d.XAxis, new Vector3d(0, 0.707, 0.707));
      var rectangle = new Rectangle3d(inclinedPlane, 2.0, 1.0);
      Brep brep = ComponentTestHelper.CreatePlanarBrep(rectangle.ToNurbsCurve());
      ComponentTestHelper.SetInput(comp, brep);

      var output = (OasysProfileGoo)ComponentTestHelper.GetOutput(comp);
      Assert.NotNull(output);
      Assert.Contains("GEO P(m) M(0|0) L(2|0) L(2|1) L(0|1)", output.ToString());
    }
  }
}
