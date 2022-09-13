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
    public static void SetItem<T>(GH_OasysDropDownComponent owner, IGH_DataAccess DA, int inputid, T data) where T : IGH_Goo
    {
      DA.SetData(inputid, data);

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

      if (!owner.ExistingOutputsSerialized.ContainsKey(inputid))
      {
        owner.ExistingOutputsSerialized[inputid] = new List<string>() { outputsSerialized };
        owner.ExpireDownStream = true;
      }
      else if (owner.ExistingOutputsSerialized[inputid][0] != outputsSerialized)
      {
        owner.ExistingOutputsSerialized[inputid][0] = outputsSerialized;
        owner.ExpireDownStream = true;
      }
      else
        owner.ExpireDownStream = false;
    }

    public static void SetList<T>(GH_OasysDropDownComponent owner, IGH_DataAccess DA, int inputid, List<T> data) where T : IGH_Goo
    {
      DA.SetDataList(inputid, data);

      if (!owner.ExistingOutputsSerialized.ContainsKey(inputid))
      {
        owner.ExistingOutputsSerialized.Add(inputid, new List<string>());
        owner.ExpireDownStream = true;
      }

      for (int i = 0; i < data.Count; i++)
      {
        string outputsSerialized = "";
        if (data[i].GetType() == typeof(GH_UnitNumber))
        {
          IQuantity quantity = ((GH_UnitNumber)(object)data[i]).Value;
          outputsSerialized = JsonConvert.SerializeObject(quantity, converter);
        }
        else
        {
          var obj = ((T)(object)data[i]).ScriptVariable();
          try
          {
            outputsSerialized = JsonConvert.SerializeObject(obj);
          }
          catch (Exception)
          {
            outputsSerialized = data[i].GetHashCode().ToString();
          }
        }
        
        if (owner.ExistingOutputsSerialized[inputid].Count == i)
        {
          owner.ExpireDownStream = true;
          owner.ExistingOutputsSerialized[inputid].Add(outputsSerialized);
        }
        else if (owner.ExistingOutputsSerialized[inputid][i] != outputsSerialized)
        {
          owner.ExpireDownStream = true;
          owner.ExistingOutputsSerialized[inputid][i] = outputsSerialized;
        }
        else
          owner.ExpireDownStream = false;
      }
    }

    public static void SetTree<T>(GH_OasysDropDownComponent owner, IGH_DataAccess DA, int inputid, DataTree<T> dataTree) where T : IGH_Goo
    {
      DA.SetDataTree(inputid, dataTree);

      if (!owner.ExistingOutputsSerialized.ContainsKey(inputid))
      {
        owner.ExistingOutputsSerialized.Add(inputid, new List<string>());
        owner.ExpireDownStream = true;
      }

      int counter = 0;
      for (int p = 0; p < dataTree.Paths.Count; p++)
      {
        List<T> data = dataTree.Branch(dataTree.Paths[p]);
        for (int i = counter; i < data.Count - counter; i++)
        {
          string outputsSerialized = "";
          if (data[i].GetType() == typeof(GH_UnitNumber))
          {
            IQuantity quantity = ((GH_UnitNumber)(object)data[i]).Value;
            outputsSerialized = JsonConvert.SerializeObject(quantity, converter);
          }
          else
          {
            var obj = ((T)(object)data[i]).ScriptVariable();
            try
            {
              outputsSerialized = JsonConvert.SerializeObject(obj);
            }
            catch (Exception)
            {
              outputsSerialized = data[i].GetHashCode().ToString();
            }
          }

          if (owner.ExistingOutputsSerialized[inputid].Count == i)
          {
            owner.ExpireDownStream = true;
            owner.ExistingOutputsSerialized[inputid].Add(outputsSerialized);
            break;
          }

          if (owner.ExistingOutputsSerialized[inputid][i] != outputsSerialized)
          {
            owner.ExpireDownStream = true;
            owner.ExistingOutputsSerialized[inputid][i] = outputsSerialized;
            break;
          }

          owner.ExpireDownStream = false;
        }
        counter = data.Count;
      }
    }
  }
}
