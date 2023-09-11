using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace OasysGH.Components {
  public interface IInputParameterCacheManager {
    void AddAddidionalInput(int index, object input);
    List<DataTree<IGH_Goo>> GetOutput(int runCount);
    bool IsExpired();
    void SetInput(List<IGH_Param> input);
    void SetOutput(List<DataTree<IGH_Goo>> output, int runCount);
  }
}
