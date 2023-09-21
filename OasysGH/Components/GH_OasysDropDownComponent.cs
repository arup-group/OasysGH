using System.Collections;
using System.Collections.Generic;
using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using OasysGH.Components.Utility;

namespace OasysGH.Components {
  public abstract class GH_OasysDropDownComponent : GH_OasysComponent, IGH_VariableParameterComponent {
    public IInputParameterCacheManager InputParameterCacheManager { get; set; }

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

        if (!InputParameterCacheManager.RunOnce) {
          InputParameterCacheManager.SetInput(Params.Input);
          InputParameterCacheManager.AddAddidionalInput(Params.Input.Count, _selectedItems);
        }
      }
    }

    protected sealed override void SolveInstance(IGH_DataAccess da) {
      if (InputParameterCacheManager != null) {
        if (!InputParameterCacheManager.IsExpired()) {
          if (RunCount == 1) {
            List<DataTree<IGH_Goo>> output = InputParameterCacheManager.GetOutput(1);
            for (int index = 0; index < output.Count; index++) {
              da.SetDataTree(index, output[index]);
            }
          }

          return;
        }

        // AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "IsExpired");
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

                list2.Add((IGH_Goo)list[j]);
              }

              tree.AddRange(list2, structure.get_Path(i));
            }

            output.Add(tree);
          }

          InputParameterCacheManager.SetOutput(output, 1);
        }
      }
    }

    protected abstract void SolveInternal(IGH_DataAccess da);

    protected internal abstract void InitialiseDropdowns();

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
