using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using OasysGH;
using OasysGH.Components;
using OasysUnits;

namespace GH_UnitNumber.Components
{
  public class ConvertUnitNumber_OBSOLETE : GH_OasysDropDownComponent
  {
    #region Name and Ribbon Layout
    // This region handles how the component in displayed on the ribbon
    // including name, exposure level and icon
    public override Guid ComponentGuid => new Guid("9e7a3b43-eb15-4f2b-9023-e1582ec63ed2");
    public ConvertUnitNumber_OBSOLETE()
      : base("Convert UnitNumber", "ConvertUnit", "Convert a unit number (quantity) into another unit",
            "Params",
            "Util")
    { Hidden = true; } // sets the initial state of the component to hidden
    public override GH_Exposure Exposure => GH_Exposure.septenary | GH_Exposure.hidden;

    protected override System.Drawing.Bitmap Icon => Properties.Resources.ConvertUnitNumber;
    public override OasysPluginInfo PluginInfo => GH_UnitNumberPluginInfo.Instance;
    #endregion

    #region Input and output

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddParameter(new GH_UnitNumberParameter());
      pManager[0].Optional = true;
    }
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddParameter(new GH_UnitNumberParameter());
    }
    #endregion

    protected override void SolveInstance(IGH_DataAccess DA)
    {
      // get input
      OasysGH.Parameters.GH_UnitNumber inUnitNumber = null;

      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(0, ref gh_typ))
      {
        // try cast directly to quantity type
        if (gh_typ.Value is OasysGH.Parameters.GH_UnitNumber)
        {
          inUnitNumber = (OasysGH.Parameters.GH_UnitNumber)gh_typ.Value;
          if (ConvertedUnitNumber == null || !ConvertedUnitNumber.Value.QuantityInfo.UnitType.Equals(inUnitNumber.Value.QuantityInfo.UnitType))
          {
            UnitDictionary = new Dictionary<string, Enum>();
            foreach (UnitInfo unit in inUnitNumber.Value.QuantityInfo.UnitInfos)
              UnitDictionary.Add(unit.Name, unit.Value);

            DropDownItems[0] = UnitDictionary.Keys.ToList();
            if (!ComingFromSave)
              SelectedItems[0] = inUnitNumber.Value.Unit.ToString();
            else
              ComingFromSave = false;
          }
        }
        else
        {
          AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert UnitNumber input");
          return;
        }
      }
      else
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input a UnitNumber to populate dropdown menu");
        return;
      }

      // update selected material
      SelectedUnit = UnitDictionary[SelectedItems.Last()];

      // convert unit to selected output
      ConvertedUnitNumber = new OasysGH.Parameters.GH_UnitNumber(inUnitNumber.Value.ToUnit(SelectedUnit));

      OasysGH.Helpers.Output.SetItem(this, DA, 0, ConvertedUnitNumber);
    }

    #region Custom UI
    OasysGH.Parameters.GH_UnitNumber ConvertedUnitNumber;
    Dictionary<string, Enum> UnitDictionary;
    Enum SelectedUnit;
    bool ComingFromSave = false;

    protected override void InitialiseDropdowns()
    {
      SpacerDescriptions = new List<string>(new string[] { "Select output unit" });

      DropDownItems = new List<List<string>>();
      SelectedItems = new List<string>();

      DropDownItems.Add(new List<string>(new string[] { " " }));
      SelectedItems.Add("   ");

      IsInitialised = true;
    }

    public override void SetSelected(int i, int j)
    {
      if (UnitDictionary != null)
      {
        SelectedItems[i] = DropDownItems[i][j];
        DropDownItems[0] = UnitDictionary.Keys.ToList();
      }
      base.UpdateUI();
    }

    protected override void UpdateUIFromSelectedItems()
    {
      ComingFromSave = true;
      base.UpdateUIFromSelectedItems();
    }
    #endregion
  }
}