using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Newtonsoft.Json;
using OasysGH.Units;
using OasysUnitsNet;
using UnitsNet.Serialization.JsonNet;

namespace OasysGH.Components
{
  public abstract class GH_OasysDropDownComponent : GH_OasysComponent, IGH_VariableParameterComponent
  {
    private static readonly UnitsNetIQuantityJsonConverter Converter = new UnitsNetIQuantityJsonConverter();
    public List<List<string>> DropDownItems;
    public List<string> SelectedItems;
    public List<string> SpacerDescriptions;
    public bool IsInitialised = false;
    public bool ExpireDownStream = true;
    public Dictionary<int, List<string>> ExistingOutputsSerialized = new Dictionary<int, List<string>>() { { 0, new List<string>() { "" } } };

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

    protected override void ExpireDownStreamObjects()
    {
      if (this.ExpireDownStream)
        base.ExpireDownStreamObjects();
    }

    public abstract void InitialiseDropdowns();

    public abstract void SetSelected(int i, int j);

    public virtual void UpdateUIFromSelectedItems()
    {
      this.CreateAttributes();
      this.UpdateUI();
    }

    public virtual void UpdateUI()
    {
      (this as IGH_VariableParameterComponent).VariableParameterMaintenance();
      this.ExpireSolution(true);
      this.Params.OnParametersChanged();
      this.OnDisplayExpired(true);
    }
    #endregion

    #region (de)serialization
    public void CheckExpireDownstream<T>(T data, int outputIndex, int index) where T : IGH_Goo
    {
      if (!this.ExistingOutputsSerialized.ContainsKey(outputIndex))
        this.ExistingOutputsSerialized.Add(outputIndex, new List<string>());

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
        this.ExpireDownStream = true;
        this.ExistingOutputsSerialized[outputIndex].Add(outputsSerialized);
        return;
      }

      if (this.ExistingOutputsSerialized[outputIndex][index] != outputsSerialized)
      {
        this.ExpireDownStream = true;
        this.ExistingOutputsSerialized[outputIndex][index] = outputsSerialized;
        return;
      }

      this.ExpireDownStream = false;
    }

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
