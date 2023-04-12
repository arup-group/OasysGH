using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;
using OasysGH;
using OasysGH.Components;
using OasysUnits;

namespace GH_UnitNumber.Components {
  public class ConvertUnitNumber : GH_OasysDropDownComponent {
    // This region handles how the component in displayed on the ribbon
    // including name, exposure level and icon
    public override Guid ComponentGuid => new Guid("267b3293-f4ac-48ab-ab66-2d194c86aa52");

    public override GH_Exposure Exposure => GH_Exposure.septenary | GH_Exposure.obscure;
    public override OasysPluginInfo PluginInfo => GH_UnitNumberPluginInfo.Instance;
    protected override System.Drawing.Bitmap Icon => Properties.Resources.ConvertUnitNumber;
    private bool _comingFromSave = false;
    private OasysGH.Parameters.GH_UnitNumber _convertedUnitNumber;
    private Enum _selectedUnit;
    private Dictionary<string, Enum> _unitDictionary;

    public ConvertUnitNumber() : base("Convert UnitNumber",
      "ConvertUnit",
      "Convert a unit number (quantity) into another unit",
      "Params",
      "Util") {
      Hidden = true; // sets the initial state of the component to hidden
    }

    public override void AppendAdditionalMenuItems(ToolStripDropDown menu) {
      Menu_AppendSeparator(menu);

      var vallist = new GH_ValueList();
      vallist.CreateAttributes();
      var panel = new GH_Panel();
      panel.CreateAttributes();

      var valueList = new ToolStripMenuItem("Create ValueList input", vallist.Icon_24x24, (s, e) => { CreateValueList(); });
      var textPanel = new ToolStripMenuItem("Create Text Panel", panel.Icon_24x24, (s, e) => { CreateTextPanel(); });

      if (_unitDictionary != null) {
        valueList.Enabled = true;
        textPanel.Enabled = true;
      }
      else {
        valueList.Enabled = false;
        textPanel.Enabled = false;
      }

      valueList.ImageScaling = ToolStripItemImageScaling.SizeToFit;
      textPanel.ImageScaling = ToolStripItemImageScaling.SizeToFit;
      menu.Items.Add(valueList);
      menu.Items.Add(textPanel);

      Menu_AppendSeparator(menu);
    }

    public void CreateTextPanel() {
      // instantiate  new panel
      var panel = new Grasshopper.Kernel.Special.GH_Panel();

      panel.CreateAttributes();

      // set the location relative to the open component on the canvas
      panel.Attributes.Pivot = new PointF((float)Attributes.DocObject.Attributes.Bounds.Left -
          panel.Attributes.Bounds.Width - 30, (float)Params.Input[0].Attributes.Pivot.Y + panel.Attributes.Bounds.Height / 2);

      string txt = "";
      foreach (string abbrName in _unitDictionary.Keys)
        txt += abbrName + "\n";
      txt = txt.TrimEnd('\n');
      panel.UserText = txt;

      Grasshopper.Instances.ActiveCanvas.Document.AddObject(panel, false);

      panel.Properties.Multiline = false;
      panel.Properties.DrawPaths = false;

      UpdateUI();
    }

    public void CreateValueList() {
      string name = _convertedUnitNumber.Value.QuantityInfo.Name + " Units";
      var values = _unitDictionary.Keys.ToList();
      float x = Attributes.Bounds.X;
      float y = Params.Input[1].Attributes.Bounds.Y;
      var vallist = new GH_ValueList();
      vallist.CreateAttributes();
      vallist.Name = name;
      vallist.NickName = name + ":";
      vallist.Description = "Select an option...";
      vallist.ListMode = GH_ValueListMode.DropDown;
      vallist.ListItems.Clear();
      foreach (KeyValuePair<string, Enum> kvp in _unitDictionary) {
        string fullName = kvp.Value.ToString();
        string abbrName = kvp.Key;
        vallist.ListItems.Add(new GH_ValueListItem(fullName, String.Format("\"{0}\"", abbrName)));
      }
      vallist.Attributes.Pivot = new PointF(x - vallist.Attributes.Bounds.Width, y);

      Grasshopper.Instances.ActiveCanvas.Document.AddObject(vallist, false);
      Params.Input[1].RemoveAllSources();
      Params.Input[1].AddSource(vallist);

      UpdateUI();
    }

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
      pManager.AddTextParameter("Unit", "u", "[Optional] desired unit to attempt to convert input into", GH_ParamAccess.item);
      pManager[1].Optional = true;
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
            foreach (UnitInfo unit in inUnitNumber.Value.QuantityInfo.UnitInfos) {
              IQuantity quantity = Quantity.From(0, unit.Value);
              string abbr = quantity.ToString().Replace("0", string.Empty).Trim();
              if (!_unitDictionary.ContainsKey(abbr))
                _unitDictionary.Add(abbr, unit.Value);
            }

            _dropDownItems[0] = _unitDictionary.Keys.ToList();
            if (!_comingFromSave) {
              IQuantity quantity = Quantity.From(0, inUnitNumber.Value.Unit);
              string abbr = quantity.ToString().Replace("0", string.Empty).Trim();
              _selectedItems[0] = abbr;
            }
            else
              _comingFromSave = false;
          }
        }
        else {
          AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert UnitNumber input");
          return;
        }
      }
      else {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input a UnitNumber to populate dropdown menu");
        return;
      }

      string unitTxt = "";
      if (DA.GetData(1, ref unitTxt)) {
        if (!char.IsNumber(unitTxt[0]))
          unitTxt = "0" + unitTxt;
        Type type = inUnitNumber.Value.QuantityInfo.ValueType;
        if (Quantity.TryParse(type, unitTxt, out IQuantity quantity)) {
          _selectedUnit = quantity.Unit;
          IQuantity quantity2 = Quantity.From(0, _selectedUnit);
          string abbr = quantity2.ToString().Replace("0", string.Empty).Trim();
          _selectedItems[0] = abbr;
        }
        else {
          AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to parse input parameter u to a recognisable unit");
          return;
        }
      }
      else {
        // update selected unit from dropdown
        _selectedUnit = _unitDictionary[_selectedItems.Last()];
      }

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
