using GH_IO.Serialization;
using OasysGH.Components;
using Xunit;

namespace GH_UnitNumberTests.Helpers {
  internal class Deserialize {
    internal static void TestDeserialize(GH_OasysComponent comp) {
      var write = new GH_Archive();
      Assert.True(write.AppendObject(comp, "Component"));
      string xml = write.Serialize_Xml();

      var read = new GH_Archive();
      Assert.True(read.Deserialize_Xml(xml));

      Assert.True(read.ExtractObject(comp, "Component"));
    }
  }
}
