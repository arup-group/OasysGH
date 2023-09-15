using System.Collections;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace OasysGH.Components {
  public class OutputParameterExpirationManager : IOutputParameterExpirationManager {
    public IParameterExpirationManager EpirationManager { get; set; }
    public bool RunOnce { get; private set; }

    private readonly Dictionary<int, List<DataTree<IGH_Goo>>> _outputCache = new Dictionary<int, List<DataTree<IGH_Goo>>>();

    public OutputParameterExpirationManager(IParameterExpirationManager epirationManager) {
      EpirationManager = epirationManager;
    }

    public bool IsExpired() {
      return EpirationManager.IsExpired();
    }

    public bool IsExpired(int outputIndex) {
      return EpirationManager.IsExpired(outputIndex);
    }

    //public void SetOutput(List<IGH_Param> input) {
    //  for (int index = 0; index < input.Count; index++) {
    //    IEnumerator<IGH_Goo> enumerator = input[index].VolatileData.AllData(false).GetEnumerator();
    //    while (enumerator.MoveNext()) {
    //      IGH_Goo goo = enumerator.Current;
    //      EpirationManager.AddItem(index, goo, 1);
    //    }
    //  }
    //}

    public List<DataTree<IGH_Goo>> GetOutput(int runCount) {
      return _outputCache[runCount];
    }

    public void Reset() {
      RunOnce = false;
    }

    public void SetInput(List<IGH_Param> input) {
      for (int index = 0; index < input.Count; index++) {
        IGH_Structure structure = input[index].VolatileData;

        // this does not always work!
        var tree = new DataTree<IGH_Goo>();
        //tree.MergeStructure(structure, null);

        int num = structure.PathCount - 1;
        for (int i = 0; i <= num; i++) {
          IList list = structure.get_Branch(i);
          var list2 = new List<IGH_Goo>(list.Count);
          int num2 = list.Count - 1;
          for (int j = 0; j <= num2; j++) {
            if (list[j] == null) {
              list2.Add(default(IGH_Goo));
              continue;
            }

            //object target = null;
            //if (hint != null) {
            //  hint.Cast(RuntimeHelpers.GetObjectValue(list[j]), out target);
            //  if (target != null && target is T) {
            //    list2.Add((T)target);
            //  }
            //} else {
            //var target2 = default(IGH_Goo);
            //if (((IGH_Goo)list[j]).CastTo<IGH_Goo>(out target2)) {
            //  list2.Add(target2);
            //} else if (list[j] is IGH_Goo) {
            //  list2.Add((IGH_Goo)list[j]);
            //} else {
            //  list2.Add(default(IGH_Goo));
            //}
            //}

            list2.Add((IGH_Goo)list[j]);
            tree.AddRange(list2, structure.get_Path(i));
          }
        }

        EpirationManager.AddTree(index, tree, 1);
      }

      RunOnce = true;
    }

    public void SetOutput(List<IGH_Param> output) => throw new System.NotImplementedException();

    public void SetOutput(List<DataTree<IGH_Goo>> output, int runCount) {
      if (_outputCache.ContainsKey(runCount)) {
        _outputCache[runCount] = output;
      } else {
        _outputCache.Add(runCount, output);
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

    public void SetOutputTree<T>(IGH_DataAccess da, int outputIndex, DataTree<T> dataTree, int runCount) where T : IGH_Goo {
      da.SetDataTree(outputIndex, dataTree);
      EpirationManager.AddTree(outputIndex, dataTree, runCount);
    }

  }
}
