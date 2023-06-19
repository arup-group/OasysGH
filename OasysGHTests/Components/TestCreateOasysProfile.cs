using System;
using OasysGH;
using OasysGH.Components;

namespace OasysGHTests.Components {
  internal class TestCreateOasysProfile : CreateOasysProfile {
    public override string DataSource => throw new NotImplementedException();

    public override OasysPluginInfo PluginInfo => OasysGH.PluginInfo.Instance;

    public override Guid ComponentGuid => new Guid("45c8f5aa-769a-40da-9824-026c4e7a28d0");

    public TestCreateOasysProfile(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory) {
    }

    public TestCreateOasysProfile() : base("name", "nickname", "description", "category", "subCategory") {
    }
  }
}
