using Grasshopper.Kernel;
using OasysGH.Helpers;

namespace OasysGH.Components {
  public abstract class GH_OasysTaskCapableComponent<T> : GH_TaskCapableComponent<T>, IExpirableComponent {
    public bool Expire { get; set; } = true;
    public abstract OasysPluginInfo PluginInfo { get; }

    public GH_OasysTaskCapableComponent(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory) {
    }

    public override void AddedToDocument(GH_Document document) {
      PostHog.AddedToDocument(this, PluginInfo);
      base.AddedToDocument(document);
    }

    public override void RemovedFromDocument(GH_Document document) {
      PostHog.RemovedFromDocument(this, PluginInfo);
      base.RemovedFromDocument(document);
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
  }
}
