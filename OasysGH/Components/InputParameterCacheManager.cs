using System;
using System.Collections;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace OasysGH.Components {
  public class InputParameterCacheManager : IInputParameterCacheManager {
    public IParameterExpirationManager EpirationManager { get; set; }
    public bool RunOnce { get; private set; }

    private readonly Dictionary<int, List<DataTree<IGH_Goo>>> _outputCache = new Dictionary<int, List<DataTree<IGH_Goo>>>();

    public InputParameterCacheManager(IParameterExpirationManager epirationManager) {
      EpirationManager = epirationManager;
    }

    public void AddAddidionalInput(int index, object item) {
      if (item is IGH_Goo goo) {
        EpirationManager.AddItem(index, goo, 1);
      } else if (item is IGH_DataTree tree) {
        EpirationManager.AddItem(index, tree, 1);
      } else if (item is List<IGH_Goo> list) {
        EpirationManager.AddItem(index, list, 1);
      } else {
        EpirationManager.AddItem(index, item, 1);
      }
    }

    public bool IsExpired() {
      return EpirationManager.IsExpired();
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

    //public void SetInput(Dictionary<int, object> data, int runCount) {
    //  foreach (int index in data.Keys) {
    //    SetInputItem(index, data[index], runCount);
    //  }
    //}

    //public void SetInputItem(int index, object item, int runCount) {
    //  if (item is IGH_Goo goo) {
    //    InputManager.AddItem(index, goo, runCount);
    //  } else if (item is IGH_DataTree tree) {
    //    InputManager.AddItem(index, tree, runCount);
    //  } else if (item is List<IGH_Goo> list) {
    //    InputManager.AddItem(index, list, runCount);
    //  } else {
    //    InputManager.AddItem(index, item, runCount);
    //  }
    //}

    //public void SetInputList(int index, List<IGH_Goo> data, int runCount) {
    //  foreach (IGH_Goo item in data) {
    //    SetInputItem(index, item, runCount);
    //  }
    //}

    //public void SetInputTree(int index, DataTree<IGH_Goo> dataTree, int runCount) {
    //  InputManager.AddTree(index, dataTree, runCount);
    //}

    //public void SetOutputData(IGH_DataAccess da, Dictionary<int, IGH_Goo> _data, int runCount) {
    //  foreach (int index in _data.Keys) {
    //    SetOutputItem(da, index, _data[index], runCount);
    //  }
    //}

    public List<DataTree<IGH_Goo>> GetOutput(int runCount) {
      return _outputCache[runCount];
    }

    public void Reset() {
      RunOnce = false;
    }

    public void SetOutput(List<DataTree<IGH_Goo>> output, int runCount) {
      if (_outputCache.ContainsKey(runCount)) {
        _outputCache[runCount] = output;
      } else {
        _outputCache.Add(runCount, output);
      }
    }


    //public void SetOutput(List<IGH_Goo> output, int runCount) {
    //  if (_outputCache.ContainsKey(runCount)) {
    //    _outputCache[runCount] = output;
    //  } else {
    //    _outputCache.Add(runCount, output);
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
