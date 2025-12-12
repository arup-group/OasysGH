using System;
using System.Reflection;
using Grasshopper.Kernel.Types;
using OasysGH.Parameters.Tests;
using OasysGHTests.TestHelpers;
using Rhino.Geometry;
using Xunit;

namespace OasysGHTests.Parameters {
  [Collection("GrasshopperFixture collection")]
  public class GhOasysGeometricGooTest {
    [Fact]
    public void IsValidTest() {
      var goo = new OasysGeometricGoo(null);
      Assert.False(goo.IsValid);
      Assert.Equal("Null", goo.ToString());

      goo = new OasysGeometricGoo(new LineCurve(Line.Unset));
      Assert.False(goo.IsValid);
      goo = new OasysGeometricGoo(new LineCurve(
        new Line(new Point3d(0, 0, 0), new Point3d(10, 0, 0))));
      Assert.True(goo.IsValid);
      Assert.Equal(10, goo.Value.GetLength());
    }

    [Fact]
    public void IsValidWhyNotTest() {
      var goo = new OasysGeometricGoo(new LineCurve(
        new Line(new Point3d(0, 0, 0), new Point3d(0, 0, 0))));
      Assert.Null(goo.IsValidWhyNot);
      goo = new OasysGeometricGoo(new LineCurve(
        new Line(new Point3d(0, 0, 0), new Point3d(10, 0, 0))));
      Assert.Equal(string.Empty, goo.IsValidWhyNot);
    }

    [Fact]
    public void CastFromTest() {
      var goo = new OasysGeometricGoo(new LineCurve(Line.Unset));
      Mesh m = null;
      Assert.False(goo.CastFrom(m));
      Assert.False(goo.CastFrom(true));
      Assert.True(goo.CastFrom(new LineCurve(
        new Line(new Point3d(0, 0, 0), new Point3d(10, 0, 0)))));
      Assert.True(goo.IsValid);
      Assert.Equal(10, goo.Value.GetLength());
    }

    [Fact]
    public void CastToTest() {
      var goo = new OasysGeometricGoo(new LineCurve(
        new Line(new Point3d(0, 0, 0), new Point3d(10, 0, 0))));
      Line castedLine = Line.Unset;
      Assert.False(goo.CastTo(ref castedLine));
      var lineCrv = new LineCurve();
      Assert.True(goo.CastTo(ref lineCrv));
      Assert.Equal(10, lineCrv.GetLength());
      goo = new OasysGeometricGoo(null);
      Assert.False(goo.CastTo(ref lineCrv));
    }

    [Fact]
    public void GetBoundingBoxTest() {
      var goo = new OasysGeometricGoo(new LineCurve(
        new Line(new Point3d(0, 0, 0), new Point3d(10, 10, 10))));
      var scale = Transform.Scale(new Point3d(0, 0, 0), 0.5);
      BoundingBox bbox = goo.GetBoundingBox(scale);
      Assert.Equal(5 * 5 * 5, bbox.Volume);
      goo = new OasysGeometricGoo(null);
      bbox = goo.GetBoundingBox(scale);
      Assert.Equal(BoundingBox.Empty, bbox);
    }

    [Fact]
    public void BoundingBoxTest() {
      var goo = new OasysGeometricGoo(new LineCurve(
        new Line(new Point3d(0, 0, 0), new Point3d(10, 10, 10))));
      BoundingBox bbox = goo.Boundingbox;
      Assert.Equal(10 * 10 * 10, bbox.Volume);
    }

    [Fact]
    public void ClippingBoxTest() {
      var goo = new OasysGeometricGoo(new LineCurve(
        new Line(new Point3d(0, 0, 0), new Point3d(10, 10, 10))));
      BoundingBox bbox = goo.ClippingBox;
      Assert.Equal(10 * 10 * 10, bbox.Volume);
    }


    [Fact]
    public void GH_OasysGeometricGooTest() {
      Type gooType = typeof(OasysGeometricGoo);
      Type wrapType = typeof(LineCurve);
      object value = Activator.CreateInstance(wrapType);
      object[] parameters = {
          value,
        };

      object objectGoo = Activator.CreateInstance(gooType, parameters);
      gooType = objectGoo.GetType();

      IGH_Goo duplicate = ((IGH_Goo)objectGoo).Duplicate();
      Duplicates.AreEqual(objectGoo, duplicate);

      bool hasValue = false;
      bool hasTypeName = false;
      bool hasTypeDescription = false;
      bool hasName = false;
      bool hasNickName = false;
      bool hasDescription = false;

      PropertyInfo[] gooPropertyInfo
        = gooType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
      foreach (PropertyInfo gooProperty in gooPropertyInfo) {
        if (gooProperty.Name == "Value") {
          object gooValue = gooProperty.GetValue(objectGoo, null);
          Duplicates.AreEqual(value, gooValue);

          MethodInfo methodInfo = gooValue.GetType().GetMethod("Duplicate");
          object duplicateValue = methodInfo.Invoke(gooValue, null);
          Duplicates.AreEqual(gooValue, duplicateValue);

          hasValue = true;
        }

        if (gooProperty.Name == "TypeName") {
          string typeName = (string)gooProperty.GetValue(objectGoo, null);
          Assert.StartsWith("OasysGH LineCurve", objectGoo.ToString());
          hasTypeName = true;
        }

        if (gooProperty.Name == "TypeDescription") {
          string typeDescription = (string)gooProperty.GetValue(objectGoo, null);
          Assert.Equal("OasysGH LineCurve Parameter", typeDescription);
          hasTypeDescription = true;
        }

        if (gooProperty.Name == "Name") {
          string name = (string)gooProperty.GetValue(objectGoo, null);
          Assert.Equal("LineCrv", name);
          hasName = true;
        }

        if (gooProperty.Name == "NickName") {
          string nickName = (string)gooProperty.GetValue(objectGoo, null);
          Assert.Equal("LC", nickName);
          hasNickName = true;
        }

        if (gooProperty.Name == "Description") {
          string description = (string)gooProperty.GetValue(objectGoo, null);
          Assert.Equal("A LineCurve example", description);
          Assert.True(description.Length > 7);
          hasDescription = true;
        }
      }

      Assert.True(hasValue);
      Assert.True(hasTypeName);
      Assert.True(hasTypeDescription);
      Assert.True(hasName);
      Assert.True(hasNickName);
      Assert.True(hasDescription);
    }
  }
}
