using System;
using System.Reflection;
using Grasshopper.Kernel.Types;
using OasysGH.Parameters.Tests;
using OasysGHTests.TestHelpers;
using Rhino.Geometry;
using Xunit;

namespace OasysGHTests.Parameters {
  [Collection("GrasshopperFixture collection")]
  public class GhOasysGooTest {
    [Fact]
    public void IsValidTest() {
      var goo = new OasysGoo(null);
      Assert.Equal("Null", goo.ToString());
      Assert.False(goo.IsValid);
      Assert.NotNull(goo.IsValidWhyNot);
      goo = new OasysGoo(new GH_Boolean(true));
      Assert.True(goo.IsValid);
      Assert.True(goo.Value.Value);
    }

    [Fact]
    public void CastFromTest() {
      var goo = new OasysGoo(null);
      Assert.False(goo.CastFrom(true));
      Assert.Null(goo.Value);
      Assert.True(goo.CastFrom(new GH_Boolean(true)));
      Assert.True(goo.Value.Value);
      Mesh m = null;
      Assert.False(goo.CastFrom(m));
    }

    [Fact]
    public void CastToTest() {
      var goo = new OasysGoo(new GH_Boolean(true));
      bool castedBool = false;
      Assert.False(goo.CastTo(ref castedBool));
      var ghBool = new GH_Boolean(false);
      Assert.True(goo.CastTo(ref ghBool));
      Assert.True(ghBool.Value);
    }

    [Fact]
    public void GH_OasysGooTest() {
      Type gooType = typeof(OasysGoo);
      Type wrapType = typeof(GH_Boolean);
      object value = Activator.CreateInstance(wrapType);
      object[] parameters = {
          value,
        };

      object objectGoo = Activator.CreateInstance(gooType, parameters);
      gooType = objectGoo.GetType();

      IGH_Goo duplicate = ((IGH_Goo)objectGoo).Duplicate();
      Duplicates.AreEqual(objectGoo, duplicate);

      bool hasValue = false;
      bool hasToString = false;
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
          Assert.StartsWith("OasysGH " + typeName + " (", objectGoo.ToString());
          hasToString = true;
        }

        if (gooProperty.Name == "TypeDescription ") {
          string typeDescription = (string)gooProperty.GetValue(objectGoo, null);
          Assert.NotNull(typeDescription);
        }

        if (gooProperty.Name == "Name") {
          string name = (string)gooProperty.GetValue(objectGoo, null);
          Assert.True(name.Length > 3);
          Assert.Equal("Boolean", name);
          hasName = true;
        }

        if (gooProperty.Name == "NickName") {
          string nickName = (string)gooProperty.GetValue(objectGoo, null);
          Assert.Equal("B", nickName);
          hasNickName = true;
        }

        if (gooProperty.Name != "Description") {
          continue;
        }

        string description = (string)gooProperty.GetValue(objectGoo, null);
        Assert.Equal("A boolean example", description);
        hasDescription = true;
      }

      Assert.True(hasValue);
      Assert.True(hasToString);
      Assert.True(hasName);
      Assert.True(hasNickName);
      Assert.True(hasDescription);
    }
  }
}
