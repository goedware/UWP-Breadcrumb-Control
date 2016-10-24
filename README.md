# UWP-Breadcrumb-Control ![version](http://img.shields.io/badge/original-v1.0.2-brightgreen.svg?style=flat) [![NuGet](https://img.shields.io/nuget/v/GoedWare.Controls.Breadcrumb.svg?label=NuGet)](https://www.nuget.org/packages/GoedWare.Controls.Breadcrumb/)
Create a Breadcrumb control for your Universal Windows Platform (UWP) app in just a few minutes.

This library allows to generate a breadcrumb for your UWP app with less effort, it's fully customizable.

**XAML (full example with all properties used, see details how the properties work)**
```xaml
<breadcrumb:BreadcrumbControl
  DisplayMemberPath="Name"
  HomeCommand="{Binding HomeCommand}"
  HomeSelected="OnHomeSelected"
  HomeTemplate="{StaticResource HomeTemplate}"
  HomeText="Home"
  ItemTemplate="{StaticResource ItemTemplate}"
  ItemCommand="{Binding ItemCommand}"
  ItemSelected="OnItemSelected"
  Seperator="/"
  SeperatorTemplate="{StaticResource SeperatorTemplate}"
  ItemsSource="{Binding Items}"
  OverFlow="..."
  OverFlowTemplate="{StaticResource OverflowTemplate}"
/>
```

## Setup

Grab the latest version from NuGet

> PM Install-Package GoedWare.Controls.Breadcrumb

## Usage
### 1. Home 
The home item is the first item in the breadcrumb control and is always visible. You can specify the home item in 4 different ways:

* Do nothing and use the default template that consists of a SymbolIcon with the *Home* symbol
* Set the **HomeIcon** property. This will display the icon specified in the default template above
* Set the **HomeText** property. This will display text as the Home item
* Set the **HomeTemplate** property. This will display your specified datatemplate as Home item

If *HomeText* is specfied. The control will use that before any template or Icon. If not it will first use the template and else the icon specified.

### 2. Breadcrumb Items 
Breadcrumb items are the items that display your information in the breadcrumb and are selectable. You can specify the items in 2 different ways:

* Set the **DisplayMemberPath** property. This will display the specified member/path of the current datacontext
* Set the **ItemTemplate** property. This will display your specified datatemplate as breadcrumb item

If *ItemTemplate* is specified it will use that instead of any *DisplayMemberPath*.

### 3. Seperator Items 
Seperator items are the items that are displayed between your breadcrumb items and between the Home item and your first crumb. You can specify the items in 3 different ways:

* Do nothing and use the default seperator string */*
* Set the **Seperator** property. This will display the specfied string as seperator
* Set the **SeperatorTemplate** property. This will display your specified datatemplate as seperator item

If *SeperatorTemplate* is specified it will use that instead of *Seperator*.

### 4. Overflow Items 
Overflow items will replace your breadcrumb items when the control is too large (horizontal) to be displayed on the screen. *The last breadcrumb will never transform into an overflow item.* You can specify the items in 3 different ways:

* Do nothing and use the default overflow string *...*
* Set the **OverFlow** property. This will display the specfied string as overflow
* Set the **OverFlowTemplate** property. This will display your specified datatemplate as overflow item

If *OverFlowTemplate* is specified it will use that instead of *OverFlow*.

### 5. Home Selected 
You can specify your own action when the Home item is selected. You can specify the action in 2 different ways:

* Connect to the **HomeSelected** event. This event will occur when the Home item is selected
* Set the **HomeCommand** property. This will execute the command when the home item is selected

```csharp
private void OnHomeSelected(object sender, EventArgs e)
{
    // Do something
}
````

### 6. Item Selected 
You can specify your own action when a breadcrumb item is selected. You can specify the action in 2 different ways:

* Connect to the **ItemSelected** event. This event will occur when a breadcrumb item is selected
* Set the **ItemCommand** property. This will execute the command when breadcrumb item is selected

The event and command will have a **BreadcrumbEventArgs** as parameter that consists of:

* **Item** the current item that is selected
* **ItemIndex** the item index of the item that is selected

```csharp
private void OnItemSelected(object sender, BreadcrumbEventArgs e)
{
    var item = e.Item;
    var index = e.ItemIndex;
}
````

### 7. ItemsSource
You can specify the breadcrumb items of the control by setting the *ItemsSource* property. You can bind any IEnumerable to this control. If you use an ObservableCollection the control will update itself when you add or remove an item in the collection.

### 8 Customization

Customize the about items to your own style by overriding the default templates & styles:

* **ItemStyle:** Override to change the default style of the breadcrumb items
* **HomeItemStyle:** Override to change the default style of the home items

## Sample Project
[sample](https://github.com/goedware/UWP-Breadcrumb-Control/tree/master/GoedWare.Controls.Breadcrumb/GoedWare.Samples.Breadcrumb)

## License

```
The MIT License (MIT)

Copyright (c) 2016 C. Goedegebuure

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
