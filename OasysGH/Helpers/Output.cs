using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using OasysGH.Components;

namespace OasysGH.Helpers {
  public class Output {
    public static void SetItem<T>(GH_OasysDropDownComponent owner, IGH_DataAccess DA, int outputIndex, T data) where T : IGH_Goo {
      DA.SetData(outputIndex, data);
      owner.OutputChanged(data, outputIndex, 0);
    }

    public static void SetList<T>(GH_OasysDropDownComponent owner, IGH_DataAccess DA, int outputIndex, List<T> data) where T : IGH_Goo {
      DA.SetDataList(outputIndex, data);
      for (int i = 0; i < data.Count; i++)
        owner.OutputChanged(data[i], outputIndex, i);
    }

    public static void SetTree<T>(GH_OasysDropDownComponent owner, IGH_DataAccess DA, int outputIndex, DataTree<T> dataTree) where T : IGH_Goo {
      DA.SetDataTree(outputIndex, dataTree);
      int counter = 0;
      for (int p = 0; p < dataTree.Paths.Count; p++) {
        List<T> data = dataTree.Branch(dataTree.Paths[p]);
        for (int i = counter; i < data.Count - counter; i++)
          owner.OutputChanged(data[i], outputIndex, i);
        counter = data.Count;
      }
    }
  }
}
