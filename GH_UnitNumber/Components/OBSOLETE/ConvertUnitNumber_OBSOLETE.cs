using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using OasysGH;
using OasysGH.Components;
using OasysUnits;

namespace GH_UnitNumber.Components {
  public class ConvertUnitNumber_OBSOLETE : GH_OasysDropDownComponent {
    // This region handles how the component in displayed on the ribbon
    // including name, exposure level and icon
    public override Guid ComponentGuid => new Guid("9e7a3b43-eb15-4f2b-9023-e1582ec63ed2");

    public override GH_Exposure Exposure => GH_Exposure.septenary | GH_Exposure.hidden;
    public override OasysPluginInfo PluginInfo => GH_UnitNumberPluginInfo.Instance;
    protected override System.Drawing.Bitmap Icon => Properties.Resources.ConvertUnitNumber;
    private bool _comingFromSave = false;
    private OasysGH.Parameters.GH_UnitNumber _convertedUnitNumber;
    private Enum _selectedUnit;
    private Dictionary<string, Enum> _unitDictionary;

    public ConvertUnitNumber_OBSOLETE()
                                  : base("Convert UnitNumber", "ConvertUnit", "Convert a unit number (quantity) into another unit",
            "Params",
            "Util") { Hidden = true; } // sets the initial state of the component to hidden

    public override void SetSelected(int i, int j) {
      if (_unitDictionary != null) {
        _selectedItems[i] = _dropDownItems[i][j];
        _dropDownItems[0] = _unitDictionary.Keys.ToList();
      }
      base.UpdateUI();
    }

    protected override void InitialiseDropdowns() {
      _spacerDescriptions = new List<string>(new string[] { "Select output unit" });

      _dropDownItems = new List<List<string>>();
      _selectedItems = new List<string>();

      _dropDownItems.Add(new List<string>(new string[] { " " }));
      _selectedItems.Add("   ");

      _isInitialised = true;
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager) {
      pManager.AddParameter(new GH_UnitNumberParameter());
      pManager[0].Optional = true;
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddParameter(new GH_UnitNumberParameter());
    }

    protected override void SolveInstance(IGH_DataAccess DA) {
      // get input
      OasysGH.Parameters.GH_UnitNumber inUnitNumber = null;

      var gh_typ = new GH_ObjectWrapper();
      if (DA.GetData(0, ref gh_typ)) {
        // try cast directly to quantity type
        if (gh_typ.Value is OasysGH.Parameters.GH_UnitNumber) {
          inUnitNumber = (OasysGH.Parameters.GH_UnitNumber)gh_typ.Value;
          if (_convertedUnitNumber == null || !_convertedUnitNumber.Value.QuantityInfo.UnitType.Equals(inUnitNumber.Value.QuantityInfo.UnitType)) {
            _unitDictionary = new Dictionary<string, Enum>();
            foreach (UnitInfo unit in inUnitNumber.Value.QuantityInfo.UnitInfos)
              _unitDictionary.Add(unit.Name, unit.Value);

            _dropDownItems[0] = _unitDictionary.Keys.ToList();
            if (!_comingFromSave)
              _selectedItems[0] = inUnitNumber.Value.Unit.ToString();
            else
              _comingFromSave = false;
          }
        } else {
          AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert UnitNumber input");
          return;
        }
      } else {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input a UnitNumber to populate dropdown menu");
        return;
      }

      // update selected material
      _selectedUnit = _unitDictionary[_selectedItems.Last()];

      // convert unit to selected output
      _convertedUnitNumber = new OasysGH.Parameters.GH_UnitNumber(inUnitNumber.Value.ToUnit(_selectedUnit));

      OasysGH.Helpers.Output.SetItem(this, DA, 0, _convertedUnitNumber);
    }

    protected override void UpdateUIFromSelectedItems() {
      _comingFromSave = true;
      base.UpdateUIFromSelectedItems();
    }
  }
}
