﻿using Grasshopper.Kernel.Types;
using OasysGH.Components.TestComponents;
using Xunit;

namespace OasysGHTests.Components {
  [Collection("GrasshopperFixture collection")]
  public class SliderComponentTests {
    [Fact]
    public static void ChangeSlider() {
      var comp = new SliderComponent();
      comp.CreateAttributes();
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      var initialValue = (GH_Number)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.Equal(500, initialValue.Value);
      var initialMax = (GH_Number)comp.Params.Output[1].VolatileData.get_Branch(0)[0];
      Assert.Equal(1000, initialMax.Value);
      var initialMin = (GH_Number)comp.Params.Output[2].VolatileData.get_Branch(0)[0];
      Assert.Equal(-250, initialMin.Value);

      double newValue = 750;
      comp.SetVal(newValue);
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      var newValueOut = (GH_Number)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.Equal(newValue, newValueOut.Value);

      double max = 1500;
      double min = 0;
      comp.SetMaxMin(max, min);
      comp.ExpireSolution(true);
      comp.Params.Output[0].CollectData();
      newValueOut = (GH_Number)comp.Params.Output[0].VolatileData.get_Branch(0)[0];
      Assert.Equal(newValue, newValueOut.Value);
      var newMax = (GH_Number)comp.Params.Output[1].VolatileData.get_Branch(0)[0];
      Assert.Equal(max, newMax.Value);
      var newMin = (GH_Number)comp.Params.Output[2].VolatileData.get_Branch(0)[0];
      Assert.Equal(min, newMin.Value);
    }
  }
}