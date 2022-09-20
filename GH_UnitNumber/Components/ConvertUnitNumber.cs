using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using OasysGH.Components;
using OasysUnits;
using OasysGH;

namespace GH_UnitNumber.Components
{
  public class ConvertUnitNumber : GH_OasysDropDownComponent
  {
    #region Name and Ribbon Layout
    // This region handles how the component in displayed on the ribbon
    // including name, exposure level and icon
    public override Guid ComponentGuid => new Guid("9e7a3b43-eb15-4f2b-9023-e1582ec63ed2");
    public ConvertUnitNumber()
      : base("Convert UnitNumber", "ConvertUnit", "Convert a unit number (quantity) into another unit",
            "Params",
            "Util")
    { this.Hidden = true; } // sets the initial state of the component to hidden
    public override GH_Exposure Exposure => GH_Exposure.septenary | GH_Exposure.obscure;

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
      OasysGH.Units.GH_UnitNumber inUnitNumber = null;

      GH_ObjectWrapper gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(0, ref gh_typ))
      {
        // try cast directly to quantity type
        if (gh_typ.Value is OasysGH.Units.GH_UnitNumber)
        {
          inUnitNumber = (OasysGH.Units.GH_UnitNumber)gh_typ.Value;
          if (this.ConvertedUnitNumber == null || !this.ConvertedUnitNumber.Value.QuantityInfo.UnitType.Equals(inUnitNumber.Value.QuantityInfo.UnitType))
          {
            this.UnitDictionary = new Dictionary<string, Enum>();
            foreach (UnitInfo unit in inUnitNumber.Value.QuantityInfo.UnitInfos)
              this.UnitDictionary.Add(unit.Name, unit.Value);
            
            this.DropDownItems[0] = this.UnitDictionary.Keys.ToList();
            if (!ComingFromSave)
              this.SelectedItems[0] = inUnitNumber.Value.Unit.ToString();
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
      this.SelectedUnit = this.UnitDictionary[SelectedItems.Last()];

      // convert unit to selected output
      this.ConvertedUnitNumber = new OasysGH.Units.GH_UnitNumber(inUnitNumber.Value.ToUnit(this.SelectedUnit));

      OasysGH.Helpers.Output.SetItem(this, DA, 0, this.ConvertedUnitNumber);
    }

    #region Custom UI
    OasysGH.Units.GH_UnitNumber ConvertedUnitNumber;
    Dictionary<string, Enum> UnitDictionary;
    Enum SelectedUnit;
    bool ComingFromSave = false;

    public override void InitialiseDropdowns()
    {
      this.SpacerDescriptions = new List<string>(new string[] { "Select output unit" });

      this.DropDownItems = new List<List<string>>();
      this.SelectedItems = new List<string>();

      this.DropDownItems.Add(new List<string>(new string[] { " " }));
      this.SelectedItems.Add("   ");

      this.IsInitialised = true;
    }

    public override void SetSelected(int i, int j)
    {
      if (this.UnitDictionary != null)
      {
        this.SelectedItems[i] = this.DropDownItems[i][j];
        this.DropDownItems[0] = this.UnitDictionary.Keys.ToList();
      }
      base.UpdateUI();
    }

    public override void UpdateUIFromSelectedItems()
    {
      this.ComingFromSave = true;
      base.UpdateUIFromSelectedItems();
    }
    #endregion
  }
}
