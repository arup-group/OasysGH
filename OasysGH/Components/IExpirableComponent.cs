using Grasshopper.Kernel;

namespace OasysGH.Components {
  public interface IExpirableComponent : IGH_DocumentObject {
    bool Expire { get; set; }
  }
}
