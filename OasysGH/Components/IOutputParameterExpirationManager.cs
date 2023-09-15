using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace OasysGH.Components {
  public interface IOutputParameterExpirationManager {
    List<DataTree<IGH_Goo>> GetOutput(int runCount);
    bool IsExpired();
    bool IsExpired(int outputIndex);
    //void SetOutput(List<IGH_Param> output);
    void SetOutput(List<DataTree<IGH_Goo>> output, int runCount);
  }
}
