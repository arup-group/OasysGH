using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
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
    private static readonly OasysUnitsIQuantityJsonConverter Converter = new OasysUnitsIQuantityJsonConverter();
    public List<List<string>> DropDownItems;
    public List<string> SelectedItems;
    public List<string> SpacerDescriptions;
    public bool IsInitialised = false;
    public bool AlwaysExpireDownStream = false;
    public Dictionary<int, List<string>> ExistingOutputsSerialized = new Dictionary<int, List<string>>();
    
    private Dictionary<int, List<bool>> OutputsAreExpired = new Dictionary<int, List<bool>>();
    private Dictionary<int, bool> OutputIsExpired = new Dictionary<int, bool>();

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

    #region expire downstream
    protected override void ExpireDownStreamObjects()
    {
      if (AlwaysExpireDownStream)
      {
        base.ExpireDownStreamObjects();
        return;
      }

      SetExpireDownStream();
      if (this.OutputIsExpired.Count > 0)
      {
        for (int outputIndex = 0; outputIndex < this.Params.Output.Count; outputIndex++)
        {
          if (this.OutputIsExpired[outputIndex])
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
      if (this.OutputsAreExpired != null && this.OutputsAreExpired.Count > 0)
      {
        this.OutputIsExpired = new Dictionary<int, bool>();
        for (int outputIndex = 0; outputIndex < this.Params.Output.Count; outputIndex++)
        {
          if (this.OutputsAreExpired.ContainsKey(outputIndex))
            this.OutputIsExpired.Add(outputIndex, OutputsAreExpired[outputIndex].Any(c => c == true));
          else
            this.OutputIsExpired.Add(outputIndex, true);
        }
      }
    }

    public void OutputChanged<T>(T data, int outputIndex, int index) where T : IGH_Goo
    {
      if (!this.ExistingOutputsSerialized.ContainsKey(outputIndex))
      {
        this.ExistingOutputsSerialized.Add(outputIndex, new List<string>());
        this.OutputsAreExpired.Add(outputIndex, new List<bool>());
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
        this.OutputsAreExpired[outputIndex].Add(true);
        return;
      }

      if (this.ExistingOutputsSerialized[outputIndex][index] != outputsSerialized)
      {
        this.ExistingOutputsSerialized[outputIndex][index] = outputsSerialized;
        this.OutputsAreExpired[outputIndex][index] = true;
        return;
      }
      this.OutputsAreExpired[outputIndex][index] = false;
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
