﻿using System.Collections.Generic;
using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace OasysGH.Components {
  public abstract class GH_OasysDropDownComponent : GH_OasysComponent, IGH_VariableParameterComponent {
    public IParameterCacheManager ParameterCacheManager { get; set; }

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

    protected override void BeforeSolveInstance() {
      if (ParameterCacheManager != null) {
        ParameterCacheManager.SetInput(Params.Input);
        ParameterCacheManager.AddAddidionalInput(6, _selectedItems);
      }
    }

    protected sealed override void SolveInstance(IGH_DataAccess da) {
      if (ParameterCacheManager != null) {
        if (!ParameterCacheManager.InputManager.IsExpired()) {
          List<DataTree<IGH_Goo>> output = ParameterCacheManager.GetOutput(RunCount);

          for (int index = 0; index < output.Count; index++) {
            da.SetDataTree(index, output[index]);
          }

          return;
        }
      }

      SolveInternal(da);
    }

    protected override void AfterSolveInstance() {
      if (ParameterCacheManager == null) {
        return;
      }

      if (ParameterCacheManager.InputManager.IsExpired()) {
        var output = new List<DataTree<IGH_Goo>>();
        for (int index = 0; index < Params.Output.Count; index++) {
          var structure = (IGH_Structure)Params.Output[index].VolatileData;

          var tree = new DataTree<IGH_Goo>();
          tree.MergeStructure(structure, null);

          output.Add(tree);
        }

        ParameterCacheManager.SetOutput(output, RunCount);
      }
    }

    protected abstract void SolveInternal(IGH_DataAccess da);

    protected internal abstract void InitialiseDropdowns();

    protected override void ExpireDownStreamObjects() {
      if (ParameterCacheManager == null) {
        base.ExpireDownStreamObjects();
        return;
      }

      if (!ParameterCacheManager.InputManager.IsExpired()) {
        return;
      }

      //for (int outputIndex = 0; outputIndex < Params.Output.Count; outputIndex++) {
      //  if (ParameterCacheManager.OutputManager.IsExpired(outputIndex)) {
      //    IGH_Param item = Params.Output[outputIndex];
      //    item.ExpireSolution(recompute: false);
      //  }
      //}

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
  }
}
