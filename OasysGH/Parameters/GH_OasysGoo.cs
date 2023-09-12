using Grasshopper.Kernel.Types;

namespace OasysGH.Parameters {
  public abstract class GH_OasysGoo<T> : GH_Goo<T> {
    public override bool IsValid => Value != null;
    public override string IsValidWhyNot {
      get {
        if (IsValid)
          return string.Empty;
        else
          return IsValid.ToString();
      }
    }

    public abstract OasysPluginInfo PluginInfo { get; }

    public override string TypeDescription => PluginInfo.ProductName + " " + TypeName + " Parameter";
    public override string TypeName => typeof(T).Name.TrimStart('I').Replace("Gsa", string.Empty).Replace("AdSec", string.Empty);

    public GH_OasysGoo() : base() { }

    public GH_OasysGoo(T item) => Value = item;

    public override bool CastFrom(object source) {
      if (source == null) {
        return false;
      }

      if (typeof(T).IsAssignableFrom(source.GetType())) {
        Value = (T)source;
        return true;
      }

      return false;
    }

    public override bool CastTo<Q>(ref Q target) {
      if (typeof(Q).IsAssignableFrom(typeof(T))) {
        if (Value == null) {
          target = default;
        }
        else {
          target = (Q)(object)Value;
        }

        return true;
      }

      target = default;
      return false;
    }

    public override string ToString() {
      if (Value == null) {
        return "Null";
      }
      else {
        return PluginInfo.ProductName + " " + TypeName + " (" + Value.ToString() + ")";
      }
    }
  }
}
