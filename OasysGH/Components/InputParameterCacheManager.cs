using System;
using System.Collections;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace OasysGH.Components {
  public class InputParameterCacheManager  {
    public IParameterExpirationManager EpirationManager { get; set; }
    public bool RunOnce { get; private set; }


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
