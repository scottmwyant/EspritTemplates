using System;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Win32;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class ConfigurationManagerTutorial : BaseTutorial
    {
        private readonly string _сonfigPrefix = "Esprit/Configurations/";
        private Form _form;

        private void Show()
        {
            _form = new Form();
            Button saveButton = new Button();
            Button loadButton = new Button();

            _form.Text = "ConfigurationManagerTutorial form";
            saveButton.Text = "Save";
            loadButton.Text = "Load";

            saveButton.SetBounds(9, 16, 75, 23);
            loadButton.SetBounds(89, 16, 75, 23);

            loadButton.AutoSize = saveButton.AutoSize = true;
            loadButton.Anchor = saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            _form.ClientSize = new System.Drawing.Size(169, 40);
            _form.Controls.AddRange(new Control[] { saveButton, loadButton });
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            saveButton.Click += OnSaveButtonClick;
            loadButton.Click += OnLoadButtonClick;

            _form.FormClosed += (o, e) => { _form = null; };

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet recall]

        private void OnLoadButtonClick(object sender, EventArgs e)
        {
            EspritApplication.EventWindow.Clear();

            var reg = RegistryHelper.GetRegistry();
            var count = int.Parse(reg.GetValue(_сonfigPrefix + "Number", 0).ToString());
            if (count == 0)
            {
                System.Windows.MessageBox.Show("No Configurations Exist to Recall", "ConfigurationManagerTutorial", MessageBoxButton.OK);
                return;
            }

            var name = reg.GetValue(_сonfigPrefix + count, "Configuration" + count).ToString();
            if (RequestUserInput("Enter Configuration Name", "Configuration Name", ref name))
            {
                bool isExist = false;
                for (var i = 1; i <= count; i++)
                {
                    if (reg.GetValue(_сonfigPrefix + i, 0).ToString() == name)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                {
                    System.Windows.MessageBox.Show($"Error: {_сonfigPrefix + name} Not Found", "ConfigurationManagerTutorial", MessageBoxButton.OK);
                    return;
                }

                var currentConfigPrefix = _сonfigPrefix + name + "/";
                var config = EspritApplication.Configuration;

                config.BmpFile.Height = int.Parse(reg.GetValue(currentConfigPrefix + "BmpFile/Height", config.BmpFile.Height).ToString());
                config.BmpFile.Width = int.Parse(reg.GetValue(currentConfigPrefix + "BmpFile/Width", config.BmpFile.Width).ToString());
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"BmpFile: Height = {config.BmpFile.Height}, Width = {config.BmpFile.Width}");


                config.ConfigurationAnnotation.Font.Bold = bool.Parse(reg.GetValue(currentConfigPrefix + "CAnnotation/Font/Bold", config.ConfigurationAnnotation.Font.Bold).ToString());
                config.ConfigurationAnnotation.Font.Charset = short.Parse(reg.GetValue(currentConfigPrefix + "CAnnotation/Font/Charset", config.ConfigurationAnnotation.Font.Charset).ToString());
                config.ConfigurationAnnotation.Font.Italic = bool.Parse(reg.GetValue(currentConfigPrefix + "CAnnotation/Font/Italic", config.ConfigurationAnnotation.Font.Italic).ToString());
                config.ConfigurationAnnotation.Font.Name = reg.GetValue(currentConfigPrefix + "CAnnotation/Font/Name", config.ConfigurationAnnotation.Font.Name).ToString();
                config.ConfigurationAnnotation.Font.Size = decimal.Parse(reg.GetValue(currentConfigPrefix + "CAnnotation/Font/Size", config.ConfigurationAnnotation.Font.Size).ToString());
                config.ConfigurationAnnotation.Font.Strikethrough = bool.Parse(reg.GetValue(currentConfigPrefix + "CAnnotation/Font/Strikethrough", config.ConfigurationAnnotation.Font.Strikethrough).ToString());
                config.ConfigurationAnnotation.Font.Underline = bool.Parse(reg.GetValue(currentConfigPrefix + "CAnnotation/Font/Underline", config.ConfigurationAnnotation.Font.Underline).ToString());
                config.ConfigurationAnnotation.Font.Weight = short.Parse(reg.GetValue(currentConfigPrefix + "CAnnotation/Font/Weight", config.ConfigurationAnnotation.Font.Weight).ToString());
                config.ConfigurationAnnotation.HeightSpacing = double.Parse(reg.GetValue(currentConfigPrefix + "CAnnotation/HeightSpacing", config.ConfigurationAnnotation.HeightSpacing).ToString());
                EspritConstants.espFontJustification fontJustification;
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CAnnotation/Justification", config.ConfigurationAnnotation.Justification).ToString(), out fontJustification);
                config.ConfigurationAnnotation.Justification = fontJustification;
                config.ConfigurationAnnotation.RotationAngle = double.Parse(reg.GetValue(currentConfigPrefix + "CAnnotation/RotationAngle", config.ConfigurationAnnotation.RotationAngle).ToString());

                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CAnnotation: Font Bold = {config.ConfigurationAnnotation.Font.Bold}, Font Charset = {config.ConfigurationAnnotation.Font.Charset}, Font Italic = {config.ConfigurationAnnotation.Font.Italic}, " +
                    $"Font Name = { config.ConfigurationAnnotation.Font.Name}, Font Size = { config.ConfigurationAnnotation.Font.Size}, Font Strikethrough = { config.ConfigurationAnnotation.Font.Strikethrough}, " +
                    $"Font Underline = { config.ConfigurationAnnotation.Font.Underline}, Font Weight = {config.ConfigurationAnnotation.Font.Weight }, HeightSpacing = {config.ConfigurationAnnotation.HeightSpacing }, " +
                    $"Justification = { config.ConfigurationAnnotation.Justification}, RotationAngle = {config.ConfigurationAnnotation.RotationAngle }");

                config.ConfigurationArcs.Color = uint.Parse(reg.GetValue(currentConfigPrefix + "CArcs/Color", config.ConfigurationArcs.Color).ToString());
                EspritConstants.espLineType lineType;
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CArcs/LineType", config.ConfigurationArcs.LineType).ToString(), out lineType);
                config.ConfigurationArcs.LineType = lineType;
                EspritConstants.espLineWeight lineWeight;
                Enum.TryParse(reg.GetValue(_сonfigPrefix + "CArcs/LineWeight", config.ConfigurationArcs.LineWeight).ToString(), out lineWeight);
                config.ConfigurationArcs.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CArcs: Color = {config.ConfigurationArcs.Color}, LineType = {config.ConfigurationArcs.LineType}, LineWeight = {config.ConfigurationArcs.LineWeight}");

                config.ConfigurationBackground.Color = uint.Parse(reg.GetValue(currentConfigPrefix + "CBackground/Color", config.ConfigurationBackground.Color).ToString());
                config.ConfigurationBackground.Gradient = bool.Parse(reg.GetValue(currentConfigPrefix + "CBackground/Gradient", config.ConfigurationBackground.Gradient).ToString());
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CBackground: Color = {config.ConfigurationBackground.Color}, Gradient = {config.ConfigurationBackground.Gradient}");

                config.ConfigurationCheckSurfaces.Color = uint.Parse(reg.GetValue(currentConfigPrefix + "CCheckSurfaces/Color", config.ConfigurationCheckSurfaces.Color).ToString());
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CCheckSurfaces/LineType", config.ConfigurationCheckSurfaces.LineType).ToString(), out lineType);
                config.ConfigurationCheckSurfaces.LineType = lineType;
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CCheckSurfaces/LineWeight", config.ConfigurationCheckSurfaces.LineWeight).ToString(), out lineWeight);
                config.ConfigurationCheckSurfaces.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CCheckSurfaces: Color = {config.ConfigurationCheckSurfaces.Color}, LineType = {config.ConfigurationCheckSurfaces.LineType}, LineWeight = {config.ConfigurationCheckSurfaces.LineWeight}");

                config.ConfigurationCircles.Color = uint.Parse(reg.GetValue( currentConfigPrefix + "CCircles/Color", config.ConfigurationCircles.Color).ToString());
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CCircles/LineType", config.ConfigurationCircles.LineType).ToString(), out lineType);
                config.ConfigurationCircles.LineType = lineType;
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CCircles/LineWeight", config.ConfigurationCircles.LineWeight).ToString(), out lineWeight);
                config.ConfigurationCircles.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CCircles: Color = {config.ConfigurationCircles.Color}, LineType = {config.ConfigurationCircles.LineType}, LineWeight = {config.ConfigurationCircles.LineWeight}");

                config.ConfigurationCurves.Color = uint.Parse(reg.GetValue(currentConfigPrefix + "CCurves/Color", config.ConfigurationCurves.Color).ToString());
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CCurves/LineType", config.ConfigurationCurves.LineType).ToString(), out lineType);
                config.ConfigurationCurves.LineType = lineType;
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CCurves/LineWeight", config.ConfigurationCurves.LineWeight).ToString(), out lineWeight);
                config.ConfigurationCurves.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CCurves: Color = {config.ConfigurationCurves.Color}, LineType = {config.ConfigurationCurves.LineType}, LineWeight = {config.ConfigurationCurves.LineWeight}");

                config.ConfigurationDimensions.Color = uint.Parse(reg.GetValue(currentConfigPrefix + "CDimensions/Color", config.ConfigurationDimensions.Color).ToString());
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CDimensions/LineType", config.ConfigurationDimensions.LineType).ToString(), out lineType);
                config.ConfigurationDimensions.LineType = lineType;
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CDimensions/LineWeight", config.ConfigurationDimensions.LineWeight).ToString(), out lineWeight);
                config.ConfigurationDimensions.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CDimensions: Color = {config.ConfigurationDimensions.Color}, LineType = {config.ConfigurationDimensions.LineType}, LineWeight = {config.ConfigurationDimensions.LineWeight}");

                config.ConfigurationFeatureChains.Color = uint.Parse(reg.GetValue(currentConfigPrefix + "CFeatureChains/Color", config.ConfigurationFeatureChains.Color).ToString());
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CFeatureChains/LineType", config.ConfigurationFeatureChains.LineType).ToString(), out lineType);
                config.ConfigurationFeatureChains.LineType = lineType;
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CFeatureChains/LineWeight", config.ConfigurationFeatureChains.LineWeight).ToString(), out lineWeight);
                config.ConfigurationFeatureChains.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CFeatureChains: Color = {config.ConfigurationFeatureChains.Color}, LineType = {config.ConfigurationFeatureChains.LineType}, LineWeight = {config.ConfigurationFeatureChains.LineWeight}");

                config.ConfigurationFeaturePtops.Color = uint.Parse(reg.GetValue(currentConfigPrefix + "CFeaturePtops/Color", config.ConfigurationFeaturePtops.Color).ToString());
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CFeaturePtops/LineType", config.ConfigurationFeaturePtops.LineType).ToString(), out lineType);
                config.ConfigurationFeaturePtops.LineType = lineType;
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CFeaturePtops/LineWeight", config.ConfigurationFeaturePtops.LineWeight).ToString(), out lineWeight);
                config.ConfigurationFeaturePtops.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CFeaturePtops: Color = {config.ConfigurationFeaturePtops.Color}, LineType = {config.ConfigurationFeaturePtops.LineType}, LineWeight = {config.ConfigurationFeaturePtops.LineWeight}");

                config.ConfigurationGroupItems.Color = uint.Parse(reg.GetValue(currentConfigPrefix + "CGroupItems/Color", config.ConfigurationGroupItems.Color).ToString());
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CGroupItems/LineType", config.ConfigurationGroupItems.LineType).ToString(), out lineType);
                config.ConfigurationGroupItems.LineType = lineType;
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CGroupItems/LineWeight", config.ConfigurationGroupItems.LineWeight).ToString(), out lineWeight);
                config.ConfigurationGroupItems.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CGroupItems: Color = {config.ConfigurationGroupItems.Color}, LineType = {config.ConfigurationGroupItems.LineType}, LineWeight = {config.ConfigurationGroupItems.LineWeight}");

                config.ConfigurationHatch.Angle = double.Parse(reg.GetValue(currentConfigPrefix + "CHatch/Angle", config.ConfigurationHatch.Angle).ToString());
                config.ConfigurationHatch.Step = double.Parse(reg.GetValue(currentConfigPrefix + "CHatch/Step", config.ConfigurationHatch.Step).ToString());
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CHatch: Ange = {config.ConfigurationHatch.Angle}, Step = {config.ConfigurationHatch.Step}");

                config.ConfigurationLines.Color = uint.Parse(reg.GetValue(currentConfigPrefix + "CLines/Color", config.ConfigurationLines.Color).ToString());
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CLines/LineType", config.ConfigurationLines.LineType).ToString(), out lineType);
                config.ConfigurationLines.LineType = lineType;
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CLines/LineWeight", config.ConfigurationLines.LineWeight).ToString(), out lineWeight);
                config.ConfigurationLines.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CLines: Color = {config.ConfigurationLines.Color}, LineType = {config.ConfigurationLines.LineType}, LineWeight = {config.ConfigurationLines.LineWeight}");

                config.ConfigurationNotes.Color = uint.Parse(reg.GetValue( currentConfigPrefix + "CNotes/Color", config.ConfigurationNotes.Color).ToString());
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CNotes: Color = {config.ConfigurationNotes.Color}");

                config.ConfigurationOperations.Color = uint.Parse(reg.GetValue( currentConfigPrefix + "COperations/Color", config.ConfigurationOperations.Color).ToString());
                Enum.TryParse(reg.GetValue( currentConfigPrefix + "COperations/LineType", config.ConfigurationOperations.LineType).ToString(), out lineType);
                config.ConfigurationOperations.LineType = lineType;
                Enum.TryParse(reg.GetValue( currentConfigPrefix + "COperations/LineWeight", config.ConfigurationOperations.LineWeight).ToString(), out lineWeight);
                config.ConfigurationOperations.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"COperations: Color = {config.ConfigurationOperations.Color}, LineType = {config.ConfigurationOperations.LineType}, LineWeight = {config.ConfigurationOperations.LineWeight}");

                config.ConfigurationPartSurfaces.Color = uint.Parse(reg.GetValue( currentConfigPrefix + "CPartSurfaces/Color", config.ConfigurationPartSurfaces.Color).ToString());
                Enum.TryParse(reg.GetValue( currentConfigPrefix + "CPartSurfaces/LineType", config.ConfigurationPartSurfaces.LineType).ToString(), out lineType);
                config.ConfigurationPartSurfaces.LineType = lineType;
                Enum.TryParse(reg.GetValue( currentConfigPrefix + "CPartSurfaces/LineWeight", config.ConfigurationPartSurfaces.LineWeight).ToString(), out lineWeight);
                config.ConfigurationPartSurfaces.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CPartSurfaces: Color = {config.ConfigurationPartSurfaces.Color}, LineType = {config.ConfigurationPartSurfaces.LineType}, LineWeight = {config.ConfigurationPartSurfaces.LineWeight}");

                config.ConfigurationPoints.Color = uint.Parse(reg.GetValue( currentConfigPrefix + "CPoints/Color", config.ConfigurationPoints.Color).ToString());
                Enum.TryParse(reg.GetValue( currentConfigPrefix + "CPoints/LineWeight", config.ConfigurationPoints.LineWeight).ToString(), out lineWeight);
                config.ConfigurationPoints.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CPoints: Color = {config.ConfigurationPoints.Color}, LineWeight = {config.ConfigurationPoints.LineWeight}");

                config.ConfigurationSegments.Color = uint.Parse(reg.GetValue(_сonfigPrefix + "CSegments/Color", config.ConfigurationSegments.Color).ToString());
                Enum.TryParse(reg.GetValue( currentConfigPrefix + "CSegments/LineType", config.ConfigurationSegments.LineType).ToString(), out lineType);
                config.ConfigurationSegments.LineType = lineType;
                Enum.TryParse(reg.GetValue( currentConfigPrefix + "CSegments/LineWeight", config.ConfigurationSegments.LineWeight).ToString(), out lineWeight);
                config.ConfigurationSegments.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CSegments: Color = {config.ConfigurationSegments.Color}, LineType = {config.ConfigurationSegments.LineType}, LineWeight = {config.ConfigurationSegments.LineWeight}");

                config.ConfigurationSolidModel.Color = uint.Parse(reg.GetValue( currentConfigPrefix + "CSolidModel/Color", config.ConfigurationSolidModel.Color).ToString());
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CSolidModel: Color = {config.ConfigurationSolidModel.Color}");

                config.ConfigurationSurfaceGeometry.Color = uint.Parse(reg.GetValue(currentConfigPrefix + "CSurfaceGeometry/Color", config.ConfigurationSurfaceGeometry.Color).ToString());
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CSurfaceGeometry/LineType", config.ConfigurationSurfaceGeometry.LineType).ToString(), out lineType);
                config.ConfigurationSurfaceGeometry.LineType = lineType;
                Enum.TryParse(reg.GetValue(currentConfigPrefix + "CSurfaceGeometry/LineWeight", config.ConfigurationSurfaceGeometry.LineWeight).ToString(), out lineWeight);
                config.ConfigurationSurfaceGeometry.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CSurfaceGeometry: Color = {config.ConfigurationSurfaceGeometry.Color}, LineType = {config.ConfigurationSurfaceGeometry.LineType}, LineWeight = {config.ConfigurationSurfaceGeometry.LineWeight}");

                config.ConfigurationSurfaces.Color = uint.Parse(reg.GetValue( currentConfigPrefix + "CSurfaces/Color", config.ConfigurationSurfaces.Color).ToString());
                Enum.TryParse(reg.GetValue( currentConfigPrefix + "CSurfaces/LineType", config.ConfigurationSurfaces.LineType).ToString(), out lineType);
                config.ConfigurationSurfaces.LineType = lineType;
                Enum.TryParse(reg.GetValue( currentConfigPrefix + "CSurfaces/LineWeight", config.ConfigurationSurfaces.LineWeight).ToString(), out lineWeight);
                config.ConfigurationSurfaces.LineWeight = lineWeight;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CSurfaces: Color = {config.ConfigurationSurfaces.Color}, LineType = {config.ConfigurationSurfaces.LineType}, LineWeight = {config.ConfigurationSurfaces.LineWeight}");

                config.ConfigurationAnnotation.Font.Italic = bool.Parse(reg.GetValue( currentConfigPrefix + "EnableGridMode", config.EnableGridMode).ToString());
                config.GapTolerance = double.Parse(reg.GetValue( currentConfigPrefix + "GapTolerance", config.GapTolerance).ToString());
                config.GridAngle = double.Parse(reg.GetValue( currentConfigPrefix + "GridAngle", config.GridAngle).ToString());

                double gridDx, gridDy, gridDz, gridRadius;
                if (Document.SystemUnit == EspritConstants.espUnitType.espInch)
                {
                    config.GridDxInch = double.Parse(reg.GetValue( currentConfigPrefix + "GridDx", config.GridDxInch).ToString());
                    gridDx = config.GridDxInch;
                }
                else
                {
                    config.GridDxMetric = double.Parse(reg.GetValue( currentConfigPrefix + "GridDx", config.GridDxMetric).ToString());
                    gridDx = config.GridDxMetric;
                }
                if (Document.SystemUnit == EspritConstants.espUnitType.espInch)
                {
                    config.GridDyInch = double.Parse(reg.GetValue( currentConfigPrefix + "GridDy", config.GridDyInch).ToString());
                    gridDy = config.GridDyInch;
                }
                else
                {
                    config.GridDyMetric = double.Parse(reg.GetValue( currentConfigPrefix + "GridDy", config.GridDyMetric).ToString());
                    gridDy = config.GridDyMetric;
                }
                if (Document.SystemUnit == EspritConstants.espUnitType.espInch)
                {
                    config.GridDzInch = double.Parse(reg.GetValue( currentConfigPrefix + "GridDz", config.GridDzInch).ToString());
                    gridDz = config.GridDzInch;
                }
                else
                {
                    config.GridDzMetric = double.Parse(reg.GetValue( currentConfigPrefix + "GridDz", config.GridDzMetric).ToString());
                    gridDz = config.GridDzMetric;
                }
                if (Document.SystemUnit == EspritConstants.espUnitType.espInch)
                {
                    config.GridRadiusInch = double.Parse(reg.GetValue( currentConfigPrefix + "GridRadius", config.GridRadiusInch).ToString());
                    gridRadius = config.GridRadiusInch;
                }
                else
                {
                    config.GridRadiusMetric = double.Parse(reg.GetValue(_сonfigPrefix + "GridRadius", config.GridRadiusMetric).ToString());
                    gridRadius = config.GridRadiusMetric;
                }

                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"Grid: EnableGridMode = {config.EnableGridMode}, GapTolerance = {config.GapTolerance}, GridAngle = {config.GridAngle}, " +
                    $"GridDx = {gridDx}, GridDy = {gridDy}, GridDz = {gridDz}, GridRadius = {gridRadius}");

                config.PlanarCreation = bool.Parse(reg.GetValue( currentConfigPrefix + "PlanarCreation", config.PlanarCreation).ToString());
                config.PromptZValues = bool.Parse(reg.GetValue( currentConfigPrefix + "PromptZValues", config.PromptZValues).ToString());
                config.ShadedResolution = double.Parse(reg.GetValue( currentConfigPrefix + "ShadedResolution", config.ShadedResolution).ToString());
                config.ShowUVWAxis = bool.Parse(reg.GetValue( currentConfigPrefix + "ShowUVWAxis", config.ShowUVWAxis).ToString());
                config.ShowXYZAxis = bool.Parse(reg.GetValue( currentConfigPrefix + "ShowXYZAxis", config.ShowXYZAxis).ToString());
                config.SurfaceSolidTolerance = double.Parse(reg.GetValue( currentConfigPrefix + "SurfaceSolidTolerance", config.SurfaceSolidTolerance).ToString());
                config.SurfaceWireFrameGridFirstDirection = int.Parse(reg.GetValue( currentConfigPrefix + "SurfaceWireFrameGridFirstDirection", config.SurfaceWireFrameGridFirstDirection).ToString());
                config.SurfaceWireFrameGridSecondDirection = int.Parse(reg.GetValue( currentConfigPrefix + "SurfaceWireFrameGridSecondDirection", config.SurfaceWireFrameGridSecondDirection).ToString());
                EspritConstants.espUnitType unitType;
                Enum.TryParse(reg.GetValue( currentConfigPrefix + "SystemUnit", config.SystemUnit).ToString(), out unitType);
                config.SystemUnit = unitType;
                EspritConstants.espTechPageComment techPageComment;
                Enum.TryParse(reg.GetValue( currentConfigPrefix + "TechPageComment", config.TechPageComment).ToString(), out techPageComment);
                config.TechPageComment = techPageComment;
                config.TechPageShowCustomTab = bool.Parse(reg.GetValue( currentConfigPrefix + "TechPageShowCustomTab", config.TechPageShowCustomTab).ToString());
                config.TechPageShowIcons = bool.Parse(reg.GetValue( currentConfigPrefix + "TechPageShowIcons", config.TechPageShowIcons).ToString());
                config.WireFrameResolution = double.Parse(reg.GetValue( currentConfigPrefix + "WireFrameResolution", config.WireFrameResolution).ToString());

                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"PlanarCreation = {config.PlanarCreation}, PromptZValues = {config.PromptZValues}, ShadedResolution = {config.ShadedResolution}, " +
                    $"ShowUVWAxis = {config.ShowUVWAxis}, ShowXYZAxis = {config.ShowXYZAxis}, SurfaceSolidTolerance = {config.SurfaceSolidTolerance}, " +
                    $"SurfaceWireFrameGridFirstDirection = {config.SurfaceWireFrameGridFirstDirection}, SurfaceWireFrameGridSecondDirection = {config.SurfaceWireFrameGridSecondDirection}, " +
                    $"SystemUnit = {config.SystemUnit}, TechPageComment = {config.TechPageComment}, TechPageShowCustomTab = {config.TechPageShowCustomTab}, " +
                    $"TechPageShowIcons = {config.TechPageShowIcons}, WireFrameResolution = {config.WireFrameResolution}");

                Document.Refresh();
            }
        }

        //! [Code snippet recall]

        //! [Code snippet save]

        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            var reg = RegistryHelper.GetRegistry();
            var count = int.Parse(reg.GetValue(_сonfigPrefix + "Number", 0).ToString());
            var index = (count + 1);
            var name = "Configuration" + index;

            var dialogResult = RequestUserInput("Enter Configuration Name", "Configuration Name", ref name);
            if (dialogResult && name.Length > 0 && name != "Number" && !int.TryParse(name, out var v))
            {
                bool isExist = false;
                for(var i = 1; i <= count; i++)
                {
                    if (reg.GetValue(_сonfigPrefix + i, 0).ToString() == name)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                {
                    reg.SetValue(_сonfigPrefix + "Number", index);
                    reg.SetValue(_сonfigPrefix + index, name);
                }

                var currentConfigPrefix = _сonfigPrefix + name + "/";
                var config = EspritApplication.Configuration;

                reg.SetValue(currentConfigPrefix + "BmpFile/Height", config.BmpFile.Height);
                reg.SetValue(currentConfigPrefix + "BmpFile/Width", config.BmpFile.Width);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"BmpFile: Height = {config.BmpFile.Height}, Width = {config.BmpFile.Width}");

                reg.SetValue(currentConfigPrefix + "CAnnotation/Font/Bold", config.ConfigurationAnnotation.Font.Bold);
                reg.SetValue(currentConfigPrefix + "CAnnotation/Font/Charset", config.ConfigurationAnnotation.Font.Charset);
                reg.SetValue(currentConfigPrefix + "CAnnotation/Font/Italic", config.ConfigurationAnnotation.Font.Italic);
                reg.SetValue(currentConfigPrefix + "CAnnotation/Font/Name", config.ConfigurationAnnotation.Font.Name);
                reg.SetValue(currentConfigPrefix + "CAnnotation/Font/Size", config.ConfigurationAnnotation.Font.Size);
                reg.SetValue(currentConfigPrefix + "CAnnotation/Font/Strikethrough", config.ConfigurationAnnotation.Font.Strikethrough);
                reg.SetValue(currentConfigPrefix + "CAnnotation/Font/Underline", config.ConfigurationAnnotation.Font.Underline);
                reg.SetValue(currentConfigPrefix + "CAnnotation/Font/Weight", config.ConfigurationAnnotation.Font.Weight);
                reg.SetValue(currentConfigPrefix + "CAnnotation/HeightSpacing", config.ConfigurationAnnotation.HeightSpacing);
                reg.SetValue(currentConfigPrefix + "CAnnotation/Justification", config.ConfigurationAnnotation.Justification);
                reg.SetValue(currentConfigPrefix + "CAnnotation/RotationAngle", config.ConfigurationAnnotation.RotationAngle);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CAnnotation: Font Bold = {config.ConfigurationAnnotation.Font.Bold}, Font Charset = {config.ConfigurationAnnotation.Font.Charset}, Font Italic = {config.ConfigurationAnnotation.Font.Italic}, " +
                    $"Font Name = { config.ConfigurationAnnotation.Font.Name}, Font Size = { config.ConfigurationAnnotation.Font.Size}, Font Strikethrough = { config.ConfigurationAnnotation.Font.Strikethrough}, " +
                    $"Font Underline = { config.ConfigurationAnnotation.Font.Underline}, Font Weight = {config.ConfigurationAnnotation.Font.Weight }, HeightSpacing = {config.ConfigurationAnnotation.HeightSpacing }, " +
                    $"Justification = { config.ConfigurationAnnotation.Justification}, RotationAngle = {config.ConfigurationAnnotation.RotationAngle }");

                reg.SetValue(currentConfigPrefix + "CArcs/Color", config.ConfigurationArcs.Color);
                reg.SetValue(currentConfigPrefix + "CArcs/LineType", config.ConfigurationArcs.LineType);
                reg.SetValue(currentConfigPrefix + "CArcs/LineWeight", config.ConfigurationArcs.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CArcs: Color = {config.ConfigurationArcs.Color}, LineType = {config.ConfigurationArcs.LineType}, LineWeight = {config.ConfigurationArcs.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CBackground/Color", config.ConfigurationBackground.Color);
                reg.SetValue(currentConfigPrefix + "CBackground/Gradient", config.ConfigurationBackground.Gradient);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CBackground: Color = {config.ConfigurationBackground.Color}, Gradient = {config.ConfigurationBackground.Gradient}");

                reg.SetValue(currentConfigPrefix + "CCheckSurfaces/Color", config.ConfigurationCheckSurfaces.Color);
                reg.SetValue(currentConfigPrefix + "CCheckSurfaces/LineType", config.ConfigurationCheckSurfaces.LineType);
                reg.SetValue(currentConfigPrefix + "CCheckSurfaces/LineWeight", config.ConfigurationCheckSurfaces.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CCheckSurfaces: Color = {config.ConfigurationCheckSurfaces.Color}, LineType = {config.ConfigurationCheckSurfaces.LineType}, LineWeight = {config.ConfigurationCheckSurfaces.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CCircles/Color", config.ConfigurationCircles.Color);
                reg.SetValue(currentConfigPrefix + "CCircles/LineType", config.ConfigurationCircles.LineType);
                reg.SetValue(currentConfigPrefix + "CCircles/LineWeight", config.ConfigurationCircles.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CCircles: Color = {config.ConfigurationCircles.Color}, LineType = {config.ConfigurationCircles.LineType}, LineWeight = {config.ConfigurationCircles.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CCurves/Color", config.ConfigurationCurves.Color);
                reg.SetValue(currentConfigPrefix + "CCurves/LineType", config.ConfigurationCurves.LineType);
                reg.SetValue(currentConfigPrefix + "CCurves/LineWeight", config.ConfigurationCurves.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CCurves: Color = {config.ConfigurationCurves.Color}, LineType = {config.ConfigurationCurves.LineType}, LineWeight = {config.ConfigurationCurves.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CDimensions/Color", config.ConfigurationDimensions.Color);
                reg.SetValue(currentConfigPrefix + "CDimensions/LineType", config.ConfigurationDimensions.LineType);
                reg.SetValue(currentConfigPrefix + "CDimensions/LineWeight", config.ConfigurationDimensions.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CDimensions: Color = {config.ConfigurationDimensions.Color}, LineType = {config.ConfigurationDimensions.LineType}, LineWeight = {config.ConfigurationDimensions.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CFeatureChains/Color", config.ConfigurationFeatureChains.Color);
                reg.SetValue(currentConfigPrefix + "CFeatureChains/LineType", config.ConfigurationFeatureChains.LineType);
                reg.SetValue(currentConfigPrefix + "CFeatureChains/LineWeight", config.ConfigurationFeatureChains.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CFeatureChains: Color = {config.ConfigurationFeatureChains.Color}, LineType = {config.ConfigurationFeatureChains.LineType}, LineWeight = {config.ConfigurationFeatureChains.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CFeaturePtops/Color", config.ConfigurationFeaturePtops.Color);
                reg.SetValue(currentConfigPrefix + "CFeaturePtops/LineType", config.ConfigurationFeaturePtops.LineType);
                reg.SetValue(currentConfigPrefix + "CFeaturePtops/LineWeight", config.ConfigurationFeaturePtops.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CFeaturePtops: Color = {config.ConfigurationFeaturePtops.Color}, LineType = {config.ConfigurationFeaturePtops.LineType}, LineWeight = {config.ConfigurationFeaturePtops.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CGroupItems/Color", config.ConfigurationGroupItems.Color);
                reg.SetValue(currentConfigPrefix + "CGroupItems/LineType", config.ConfigurationGroupItems.LineType);
                reg.SetValue(currentConfigPrefix + "CGroupItems/LineWeight", config.ConfigurationGroupItems.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CGroupItems: Color = {config.ConfigurationGroupItems.Color}, LineType = {config.ConfigurationGroupItems.LineType}, LineWeight = {config.ConfigurationGroupItems.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CHatch/Angle", config.ConfigurationHatch.Angle);
                reg.SetValue(currentConfigPrefix + "CHatch/Step", config.ConfigurationHatch.Step);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CHatch: Ange = {config.ConfigurationHatch.Angle}, Step = {config.ConfigurationHatch.Step}");

                reg.SetValue(currentConfigPrefix + "CLines/Color", config.ConfigurationLines.Color);
                reg.SetValue(currentConfigPrefix + "CLines/LineType", config.ConfigurationLines.LineType);
                reg.SetValue(currentConfigPrefix + "CLines/LineWeight", config.ConfigurationLines.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial", 
                    $"CLines: Color = {config.ConfigurationLines.Color}, LineType = {config.ConfigurationLines.LineType}, LineWeight = {config.ConfigurationLines.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CNotes/Color", config.ConfigurationNotes.Color);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CNotes: Color = {config.ConfigurationNotes.Color}");

                reg.SetValue(currentConfigPrefix + "COperations/Color", config.ConfigurationOperations.Color);
                reg.SetValue(currentConfigPrefix + "COperations/LineType", config.ConfigurationOperations.LineType);
                reg.SetValue(currentConfigPrefix + "COperations/LineWeight", config.ConfigurationOperations.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"COperations: Color = {config.ConfigurationOperations.Color}, LineType = {config.ConfigurationOperations.LineType}, LineWeight = {config.ConfigurationOperations.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CPartSurfaces/Color", config.ConfigurationPartSurfaces.Color);
                reg.SetValue(currentConfigPrefix + "CPartSurfaces/LineType", config.ConfigurationPartSurfaces.LineType);
                reg.SetValue(currentConfigPrefix + "CPartSurfaces/LineWeight", config.ConfigurationPartSurfaces.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CPartSurfaces: Color = {config.ConfigurationPartSurfaces.Color}, LineType = {config.ConfigurationPartSurfaces.LineType}, LineWeight = {config.ConfigurationPartSurfaces.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CPoints/Color", config.ConfigurationPoints.Color);
                reg.SetValue(currentConfigPrefix + "CPoints/LineWeight", config.ConfigurationPoints.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CPoints: Color = {config.ConfigurationPoints.Color}, LineWeight = {config.ConfigurationPoints.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CSegments/Color", config.ConfigurationSegments.Color);
                reg.SetValue(currentConfigPrefix + "CSegments/LineType", config.ConfigurationSegments.LineType);
                reg.SetValue(currentConfigPrefix + "CSegments/LineWeight", config.ConfigurationSegments.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CSegments: Color = {config.ConfigurationSegments.Color}, LineType = {config.ConfigurationSegments.LineType}, LineWeight = {config.ConfigurationSegments.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CSolidModel/Color", config.ConfigurationSolidModel.Color);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CSolidModel: Color = {config.ConfigurationSolidModel.Color}");

                reg.SetValue(currentConfigPrefix + "CSurfaceGeometry/Color", config.ConfigurationSurfaceGeometry.Color);
                reg.SetValue(currentConfigPrefix + "CSurfaceGeometry/LineType", config.ConfigurationSurfaceGeometry.LineType);
                reg.SetValue(currentConfigPrefix + "CSurfaceGeometry/LineWeight", config.ConfigurationSurfaceGeometry.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CSurfaceGeometry: Color = {config.ConfigurationSurfaceGeometry.Color}, LineType = {config.ConfigurationSurfaceGeometry.LineType}, LineWeight = {config.ConfigurationSurfaceGeometry.LineWeight}");

                reg.SetValue(currentConfigPrefix + "CSurfaces/Color", config.ConfigurationSurfaces.Color);
                reg.SetValue(currentConfigPrefix + "CSurfaces/LineType", config.ConfigurationSurfaces.LineType);
                reg.SetValue(currentConfigPrefix + "CSurfaces/LineWeight", config.ConfigurationSurfaces.LineWeight);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"CSurfaces: Color = {config.ConfigurationSurfaces.Color}, LineType = {config.ConfigurationSurfaces.LineType}, LineWeight = {config.ConfigurationSurfaces.LineWeight}");

                reg.SetValue(currentConfigPrefix + "EnableGridMode", config.EnableGridMode);
                reg.SetValue(currentConfigPrefix + "GapTolerance", config.GapTolerance);
                reg.SetValue(currentConfigPrefix + "GridAngle", config.GridAngle);
                var gridDx = Document.SystemUnit == EspritConstants.espUnitType.espInch
                    ? config.GridDxInch
                    : config.GridDxMetric;
                var gridDy = Document.SystemUnit == EspritConstants.espUnitType.espInch
                    ? config.GridDyInch
                    : config.GridDyMetric;
                var gridDz = Document.SystemUnit == EspritConstants.espUnitType.espInch
                    ? config.GridDzInch
                    : config.GridDzMetric;
                var gridRadius = Document.SystemUnit == EspritConstants.espUnitType.espInch
                    ? config.GridRadiusInch
                    : config.GridRadiusMetric;

                reg.SetValue(currentConfigPrefix + "GridDx", gridDx);
                reg.SetValue(currentConfigPrefix + "GridDy", gridDy);
                reg.SetValue(currentConfigPrefix + "GridDz", gridDz);
                reg.SetValue(currentConfigPrefix + "GridRadius", gridRadius);
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"Grid: EnableGridMode = {config.EnableGridMode}, GapTolerance = {config.GapTolerance}, GridAngle = {config.GridAngle}, " +
                    $"GridDx = {gridDx}, GridDy = {gridDy}, GridDz = {gridDz}, GridRadius = {gridRadius}");

                reg.SetValue(currentConfigPrefix + "PlanarCreation", config.PlanarCreation);
                reg.SetValue(currentConfigPrefix + "PromptZValues", config.PromptZValues);
                reg.SetValue(currentConfigPrefix + "ShadedResolution", config.ShadedResolution);
                reg.SetValue(currentConfigPrefix + "ShowUVWAxis", config.ShowUVWAxis);
                reg.SetValue(currentConfigPrefix + "ShowXYZAxis", config.ShowXYZAxis);
                reg.SetValue(currentConfigPrefix + "SurfaceSolidTolerance", config.SurfaceSolidTolerance);
                reg.SetValue(currentConfigPrefix + "SurfaceWireFrameGridFirstDirection", config.SurfaceWireFrameGridFirstDirection);
                reg.SetValue(currentConfigPrefix + "SurfaceWireFrameGridSecondDirection", config.SurfaceWireFrameGridSecondDirection);
                reg.SetValue(currentConfigPrefix + "SystemUnit", config.SystemUnit);
                reg.SetValue(currentConfigPrefix + "TechPageComment", config.TechPageComment);
                reg.SetValue(currentConfigPrefix + "TechPageShowCustomTab", config.TechPageShowCustomTab);
                reg.SetValue(currentConfigPrefix + "TechPageShowIcons", config.TechPageShowIcons);
                reg.SetValue(currentConfigPrefix + "WireFrameResolution", config.WireFrameResolution);

                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ConfigurationManagerTutorial",
                    $"PlanarCreation = {config.PlanarCreation}, PromptZValues = {config.PromptZValues}, ShadedResolution = {config.ShadedResolution}, " +
                    $"ShowUVWAxis = {config.ShowUVWAxis}, ShowXYZAxis = {config.ShowXYZAxis}, SurfaceSolidTolerance = {config.SurfaceSolidTolerance}, " +
                    $"SurfaceWireFrameGridFirstDirection = {config.SurfaceWireFrameGridFirstDirection}, SurfaceWireFrameGridSecondDirection = {config.SurfaceWireFrameGridSecondDirection}, " +
                    $"SystemUnit = {config.SystemUnit}, TechPageComment = {config.TechPageComment}, TechPageShowCustomTab = {config.TechPageShowCustomTab}, " +
                    $"TechPageShowIcons = {config.TechPageShowIcons}, WireFrameResolution = {config.WireFrameResolution}");
            }
        }

        //! [Code snippet save]

        public ConfigurationManagerTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "Configuration Manager";
        public override string HtmlPath => "html/configuration_manager_tutorial.html";

    }
}
