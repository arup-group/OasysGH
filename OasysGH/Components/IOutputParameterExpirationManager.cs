using System.Collections.Generic;
using Grasshopper.Kernel;

namespace OasysGH.Components {
  public interface IOutputParameterExpirationManager {
    bool IsExpired(int outputIndex);
    void SetOutput(List<IGH_Param> output);
  }
}
