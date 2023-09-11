using System.Collections;
using System.Collections.Generic;
using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace OasysGH.Components {
  public abstract class GH_OasysDropDownComponent : GH_OasysComponent, IGH_VariableParameterComponent {
    public IInputParameterCacheManager InputParameterCacheManager { get; set; }
    public IOutputParameterExpirationManager OutputParameterExpirationManager { get; set; }

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
      if (InputParameterCacheManager != null) {
        InputParameterCacheManager.Reset();
      }
    }
    protected sealed override void SolveInstance(IGH_DataAccess da) {
      if (InputParameterCacheManager != null) {
        if (!InputParameterCacheManager.IsExpired()) {
          List<DataTree<IGH_Goo>> output = InputParameterCacheManager.GetOutput(1);

          for (int index = 0; index < output.Count; index++) {
            da.SetDataTree(index, output[index]);
          }

          return;
        }
      }

      SolveInternal(da);
    }

    protected override void AfterSolveInstance() {
      if (InputParameterCacheManager != null) {
        if (InputParameterCacheManager.IsExpired()) {
          var output = new List<DataTree<IGH_Goo>>();
          for (int index = 0; index < Params.Output.Count; index++) {
            var structure = (IGH_Structure)Params.Output[index].VolatileData;

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



            output.Add(tree);
          }

          InputParameterCacheManager.SetOutput(output, 1);
        }
      }

      if (OutputParameterExpirationManager != null) {
        OutputParameterExpirationManager.SetOutput(Params.Output);
      }
    }

    protected abstract void SolveInternal(IGH_DataAccess da);

    protected internal abstract void InitialiseDropdowns();

    protected override void ExpireDownStreamObjects() {
      if (InputParameterCacheManager == null && OutputParameterExpirationManager == null) {
        base.ExpireDownStreamObjects();
        return;
      }

      if (InputParameterCacheManager != null) {
        if (!InputParameterCacheManager.RunOnce) {
          InputParameterCacheManager.SetInput(Params.Input);
          InputParameterCacheManager.AddAddidionalInput(Params.Input.Count, _selectedItems);
        }

        if (!InputParameterCacheManager.IsExpired()) {
          return;
        }
      }

      if (OutputParameterExpirationManager != null) {
        for (int outputIndex = 0; outputIndex < Params.Output.Count; outputIndex++) {
          if (OutputParameterExpirationManager.IsExpired(outputIndex)) {
            IGH_Param item = Params.Output[outputIndex];
            item.ExpireSolution(recompute: false);
          }
        }
      }

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
