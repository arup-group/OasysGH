using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Newtonsoft.Json;
using OasysGH.Parameters;
using OasysUnits;
using OasysUnits.Serialization.JsonNet;

namespace OasysGH.Components {

  public abstract class GH_OasysDropDownComponent : GH_OasysComponent, IGH_VariableParameterComponent {
    protected internal bool _alwaysExpireDownStream = false;
    protected internal List<List<string>> _dropDownItems;
    protected internal Dictionary<int, List<string>> _existingOutputsSerialized = new Dictionary<int, List<string>>();
    protected internal bool _isInitialised = false;
    protected internal List<string> _selectedItems;
    protected internal List<string> _spacerDescriptions;
    private static readonly OasysUnitsIQuantityJsonConverter converter = new OasysUnitsIQuantityJsonConverter();
    private Dictionary<int, bool> _outputIsExpired = new Dictionary<int, bool>();
    private Dictionary<int, List<bool>> _outputsAreExpired = new Dictionary<int, List<bool>>();
    public GH_OasysDropDownComponent(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory) {
    }

    bool IGH_VariableParameterComponent.CanInsertParameter(GH_ParameterSide side, int index) => false;

    bool IGH_VariableParameterComponent.CanRemoveParameter(GH_ParameterSide side, int index) => false;

    public override void CreateAttributes() {
      if (!_isInitialised)
        InitialiseDropdowns();

      m_attributes = new UI.DropDownComponentAttributes(this, SetSelected, _dropDownItems, _selectedItems, _spacerDescriptions);
    }

    IGH_Param IGH_VariableParameterComponent.CreateParameter(GH_ParameterSide side, int index) => null;

    bool IGH_VariableParameterComponent.DestroyParameter(GH_ParameterSide side, int index) => false;

    public void OutputChanged<T>(T data, int outputIndex, int index) where T : IGH_Goo {
      if (!_existingOutputsSerialized.ContainsKey(outputIndex)) {
        _existingOutputsSerialized.Add(outputIndex, new List<string>());
        _outputsAreExpired.Add(outputIndex, new List<bool>());
      }

      string outputsSerialized = "";
      if (data.GetType() == typeof(GH_UnitNumber)) {
        // use IQuantity converter if data is a IQuantity (struct)
        IQuantity quantity = ((GH_UnitNumber)(object)data).Value;
        outputsSerialized = JsonConvert.SerializeObject(quantity, converter);
      }
      else {
        object obj = ((T)(object)data).ScriptVariable();
        try {
          outputsSerialized = JsonConvert.SerializeObject(obj);
        }
        catch (Exception) {
          outputsSerialized = data.GetHashCode().ToString();
        }
      }

      if (_existingOutputsSerialized[outputIndex].Count == index) {
        _existingOutputsSerialized[outputIndex].Add(outputsSerialized);
        _outputsAreExpired[outputIndex].Add(true);
        return;
      }

      if (_existingOutputsSerialized[outputIndex][index] != outputsSerialized) {
        _existingOutputsSerialized[outputIndex][index] = outputsSerialized;
        _outputsAreExpired[outputIndex][index] = true;
        return;
      }
      _outputsAreExpired[outputIndex][index] = false;
    }

    public override bool Read(GH_IO.Serialization.GH_IReader reader) {
      Helpers.DeSerialization.ReadDropDownComponents(ref reader, ref _dropDownItems, ref _selectedItems, ref _spacerDescriptions);

      _isInitialised = true;
      UpdateUIFromSelectedItems();

      return base.Read(reader);
    }

    public abstract void SetSelected(int i, int j);

    public virtual void VariableParameterMaintenance() {
    }

    public override bool Write(GH_IO.Serialization.GH_IWriter writer) {
      Helpers.DeSerialization.WriteDropDownComponents(ref writer, _dropDownItems, _selectedItems, _spacerDescriptions);
      return base.Write(writer);
    }

    protected internal abstract void InitialiseDropdowns();

    protected override void ExpireDownStreamObjects() {
      if (_alwaysExpireDownStream) {
        base.ExpireDownStreamObjects();
        return;
      }

      SetExpireDownStream();
      if (_outputIsExpired.Count > 0) {
        for (int outputIndex = 0; outputIndex < Params.Output.Count; outputIndex++) {
          if (_outputIsExpired[outputIndex]) {
            IGH_Param item = Params.Output[outputIndex];
            item.ExpireSolution(recompute: false);
          }
        }
      }
      else
        base.ExpireDownStreamObjects();
    }

    protected virtual void UpdateUI() {
      (this as IGH_VariableParameterComponent).VariableParameterMaintenance();
      ExpireSolution(true);
      Params.OnParametersChanged();
      OnDisplayExpired(true);
    }

    protected virtual void UpdateUIFromSelectedItems() {
      CreateAttributes();
      UpdateUI();
    }

    private void SetExpireDownStream() {
      if (_outputsAreExpired != null && _outputsAreExpired.Count > 0) {
        _outputIsExpired = new Dictionary<int, bool>();
        for (int outputIndex = 0; outputIndex < Params.Output.Count; outputIndex++) {
          if (_outputsAreExpired.ContainsKey(outputIndex))
            _outputIsExpired.Add(outputIndex, _outputsAreExpired[outputIndex].Any(c => c == true));
          else
            _outputIsExpired.Add(outputIndex, true);
        }
      }
    }
  }
}
