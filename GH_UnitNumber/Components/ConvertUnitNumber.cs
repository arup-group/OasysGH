using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using OasysGH;
using OasysGH.Components;
using OasysUnits;
using static System.Net.Mime.MediaTypeNames;
using Grasshopper.Kernel.Special;
using System.Drawing;
using OasysGH.Units.Helpers;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace GH_UnitNumber.Components
{
  public class ConvertUnitNumber : GH_OasysDropDownComponent
  {
    #region Name and Ribbon Layout
    // This region handles how the component in displayed on the ribbon
    // including name, exposure level and icon
    public override Guid ComponentGuid => new Guid("267b3293-f4ac-48ab-ab66-2d194c86aa52");
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
      pManager.AddTextParameter("Unit", "u", "[Optional] desired unit to attempt to convert input into", GH_ParamAccess.item);
      pManager[1].Optional = true;
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
          if (this.ConvertedUnitNumber == null || !this.ConvertedUnitNumber.Value.QuantityInfo.UnitType.Equals(inUnitNumber.Value.QuantityInfo.UnitType))
          {
            this.UnitDictionary = new Dictionary<string, Enum>();
            foreach (UnitInfo unit in inUnitNumber.Value.QuantityInfo.UnitInfos)
            {
              IQuantity quantity = Quantity.From(0, unit.Value);
              string abbr = quantity.ToString().Replace("0", string.Empty).Trim();
              if (!this.UnitDictionary.ContainsKey(abbr))
                this.UnitDictionary.Add(abbr, unit.Value);
            }

            this.DropDownItems[0] = this.UnitDictionary.Keys.ToList();
            if (!ComingFromSave)
            {
              IQuantity quantity = Quantity.From(0, inUnitNumber.Value.Unit);
              string abbr = quantity.ToString().Replace("0", string.Empty).Trim();
              this.SelectedItems[0] = abbr;
            }
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

      string unitTxt = "";
      if (DA.GetData(1, ref unitTxt))
      {
        if (!char.IsNumber(unitTxt[0]))
          unitTxt = "0" + unitTxt;
        Type type = inUnitNumber.Value.QuantityInfo.ValueType;
        if (Quantity.TryParse(type, unitTxt, out IQuantity quantity))
        {
          this.SelectedUnit = quantity.Unit;
          IQuantity quantity2 = Quantity.From(0, this.SelectedUnit);
          string abbr = quantity2.ToString().Replace("0", string.Empty).Trim();
          this.SelectedItems[0] = abbr;
        }
        else
        {
          AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to parse input parameter u to a recognisable unit");
          return;
        }
      }
      else
      {
        // update selected unit from dropdown
        this.SelectedUnit = this.UnitDictionary[SelectedItems.Last()];
      }

      // convert unit to selected output
      this.ConvertedUnitNumber = new OasysGH.Parameters.GH_UnitNumber(inUnitNumber.Value.ToUnit(this.SelectedUnit));

      OasysGH.Helpers.Output.SetItem(this, DA, 0, this.ConvertedUnitNumber);
    }

    #region Custom UI
    OasysGH.Parameters.GH_UnitNumber ConvertedUnitNumber;
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

    public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
    {
      Menu_AppendSeparator(menu);

      var vallist = new GH_ValueList();
      vallist.CreateAttributes();
      var panel = new GH_Panel();
      panel.CreateAttributes();

      ToolStripMenuItem valueList = new ToolStripMenuItem("Create ValueList input", vallist.Icon_24x24, (s, e) => { CreateValueList(); });
      ToolStripMenuItem textPanel = new ToolStripMenuItem("Create Text Panel", panel.Icon_24x24, (s, e) => { CreateTextPanel(); });

      if (this.UnitDictionary != null)
      {
        valueList.Enabled = true;
        textPanel.Enabled = true;
      }
      else
      {
        valueList.Enabled = false;
        textPanel.Enabled = false;
      }

      valueList.ImageScaling = ToolStripItemImageScaling.SizeToFit;
      textPanel.ImageScaling = ToolStripItemImageScaling.SizeToFit;
      menu.Items.Add(valueList);
      menu.Items.Add(textPanel);

      Menu_AppendSeparator(menu);
    }

    public void CreateValueList()
    {
      string name = this.ConvertedUnitNumber.Value.QuantityInfo.Name + " Units";
      List<string> values = this.UnitDictionary.Keys.ToList();
      float x = this.Attributes.Bounds.X;
      float y = this.Params.Input[1].Attributes.Bounds.Y;
      var vallist = new GH_ValueList();
      vallist.CreateAttributes();
      vallist.Name = name;
      vallist.NickName = name + ":";
      vallist.Description = "Select an option...";
      vallist.ListMode = GH_ValueListMode.DropDown;
      vallist.ListItems.Clear();
      foreach (KeyValuePair<string, Enum> kvp in this.UnitDictionary)
      {
        string fullName = kvp.Value.ToString();
        string abbrName = kvp.Key;
        vallist.ListItems.Add(new GH_ValueListItem(fullName, String.Format("\"{0}\"", abbrName)));
      }
      vallist.Attributes.Pivot = new PointF(x - vallist.Attributes.Bounds.Width, y);
      
      Grasshopper.Instances.ActiveCanvas.Document.AddObject(vallist, false);
      this.Params.Input[1].RemoveAllSources();
      this.Params.Input[1].AddSource(vallist);

      this.UpdateUI();
    }

    public void CreateTextPanel()
    {
      // instantiate  new panel
      var panel = new Grasshopper.Kernel.Special.GH_Panel();

      panel.CreateAttributes();
      
      // set the location relative to the open component on the canvas
      panel.Attributes.Pivot = new PointF((float)Attributes.DocObject.Attributes.Bounds.Left -
          panel.Attributes.Bounds.Width - 30, (float)Params.Input[0].Attributes.Pivot.Y + panel.Attributes.Bounds.Height / 2);

      string txt = "";
      foreach (string abbrName in this.UnitDictionary.Keys)
        txt += abbrName + "\n";
      txt = txt.TrimEnd('\n');
      panel.UserText = txt;
      
      Grasshopper.Instances.ActiveCanvas.Document.AddObject(panel, false);
      
      panel.Properties.Multiline = false;
      panel.Properties.DrawPaths = false;
      
      this.UpdateUI();
    }
    #endregion
  }
}
