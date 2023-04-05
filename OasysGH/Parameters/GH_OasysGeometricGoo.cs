using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace OasysGH.Parameters
{
  public abstract class GH_OasysGeometricGoo<T> : GH_GeometricGoo<T>, IGH_PreviewData
  {
    abstract public OasysPluginInfo PluginInfo { get; }

    public override string TypeName => typeof(T).Name.TrimStart('I').Replace("Gsa", string.Empty).Replace("AdSec", string.Empty);
    public override string TypeDescription => PluginInfo.ProductName + " " + TypeName + " Parameter";
    public override bool IsValid => (GetGeometry() == null) ? false : GetGeometry().IsValid ? true : false;
    public override string IsValidWhyNot
    {
      get
      {
        if (IsValid)
          return string.Empty;
        else
        {
          string whyNot = "";
          GetGeometry().IsValidWithLog(out whyNot);
          return whyNot;
        }
      }
    }

    public GH_OasysGeometricGoo(T item)
    {
      if (item == null)
        Value = item;
      else
        Value = (T)item.Duplicate();
    }

    public override string ToString()
    {
      if (Value == null)
        return "Null";
      else
        return PluginInfo.ProductName + " " + TypeName + " (" + Value.ToString() + ")";
    }

    #region casting methods
    public override bool CastTo<Q>(ref Q target)
    {
      // This function is called when Grasshopper needs to convert this 
      // instance of our custom class into some other type Q.            

      if (typeof(Q).IsAssignableFrom(typeof(T)))
      {
        if (Value == null)
          target = default;
        else
          target = (Q)(object)Value;
        return true;
      }

      target = default;
      return false;
    }

    public override bool CastFrom(object source)
    {
      // This function is called when Grasshopper needs to convert other data 
      // into our custom class.

      if (source == null)
        return false;

      //Cast from this type
      if (typeof(T).IsAssignableFrom(source.GetType()))
      {
        Value = (T)source;
        return true;
      }

      return false;
    }
    #endregion

    #region geometry
    public override IGH_GeometricGoo DuplicateGeometry()
    {
      return Duplicate();
    }
    public abstract new IGH_GeometricGoo Duplicate();

    public abstract GeometryBase GetGeometry();

    public override BoundingBox Boundingbox
    {
      get
      {
        if (!m_BoundingBox.IsValid)
        {
          m_BoundingBox = GetBoundingBox(Rhino.Geometry.Transform.ZeroTransformation);
        }
        return m_BoundingBox;
      }
    }
    private BoundingBox m_BoundingBox;
    public override BoundingBox GetBoundingBox(Transform xform)
    {
      GeometryBase geom = GetGeometry();
      if (geom == null)
        return BoundingBox.Empty;
      return geom.GetBoundingBox(Rhino.Geometry.Transform.ZeroTransformation);
    }
    #endregion

    #region transformation methods
    public abstract override IGH_GeometricGoo Morph(SpaceMorph xmorph);

    public abstract override IGH_GeometricGoo Transform(Transform xform);
    #endregion

    #region drawing methods
    public virtual BoundingBox ClippingBox
    {
      get { return Boundingbox; }
    }
    public abstract void DrawViewportMeshes(GH_PreviewMeshArgs args);

    public abstract void DrawViewportWires(GH_PreviewWireArgs args);
    #endregion
  }
}
