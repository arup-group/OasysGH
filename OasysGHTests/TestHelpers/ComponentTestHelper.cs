using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace GsaGHTests.Helpers {
  public partial class ComponentTestHelper {

    public static Brep CreatePlanarBrep(Curve curve) {
      return Brep.CreatePlanarBreps(curve, 1e-6)[0];
    }

    public static Brep[] CreateBooleanDifference(Brep brepA, Brep brepB) {
      return Brep.CreateBooleanDifference(brepA, brepB, 1e-6);
    }

    public static void SetInput(GH_Component component, object inputObject, int index = 0) {
      var input = new Param_GenericObject();
      input.CreateAttributes();
      input.PersistentData.Append(new GH_ObjectWrapper(inputObject));
      component.Params.Input[index].AddSource(input);
    }

    public static object GetOutput(
      GH_Component component, int index = 0, int branch = 0, int item = 0, bool forceUpdate = false) {
      if (forceUpdate || component.Params.Output[index].VolatileDataCount == 0) {
        ComputeOutput(component, index);
      }

      return component.Params.Output[index].VolatileData.get_Branch(branch)[item];
    }

    public static void ComputeOutput(GH_Component component, int index = 0) {
      component.ExpireSolution(true);
      component.Params.Output[index].ExpireSolution(true);
      component.Params.Output[index].CollectData();
    }
  }
}
