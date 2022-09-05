using Grasshopper;
using Grasshopper.Kernel;
using System.Collections.Generic;
using OasysGH.Components;
//using Newtonsoft.Json;
//using UnitsNet.GH;
//using UnitsNet.Serialization.JsonNet;

namespace OasysGH.Helpers
{
  public class Output
  {
    //private static UnitsNetIQuantityJsonConverter converter = new UnitsNetIQuantityJsonConverter();
    public static void SetItem(GH_OasysDropDownComponent owner, IGH_DataAccess DA, int inputid, object data)
    {
      DA.SetData(inputid, data);

      int outputsSerialized = data.GetHashCode(); //JsonConvert.SerializeObject(data, converter).GetHashCode();

      if (!owner.ExistingOutputsSerialized.ContainsKey(inputid))
      {
        owner.ExistingOutputsSerialized[inputid] = new List<int>() { outputsSerialized };
        owner.UpdateOutput = true;
      }
      else if (owner.ExistingOutputsSerialized[inputid][0] != outputsSerialized)
      {
        owner.ExistingOutputsSerialized[inputid][0] = outputsSerialized;
        owner.UpdateOutput = true;
      }
      else
        owner.UpdateOutput = false;
    }
    public static void SetList<GH_Goo>(GH_OasysDropDownComponent owner, IGH_DataAccess DA, int inputid, List<GH_Goo> data)
    {
      DA.SetDataList(inputid, data);

      if (!owner.ExistingOutputsSerialized.ContainsKey(inputid))
      {
        owner.ExistingOutputsSerialized.Add(inputid, new List<int>());
        owner.UpdateOutput = true;
      }

      for (int i = 0; i < data.Count; i++)
      {
        int outputsSerialized = data[i].GetHashCode(); //JsonConvert.SerializeObject(data[i], converter).GetHashCode();
        if (owner.ExistingOutputsSerialized[inputid].Count == i)
        {
          owner.UpdateOutput = true;
          owner.ExistingOutputsSerialized[inputid].Add(outputsSerialized);
        }
        else if (owner.ExistingOutputsSerialized[inputid][i] != outputsSerialized)
        {
          owner.UpdateOutput = true;
          owner.ExistingOutputsSerialized[inputid][i] = outputsSerialized;
        }
        else
          owner.UpdateOutput = false;
      }

    }

    public static void SetTree<T>(GH_OasysDropDownComponent owner, IGH_DataAccess DA, int inputid, DataTree<T> dataTree)
    {
      DA.SetDataTree(inputid, dataTree);

      if (!owner.ExistingOutputsSerialized.ContainsKey(inputid))
      {
        owner.ExistingOutputsSerialized.Add(inputid, new List<int>());
        owner.UpdateOutput = true;
      }

      int counter = 0;
      for (int p = 0; p < dataTree.Paths.Count; p++)
      {
        List<T> data = dataTree.Branch(dataTree.Paths[p]);
        for (int i = counter; i < data.Count - counter; i++)
        {
          // bug in JsonConvert: System.ArrayTypeMismatchException: 'Attempted to access an element as a type incompatible with the array.'
          // using GetHashCode but not sure if it is unique enough?
          int outputsSerialized = data[i].GetHashCode(); //JsonConvert.SerializeObject(data[i], converter).GetHashCode();
          if (owner.ExistingOutputsSerialized[inputid].Count == i)
          {
            owner.UpdateOutput = true;
            owner.ExistingOutputsSerialized[inputid].Add(outputsSerialized);
            break;
          }

          if (owner.ExistingOutputsSerialized[inputid][i] != outputsSerialized)
          {
            owner.UpdateOutput = true;
            owner.ExistingOutputsSerialized[inputid][i] = outputsSerialized;
            break;
          }

          owner.UpdateOutput = false;
        }
        counter = data.Count;
      }
    }
  }
}
