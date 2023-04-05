using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Newtonsoft.Json;
using OasysGH.Parameters;
using OasysUnits;
using OasysUnits.Serialization.JsonNet;

namespace OasysGH.Components
{
  public abstract class GH_OasysDropDownComponent : GH_OasysComponent, IGH_VariableParameterComponent
  {
    protected internal List<List<string>> _dropDownItems;
    protected internal List<string> _selectedItems;
    protected internal List<string> _spacerDescriptions;
    protected internal bool _isInitialised = false;
    protected internal bool _alwaysExpireDownStream = false;
    protected internal Dictionary<int, List<string>> _existingOutputsSerialized = new Dictionary<int, List<string>>();

    private Dictionary<int, List<bool>> _outputsAreExpired = new Dictionary<int, List<bool>>();
    private Dictionary<int, bool> _outputIsExpired = new Dictionary<int, bool>();
    private static readonly OasysUnitsIQuantityJsonConverter _converter = new OasysUnitsIQuantityJsonConverter();

    public GH_OasysDropDownComponent(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory)
    {
    }

    #region UI
    public override void CreateAttributes()
    {
      if (!_isInitialised)
        InitialiseDropdowns();

      m_attributes = new UI.DropDownComponentAttributes(this, SetSelected, _dropDownItems, _selectedItems, _spacerDescriptions);
    }

    protected internal abstract void InitialiseDropdowns();

    public abstract void SetSelected(int i, int j);

    protected virtual void UpdateUIFromSelectedItems()
    {
      CreateAttributes();
      UpdateUI();
    }

    protected virtual void UpdateUI()
    {
      (this as IGH_VariableParameterComponent).VariableParameterMaintenance();
      ExpireSolution(true);
      Params.OnParametersChanged();
      OnDisplayExpired(true);
    }
    #endregion

    #region expire downstream
    protected override void ExpireDownStreamObjects()
    {
      if (_alwaysExpireDownStream)
      {
        base.ExpireDownStreamObjects();
        return;
      }

      SetExpireDownStream();
      if (_outputIsExpired.Count > 0)
      {
        for (int outputIndex = 0; outputIndex < Params.Output.Count; outputIndex++)
        {
          if (_outputIsExpired[outputIndex])
          {
            IGH_Param item = Params.Output[outputIndex];
            item.ExpireSolution(recompute: false);
          }
        }
      }
      else
        base.ExpireDownStreamObjects();
    }

    private void SetExpireDownStream()
    {
      if (_outputsAreExpired != null && _outputsAreExpired.Count > 0)
      {
        _outputIsExpired = new Dictionary<int, bool>();
        for (int outputIndex = 0; outputIndex < Params.Output.Count; outputIndex++)
        {
          if (_outputsAreExpired.ContainsKey(outputIndex))
            _outputIsExpired.Add(outputIndex, _outputsAreExpired[outputIndex].Any(c => c == true));
          else
            _outputIsExpired.Add(outputIndex, true);
        }
      }
    }

    public void OutputChanged<T>(T data, int outputIndex, int index) where T : IGH_Goo
    {
      if (!_existingOutputsSerialized.ContainsKey(outputIndex))
      {
        _existingOutputsSerialized.Add(outputIndex, new List<string>());
        _outputsAreExpired.Add(outputIndex, new List<bool>());
      }

      string outputsSerialized = "";
      if (data.GetType() == typeof(GH_UnitNumber))
      {
        // use IQuantity converter if data is a IQuantity (struct)
        IQuantity quantity = ((GH_UnitNumber)(object)data).Value;
        outputsSerialized = JsonConvert.SerializeObject(quantity, _converter);
      }
      else
      {
        var obj = ((T)(object)data).ScriptVariable();
        try
        {
          outputsSerialized = JsonConvert.SerializeObject(obj);
        }
        catch (Exception)
        {
          outputsSerialized = data.GetHashCode().ToString();
        }
      }

      if (_existingOutputsSerialized[outputIndex].Count == index)
      {
        _existingOutputsSerialized[outputIndex].Add(outputsSerialized);
        _outputsAreExpired[outputIndex].Add(true);
        return;
      }

      if (_existingOutputsSerialized[outputIndex][index] != outputsSerialized)
      {
        _existingOutputsSerialized[outputIndex][index] = outputsSerialized;
        _outputsAreExpired[outputIndex][index] = true;
        return;
      }
      _outputsAreExpired[outputIndex][index] = false;
    }
    #endregion

    #region (de)serialization
    public override bool Write(GH_IO.Serialization.GH_IWriter writer)
    {
      Helpers.DeSerialization.WriteDropDownComponents(ref writer, _dropDownItems, _selectedItems, _spacerDescriptions);
      return base.Write(writer);
    }

    public override bool Read(GH_IO.Serialization.GH_IReader reader)
    {
      Helpers.DeSerialization.ReadDropDownComponents(ref reader, ref _dropDownItems, ref _selectedItems, ref _spacerDescriptions);

      _isInitialised = true;
      UpdateUIFromSelectedItems();

      return base.Read(reader);
    }
    #endregion

    #region IGH_VariableParameterComponent null implementation
    public virtual void VariableParameterMaintenance() { }

    bool IGH_VariableParameterComponent.CanInsertParameter(GH_ParameterSide side, int index) => false;

    bool IGH_VariableParameterComponent.CanRemoveParameter(GH_ParameterSide side, int index) => false;

    IGH_Param IGH_VariableParameterComponent.CreateParameter(GH_ParameterSide side, int index) => null;

    bool IGH_VariableParameterComponent.DestroyParameter(GH_ParameterSide side, int index) => false;
    #endregion
  }
}
