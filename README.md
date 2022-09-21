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

### Custom component UI
This library contains component `Attribute` methods to create:
- Bool6 component (six check boxes)
- Button component (single button for 'Open' components)
- Dropdown(s) with check boxes
- Dropdown(s) component
- Dropdown(s) with slider (for scaling results)
- Releases component (12 check boxes)
- Support component (three buttons with six check boxes)
- Three button component (for 'Save' components, buttons for 'Save', 'Save As' and 'Open in X')

### Abstract component classes
Inherit from `GH_OasysComponent`, `GH_OasysDropDownComponent` or `GH_OasysTaskCapableComponent<T>` to:
- Track usage with posthog
- Get simple scaffolding code to create for instance complex dropdown menus components. The abstract base classes take care of most things for you.
to be done

### Abstract parameter classes
to be done
