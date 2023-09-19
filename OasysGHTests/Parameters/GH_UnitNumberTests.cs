using System;
using System.Reflection;
using Grasshopper.Kernel.Types;
using OasysGHTests.TestHelpers;
using OasysUnits;
using OasysUnits.Units;
using Rhino.Geometry;
using Xunit;

namespace OasysGHTests.Parameters {
  [Collection("GrasshopperFixture collection")]
  public class GH_UnitNumberTests {
    [Fact]
    public void IsValidTest() {
      var goo = new OasysGH.Parameters.GH_UnitNumber(null);
      Assert.Equal("Null", goo.ToString());
      Assert.False(goo.IsValid);
      Assert.NotNull(goo.IsValidWhyNot);
      goo = new OasysGH.Parameters.GH_UnitNumber(new Length(1, LengthUnit.Meter));
      Assert.True(goo.IsValid);
      Assert.Equal(1, goo.Value.Value);
    }

    [Fact]
    public void CastFromTest() {
      var goo = new OasysGH.Parameters.GH_UnitNumber(null);
      Assert.False(goo.CastFrom(true));
      Assert.Null(goo.Value);
      Assert.True(goo.CastFrom(new Length(1, LengthUnit.Meter)));
      Assert.True(goo.IsValid);
      Assert.Equal(1, goo.Value.Value);
      var otherGoo = new OasysGH.Parameters.GH_UnitNumber(new Length(1, LengthUnit.Meter));
      Assert.True(goo.CastFrom(otherGoo));
    }

    [Fact]
    public void CastToTest() {
      var goo = new OasysGH.Parameters.GH_UnitNumber(new Length(1, LengthUnit.Meter));
      IQuantity castedQ = null;
      Assert.False(goo.CastTo(ref castedQ));
      var ghUnitNumber = new OasysGH.Parameters.GH_UnitNumber(castedQ);
      Assert.True(goo.CastTo(ref ghUnitNumber));
      Assert.True(goo.IsValid);
      Assert.Equal(1, goo.Value.Value);
      var ghNumber = new GH_Number();
      Assert.True(goo.CastTo(ref ghNumber));
      Assert.Equal(1, ghNumber.Value);
    }

    [Fact]
    public void GH_UnitNumberTest() {
      Type gooType = typeof(OasysGH.Parameters.GH_UnitNumber);
      object value = new Length(1, LengthUnit.Meter);
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
          hasValue = true;
        }

        if (gooProperty.Name == "TypeName") {
          string typeName = (string)gooProperty.GetValue(objectGoo, null);
          Assert.Equal("Quantity", typeName);
          hasToString = true;
        }

        if (gooProperty.Name == "TypeDescription") {
          string typeDescription = (string)gooProperty.GetValue(objectGoo, null);
          Assert.NotNull(typeDescription);
        }

        if (gooProperty.Name == "Name") {
          string name = (string)gooProperty.GetValue(objectGoo, null);
          Assert.True(name.Length > 3);
          Assert.Equal("UnitNumber", name);
          hasName = true;
        }

        if (gooProperty.Name == "NickName") {
          string nickName = (string)gooProperty.GetValue(objectGoo, null);
          Assert.Equal("UN", nickName);
          hasNickName = true;
        }

        if (gooProperty.Name != "Description") {
          continue;
        }

        string description = (string)gooProperty.GetValue(objectGoo, null);
        Assert.StartsWith("A value with a unit measure.", description);
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
