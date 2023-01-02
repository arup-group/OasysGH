# OasysGH

![Downloads](https://img.shields.io/nuget/dt/oasysgh?style=flat-square&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAB3SURBVHgB7ZRBDoAgDASr8SH8/1PwE6yHJogEd5UeNMyNpN2hhFTEmQUtzMqpUUH6VnFmCqbASaBfPuaKRk1NhAXu6G1Ca4oOR21gHIyEDyckz8MByfvwjoQLty5Qcht+yUMEDGXe91fFVh5GPpNhEyQZT5JfsAN5UByV3bhHmAAAAABJRU5ErkJggg==) [![Install plugin](https://img.shields.io/badge/install-Food4Rhino-green?style=flat-square&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAADxSURBVHgB3ZTtDcIgEIaPxgHqBnUCV3CEbmA3cAVXcIJ2A0foCm7QOoHdAF8SGgFpORB/6JO8oSUH3Acc0ZcRHCMpZYmhgSYhREe5wQGtfNHHrOVGIK1FgJgUTLvLwnceVA3m/FAkG44RMjJh74kSsHKZ4qHhhLcu3Bok838HjHq8QVtop7/zgBpX0Fm3hnnuIHmU2r7RKmMO5nD1tZTgk1dRYRgoAXV1C2ezEzRAtTF9pDTGtxkn5NYJOYTKe6UdfMxOxr7kDrpDe8iMktdhAx7Wjq11u5b2ZEfg89C0/7gXKY/X/rmbrNE7ti0nRb/PEyfNcxAV2WX+AAAAAElFTkSuQmCC)](https://www.food4rhino.com/en/app/unitnumber)

A library with shared content for Oasys Grasshopper plugins.

| Latest | CI Pipeline | Deployment | Dependencies |
| ------ | ----------- | ---------- | ------------ |
| [![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/arup-group/OasysGH?include_prereleases&logo=github&style=flat-square)](https://github.com/arup-group/OasysGH/releases) <br /> ![Yak](https://img.shields.io/badge/dynamic/json?color=blue&label=yak&prefix=v&query=version&url=https%3A%2F%2Fyak.rhino3d.com%2Fpackages%2Funitnumber&logo=rhinoceros&style=flat-square) <br /> ![Nuget](https://img.shields.io/nuget/vpre/oasysgh?logo=nuget&style=flat-square) | [![Build Status](https://dev.azure.com/oasys-software/OASYS%20libraries/_apis/build/status/arup-group.OasysGH?branchName=main?style=flat-square)](https://dev.azure.com/oasys-software/OASYS%20libraries/_build/latest?definitionId=140&branchName=main) <br /> ![Azure DevOps builds](https://img.shields.io/azure-devops/build/oasys-software/89fd051d-5c77-48bf-9b0e-05bca3e3e596/140?logo=azurepipelines&style=flat-square) <br /> ![GitHub branch checks state](https://img.shields.io/github/checks-status/arup-group/oasysgh/main?logo=github&style=flat-square) | ![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/arup-group/oasysgh/github-release-yak.yml?label=Push%20UnitNumber%20Yak%20package&logo=rhinoceros&style=flat-square) <br /> ![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/arup-group/oasysgh/github-release-nuget.yml?label=Push%20OasysGH%20NuGet%20package&logo=nuget&style=flat-square) | ![Libraries.io dependency status for GitHub repo](https://img.shields.io/librariesio/github/arup-group/oasysgh?logo=nuget&style=flat-square) <br /> ![Dependents (via libraries.io)](https://img.shields.io/librariesio/dependents/nuget/oasysgh?logo=librariesdotio&logoColor=white) |

## Contents
1. [Introduction](#introduction)
1. [How to use](#how-to-use)
    1. [Shared units](#shared-units)
1. [Content overview](#content-overview)
    1. [Units](#units)
    1. [Component Attributes](#component-attributes)
    1. [Abstract component classes](#abstract-component-classes)
    1. [Abstract parameter classes](#abstract-parameter-classes)
    1. [Input helpers](#input-helpers)
    1. [Output helpers](#output-helpers)

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

### Input helpers
OasysGH provides helper methods to get generic input parameters. This enables a single line of code in SolveInstance to retreive custom input parameters in a coherent way that will act similar to how Grasshopper acts to build-in parameters (same type of errors/warnings). For instance a UnitNumber length input can be retrieved like this:
```cs
Length inputLength = (Length)Input.UnitNumber(this, DA, 0, this.LengthUnit);
```

The helpers include methods for getting:
- `GenericGoo<T>` (item input)
- `List<GenericGoo<T>>` (list input)
- `UnitNumber` (item input)
- `List<UnitNumber>` (list input)

The UnitNumber inputs are special in the sense that they include additional helpers to cast from `string` (text) and `double` (number) inputs, allowing users to input `IQuanity` as either a:
- UnitNumber
- Number (unit will be taken from the dropdown or you can implement a static unit like `Hertz` for `FrequencyUnit`)
- Text (for instance `15 kNm` or `130mm`)

![image](https://user-images.githubusercontent.com/25223248/191968096-a536d2a2-7001-4f00-a338-fb2b7906e343.png)

In the image above, the unit for the Height input (cm) is taken from dropdown. Note that the text inputs can be either with or without space between value and unit. Not shown is the example of inputting another UnitNumber.

### Output helpers
In order to prevent the dropdown components to continuously expire downstream components, OasysGH provides helper functions to set outputs as item, list or tree which will only expire the output if the output has actually been changed. This allows the user to toggle the dropdown lists without the entire Grasshopper script having to recalculate.

## License
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?&style=flat-square&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAADLSURBVHgB7ZPRDcIwDESvnYAROgIbkA1ghG7CKDACTNBs0m5QNgiO5I/IuLGLhJCgT7JUWXe5OHKBvyIxazytckhIC8gghQDHLYf0PoN1eFe7jZzE45NPdC6+T/Bj+yh5J8adc09oXiawfG0lOYt62FR9ZcBRMQfY+Hw8mmQWGu2Jqr6mNEOhIaRG6y35yieKiu4Gm+jy8S5feeRcF+cWmT43WoBFiw+zBXw/oNGavGY91YFqz+1OyB5UE9edKtK/NcEDBYxpPSN+kidmAJvClBsULQAAAABJRU5ErkJggg==)](/LICENSE)
