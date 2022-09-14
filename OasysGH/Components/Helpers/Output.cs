using Grasshopper;
using Grasshopper.Kernel;
using System.Collections.Generic;
using OasysGH.Components;
using Newtonsoft.Json;
using OasysGH.Units;
using UnitsNet.Serialization.JsonNet;
using UnitsNet;
using System;
using Grasshopper.Kernel.Types;

namespace OasysGH.Helpers
{
  public class Output
  {
    private static UnitsNetIQuantityJsonConverter converter = new UnitsNetIQuantityJsonConverter();
    public static void SetItem<T>(GH_OasysDropDownComponent owner, IGH_DataAccess DA, int outputIndex, T data) where T : IGH_Goo
    {
      DA.SetData(outputIndex, data);

      CheckIfDataIsUpdated(owner, data, outputIndex, 0);
    }

    public static void SetList<T>(GH_OasysDropDownComponent owner, IGH_DataAccess DA, int outputIndex, List<T> data) where T : IGH_Goo
    {
      DA.SetDataList(outputIndex, data);

      for (int i = 0; i < data.Count; i++)
        CheckIfDataIsUpdated(owner, data[i], outputIndex, i);
    }

    public static void SetTree<T>(GH_OasysDropDownComponent owner, IGH_DataAccess DA, int outputIndex, DataTree<T> dataTree) where T : IGH_Goo
    {
      DA.SetDataTree(outputIndex, dataTree);

      int counter = 0;
      for (int p = 0; p < dataTree.Paths.Count; p++)
      {
        List<T> data = dataTree.Branch(dataTree.Paths[p]);
        for (int i = counter; i < data.Count - counter; i++)
          CheckIfDataIsUpdated(owner, data[i], outputIndex, i);
        
        counter = data.Count;
      }
    }

    private static void CheckIfDataIsUpdated<T>(GH_OasysDropDownComponent owner, T data, int outputIndex, int index) where T : IGH_Goo
    {
      if (!owner.ExistingOutputsSerialized.ContainsKey(outputIndex))
      {
        owner.ExistingOutputsSerialized.Add(outputIndex, new List<string>());
        owner.ExpireDownStream = true;
      }

      string outputsSerialized = "";
      if (data.GetType() == typeof(GH_UnitNumber))
      {
        IQuantity quantity = ((GH_UnitNumber)(object)data).Value;
        outputsSerialized = JsonConvert.SerializeObject(quantity, converter);
      }
      else
      {
        var obj = ((T)(object)data).ScriptVariable();
        try
        {
          outputsSerialized = JsonConvert.SerializeObject(obj);
        }
        catch (Exception)
        {
          outputsSerialized = data.GetHashCode().ToString();
        }
      }

      if (owner.ExistingOutputsSerialized[outputIndex].Count == index)
      {
        owner.ExpireDownStream = true;
        owner.ExistingOutputsSerialized[outputIndex].Add(outputsSerialized);
        return;
      }

      if (owner.ExistingOutputsSerialized[outputIndex][index] != outputsSerialized)
      {
        owner.ExpireDownStream = true;
        owner.ExistingOutputsSerialized[outputIndex][index] = outputsSerialized;
        return;
      }

      owner.ExpireDownStream = false;
    }
  }
}
