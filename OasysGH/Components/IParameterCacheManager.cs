using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace OasysGH.Components {
  public interface IParameterCacheManager {
    IParameterExpirationManager InputManager { get; set; }
    IParameterExpirationManager OutputManager { get; set; }

    void AddAddidionalInput(int index, object input);
    List<DataTree<IGH_Goo>> GetOutput(int runCount);
    void SetInput(List<IGH_Param> input);
    void SetOutput(List<DataTree<IGH_Goo>> output, int runCount);
  }
}
