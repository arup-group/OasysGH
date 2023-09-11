using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace OasysGH.Components {
  public class OutputParameterExpirationManager : IOutputParameterExpirationManager {
    public IParameterExpirationManager EpirationManager { get; set; }

    public OutputParameterExpirationManager(IParameterExpirationManager epirationManager) {
      EpirationManager = epirationManager;
    }

    public bool IsExpired(int outputIndex) {
      return EpirationManager.IsExpired(outputIndex);
    }

    public void SetOutput(List<IGH_Param> input) {
      for (int index = 0; index < input.Count; index++) {
        IEnumerator<IGH_Goo> enumerator = input[index].VolatileData.AllData(false).GetEnumerator();
        while (enumerator.MoveNext()) {
          IGH_Goo goo = enumerator.Current;
          EpirationManager.AddItem(index, goo, 1);
        }
      }
    }

    //public void SetOutputData(IGH_DataAccess da, Dictionary<int, IGH_Goo> _data, int runCount) {
    //  foreach (int index in _data.Keys) {
    //    SetOutputItem(da, index, _data[index], runCount);
    //  }
    //}

    //public void SetOutputItem<T>(IGH_DataAccess da, int outputIndex, T item, int runCount) where T : IGH_Goo {
    //  da.SetData(outputIndex, item);
    //  OutputManager.AddItem(outputIndex, item, runCount);
    //}

    //public void SetOutputList<T>(IGH_DataAccess da, int outputIndex, List<T> data, int runCount) where T : IGH_Goo {
    //  da.SetDataList(outputIndex, data);
    //  OutputManager.AddList(outputIndex, data, runCount);
    //}

    //public void SetOutputTree<T>(IGH_DataAccess da, int outputIndex, DataTree<T> dataTree, int runCount) where T : IGH_Goo {
    //  da.SetDataTree(outputIndex, dataTree);
    //  OutputManager.AddTree(outputIndex, dataTree, runCount);
    //}
  }
}
