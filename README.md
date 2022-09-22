# OasysGH

## Contents
1. [Introduction](#introduction)
1. [How to use](#how-to-use)
1. [Content overview](#content-overview)

## Introduction
OasysGH is a library with shared content for Oasys Grasshopper plugins. 

You can use this library for:
- Units shared by multiple plugins, with form to set defaults.
- GH_UnitNumber parameter to pass [OasysUnits](https://github.com/arup-group/oasysunits/) (fork of [UnitsNet](https://github.com/angularsen/UnitsNet)) based objects around between components.
- Custom component UI (dropdown, sliders, buttons, etc), also known s "Component Attributes".
- Abstract classes for parameters to help handle wrapping an object oriented API.
- Abstract classes for components to help implement custom component UI.
- Helper methods for input/output of various custom parameters

## How to use
Reference this library as a [NuGet package](https://www.nuget.org/packages/OasysGH/latest) in your project. 

Setup a OasysPluginInfo `PluginInfo` singleton like this for your plugin:
```cs
internal sealed class PluginInfo
{
  private static readonly Lazy<OasysPluginInfo> lazy =
      new Lazy<OasysPluginInfo>(() => new OasysPluginInfo(
        "Oasys Shared Grasshopper", "OasysGH", "0.1.0.0", true, "phc_alOp3OccDM3D18xJTWDoW44Y1cJvbEScm5LJSX8qnhs"
        ));
   public static OasysPluginInfo Instance { get { return lazy.Value; } }
   private PluginInfo()
  {
  }
}
```

### Shared units
To initialise shared default units along with the Windows Form to allow user to set their default units, simply call the method:
```cs
OasysGH.Utility.InitialiseMainMenuAndDefaultUnits();
```
![DefaultUnits](https://user-images.githubusercontent.com/25223248/191560129-0c2643a6-f6ff-4020-b3cb-755c8bf7ac52.PNG)
This will also automatically download the latest version of the [UnitNumber plugin](/GH_UnitNumber), which is a separate project in this repository.

## Content overview
### Units
This library contains: 
- A [shared class](/OasysGH/Units/DefaultUnits.cs) with default units for typical engineering units. 
- Filters to get relevant engineering units (UnitsNet contains many units unrelated to engineering, for instance lengths in lightyears or printer points)
- Helper methods to get for instance a moment unit from length and force units.
- A custom grasshopper parameter called `GH_UnitNumber` which wraps `OasysUnits.IQuantity` which will allow you to pass numbers with units around between components. 
- Input helpers for `GH_UnitNumber` which will allow users to input:
  - another `GH_UnitNumber`, 
  - a number (for instance from a slider) which will be converted into a Quantity with the selected unit (can be combined with dropdown UI component to allow users to select the unit)
  - a text string, for instance `15 kNm` which will be parsed into a `OasysUnits.Moment`.

### Component Attributes
Attributes for components is a custom way to override the drawing methods creating the UI of a component on the Grasshopper canvas.

It is used to set the `m_attributes` field in a Grasshopper component:
```cs
public override void CreateAttributes()
{
  m_attributes = new UI.DropDownComponentAttributes(this, this.SetSelected, this.DropDownItems, this.SelectedItems, this.SpacerDescriptions);
}
```

OasysGH contains component `Attribute` methods to create:
- Bool6 component (six check boxes)
- Button component (single button for 'Open' components)
- Dropdown(s) with check boxes
- Dropdown(s) component
- Dropdown(s) with slider (for scaling results)
- Releases component (12 check boxes)
- Support component (three buttons with six check boxes)
- Three button component (for 'Save' components, buttons for 'Save', 'Save As' and 'Open in X')

Feel free to add your own custom components UI.

### Abstract component classes
Inherit from `GH_OasysComponent`, `GH_OasysDropDownComponent` or `GH_OasysTaskCapableComponent<T>` to:
- Track usage with posthog
- Use simple scaffolding code to create, for instance, complex dropdown menus components. The abstract base classes take care of most things for you.
- By default, the `GH_OasysDropDownComponent` will use `DropDownComponentAttributes` so you do not have to override the `CreateAttributes()` method unless you want to use one of the other component attributes.

#### GH_OasysComponent
Inherit from this class if you have a simple, normal component with nothing fancy happening. This will simply add posthog tracking to your components.

#### GH_OasysDropDownComponent
Inherit from this class to use the custom UI. As mentioned above, the default behaviour is for `GH_OasysDropDownComponent` to use `DropDownComponentAttributes` but if you want to use one of the other attributes just override the `CreateAttributes()` method.

A good example for simple implementation of the abstract/virtual methods in `GH_OasysDropDownComponent` is shown below, this is taken from the [ComposGH repo](https://github.com/arup-group/Compos-Grasshopper/blob/main/ComposGH/Components/2_Studs/4_Quarternary/CreateCustomStudSpacing.cs)

This example will create a component with a dropdown menu to set a length unit. It will take the initial unit from the default units, but then afterwards save the selected unit in the component so that the settings is stored when users reopen their Grasshopper document.

First, we need to initialise the dropdown lists
```cs
private LengthUnit LengthUnit = DefaultUnits.LengthUnitSection; // get default length unit

public override void InitialiseDropdowns()
{
  this.SpacerDescriptions = new List<string>(new string[] { "Unit" });

  this.DropDownItems = new List<List<string>>();
  this.SelectedItems = new List<string>();

  // add length
  this.DropDownItems.Add(Units.FilteredLengthUnits);
  this.SelectedItems.Add(this.LengthUnit.ToString());

  this.IsInitialised = true;
}
```

Secondly, we need to implement what happens when users make a selection in the dropdown:
```cs
public override void SetSelected(int i, int j)
{
  // change selected item
  this.SelectedItems[i] = this.DropDownItems[i][j];
  if (this.LengthUnit.ToString() == this.SelectedItems[i])
    return;

  this.LengthUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), this.SelectedItems[i]);

  base.UpdateUI();
}
```

Then we need to handle what happens when the component is recreated when a saved document is opened again (or when the component is copied).
The contents of SelectedItems and DropDownItems are both stored and recreated automatically by `GH_OasysDropDownComponent` base.
```cs
public override void UpdateUIFromSelectedItems()
{
  this.LengthUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), this.SelectedItems[0]);

  base.UpdateUIFromSelectedItems();
}
```

Finally, and optionally, we may want to change the name of the input parameters to display the selected unit on for instance sliders:
```cs
public override void VariableParameterMaintenance()
{
  string unitAbbreviation = Length.GetAbbreviation(this.LengthUnit);
  Params.Input[0].Name = "Pos x [" + unitAbbreviation + "]";
  Params.Input[3].Name = "Spacing [" + unitAbbreviation + "]";
}
```
![ChangeDropdown](https://user-images.githubusercontent.com/25223248/191702105-e680c742-2002-425f-bcea-2710436e8fbc.gif)

### Abstract parameter classes
The abstract parameter classes makes it easy to implement a custom Grasshopper parameter by wrapping another API object.

#### GH_OasysGoo
An example of how easy it is to inherit from `GH_OasysGoo<T>` is the GH_UnitNumber Goo wrapper class, that makes sure s OasysUnits `IQuantity` can be used in Grasshopper and passed around between components.
```cs
public class GH_UnitNumber : GH_OasysGoo<IQuantity>
{
  public static string Name => "UnitNumber";
  public static string NickName => "UN";
  public static string Description => "A value with a unit measure";
  public override OasysPluginInfo PluginInfo => GH_UnitNumberPluginInfo.Instance;

  public GH_UnitNumber(IQuantity item) : base(item) { }
  public override IGH_Goo Duplicate() => new GH_UnitNumber(this.Value);  
}
```

#### GH_OasysGeometryGoo
This class is still to be implemented, but is in the making...
