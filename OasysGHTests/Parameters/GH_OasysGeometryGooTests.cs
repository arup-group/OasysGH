﻿using System;
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

        if (gooProperty.Name != "Description") {
          continue;
        }

        string description = (string)gooProperty.GetValue(objectGoo, null);
        Assert.Equal("A LineCurve example", description);
        Assert.True(description.Length > 7);
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
