using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Oasys.Taxonomy.Geometry;
using Oasys.Taxonomy.Profiles;
using OasysGH.Helpers;
using OasysGH.Parameters;
using OasysGH.Units;
using OasysGH.Units.Helpers;
using OasysUnits;
using OasysUnits.Units;
using Rhino.Geometry;

namespace OasysGH.Components {
  public abstract class CreateOasysProfile : GH_OasysDropDownComponent {
    protected enum FoldMode {
      Catalogue,
      Other
    }

    public abstract string DataSource { get; }

    protected LengthUnit _lengthUnit = DefaultUnits.LengthUnitSection;

    protected FoldMode _mode = FoldMode.Other;

    // list of sections as outcome from selections
    protected List<string> _profileDescriptions = new List<string>() { "CAT HE HE200.B" };

    private static readonly Dictionary<string, Type> profileTypes = new Dictionary<string, Type> {
      { "Angle", typeof(IAngleProfile) },
      { "Catalogue", typeof(ICatalogueProfile) },
      { "Channel", typeof(IChannelProfile) },
      { "Circle Hollow", typeof(ICircleHollowProfile) },
      { "Circle", typeof(ICircleProfile) },
      { "Cruciform Symmetrical", typeof(ICruciformSymmetricalProfile) },
      { "Ellipse Hollow", typeof(IEllipseHollowProfile) },
      { "Ellipse", typeof(IEllipseProfile) },
      { "General C", typeof(IGeneralCProfile) },
      { "General Z", typeof(IGeneralZProfile) },
      { "I Beam Asymmetrical", typeof(IIBeamAsymmetricalProfile) },
      { "I Beam Cellular", typeof(IIBeamCellularProfile) },
      { "I Beam Symmetrical", typeof(IIBeamProfile) },
      { "Perimeter", typeof(IPerimeterProfile) },
      { "Rectangle Hollow", typeof(IRectangleHollowProfile) },
      { "Rectangle", typeof(IRectangleProfile) },
      { "Recto Ellipse", typeof(IRectoEllipseProfile) },
      { "Recto Circle", typeof(IRectoCircleProfile) },
      { "Secant Pile", typeof(ISecantPileProfile) },
      { "Sheet Pile", typeof(ISheetPileProfile) },
      { "Trapezoid", typeof(ITrapezoidProfile) },
      { "T Section", typeof(ITSectionProfile) },
    };

    private static readonly List<string> easterCat = new List<string>() {
      "▌─────────────────────────▐█─────▐" + Environment.NewLine +
      "▌────▄──────────────────▄█▓█▌────▐" + Environment.NewLine +
      "▌───▐██▄───────────────▄▓░░▓▓────▐" + Environment.NewLine +
      "▌───▐█░██▓────────────▓▓░░░▓▌────▐" + Environment.NewLine +
      "▌───▐█▌░▓██──────────█▓░░░░▓─────▐" + Environment.NewLine +
      "▌────▓█▌░░▓█▄███████▄███▓░▓█─────▐" + Environment.NewLine +
      "▌────▓██▌░▓██░░░░░░░░░░▓█░▓▌─────▐" + Environment.NewLine +
      "▌─────▓█████░░░░░░░░░░░░▓██──────▐" + Environment.NewLine +
      "▌─────▓██▓░░░░░░░░░░░░░░░▓█──────▐" + Environment.NewLine +
      "▌─────▐█▓░░░░░░█▓░░▓█░░░░▓█▌─────▐" + Environment.NewLine +
      "▌─────▓█▌░▓█▓▓██▓░█▓▓▓▓▓░▓█▌─────▐" + Environment.NewLine +
      "▌─────▓▓░▓██████▓░▓███▓▓▌░█▓─────▐" + Environment.NewLine +
      "▌────▐▓▓░█▄▐▓▌█▓░░▓█▐▓▌▄▓░██─────▐" + Environment.NewLine +
      "▌────▓█▓░▓█▄▄▄█▓░░▓█▄▄▄█▓░██▌────▐" + Environment.NewLine +
      "▌────▓█▌░▓█████▓░░░▓███▓▀░▓█▓────▐" + Environment.NewLine +
      "▌───▐▓█░░░▀▓██▀░░░░░─▀▓▀░░▓█▓────▐" + Environment.NewLine +
      "▌───▓██░░░░░░░░▀▄▄▄▄▀░░░░░░▓▓────▐" + Environment.NewLine +
      "▌───▓█▌░░░░░░░░░░▐▌░░░░░░░░▓▓▌───▐" + Environment.NewLine +
      "▌───▓█░░░░░░░░░▄▀▀▀▀▄░░░░░░░█▓───▐" + Environment.NewLine +
      "▌──▐█▌░░░░░░░░▀░░░░░░▀░░░░░░█▓▌──▐" + Environment.NewLine +
      "▌──▓█░░░░░░░░░░░░░░░░░░░░░░░██▓──▐" + Environment.NewLine +
      "▌──▓█░░░░░░░░░░░░░░░░░░░░░░░▓█▓──▐" + Environment.NewLine +
      "▌──██░░░░░░░░░░░░░░░░░░░░░░░░█▓──▐" + Environment.NewLine +
      "▌──█▌░░░░░░░░░░░░░░░░░░░░░░░░▐▓▌─▐" + Environment.NewLine +
      "▌─▐▓░░░░░░░░░░░░░░░░░░░░░░░░░░█▓─▐" + Environment.NewLine +
      "▌─█▓░░░░░░░░░░░░░░░░░░░░░░░░░░▓▓─▐" + Environment.NewLine +
      "▌─█▓░░░░░░░░░░░░░░░░░░░░░░░░░░▓▓▌▐" + Environment.NewLine +
      "▌▐█▓░░░░░░░░░░░░░░░░░░░░░░░░░░░██▐" + Environment.NewLine +
      "▌█▓▌░░░░░░░░░░░░░░░░░░░░░░░░░░░▓█▐" };

    private int _catalogueIndex = -1;

    // Catalogues
    private List<string> _catalogueNames = new List<string>();

    // list of displayed catalogues
    private List<int> _catalogueNumbers = new List<int>();

    // internal db catalogue numbers
    private bool _inclSS;

    private bool _lastInputWasSecant;

    // The path to the database file.
    private string _search = "";

    // Sections
    private List<string> _sectionNames;

    private Tuple<List<string>, List<int>> _typeData;

    // -1 is all
    private int _typeIndex = -1;

    private List<string> _typeNames = new List<string>();
    private List<int> _typeNumbers = new List<int>(); // internal db type numbers

    // list of displayed types
    private Type _type = typeof(IRectangleProfile);
    private int _sectionIndex = 0;

    protected CreateOasysProfile(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory) {
      Tuple<List<string>, List<int>> catalogueData = SqlReader.Instance.GetCataloguesDataFromSQLite(DataSource);
      _catalogueNames = catalogueData.Item1;
      _catalogueNumbers = catalogueData.Item2;
      _typeData = SqlReader.Instance.GetTypesDataFromSQLite(-1, DataSource, false);
      _sectionNames = SqlReader.Instance.GetSectionsDataFromSQLite(new List<int> { -1 }, DataSource, false);
    }

    public override bool Read(GH_IReader reader) {
      _mode = (FoldMode)Enum.Parse(typeof(FoldMode), reader.GetString("mode"));
      _lengthUnit = (LengthUnit)UnitsHelper.Parse(typeof(LengthUnit), reader.GetString("lengthUnit"));
      _catalogueIndex = reader.GetInt32("catalogueIndex");
      _typeIndex = reader.GetInt32("typeIndex");
      _sectionIndex = reader.GetInt32("sectionIndex");

      bool flag = base.Read(reader);
      Params.Output[0].Access = GH_ParamAccess.tree;
      return flag;
    }

    public override void SetSelected(int i, int j) {
      // input -1 to force update of catalogue sections to include/exclude superseeded
      bool updateCat = false;
      if (i == -1) {
        _selectedItems[0] = "Catalogue";
        updateCat = true;
        i = 0;
      } else {
        // change selected item
        _selectedItems[i] = _dropDownItems[i][j];
      }

      if (_selectedItems[0] == "Catalogue") {
        // update spacer description to match catalogue dropdowns
        _spacerDescriptions[1] = "Catalogue";

        // if FoldMode is not currently catalogue state, then we update all lists
        UpdateSelectedItemsForNonCatalogue(updateCat);
        UpdateDropdownItems(_catalogueNames);

        if (i == 1) {
          _catalogueIndex = _catalogueNumbers[j];

          List<int> types = CreateTypeList();
          SetSectionNames(types);
          ChangeSelectedItems(j);
        }
        _dropDownItems.Add(_typeNames);

        if (i == 2) {
          _typeIndex = _typeNumbers[j];
          _selectedItems[2] = _typeNames[j];

          List<int> types = CreateTypeList();

          SetSectionNames(types);

          _selectedItems[3] = _sectionNames[0];
        }
        _dropDownItems.Add(_sectionNames);

        if (i == 3) {
          _sectionIndex = _sectionNames.IndexOf(_sectionNames[j]);
          _selectedItems[3] = _sectionNames[_sectionIndex];
        }

        if (_search == "")
          UpdateProfileDescriptions();

        base.UpdateUI();
      } else { // non catalogue selection
        _spacerDescriptions[1] = "Measure";

        if (_mode != FoldMode.Other) {
          UpdateDropdownItems(UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Length));

          // set selected length
          _selectedItems[1] = _lengthUnit.ToString();
        }

        if (i == 0) {
          _type = profileTypes[_selectedItems[0]];
          Mode2Clicked();
        } else {
          _lengthUnit = (LengthUnit)UnitsHelper.Parse(typeof(LengthUnit), _selectedItems[i]);

          base.UpdateUI();
        }
      }
    }


    public override void VariableParameterMaintenance() {
      if (_mode == FoldMode.Catalogue) {
        int i = 0;
        Params.Input[i].NickName = "S";
        Params.Input[i].Name = "Search";
        Params.Input[i].Description = "Text to search from";
        Params.Input[i].Access = GH_ParamAccess.item;
        Params.Input[i].Optional = true;

        i++;
        Params.Input[i].NickName = "iSS";
        Params.Input[i].Name = "InclSuperseeded";
        Params.Input[i].Description = "Input true to include superseeded catalogue sections";
        Params.Input[i].Access = GH_ParamAccess.item;
        Params.Input[i].Optional = true;
      } else {
        string unitAbbreviation = Length.GetAbbreviation(_lengthUnit);

        int i = 0;
        if (_type == typeof(IAngleProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth of the angle profile (leg in the local z axis).";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "W";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The width of the angle profile (leg in the local y axis).";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tw";
          Params.Input[i].Name = "Web Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The web thickness of the angle profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tf";
          Params.Input[i].Name = "Flange Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The flange thickness of the angle profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(IChannelProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth of the channel profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The width of the flange of the channel profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tw";
          Params.Input[i].Name = "Web Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The web thickness of the channel profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tf";
          Params.Input[i].Name = "Flange Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The flange thickness of the channel profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(ICircleHollowProfile)) {
          Params.Input[i].NickName = "Ø";
          Params.Input[i].Name = "Diameter [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The diameter of the hollow circle.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "t";
          Params.Input[i].Name = "Thickness [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The wall thickness of the hollow circle.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(ICircleProfile)) {
          Params.Input[i].NickName = "Ø";
          Params.Input[i].Name = "Diameter [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The diameter of the circle.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(ICruciformSymmetricalProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth (local z axis leg) of the profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The width of the flange (local y axis leg) of the cruciform.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tw";
          Params.Input[i].Name = "Web Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The web thickness of the cruciform.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tf";
          Params.Input[i].Name = "Flange Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The flange thickness of the cruciform.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(IEllipseHollowProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth of the hollow ellipse.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The width of the hollow ellipse.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "t";
          Params.Input[i].Name = "Thickness [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The wall thickness of the hollow ellipse.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(IEllipseProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth of the ellipse.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The width of the ellipse.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(IGeneralCProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth of the generic c section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The flange width of the generic c section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "L";
          Params.Input[i].Name = "Lip [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The lip of the generic c section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "t";
          Params.Input[i].Name = "Thickness [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The thickness of the generic c section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(IGeneralZProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth of the generic z section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Bt";
          Params.Input[i].Name = "TopWidth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The top flange width of the generic z section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Bb";
          Params.Input[i].Name = "BottomWidth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The bottom flange width of the generic z section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Lt";
          Params.Input[i].Name = "Top Lip [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The top lip of the generic z section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Lb";
          Params.Input[i].Name = "Lip [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The top lip of the generic z section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "t";
          Params.Input[i].Name = "Thickness [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The thickness of the generic z section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(IIBeamAsymmetricalProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth of the profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Bt";
          Params.Input[i].Name = "TopFlangeWidth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The width of the top flange of the beam. Top is relative to the beam local access.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Bb";
          Params.Input[i].Name = "BottomFlangeWidth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The width of the bottom flange of the beam. Bottom is relative to the beam local access.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Web";
          Params.Input[i].Name = "Web Thickness [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The web thickness of the beam.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tt";
          Params.Input[i].Name = "TopFlangeThk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The top flange thickness.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tb";
          Params.Input[i].Name = "BottomFlangeThk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The bpttom flange thickness.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(IIBeamCellularProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth of the profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The width of the flanges of the beam.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tw";
          Params.Input[i].Name = "Web Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The web thickness of the angle profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tf";
          Params.Input[i].Name = "Flange Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The flange thickness of the angle profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "O";
          Params.Input[i].Name = "WebOpening [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The size of the web opening.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "P";
          Params.Input[i].Name = "Pitch [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The pitch (spacing) between the web openings.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(IIBeamProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth of the profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The width of the flanges of the beam.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tw";
          Params.Input[i].Name = "Web Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The web thickness of the angle profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tf";
          Params.Input[i].Name = "Flange Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The flange thickness of the angle profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(IRectangleHollowProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth of the hollow rectangle.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The width of the hollow rectangle.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tw";
          Params.Input[i].Name = "Web Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The webs (side walls) thickness of the hollow rectangle.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tf";
          Params.Input[i].Name = "Flange Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The flanges (top and bottom) thickness of the hollow rectangle.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(IRectangleProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "Depth of the rectangle, in local z-axis direction.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "Width of the rectangle, in local y-axis direction.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(IRectoEllipseProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The overall depth of the recto-ellipse profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Df";
          Params.Input[i].Name = "DepthFlat [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The flat length of the profile's overall depth.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The overall width of the recto-ellipse profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Bf";
          Params.Input[i].Name = "WidthFlat [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The flat length of the profile's overall width.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(ISecantPileProfile)) {
          Params.Input[i].NickName = "Ø";
          Params.Input[i].Name = "Diameter [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The diameter of the piles.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "c/c";
          Params.Input[i].Name = "PileCentres [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The centre to centre distance between adjacent piles.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "No";
          Params.Input[i].Name = "PileCount";
          Params.Input[i].Description = "The number of piles in the profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "W/S";
          Params.Input[i].Name = "isWall";
          Params.Input[i].Description = "Converts the profile into a wall secant pile profile if true -- Converts the profile into a section secant pile profile if false.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(ISheetPileProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth of the sheet pile section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The overall width of the sheet pile section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Bt";
          Params.Input[i].Name = "TopFlangeWidth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The top flange width of the sheet pile section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Bb";
          Params.Input[i].Name = "BottomFlangeWidth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The bottom flange width of the sheet pile section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Ft";
          Params.Input[i].Name = "FlangeThickness [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The flange thickness of the sheet pile section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Wt";
          Params.Input[i].Name = "WebThickness [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The web thickness of the sheet pile section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(IRectoCircleProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The profile's overall depth considering the side length of the rectangle and the radii of the semicircles on the two ends.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The profile's width (diameter of the semicircles).";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(ITrapezoidProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth in z-axis direction of trapezoidal profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Bt";
          Params.Input[i].Name = "TopWidth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The top width of trapezoidal profile. Top is relative to the local z-axis.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Bb";
          Params.Input[i].Name = "BottomWidth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The bottom width of trapezoidal profile. Bottom is relative to the local z-axis.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(ITSectionProfile)) {
          Params.Input[i].NickName = "D";
          Params.Input[i].Name = "Depth [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The depth of the T section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Width [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The width of the T section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tw";
          Params.Input[i].Name = "Web Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The web thickness of the T section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;

          i++;
          Params.Input[i].NickName = "Tf";
          Params.Input[i].Name = "Flange Thk [" + unitAbbreviation + "]";
          Params.Input[i].Description = "The flange thickness of the T section profile.";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        } else if (_type == typeof(IPerimeterProfile)) {
          Params.Input[i].NickName = "B";
          Params.Input[i].Name = "Boundary";
          Params.Input[i].Description = "The outer edge polyline or Brep. If Brep contains openings these will be added as voids";
          Params.Input[i].Access = GH_ParamAccess.item;
          Params.Input[i].Optional = false;
        }
      }
    }

    public override bool Write(GH_IWriter writer) {
      writer.SetString("mode", _mode.ToString());
      writer.SetString("lengthUnit", _lengthUnit.ToString());
      writer.SetInt32("catalogueIndex", _catalogueIndex);
      writer.SetInt32("typeIndex", _typeIndex);
      writer.SetInt32("sectionIndex", _sectionIndex);
      return base.Write(writer);
    }

    protected internal override void InitialiseDropdowns() {
      _spacerDescriptions = new List<string>(new string[] {
        "Profile type",
        "Measure",
        "Type",
        "Profile"
      });

      _dropDownItems = new List<List<string>>();
      _selectedItems = new List<string>();

      // Profile type
      _dropDownItems.Add(profileTypes.Keys.ToList());
      _selectedItems.Add("Rectangle");

      // Length
      _dropDownItems.Add(UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Length));
      _selectedItems.Add(Length.GetAbbreviation(_lengthUnit));

      _isInitialised = true;
    }

    protected virtual int GetNumberOfGenericInputs() {
      if (_type == typeof(IAngleProfile))
        return 4;
      else if (_type == typeof(IChannelProfile))
        return 4;
      else if (_type == typeof(ICircleHollowProfile))
        return 2;
      else if (_type == typeof(ICircleProfile))
        return 1;
      else if (_type == typeof(ICruciformSymmetricalProfile))
        return 4;
      else if (_type == typeof(IEllipseHollowProfile))
        return 3;
      else if (_type == typeof(IEllipseProfile))
        return 2;
      else if (_type == typeof(IGeneralCProfile))
        return 4;
      else if (_type == typeof(IGeneralZProfile))
        return 6;
      else if (_type == typeof(IIBeamAsymmetricalProfile))
        return 6;
      else if (_type == typeof(IIBeamCellularProfile))
        return 6;
      else if (_type == typeof(IIBeamProfile))
        return 4;
      else if (_type == typeof(IRectangleHollowProfile))
        return 4;
      else if (_type == typeof(IRectangleProfile))
        return 2;
      else if (_type == typeof(IRectoCircleProfile))
        return 2;
      else if (_type == typeof(IRectoEllipseProfile))
        return 4;
      else if (_type == typeof(ISecantPileProfile))
        return 4;
      else if (_type == typeof(ISheetPileProfile))
        return 6;
      else if (_type == typeof(ITrapezoidProfile))
        return 3;
      else if (_type == typeof(ITSectionProfile))
        return 4;
      else if (_type == typeof(IPerimeterProfile))
        return 1;

      return -1;
    }

    protected override string HtmlHelp_Source() {
      string help = "GOTO:https://arup-group.github.io/oasys-combined/adsec-api/api/Oasys.Profiles.html";
      return help;
    }

    protected virtual void Mode1Clicked() {
      // remove input parameters
      while (Params.Input.Count > 0)
        Params.UnregisterInputParameter(Params.Input[0], true);

      // register input parameter
      Params.RegisterInputParam(new Param_String());
      Params.RegisterInputParam(new Param_Boolean());

      _mode = FoldMode.Catalogue;

      base.UpdateUI();
    }

    protected virtual void Mode2Clicked() {
      // check if mode is correct
      if (_mode != FoldMode.Other) {
        // if we come from catalogue mode remove all input parameters
        while (Params.Input.Count > 0)
          Params.UnregisterInputParameter(Params.Input[0], true);

        // set mode to other
        _mode = FoldMode.Other;
      }

      UpdateParameters();

      (this as IGH_VariableParameterComponent).VariableParameterMaintenance();
      Params.OnParametersChanged();
      ExpireSolution(true);
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager) {
      string unitAbbreviation = Length.GetAbbreviation(_lengthUnit);
      pManager.AddGenericParameter("Width [" + unitAbbreviation + "]", "B", "Profile width", GH_ParamAccess.item);
      pManager.AddGenericParameter("Depth [" + unitAbbreviation + "]", "H", "Profile depth", GH_ParamAccess.item);
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddTextParameter("Profile", "Pf", "Profile for a GSA Section", GH_ParamAccess.tree);
    }

    protected override void SolveInternal(IGH_DataAccess da) {
      ClearRuntimeMessages();
      for (int i = 0; i < Params.Input.Count; i++)
        Params.Input[i].ClearRuntimeMessages();

      if (_mode == FoldMode.Catalogue) {
        int pathCount = 0;
        if (Params.Output[0].VolatileDataCount > 0) {
          pathCount = Params.Output[0].VolatileData.PathCount;
        }

        var path = new GH_Path(pathCount);
        List<IProfile> profiles = SolveInstanceForCatalogueProfile(da);
        var tree = new DataTree<OasysProfileGoo>();
        foreach (IProfile profile in profiles) {
          tree.Add(new OasysProfileGoo(profile), path);
        }

        da.SetDataTree(0, tree);

      } else if (_mode == FoldMode.Other) {
        IProfile profile = SolveInstanceForStandardProfile(da);

        da.SetData(0, new OasysProfileGoo(profile));
      }
    }

    protected List<IProfile> SolveInstanceForCatalogueProfile(IGH_DataAccess da) {
      var profiles = new List<IProfile>();
      // get user input filter search string
      bool incl = false;
      if (da.GetData(1, ref incl)) {
        if (_inclSS != incl) {
          _inclSS = incl;
          SetTypeList();
          _sectionNames = SqlReader.Instance.GetSectionsDataFromSQLite(_typeNumbers, DataSource, _inclSS);

          _selectedItems[2] = _typeNames[0];
          _dropDownItems[2] = _typeNames;

          _selectedItems[3] = _sectionNames[0];
          _dropDownItems[3] = _sectionNames;

          base.UpdateUI();
        }
      }

      // get user input filter search string
      _search = null;
      string inSearch = "";
      if (da.GetData(0, ref inSearch)) {
        _search = inSearch.Trim().ToLower().Replace(".", string.Empty).Replace("*", ".*").Replace(" ", ".*");
        if (_search == "cat") {
          string eventName = "EasterCat";
          var properties = new Dictionary<string, object>();
          _ = PostHog.SendToPostHog(PluginInfo, eventName, properties);

          foreach (string catPart in easterCat)
            profiles.Add(new CatalogueProfile(catPart));

          return profiles;
        } else if (_search.Contains("cat")) {
          string[] s = _search.Split(new string[] { "cat" }, StringSplitOptions.None);
          _search = s[s.Length - 1];
        }

        // boolean to save state of search string being 'problematic' to avoid checking this every single time
        bool tryHard = Regex.Match(_search, "he[abcm]", RegexOptions.Singleline).Success;

        // filter by search pattern
        var filteredlist = new List<string>();
        if (_selectedItems[3] != "All") {
          if (!MatchAndAdd(_selectedItems[3], _search, ref filteredlist, tryHard)) {
            _profileDescriptions = new List<string>();
            AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No profile found that matches selected profile and search!");
          }
        } else if (_search != "") {
          for (int k = 0; k < _sectionNames.Count; k++) {
            if (MatchAndAdd(_sectionNames[k], _search, ref filteredlist, tryHard)) {
            } else if (!_search.Any(char.IsDigit)) {
              string test = _sectionNames[k].ToString();
              test = Regex.Replace(test, "[0-9]", string.Empty);
              test = test.Replace(".", string.Empty);
              test = test.Replace("-", string.Empty);
              test = test.ToLower();
              if (test.Contains(_search)) {
                filteredlist.Add(_sectionNames[k]);
              }
            }
          }
        }
        _profileDescriptions = new List<string>();
        if (filteredlist.Count > 0) {
          foreach (string profileDescription in filteredlist)
            _profileDescriptions.Add("CAT " + profileDescription);
        } else {
          AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No profile found that matches selection and search!");
          return profiles;
        }
      }

      if (_search == null)
        UpdateProfileDescriptions();

      foreach (string description in _profileDescriptions)
        profiles.Add(new CatalogueProfile(description));

      return profiles;
    }

    protected IProfile SolveInstanceForStandardProfile(IGH_DataAccess DA) {
      IProfile profile;
      if (_type == typeof(IAngleProfile)) {
        var flange = new Flange(
          (Length)Input.LengthOrRatio(this, DA, 3, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit));

        var web = new WebConstant(
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit));

        profile = new AngleProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          flange,
          web);
      } else if (_type == typeof(IChannelProfile)) {
        var flanges = new Flange(
          (Length)Input.LengthOrRatio(this, DA, 3, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit));

        var web = new WebConstant(
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit));

        profile = new ChannelProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          flanges,
          web);
      } else if (_type == typeof(ICircleHollowProfile)) {
        profile = new CircleHollowProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit));
      } else if (_type == typeof(ICircleProfile)) {
        profile = new CircleProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit));
      } else if (_type == typeof(ICruciformSymmetricalProfile)) {
        var flange = new Flange(
          (Length)Input.LengthOrRatio(this, DA, 3, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit));

        var web = new WebConstant(
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit));

        profile = new CruciformSymmetricalProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          flange,
          web);
      } else if (_type == typeof(IEllipseHollowProfile)) {
        profile = new EllipseHollowProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit));
      } else if (_type == typeof(IEllipseProfile)) {
        profile = new EllipseProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit));
      } else if (_type == typeof(IGeneralCProfile)) {
        profile = new GeneralCProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 3, _lengthUnit));
      } else if (_type == typeof(IGeneralZProfile)) {
        profile = new GeneralZProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 3, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 4, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 5, _lengthUnit));
      } else if (_type == typeof(IIBeamAsymmetricalProfile)) {
        var topFlange = new Flange(
          (Length)Input.LengthOrRatio(this, DA, 4, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit));
        var bottomFlange = new Flange(
          (Length)Input.LengthOrRatio(this, DA, 5, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit));

        var web = new WebConstant(
          (Length)Input.LengthOrRatio(this, DA, 3, _lengthUnit));

        profile = new IBeamAsymmetricalProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          topFlange,
          bottomFlange,
          web);
      } else if (_type == typeof(IIBeamCellularProfile)) {
        var flanges = new Flange(
          (Length)Input.LengthOrRatio(this, DA, 3, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit));

        var web = new WebConstant(
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit));

        profile = new IBeamCellularProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          flanges, web,
          IBeamOpeningType.Cellular,
          (Length)Input.LengthOrRatio(this, DA, 4, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 5, _lengthUnit));
      } else if (_type == typeof(IIBeamProfile)) {
        var flanges = new Flange(
          (Length)Input.LengthOrRatio(this, DA, 3, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit));

        var web = new WebConstant(
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit));

        profile = new IBeamProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          flanges,
          web);
      } else if (_type == typeof(IRectangleHollowProfile)) {
        var flanges = new Flange(
          (Length)Input.LengthOrRatio(this, DA, 3, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit));
        var webs = new WebConstant(
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit));
        profile = new RectangleHollowProfile(
            (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
            flanges,
            webs);
      } else if (_type == typeof(IRectangleProfile)) {
        profile = new RectangleProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit));
      } else if (_type == typeof(IRectoEllipseProfile)) {
        profile = new RectoEllipseProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 3, _lengthUnit));
      } else if (_type == typeof(ISecantPileProfile)) {
        int pileCount = 0;
        if (!DA.GetData(2, ref pileCount)) {
          AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert input PileCount to integer.");
          return null;
        }

        bool isWallNotSection = false;
        if (!DA.GetData(3, ref isWallNotSection)) {
          AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert input isWall to boolean.");
          return null;
        }

        profile = new SecantPileProfile((Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit), (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit), pileCount, isWallNotSection);
      } else if (_type == typeof(ISheetPileProfile)) {
        profile = new SheetPileProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 3, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 4, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 5, _lengthUnit));
      } else if (_type == typeof(IRectoCircleProfile)) {
        profile = new RectoCircleProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit));
      } else if (_type == typeof(ITrapezoidProfile)) {
        profile = new TrapezoidProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit));
      } else if (_type == typeof(ITSectionProfile)) {
        var flange = new Flange(
          (Length)Input.LengthOrRatio(this, DA, 3, _lengthUnit),
          (Length)Input.LengthOrRatio(this, DA, 1, _lengthUnit));

        var web = new WebConstant(
          (Length)Input.LengthOrRatio(this, DA, 2, _lengthUnit));

        profile = new TSectionProfile(
          (Length)Input.LengthOrRatio(this, DA, 0, _lengthUnit),
          flange,
          web);
      } else if (_type == typeof(IPerimeterProfile)) {
        var gh_typ = new GH_ObjectWrapper();
        if (DA.GetData(0, ref gh_typ)) {
          Brep brep = null;
          Curve crv = null;
          if (GH_Convert.ToBrep(gh_typ.Value, ref brep, GH_Conversion.Both)) {
            // get edge curves from brep
            Curve[] edgeSegments = brep.DuplicateEdgeCurves();
            Curve[] edges = Curve.JoinCurves(edgeSegments);

            // find the best fit plane
            List<Point3d> ctrlPts;
            if (edges[0].TryGetPolyline(out Polyline tempCrv)) {
              ctrlPts = tempCrv.ToList();
            } else {
              throw new Exception("Data conversion failed to create a polyline from input geometry. Please input a polyline approximation of your Brep/outline.");
            }

            Plane.FitPlaneToPoints(ctrlPts, out Plane plane);
            plane.Origin = tempCrv.CenterPoint();

            var solidpts = new List<Point3d>();
            foreach (Point3d pt3d in ctrlPts) {
              solidpts.Add(pt3d);
            }
            var solid = new Polyline(solidpts);

            IPolygon perimeter = Geometry.PolygonFromRhinoPolyline(solid, _lengthUnit, plane);

            // first set of curve segments is the solid perimeter
            // consecutive ones are describing voids in the solid perimeter
            IList<IPolygon> voidPolygons = new List<IPolygon>();
            if (edges.Length > 1) {
              for (int i = 1; i < edges.Length; i++) {
                ctrlPts.Clear();
                var voidpts = new List<Point3d>();
                if (!edges[i].IsPlanar()) {
                  for (int j = 0; j < edges.Length; j++) {
                    edges[j] = Curve.ProjectToPlane(edges[j], plane);
                  }
                }

                if (edges[i].TryGetPolyline(out tempCrv)) {
                  ctrlPts = tempCrv.ToList();

                  foreach (Point3d pt3d in ctrlPts) {
                    //pt3d.Transform(xform);
                    voidpts.Add(pt3d);
                  }
                } else {
                  throw new Exception("Cannot convert internal edge to polyline.");
                }

                var voidCrv = new Polyline(voidpts);
                voidPolygons.Add(Geometry.PolygonFromRhinoPolyline(voidCrv, _lengthUnit, plane));
              }
            }
            profile = new PerimeterProfile(perimeter, voidPolygons);
          } else if (GH_Convert.ToCurve(gh_typ.Value, ref crv, GH_Conversion.Both)) {
            if (crv.TryGetPolyline(out Polyline solid)) {
              // get local plane
              Plane.FitPlaneToPoints(solid.ToList(), out Plane plane);

              IPolygon perimeter = Geometry.PolygonFromRhinoPolyline(solid, _lengthUnit, plane);
              IList<IPolygon> voidPolygons = new List<IPolygon>();

              profile = new PerimeterProfile(perimeter, voidPolygons);
            } else {
              AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert input " + Params.Input[0].NickName + " to polyline");
              return null;
            }
          } else {
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + Params.Input[0].NickName + " to boundary");
            return null;
          }
        } else {
          AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to convert " + Params.Input[0].NickName + " to boundary");
          return null;
        }
      } else {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unable to create perimeter profile.");
        return null;
      }

      return profile;
    }

    protected virtual void UpdateParameters() {
      int numberOfGenericInputs = GetNumberOfGenericInputs();
      bool isSecantPile = false;
      bool isPerimeter = false;

      if (_type == typeof(ISecantPileProfile))
        isSecantPile = true;
      else if (_type == typeof(IPerimeterProfile))
        isPerimeter = true;

      // if last input previously was a bool and we no longer need that
      if (_lastInputWasSecant || isSecantPile || isPerimeter) {
        if (Params.Input.Count > 0) {
          // make sure to remove last param
          Params.UnregisterInputParameter(Params.Input[Params.Input.Count - 1], true);
          Params.UnregisterInputParameter(Params.Input[Params.Input.Count - 1], true);
        }
      }

      // remove any additional inputs
      while (Params.Input.Count > numberOfGenericInputs)
        Params.UnregisterInputParameter(Params.Input[numberOfGenericInputs], true);

      if (isSecantPile) // add two less generic than input says
      {
        while (Params.Input.Count > numberOfGenericInputs + 2)
          Params.UnregisterInputParameter(Params.Input[numberOfGenericInputs + 2], true);
        numberOfGenericInputs -= 2;
      }

      // add inputs parameter
      while (Params.Input.Count < numberOfGenericInputs)
        Params.RegisterInputParam(new Param_GenericObject());

      if (isSecantPile) // finally add int and bool param if secant
      {
        Params.RegisterInputParam(new Param_Integer());
        Params.RegisterInputParam(new Param_Boolean());
        _lastInputWasSecant = true;
      } else
        _lastInputWasSecant = false;

      //if (isPerimeter)
      //  Params.RegisterInputParam(new Param_Plane());
    }

    protected override void UpdateUIFromSelectedItems() {
      if (_selectedItems[0] == "Catalogue") {
        _spacerDescriptions = new List<string>(new string[]
        {
          "Profile type", "Catalogue", "Type", "Profile"
        });

        SetTypeList();
        List<int> types = CreateTypeList();
        SetSectionNames(types);
        CreateSectionList();
        UpdateDropdownItems(_catalogueNames);
        UpdateDropdownItems(_typeNames, false);
        UpdateDropdownItems(_sectionNames, false);

        int catIndex = _catalogueNumbers.IndexOf(_catalogueIndex);
        int typeIndex = _typeNumbers.IndexOf(_typeIndex);
        ChangeSelectedItems(catIndex, typeIndex, _sectionIndex);

        UpdateProfileDescriptions();
        Mode1Clicked();


      } else {
        _spacerDescriptions = new List<string>(new string[]
        {
          "Profile type", "Measure", "Type", "Profile"
        });
        UpdateDropdownItems(UnitsHelper.GetFilteredAbbreviations(EngineeringUnits.Length));

        _selectedItems[1] = _lengthUnit.ToString();
        _type = profileTypes[_selectedItems[0]];
        Mode2Clicked();
      }

      base.UpdateUIFromSelectedItems();
    }

    private static Tuple<List<string>, List<int>> GetTypesDataFromSqLite(int catalogueIndex, string filePath, bool inclSuperseeded) => SqlReader.Instance.GetTypesDataFromSQLite(catalogueIndex, filePath, inclSuperseeded);

    private static bool MatchAndAdd(string item, string pattern, ref List<string> list, bool tryHard = false) {
      string input = item.ToLower().Replace(".", String.Empty);
      if (Regex.Match(input, pattern, RegexOptions.Singleline).Success) {
        list.Add(item);
        return true;
      } else if (tryHard && Regex.Match(pattern, "he[abcm]", RegexOptions.Singleline).Success) {
        string[] substring = pattern.Split(new string[] { "he" }, StringSplitOptions.None);
        int count = 1;
        if (substring[substring.Length - 1].Length > 1 && !Char.IsNumber(substring[substring.Length - 1][1]))
          count = 2;

        pattern = "he" + substring[substring.Length - 1].Remove(0, count) + substring[substring.Length - 1].Substring(0, count);
        if (Regex.Match(input, pattern, RegexOptions.Singleline).Success) {
          list.Add(item);
          return true;
        }
      }
      return false;
    }

    private void UpdateProfileDescriptions() {
      if (_selectedItems[3] == "All") {
        _profileDescriptions = new List<string>();
        foreach (string profile in _sectionNames) {
          if (profile == "All")
            continue;
          _profileDescriptions.Add("CAT " + profile);
        }
      } else
        _profileDescriptions = new List<string>() { "CAT " + _selectedItems[3] };
    }
    
    private List<int> CreateTypeList() {
      List<int> types;
      if (_typeIndex == -1) // if all
      {
        types = _typeNumbers.ToList();
        types.RemoveAt(0);
        if (types.Count == 0) {
          AddRuntimeMessage(GH_RuntimeMessageLevel.Warning,
            "Selected catalogue contains no types. Try include superseeded.");
        }
      } else
        types = new List<int> { _typeIndex };

      return types;
    }

    private void CreateSectionList() {
      List<string> sections;
      if (_sectionIndex == -1) // if all
      {
        sections = _sectionNames.ToList();
        sections.RemoveAt(0);
        if (sections.Count == 0) {
          AddRuntimeMessage(GH_RuntimeMessageLevel.Warning,
            "Selected catalogue contains no sections. ");
        }

        _sectionNames = sections;
      }
    }
    
    private void UpdateSelectedItemsForNonCatalogue(bool updateCat) {
      if (!(_mode != FoldMode.Catalogue | updateCat)) {
        return;
      }

      RemoveSelection();

      _catalogueIndex = -1;
      _typeIndex = -1;
      _sectionIndex = 0;

      SetTypeList();
      SetSectionNames(_typeNumbers);
      CreateSectionList();
      ChangeSelectedItems();

      Mode1Clicked();
    }

    private void SetTypeList() {
      _typeData = GetTypesDataFromSqLite(_catalogueIndex, DataSource, _inclSS);
      _typeNames = _typeData.Item1;
      _typeNumbers = _typeData.Item2;
    }

    private void SetSectionNames(List<int> types) =>
      _sectionNames = SqlReader.Instance.GetSectionsDataFromSQLite(types, DataSource, _inclSS);

    private void ChangeSelectedItems(int catIndex = 0, int typeIndex = 0, int sectionIndex = 0) {
      RemoveSelection();

      _selectedItems.Add(_catalogueNames[catIndex]);
      _selectedItems.Add(_typeNames[typeIndex]);
      _selectedItems.Add(_sectionNames[sectionIndex]);
    }

    private void RemoveSelection() {
      while (_selectedItems.Count > 1)
        _selectedItems.RemoveAt(1);
    }

    private void UpdateDropdownItems(List<string> list, bool remove = true) {
      if (remove) {
        while (_dropDownItems.Count > 1)
          _dropDownItems.RemoveAt(1);
      }

      _dropDownItems.Add(list);
    }
  }
}
