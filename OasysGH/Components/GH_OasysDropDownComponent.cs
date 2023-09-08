using System;
using System.Collections.Generic;
using System.Linq;
using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using OasysGH.Helpers;
using OasysUnits;

namespace OasysGH.Components {
  public abstract class GH_OasysDropDownComponent : GH_OasysComponent, IGH_VariableParameterComponent {
    public IParameterExpirationManager OutputManager { get; set; }
    public IParameterExpirationManager InputManager { get; set; }

    protected internal List<List<string>> _dropDownItems;
    protected internal bool _isInitialised = false;
    protected internal List<string> _selectedItems;
    protected internal List<string> _spacerDescriptions;

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

    public override bool Read(GH_IReader reader) {
      Helpers.DeSerialization.ReadDropDownComponents(ref reader, ref _dropDownItems, ref _selectedItems, ref _spacerDescriptions);

      _isInitialised = true;
      UpdateUIFromSelectedItems();

      return base.Read(reader);
    }

    public abstract void SetSelected(int i, int j);

    public virtual void VariableParameterMaintenance() {
    }

    public override bool Write(GH_IWriter writer) {
      Helpers.DeSerialization.WriteDropDownComponents(ref writer, _dropDownItems, _selectedItems, _spacerDescriptions);
      return base.Write(writer);
    }

    protected sealed override void SolveInstance(IGH_DataAccess da) {
      if (InputManager != null) {

        int paramCount;
        for (paramCount = 0; paramCount < Params.Input.Count; paramCount++) {
          IGH_Goo goo = Params.Input[paramCount].VolatileData.AllData(false).FirstOrDefault();
          InputManager.AddItem(paramCount, goo, RunCount);
        }

        InputManager.AddItem(paramCount++, _selectedItems, RunCount);

        if (!InputManager.IsExpired()) {
          return;
        }
      }

      SolveInternal(da);
    }

    protected abstract void SolveInternal(IGH_DataAccess da);


    protected internal abstract void InitialiseDropdowns();

    protected override void ExpireDownStreamObjects() {
      if (OutputManager == null && InputManager == null) {
        base.ExpireDownStreamObjects();
        return;
      }

      if (InputManager != null) {
        if (InputManager.IsExpired()) {
          base.ExpireDownStreamObjects();
        }

        return;
      }

      if (OutputManager != null) {
        for (int outputIndex = 0; outputIndex < Params.Output.Count; outputIndex++) {
          if (OutputManager.IsExpired(outputIndex)) {
            IGH_Param item = Params.Output[outputIndex];
            item.ExpireSolution(recompute: false);
          }
        }
      }
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

    public void SetItem<T>(IGH_DataAccess da, int outputIndex, T item) where T : IGH_Goo {
      da.SetData(outputIndex, item);

      if (OutputManager != null) {
        OutputManager.AddItem(outputIndex, item, RunCount);
      }
    }

    public void SetList<T>(IGH_DataAccess da, int outputIndex, List<T> data) where T : IGH_Goo {
      da.SetDataList(outputIndex, data);

      if (OutputManager != null) {
        OutputManager.AddList(outputIndex, data, RunCount);
      }
    }

    public void SetTree<T>(IGH_DataAccess da, int outputIndex, DataTree<T> dataTree) where T : IGH_Goo {
      da.SetDataTree(outputIndex, dataTree);

      if (OutputManager != null) {
        OutputManager.AddTree(outputIndex, dataTree, RunCount);
      }
    }
  }
}
