using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using OasysGH.Helpers;

namespace OasysGH.Components.Utility {
  public class DuplicateExpirationManager : IParameterExpirationManager {
    public int ParamCount { get; private set; } = 0;
    private Dictionary<int, List<object>> _existingParams = new Dictionary<int, List<object>>();
    private Dictionary<int, int> _index = new Dictionary<int, int>();
    private Dictionary<int, bool> _paramIsExpired = new Dictionary<int, bool>();
    private Dictionary<int, int> _lastIndex = new Dictionary<int, int>();
    private Dictionary<int, int> _lastRunCount = new Dictionary<int, int>();
    private Dictionary<int, int> _runCount = new Dictionary<int, int>();

    public DuplicateExpirationManager() {
      UpdateParamIndex(1);
    }

    public void AddItem<T>(int paramIndex, T item, int runCount) where T : IGH_Goo {
      UpdateParamIndex(paramIndex);

      if (runCount == 1) {
        Reset(paramIndex);
      } else if (_paramIsExpired[paramIndex]) {
        return;
      }

      if (_runCount[paramIndex] < runCount) {
        _runCount[paramIndex] = runCount;
      }

      _paramIsExpired[paramIndex] = ParamChanged(item, paramIndex);
    }

    public void AddItem(int paramIndex, object item, int runCount) {
      UpdateParamIndex(paramIndex);

      if (runCount == 1) {
        Reset(paramIndex);
      } else if (_paramIsExpired[paramIndex]) {
        return;
      }

      if (_runCount[paramIndex] < runCount) {
        _runCount[paramIndex] = runCount;
      }

      _paramIsExpired[paramIndex] = ParamChanged(item, paramIndex);
    }

    //public void AddList<T>(int paramIndex, List<T> data, int runCount) where T : IGH_Goo {
    //  UpdateParamIndex(paramIndex);

    //  if (runCount == 1) {
    //    Reset(paramIndex);
    //  } else if (_paramIsExpired[paramIndex]) {
    //    return;
    //  }

    //  if (_runCount[paramIndex] < runCount) {
    //    _runCount[paramIndex] = runCount;
    //  }

    //  _paramIsExpired[paramIndex] = ParamChanged(data, paramIndex);
    //}

    public void AddTree<T>(int paramIndex, DataTree<T> dataTree, int runCount) where T : IGH_Goo {
      UpdateParamIndex(paramIndex);

      if (runCount == 1) {
        Reset(paramIndex);
      } else if (_paramIsExpired[paramIndex]) {
        return;
      }

      if (_runCount[paramIndex] < runCount) {
        _runCount[paramIndex] = runCount;
      }

      if (dataTree.Paths.Count == 0) {
        _paramIsExpired[paramIndex] = ParamChanged(null, paramIndex);
        return;
      }

      int dataCount = dataTree.AllData().Count;
      if(_existingParams[paramIndex].Count > dataCount) {
        _existingParams[paramIndex] = _existingParams[paramIndex].GetRange(0, dataCount);
      }

      foreach (GH_Path path in dataTree.Paths) {
        List<T> data = dataTree.Branch(path);
        if (data.Count == 0) {
          _paramIsExpired[paramIndex] = ParamChanged(null, paramIndex);
        } else {
          _paramIsExpired[paramIndex] = ParamChanged(data, paramIndex);
        }
      }
    }

    public bool IsExpired() {
      for (int paramIndex = 0; paramIndex < ParamCount; paramIndex++) {
        if (IsExpired(paramIndex)) {
          return true;
        }
      }

      return false;
    }

    public bool IsExpired(int paramIndex) {
      if (_runCount[paramIndex] != _lastRunCount[paramIndex]) {
        return true;
      } else if (_index[paramIndex] != _lastIndex[paramIndex]) {
        return true;
      } else if (_paramIsExpired[paramIndex]) {
        return true;
      }

      return false;
    }

    private bool ParamChanged<T>(T item, int paramIndex) where T : IGH_Goo {
      if (item == null) {
        return ParamChanged((object)item, paramIndex);
      }

      object obj = ((T)(object)item).ScriptVariable();
      return ParamChanged(obj, paramIndex);
    }

    private bool ParamChanged(object obj, int paramIndex) {
      bool expired = false;
      if (_existingParams[paramIndex].Count == _index[paramIndex]) {
        // the number of entries for the current param has increased:
        // add an entry and expire param
        _existingParams[paramIndex].Add(obj);
        expired = true;
      } else if (!Duplicates.AreEqual(_existingParams[paramIndex][_index[paramIndex]], obj)) {
        _existingParams[paramIndex][_index[paramIndex]] = obj;
        expired = true;
      }

      _index[paramIndex]++;
      return expired;
    }

    private bool ParamChanged<T>(List<T> data, int paramIndex) where T : IGH_Goo {
      bool expired = false;
      foreach (T item in data) {
        if (ParamChanged(item, paramIndex)) {
          expired = true;
        }
      }

      return expired;
    }

    private void Reset(int paramIndex) {
      _paramIsExpired[paramIndex] = true;
      _lastIndex[paramIndex] = _index[paramIndex];
      _lastRunCount[paramIndex] = _runCount[paramIndex];

      _index[paramIndex] = 0;
      _runCount[paramIndex] = 0;
    }

    private void UpdateParamIndex(int index) {
      if (index >= ParamCount) {
        for (int paramIndex = ParamCount; paramIndex <= index; paramIndex++) {
          {
            _existingParams.Add(paramIndex, new List<object>());
            _index.Add(paramIndex, 0);
            _paramIsExpired.Add(paramIndex, true);
            _lastIndex.Add(paramIndex, 0);
            _lastRunCount.Add(paramIndex, 0);
            _runCount.Add(paramIndex, 0);
          }
        }

        ParamCount = index + 1;
      }
    }
  }
}
