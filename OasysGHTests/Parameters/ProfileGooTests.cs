using System;
using System.Reflection;
using OasysGH.Parameters;
using Xunit;

namespace OasysGHTests.Parameters {
  [Collection("GrasshopperFixture collection")]
  public class ProfileGooTests {
    [Fact]
    public void OasysProfileGooTest() {
      Type gooType = typeof(OasysProfileGoo);

      object objectGoo = Activator.CreateInstance(gooType, null);
      gooType = objectGoo.GetType();

      bool hasName = false;
      bool hasNickName = false;
      bool hasDescription = false;

      PropertyInfo[] gooPropertyInfo
        = gooType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
      foreach (PropertyInfo gooProperty in gooPropertyInfo) {
        if (gooProperty.Name == "TypeDescription") {
          string typeDescription = (string)gooProperty.GetValue(objectGoo, null);
          Assert.NotNull(typeDescription);
        }

        if (gooProperty.Name == "Name") {
          string name = (string)gooProperty.GetValue(objectGoo, null);
          Assert.True(name.Length > 3);
          Assert.Equal("Profile", name);
          hasName = true;
        }

        if (gooProperty.Name == "NickName") {
          string nickName = (string)gooProperty.GetValue(objectGoo, null);
          Assert.Equal("Pf", nickName);
          hasNickName = true;
        }

        if (gooProperty.Name == "Description") {
          string description = (string)gooProperty.GetValue(objectGoo, null);
          Assert.StartsWith("GSA Profile", description);
          hasDescription = true;
        }
      }

      Assert.True(hasName);
      Assert.True(hasNickName);
      Assert.True(hasDescription);
    }
  }
}
