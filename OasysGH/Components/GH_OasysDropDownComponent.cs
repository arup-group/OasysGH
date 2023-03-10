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
    protected List<List<string>> DropDownItems;
    protected List<string> SelectedItems;
    protected List<string> SpacerDescriptions;
    protected bool IsInitialised = false;
    protected bool AlwaysExpireDownStream = false;
    protected Dictionary<int, List<string>> ExistingOutputsSerialized = new Dictionary<int, List<string>>();

    private Dictionary<int, List<bool>> _outputsAreExpired = new Dictionary<int, List<bool>>();
    private Dictionary<int, bool> _outputIsExpired = new Dictionary<int, bool>();
    private static readonly OasysUnitsIQuantityJsonConverter Converter = new OasysUnitsIQuantityJsonConverter();

    public GH_OasysDropDownComponent(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory)
    {
    }

    #region UI
    public override void CreateAttributes()
    {
      if (!this.IsInitialised)
        this.InitialiseDropdowns();

      m_attributes = new UI.DropDownComponentAttributes(this, this.SetSelected, this.DropDownItems, this.SelectedItems, this.SpacerDescriptions);
    }

    protected abstract void InitialiseDropdowns();

    public abstract void SetSelected(int i, int j);

    protected virtual void UpdateUIFromSelectedItems()
    {
      this.CreateAttributes();
      this.UpdateUI();
    }

    protected virtual void UpdateUI()
    {
      (this as IGH_VariableParameterComponent).VariableParameterMaintenance();
      this.ExpireSolution(true);
      this.Params.OnParametersChanged();
      this.OnDisplayExpired(true);
    }
    #endregion

    #region expire downstream
    protected override void ExpireDownStreamObjects()
    {
      if (this.AlwaysExpireDownStream)
      {
        base.ExpireDownStreamObjects();
        return;
      }

      SetExpireDownStream();
      if (this._outputIsExpired.Count > 0)
      {
        for (int outputIndex = 0; outputIndex < this.Params.Output.Count; outputIndex++)
        {
          if (this._outputIsExpired[outputIndex])
          {
            IGH_Param item = this.Params.Output[outputIndex];
            item.ExpireSolution(recompute: false);
          }
        }
      }
      else
        base.ExpireDownStreamObjects();
    }

    private void SetExpireDownStream()
    {
      if (this._outputsAreExpired != null && this._outputsAreExpired.Count > 0)
      {
        this._outputIsExpired = new Dictionary<int, bool>();
        for (int outputIndex = 0; outputIndex < this.Params.Output.Count; outputIndex++)
        {
          if (this._outputsAreExpired.ContainsKey(outputIndex))
            this._outputIsExpired.Add(outputIndex, _outputsAreExpired[outputIndex].Any(c => c == true));
          else
            this._outputIsExpired.Add(outputIndex, true);
        }
      }
    }

    public void OutputChanged<T>(T data, int outputIndex, int index) where T : IGH_Goo
    {
      if (!this.ExistingOutputsSerialized.ContainsKey(outputIndex))
      {
        this.ExistingOutputsSerialized.Add(outputIndex, new List<string>());
        this._outputsAreExpired.Add(outputIndex, new List<bool>());
      }

      string outputsSerialized = "";
      if (data.GetType() == typeof(GH_UnitNumber))
      {
        // use IQuantity converter if data is a IQuantity (struct)
        IQuantity quantity = ((GH_UnitNumber)(object)data).Value;
        outputsSerialized = JsonConvert.SerializeObject(quantity, Converter);
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

      if (this.ExistingOutputsSerialized[outputIndex].Count == index)
      {
        this.ExistingOutputsSerialized[outputIndex].Add(outputsSerialized);
        this._outputsAreExpired[outputIndex].Add(true);
        return;
      }

      if (this.ExistingOutputsSerialized[outputIndex][index] != outputsSerialized)
      {
        this.ExistingOutputsSerialized[outputIndex][index] = outputsSerialized;
        this._outputsAreExpired[outputIndex][index] = true;
        return;
      }
      this._outputsAreExpired[outputIndex][index] = false;
    }
    #endregion

    #region (de)serialization
    public override bool Write(GH_IO.Serialization.GH_IWriter writer)
    {
      Helpers.DeSerialization.WriteDropDownComponents(ref writer, this.DropDownItems, this.SelectedItems, this.SpacerDescriptions);
      return base.Write(writer);
    }

    public override bool Read(GH_IO.Serialization.GH_IReader reader)
    {
      Helpers.DeSerialization.ReadDropDownComponents(ref reader, ref this.DropDownItems, ref this.SelectedItems, ref this.SpacerDescriptions);

      this.IsInitialised = true;
      this.UpdateUIFromSelectedItems();

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
