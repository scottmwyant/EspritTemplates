# Esprit Templates

This repository contains templates for EspritTNG Extensions.  EspritTNG provides Visual Studio templates.  The templates in this repository are based on those "OEM" templates, but are able to be modified directly.  Clone this repo to get started.

## Basic Extension

Project generated from the Visual Studio template found in the following directory:

C:\Program Files\D.P.Technology\ESPRIT TNG\Data\VSTemplates\ProjectTemplates\Visual C#\ESPRIT TNG\Basic Extension.zip

## Extension with Managed UI

Project generated from teh Visual Studio template found in the following directory:

C:\Program Files\D.P.Technology\ESPRIT TNG\Data\VSTemplates\ProjectTemplates\Visual C#\ESPRIT TNG\Managed Extension.zip

## The C# Tutorials Extension

The source code for a tutorials extension is provided with Esprit:

C:\Program Files\D.P.Technology\ESPRIT TNG\Help\1033\API Tutorial Samples

Reference: https://ew.dptechnology.com/ew/help/Esprit/TNG/en/20.409/main/API/_c_sharp_tutorials_enable.html

## How to get Esprit to see the extension

The extension dll needs to be dropped into one of these folders.  The public folder is used in these templates as the output folder for the build, so the latest build will automatically be visible to Esprit, next time it is opened.  The `\Prog\Extensions` folder can also be used.  It's not clear how these two locations co-exist.  What if we put the dll in both places, does it show up twice?  If the extension is the same name but different versions, does one folder consistently get loaded and not the other, it's not clear.  Answers to these questions may be in the documentation. 

`C:\Program Files\D.P.Technology\ESPRIT TNG\Prog\Extensions`

`C:\Users\Public\Documents\D.P.Technology\ESPRIT TNG\Data\Extensions`

## Developer resources

In the Esprit TNG application, `File -> Help -> API/MacroHelp` brings up the documentation for the API:

https://ew.dptechnology.com/ew/help/Esprit/TNG/en/20.409/main/API/index.html
