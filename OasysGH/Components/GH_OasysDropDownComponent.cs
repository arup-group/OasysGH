using System.Collections.Generic;
using GH_IO.Serialization;
using Grasshopper.Kernel;

namespace OasysGH.Components {
  public abstract class GH_OasysDropDownComponent : GH_OasysComponent, IGH_VariableParameterComponent,
    IExpirableComponent {
    public bool Expire { get; set; } = true;
    public List<List<string>> DropDownItems { get; protected set; }
    public bool IsInitialised { get; protected set; } = false;
    public List<string> SelectedItems { get; protected set; }
    public List<string> SpacerDescriptions { get; protected set; }

    public GH_OasysDropDownComponent(string name, string nickname, string description, string category,
      string subCategory) : base(name, nickname, description, category, subCategory) {
    }

    bool IGH_VariableParameterComponent.CanInsertParameter(GH_ParameterSide side, int index) => false;

    bool IGH_VariableParameterComponent.CanRemoveParameter(GH_ParameterSide side, int index) => false;

    public override void CreateAttributes() {
      if (!IsInitialised)
        InitialiseDropdowns();

      m_attributes =
        new UI.DropDownComponentAttributes(this, SetSelected, DropDownItems, SelectedItems, SpacerDescriptions);
    }

    IGH_Param IGH_VariableParameterComponent.CreateParameter(GH_ParameterSide side, int index) => null;

    bool IGH_VariableParameterComponent.DestroyParameter(GH_ParameterSide side, int index) => false;

    public override bool Read(GH_IReader reader) {
      List<string> selectedItems = SelectedItems;
      List<string> spacerDescriptions = SpacerDescriptions;
      List<List<string>> dropDownItems = DropDownItems;
      Helpers.DeSerialization.ReadDropDownComponents(ref reader, ref dropDownItems, ref selectedItems,
        ref spacerDescriptions);

      IsInitialised = true;
      UpdateUIFromSelectedItems();

      return base.Read(reader);
    }

    public abstract void SetSelected(int i, int j);

    public virtual void VariableParameterMaintenance() {
    }

    public override bool Write(GH_IWriter writer) {
      Helpers.DeSerialization.WriteDropDownComponents(ref writer, DropDownItems, SelectedItems, SpacerDescriptions);
      return base.Write(writer);
    }

    protected override void ExpireDownStreamObjects() {
      if (Expire) {
        base.ExpireDownStreamObjects();
      }
    }

    protected sealed override void SolveInstance(IGH_DataAccess da) {
      SolveInternal(da);
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

    internal void Unselect() {
      SelectedItems = null;
      IsInitialised = true;
    }

  }
}
