using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel.Types;

namespace OasysGH.Components {
  public interface IParameterExpirationManager {
    void AddItem<T>(int paramIndex, T item, int runCount) where T : IGH_Goo;
    void AddItem(int paramIndex, object item, int runCount);
    void AddList<T>(int paramIndex, List<T> data, int runCount) where T : IGH_Goo;
    void AddTree<T>(int paramIndex, DataTree<T> dataTree, int runCount) where T : IGH_Goo;
    bool IsExpired();
    bool IsExpired(int paramIndex);
  }
}
