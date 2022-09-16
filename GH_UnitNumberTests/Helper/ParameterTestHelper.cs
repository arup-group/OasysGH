using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using System.Collections.Generic;

namespace GH_UnitNumberTests.Helpers
{
  public class ParameterTestHelper
  {
    public static T GetOutput<T>(GH_Param<T> param, int branch = 0, int item = 0) where T : class, IGH_Goo
    {
      param.CollectData();
      return (T)param.VolatileData.get_Branch(branch)[item];
    }
  }
}
