using System;
using System.Collections.Generic;
using System.IO;
using TutorialCSharp.Tutorials.Helpers;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class AdditiveDEDTutorial : BaseTutorial
    {
        public AdditiveDEDTutorial(Esprit.Application app) : base(app)
        {
        }

        private Tutorial_AdditiveDED.AdditiveDEDDialog _dialog;

        private void CreateDialog()
        {
            _dialog = new Tutorial_AdditiveDED.AdditiveDEDDialog();
            _dialog.ImportCADmodelButton.Click += OnButtonOpenCADClick;
            _dialog.PartSetupButton.Click += OnButtonCreateBlockStockClick;
            _dialog.MachineButton.Click += OnButtonMachineSetupClick;
            _dialog.AddAdditiveButton.Click += OnButtonAddAdditiveClick;
            _dialog.DEDCycle.Click += OnButtonDEDCycleClick;
            _dialog.SimulateButton.Click += OnButtonSimulateClick;

            _dialog.ImportCADmodelButton.IsEnabled = true;
            _dialog.PartSetupButton.IsEnabled = false;
            _dialog.MachineButton.IsEnabled = false;
            _dialog.AddAdditiveButton.IsEnabled = false;
            _dialog.DEDCycle.IsEnabled = false;
            _dialog.SimulateButton.IsEnabled = false;

            _dialog.Show();
        }

        private void OnButtonOpenCADClick(object sender, RoutedEventArgs e)
        {
            _dialog.ImportCADmodelButton.IsEnabled = false;

            try
            {
                ImportCAD();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Load right project for demonstrating this tutorial", "AdditiveDEDTutorial", MessageBoxButton.OK);
                _dialog.Close();
                return;
            }

            _dialog.PartSetupButton.IsEnabled = true;
        }

        private void OnButtonCreateBlockStockClick(object sender, RoutedEventArgs e)
        {
            _dialog.PartSetupButton.IsEnabled = false;

            AddBlockStock();

            _dialog.MachineButton.IsEnabled = true;

            WindowHelper.FitAllWindows(Document.Windows);
        }

        private void OnButtonMachineSetupClick(object sender, RoutedEventArgs e)
        {
            _dialog.MachineButton.IsEnabled = false;

            CreateMachineSetup();

            _dialog.AddAdditiveButton.IsEnabled = true;
        }

        private void OnButtonAddAdditiveClick(object sender, RoutedEventArgs e)
        {
            _dialog.AddAdditiveButton.IsEnabled = false;

            AddAdditiveTool();

            _dialog.DEDCycle.IsEnabled = true;
        }

        private void OnButtonDEDCycleClick(object sender, RoutedEventArgs e)
        {
            _dialog.DEDCycle.IsEnabled = false;

            AddDepositionCycle();

            _dialog.SimulateButton.IsEnabled = true;
        }

        private void OnButtonSimulateClick(object sender, RoutedEventArgs e)
        {
            StartSimulation();
            _dialog.Close();
        }

        //! [Code snippet simulation]

        private void StartSimulation()
        {
            Document.Simulation.SetCollisions(true);
            Document.Simulation.PartViolationDetection = false;
            Document.Simulation.Play();
        }

        //! [Code snippet simulation]

        //! [Code snippet import]

        private void ImportCAD()
        {
            var projectDirectoryPath = Path.GetDirectoryName(Document.DocumentProperties.Path);

            var cadModelFilePath = Path.Combine(projectDirectoryPath, "testpart.SLDPRT");

            Document.MergeFile2(cadModelFilePath, false);

            if (Document.Solids.Count != 1)
            {
                System.Windows.MessageBox.Show("Import solid error. The document must contain 1 solid.", "RotaryMillingTutorial", MessageBoxButton.OK);
                return;
            }

            Esprit.Solid solid = Document.Solids[1];
            solid.Grouped = false;

            Document.PartSetup.BeginEdit();
            var part = Document.Parts[1];
            part.Geometry.Add(Document.Solids[1]);
            Document.PartSetup.EndEdit();

            var set = SelectionSetHelper.GetOrAddSelectionSet(Document.SelectionSets, "Temp");
            set.RemoveAll();

            set.Add(Document.Solids[1]);

            if (set.Count > 0)
            {
                try
                {
                    set.Rotate(Document.Lines[3], Math.PI/2);
                }
                catch (System.Runtime.InteropServices.COMException)
                { }
            }
            Document.SelectionSets.Remove(set.Name);

            WindowHelper.FitAllWindows(Document.Windows);

            Document.Refresh();
        }

        //! [Code snippet import]

        //! [Code snippet block]

        private void AddBlockStock()
        {
            Document.PartSetup.BeginEdit();

            var wp = Document.Workpieces[1];
            var bs = wp.Stocks.AddBlockStock();

            bs.AutoSize();

            var lengthAlignment = new EspritConstants.STOCKALIGNMENT()
            {
                AlignmentType = EspritConstants.espStockAlignmentType.espStockAlignmentTypeCentered,
                Size = 250
            };

            var widthAlignment = new EspritConstants.STOCKALIGNMENT()
            {
                AlignmentType = EspritConstants.espStockAlignmentType.espStockAlignmentTypeCentered,
                Size = 250
            };

            var heightAlignment = new EspritConstants.STOCKALIGNMENT()
            {
                AlignmentType = EspritConstants.espStockAlignmentType.espStockAlignmentTypeNegative,
                Size = 5,
                Margin = 5
            };

            bs.set_LengthAlignment(lengthAlignment);
            bs.set_WidthAlignment(widthAlignment);
            bs.set_HeightAlignment(heightAlignment);

            Document.PartSetup.EndEdit();
        }

        //! [Code snippet block]

        //! [Code snippet machine]

        private void CreateMachineSetup()
        {
            Document.InitialMachineSetup.BeginEdit();

            for (var i = Document.InitialMachineSetup.MachineItems.Count; i > 0; i--)
            {
                Document.InitialMachineSetup.MachineItems.Remove(i);
            }

            try
            {
                var path = Path.GetDirectoryName(Document.DocumentProperties.Path) + "\\3x Mill - S.mprj";
                Document.InitialMachineSetup.MachineFileName = path;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                System.Windows.MessageBox.Show("Fail on changing machine setup. To run this tutorial create a new document and try again.");
                return;
            }

            var workpieceItem = Document.InitialMachineSetup.MachineItems.AddWorkpieceInstance();
            workpieceItem.Name = "Workpiece 1";
            workpieceItem.WorkOffsets[1].WorkOffsetType = EspritConstants.espWorkOffsetType.espWorkOffsetTypeStandard;
            workpieceItem.ZTranslation = 5;
            Document.InitialMachineSetup.EndEdit();
        }

        //! [Code snippet machine]

        //! [Code snippet additive tool]

        private void SetParameter<T>(EspritTechnology.ITechnology tool, string name, T value)
        {
            var parameter = tool.FindParameter(name);
            if (parameter != null)
            {
                parameter.Value = value;
            }
        }

        private void AddAdditiveTool()
        {
            var technologyUtility = Document.TechnologyUtility as EspritTechnology.TechnologyUtility;
            var additiveTool = technologyUtility.CreateTechnology(EspritConstants.espTechnologyType.espToolAdditiveCustom, Document.SystemUnit);

            SetParameter(additiveTool, "ToolDiameter", 4);
            SetParameter(additiveTool, "ToolLength", 20);
            SetParameter(additiveTool, "CuttingLength", 4.1);
            SetParameter(additiveTool, "ShankDiameter", 4);
            SetParameter(additiveTool, "ToolID", "Additive");
            SetParameter(additiveTool, "ToolNumber", 1);
            SetParameter(additiveTool, "InitialClearance", 50);
            SetParameter(additiveTool, "ToolUnit", EspritConstants.espUnitType.espMetric);
            SetParameter(additiveTool, "SubTechno", EspritConstants.espDedSubTechnoType.espDedSubTechnoTypeArcwelding);
            SetParameter(additiveTool, "ShankType", EspritConstants.espToolMillCustomShankType.espToolMillCustomShankCylindrical);
            SetParameter(additiveTool, "ShankTopDiameter", 25);
            SetParameter(additiveTool, "ShankBottomLength", 30);
            SetParameter(additiveTool, "ShankAngle", 10);

            var geometryPath = Path.GetDirectoryName(Document.DocumentProperties.Path) + "\\BeaM4mm.ect";

            var parameter = additiveTool.FindParameter("ToolGeometry");
            if (parameter != null)
            {
                parameter.Value.Load(geometryPath);
            }

            var toolAssemblies = Document.ToolAssemblies;
            var machineSetup = Document.InitialMachineSetup;

            var adaptiveItemfilePath = Path.GetDirectoryName(Document.DocumentProperties.Path) + "\\test2.gdml";

            var targetStation = machineSetup.MachineDefinition.Turrets[1].Stations[1];
            var newToolAssembly = toolAssemblies.Add(adaptiveItemfilePath, targetStation);
            var mountedAdaptiveItem = newToolAssembly.RootAdaptiveItem;
            mountedAdaptiveItem.ToolAdapters[1].MountTool(additiveTool);
        }

        //! [Code snippet additive tool]

        //! [Code snippet ded]

        private EspritTechnology.Technology DefaultAdditiveSubFeatures(Esprit.AdditiveFeature additiveFeature)
        {
            var techUtils = Document.TechnologyUtility;

            EspritTechnology.Technology technology;

            EspritFeatures.ComAdditiveSubFeature subFeature = additiveFeature.Item[1];

            technology = techUtils.CreateTechnology(EspritConstants.espTechnologyType.espTechAdditiveSubFeature, Document.SystemUnit);
            technology.FindParameter(7401).Value = EspritConstants.espAdditiveSubFeatureType.espAdditiveSubFeatureParameters;
            technology.FindParameter(7402).Value = subFeature.Name;

            var objs = new List<Object>();
            for (int idx = 1; idx <= additiveFeature.Count; ++idx)
            {
                EspritFeatures.ComAdditiveFeatureElement subFeatureItem = subFeature[idx];
                objs.Add(subFeatureItem.Object);
            }

            var sEncode = Document.EncodeTechnologyGeometry(objs.ToArray());
            technology.FindParameter(7404).Value = sEncode;

            return technology;

        }

        private void AddDepositionCycle()
        {
            Esprit.AdditiveFeature feature = Document.AdditiveFeatures.Add();
            feature.Name = "1 Additive";
            feature.Plane = Document.ActivePlane;

            var subFeature = feature.Add("Sub Feature 1");
            subFeature.Add(Document.Solids[1], EspritConstants.espFreeFormElementType.espFreeFormPartSurfaceItem);

            Document.Refresh();

            EspritTechnology.TechAdditiveFlatDED cycle = new EspritTechnology.TechAdditiveFlatDED()
            {
                OperationName = "Flat Deposition",
                ToolID = "Additive",
                SubTechno = EspritConstants.espDedSubTechnoType.espDedSubTechnoTypeArcwelding,
                FeedratePM = 493.4,
                FeedratePT = 0.1,
                ChannelID = 1,
                SpindleSpeedRPM = 2467,
                SpindleSpeedSPM = 31,
                GasSpeed = 3,
                GasFeed = 30,
                WireFeed = 1,
                PlungeFeedRatePercent = 50,
                LateralFeedRatePercent = 100,
                ReferenceDepthOfCut = 5,
                AutomaticWorkOffsetRotation = true,
                FeedUnit = EspritConstants.espFeedUnit.espFeedPerMinute,
                UseFeedSpeedKB = false,
                WorkOffsetTransformation = EspritConstants.espWorkOffsetTransformation.espWorkOffsetTransformationNone,
                RotaryMachining = false,
                MoldLinearAxesMovement = EspritConstants.espMoldLinearAxesMovement.espMoldLinearAxesMovementRapid,
                Tolerance = 0.1,
                SliceThickness = 4,
                StopLaserShortMove = true,
                ContourType = EspritConstants.espDedContourStrategyType.espDedContourStrategyTypeNone,
                MaxLaserPower = 1000,
                FillingType = EspritConstants.espDedFillingStrategyType.espDedFillingStrategyTypeZigZag,
                ContourNumber = 1,
                TrackThickness = 4
            };

            EspritTechnology.TechMoldApproach entryStrategy = new EspritTechnology.TechMoldApproach()
            {
                EntryMovesType = EspritConstants.espMoldEntryMovesType.espMoldEntryMovesAdditive3xVertical,
                VerticalDistance = 2,
                DwellTime = 0,
                TurretName = 1,
                ChannelID = 1,
                ArcAngle = 30,
                ArcRadius = 5,
                ReferenceDepthOfCut = 5,
                LateralDistance = 2,
                RampAngle = 2,
                RampHeight = 2,
                MinimumRampWidth = 10,
                MaximumX = 100,
                MaximumY = 100,
                HelixAngle = 2,
                HelixDiameter = 10,
                Retraction = 2,
                LeadReferenceDirection = EspritConstants.espLeadReferenceDirection.espLeadReferenceDirectionToolTip
            };

            var entries = new List<EspritTechnology.TechMoldApproach>()
            {
                 entryStrategy
            };

            cycle.set_EntryMovesStrategies(entries.ToArray());

            EspritTechnology.TechMoldDetach exitStrategy = new EspritTechnology.TechMoldDetach()
            {
                ExitMovesType = EspritConstants.espMoldExitMovesType.espMoldExitMovesAdditive3xVertical,
                VerticalDistance = 2,
                TurretName = 1,
                ChannelID = 1,
                ReferenceDepthOfCut = 5,
                ArcRadius = 5,
                ArcAngle = 30,
                LateralDistance = 2,
                RampAngle = 2,
                RampHeight = 2,
                MinimumRampWidth = 10,
                MaximumX = 100,
                MaximumY = 100,
                HelixAngle = 2,
                HelixDiameter = 10,
                LeadReferenceDirection = EspritConstants.espLeadReferenceDirection.espLeadReferenceDirectionToolTip
            };

            var exitModes = new List<EspritTechnology.TechMoldDetach>()
            {
                 exitStrategy
            };

            cycle.set_ExitMovesStrategies(exitModes.ToArray());

            EspritTechnology.TechAdditiveLayerChange layerChangeStrategy = new EspritTechnology.TechAdditiveLayerChange()
            {
                LayerChangeType = EspritConstants.espAdditiveLayerChangeType.espAdditiveLayerChangeDefault,
                DwellTime = 0
            };

            var layerChangeModes = new List<EspritTechnology.TechAdditiveLayerChange>()
            {
                 layerChangeStrategy
            };

            cycle.set_LayerChangeStrategies(layerChangeModes.ToArray());

            var subFeatureStrategy = DefaultAdditiveSubFeatures(feature);

            SetParameter(subFeatureStrategy, "TrackThickness", 4);
            SetParameter(subFeatureStrategy, "FillingType", EspritConstants.espDedFillingStrategyType.espDedFillingStrategyTypeZigZag);
            SetParameter(subFeatureStrategy, "ContourType", EspritConstants.espDedContourStrategyType.espDedContourStrategyTypeNone);
            SetParameter(subFeatureStrategy, "SubFeatureParameter", EspritConstants.espAdditiveSubFeatureType.espAdditiveSubFeatureParameters);

            var subFeatures = new List<Object>()
            {
                 subFeatureStrategy
            };

            cycle.set_AdditiveSubFeatures(subFeatures.ToArray());

            Document.PartOperations.Add(cycle, Document.AdditiveFeatures[1]);

            Document.PartOperations.RebuildAll();

            Document.Group.Clear();
        }

        //! [Code snippet ded]

        public override void Execute()
        {
            CreateDialog();
        }

        public override string Name => "Additive Direct Energy Deposition";
        public override string HtmlPath => "html/additive_ded_tutorial.html";
    }
}
