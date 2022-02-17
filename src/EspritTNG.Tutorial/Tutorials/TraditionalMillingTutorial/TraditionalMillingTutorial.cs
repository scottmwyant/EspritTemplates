using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using TutorialCSharp.Tutorials.Helpers;
using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    class TraditionalMillingTutorial : BaseTutorial
    {

        public TraditionalMillingTutorial(Esprit.Application app) : base(app)
        {
        }

        private Tutorial_TraditionalMilling.TraditionalMillingDialog _dialog;
        private Esprit.BlockStock _stock;

        private void CreateDialog()
        {
            _dialog = new Tutorial_TraditionalMilling.TraditionalMillingDialog();
            _dialog.ImportCADmodelButton.Click += OnButtonOpenCADClick;
            _dialog.PartSetupButton.Click += OnButtonCreateBlockStockClick;
            _dialog.PocketingButton.Click += OnButtonPocketingClick;
            _dialog.MillToolButton.Click += OnButtonMillingToolClick;
            _dialog.MachineButton.Click += OnButtonMachineSetupClick;
            _dialog.PocketOpButton.Click += OnButtonPocketingOperationClick;
            _dialog.FacingOpButton.Click += OnButtonFacingClick;
            _dialog.DrillingButton.Click += OnButtonDrillingOperationClick;
            _dialog.SimulateButton.Click += OnButtonSimulateClick;

            _dialog.ImportCADmodelButton.IsEnabled = true;
            _dialog.PartSetupButton.IsEnabled = false;
            _dialog.PocketingButton.IsEnabled = false;
            _dialog.MillToolButton.IsEnabled = false;
            _dialog.MachineButton.IsEnabled = false;
            _dialog.PocketOpButton.IsEnabled = false;
            _dialog.FacingOpButton.IsEnabled = false;
            _dialog.DrillingButton.IsEnabled = false;
            _dialog.SimulateButton.IsEnabled = false;

            _dialog.ShowDialog();
        }

        private void OnButtonOpenCADClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ImportCAD();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Load right project for demonstrating this tutorial", "TraditionalMillingTutorial", MessageBoxButton.OK);
                _dialog.Close();
                return;
            }

            Document.Solids[1].FaceColors = false;

            _dialog.ImportCADmodelButton.IsEnabled = false;
            _dialog.PartSetupButton.IsEnabled = true;
        }
        private void OnButtonCreateBlockStockClick(object sender, RoutedEventArgs e)
        {
            _stock = AddBlockStock();

            _dialog.PartSetupButton.IsEnabled = false;
            _dialog.PocketingButton.IsEnabled = true;

            WindowHelper.FitAllWindows(Document.Windows);
        }

        private void OnButtonPocketingClick(object sender, RoutedEventArgs e)
        {
            Pocketing();

            _dialog.PocketingButton.IsEnabled = false;
            _dialog.MachineButton.IsEnabled = true;
        }

        private void OnButtonMillingToolClick(object sender, RoutedEventArgs e)
        {
            CreateMill();

            _dialog.MillToolButton.IsEnabled = false;
            _dialog.PocketOpButton.IsEnabled = true;
        }

        private void OnButtonMachineSetupClick(object sender, RoutedEventArgs e)
        {
            CreateMachineSetup();

            _dialog.MachineButton.IsEnabled = false;
            _dialog.MillToolButton.IsEnabled = true;
        }

        private void OnButtonPocketingOperationClick(object sender, RoutedEventArgs e)
        {
            CreatePocketingOperation();

            _dialog.PocketOpButton.IsEnabled = false;
            _dialog.FacingOpButton.IsEnabled = true;
            _dialog.SimulateButton.IsEnabled = true;
        }

        private void OnButtonFacingClick(object sender, RoutedEventArgs e)
        {
            Document.PartSetup.BeginEdit();

            var alignment = new EspritConstants.STOCKALIGNMENT()
            {
                AlignmentType = EspritConstants.espStockAlignmentType.espStockAlignmentTypeMargins,
                Margin = 2
            };

            _stock.set_HeightAlignment(alignment);
            Document.PartSetup.EndEdit();

            AddFaceMill();

            AddFacingOperation();

            _dialog.FacingOpButton.IsEnabled = false;
            _dialog.DrillingButton.IsEnabled = true;
        }

        private void OnButtonDrillingOperationClick(object sender, RoutedEventArgs e)
        {
            AddDrillingTools();

            RecognizeHoles();

            AddDrillingOperations();

            _dialog.DrillingButton.IsEnabled = false;
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
            Document.Simulation.Play();
        }

        //! [Code snippet simulation]

        //! [Code snippet import]

        private void ImportCAD()
        {
            var projectDirectoryPath = Path.GetDirectoryName(Document.DocumentProperties.Path);

            var cadModelFilePath = Path.Combine(projectDirectoryPath, "SumpPan.x_t");

            Document.MergeFile2(cadModelFilePath, false);

            if (Document.Solids.Count != 1)
            {
                System.Windows.MessageBox.Show("Import solid error. The document must contain 1 solid.", "TraditionalMillingTutorial", MessageBoxButton.OK);
                return;
            }

            Esprit.Solid solid = Document.Solids[1];
            solid.Grouped = false;

            Document.PartSetup.BeginEdit();
            var part = Document.Parts[1];
            part.Geometry.Add(Document.Solids[1]);
            Document.PartSetup.EndEdit();

            EspritSolids.SolidBody body = solid.SolidBody;

            foreach (EspritSolids.SolidFace face in body.SolidFaces)
            {
                if (face.Identity == 1)
                {
                    Document.Group.Add(face);
                    Document.AlignAlongAxis("Z");
                    Document.Group.Remove(face);
                    return;
                }
            }
        }

        //! [Code snippet import]

        //! [Code snippet block]

        private Esprit.BlockStock AddBlockStock()
        {
            Document.PartSetup.BeginEdit();

            var wp = Document.Workpieces[1];
            var bs = wp.Stocks.AddBlockStock();

            var alignment = new EspritConstants.STOCKALIGNMENT()
            {
                AlignmentType = EspritConstants.espStockAlignmentType.espStockAlignmentTypeCentered
            };

            bs.set_LengthAlignment(alignment);
            bs.set_WidthAlignment(alignment);
            bs.set_HeightAlignment(alignment);

            bs.AutoSize();

            Document.PartSetup.EndEdit();
            return bs;
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
                Document.InitialMachineSetup.MachineFileName = EspritApplication.Configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeMachineSetup) + "/Samples/Mill/Mill 3-axis/Mini_3x_Mill.mprj";
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                System.Windows.MessageBox.Show("Fail on changing machine setup. To run this tutorial create a new document and try again.");
                return;
            }

            var fixtureResourceDirectoryPath = EspritApplication.Configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeFixtures) + "/Samples/mill_vice.gdml";

            try
            {
                var fixture = Document.InitialMachineSetup.MachineItems.AddFixture(fixtureResourceDirectoryPath) as Esprit.IFixture;
                fixture.Name = "Fixture";
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                System.Windows.MessageBox.Show("Fail on adding fixture.");
                return;
            }

            var workpieceItem = Document.InitialMachineSetup.MachineItems[1].MachineItems.AddWorkpieceInstance();
            workpieceItem.Name = "Workpiece 1";
            workpieceItem.WorkOffsets[1].WorkOffsetType = EspritConstants.espWorkOffsetType.espWorkOffsetTypeCustom;
            workpieceItem.ZTranslation = 30;
            Document.InitialMachineSetup.EndEdit();
        }

        //! [Code snippet machine]

        //! [Code snippet pocketing]

        private void Pocketing()
        {
            var solidArray = new List<Esprit.IGraphicObject>();

            solidArray.Add(Document.Solids[1]);
            try
            {
                Document.FeatureRecognition.CreatePocketFeatures2(solidArray.ToArray(), Document.ActivePlane, out var comFaults);

                for (var i = 1; i <= comFaults.Count; i++)
                {
                    var msgType = comFaults[i].Severity == EspritComBase.espFaultSeverity.espFaultWarning ? EspritConstants.espMessageType.espMessageTypeWarning : EspritConstants.espMessageType.espMessageTypeError;
                    EspritApplication.EventWindow.AddMessage(msgType, "TraditionalMillingTutorial", comFaults[i].Description);
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Error on Pocket Features recognition");
            }
        }

        //! [Code snippet pocketing]

        //! [Code snippet pocket operation]

        private void CreateMill()
        {
            var toolAssemblies = Document.ToolAssemblies;
            var machineSetup = Document.InitialMachineSetup;

            var projectDirectoryPath = Path.GetDirectoryName(Document.DocumentProperties.Path);
            var adaptiveItemfilePath = Path.Combine(projectDirectoryPath, "MillToolHolder_CAT-40.gdml");

            var targetStation = machineSetup.MachineDefinition.Turrets[1].Stations[1];
            var newMillToolAssembly = toolAssemblies.Add(adaptiveItemfilePath, targetStation);
            var mountedAdaptiveItem = newMillToolAssembly.RootAdaptiveItem;

            var technologyUtility = Document.TechnologyUtility as EspritTechnology.TechnologyUtility;
            var millTool = technologyUtility.CreateTechnology(EspritConstants.espTechnologyType.espToolMillEndMill, Document.SystemUnit);

            SetParameter(millTool, "ToolID", "10mm Endmill");
            SetParameter(millTool, "ToolNumber", 3);
            SetParameter(millTool, "Coolant", EspritConstants.espCoolantType.espCoolantOn);
            SetParameter(millTool, "InitialClearance", 50);
            SetParameter(millTool, "LengthCompRegister", 1);
            SetParameter(millTool, "ToolLength", 80);
            SetParameter(millTool, "OverallLength", 80);
            SetParameter(millTool, "ExtensionDiameter", 35);
            SetParameter(millTool, "ShankDiameter", 10);
            SetParameter(millTool, "CuttingLength", 30);
            SetParameter(millTool, "ToolDiameter", 10);

            mountedAdaptiveItem.ToolAdapters[1].MountTool(millTool);
        }

        private void SetParameter<T>(EspritTechnology.ITechnology tool, string name, T value)
        {
            var parameter = tool.FindParameter(name);
            if (parameter != null)
            {
                parameter.Value = value;
            }
        }

        private void CreatePocketingOperation()
        {
            EspritTechnology.TechMillPocket2 millPocketOperation = new EspritTechnology.TechMillPocket2();

            millPocketOperation.OperationName = "Pocketing";
            millPocketOperation.ToolID = "10mm Endmill";
            millPocketOperation.FeedUnit = EspritConstants.espFeedUnit.espFeedPerMinute;
            millPocketOperation.CalculateStock = true;

            millPocketOperation.ToolMotionPattern = EspritConstants.espMillPocketPattern.espMillPocketPatternProfitMilling;
            millPocketOperation.TrochoidTransitionFeedPercent = 500;
            millPocketOperation.MinimumTrochoidRadius = 40;
            millPocketOperation.MaxTrochoidWidthRatio = 400;
            millPocketOperation.MinimumCornerRadius = 4;
            millPocketOperation.TotalDepth = 25;
            millPocketOperation.IncrementalDepth = 25;

            millPocketOperation.RetractForIdepth = EspritConstants.espRetractMode.espRetractClearanceAboveSurface;
            millPocketOperation.IncrDepthCalculation = EspritConstants.espPocketIncrDepthCalculation.espPocketIncrDepthCalculationVarying;

            millPocketOperation.ProfitMillingIncrementalDepth = 10;
            millPocketOperation.ProfitMillingSpindleSpeedRPM = 5000;
            millPocketOperation.ProfitMillingXYFeedratePM = 500;
            millPocketOperation.StayInsideFeature = true;
            millPocketOperation.OutputRoughPasses = true;

            millPocketOperation.XYFeedratePM = 500;
            millPocketOperation.ZFeedratePM = 500;
            millPocketOperation.SpindleSpeedRPM = 5000;
            millPocketOperation.MaximumFeedratePM = 25000;

            millPocketOperation.StepPercentOfDiameter = 50;
            millPocketOperation.StepOver = 5;

            millPocketOperation.RapidPartialDepth = true;
            millPocketOperation.OpenEdgeOffset = 5.5;

            millPocketOperation.OpenPocketLeadIn = 5;
            millPocketOperation.OpenPocketLeadOut = 5;
            millPocketOperation.ReadTaperFromFeature = true;

            millPocketOperation.EngagementAngle = 90;
            millPocketOperation.EntryMode = EspritConstants.espMillPocketEntryMode.espMillPocketEntryModeRampAlongPass;
            millPocketOperation.MinimumEntryWidth = 2.5;
            millPocketOperation.MaximumEntryWidth = 20;
            millPocketOperation.RampAngle = 10;
            millPocketOperation.EdgeClearance = 1;
            millPocketOperation.EntryFailure = EspritConstants.espMillPocketEntryFailure.espMillPocketEntryFailureSkip;
            millPocketOperation.ExitMode = EspritConstants.espMillPocketExitMode.espMillPocketExitModeRamp;
            millPocketOperation.RetractDistance = 2;
            millPocketOperation.Clearance = 2;

            foreach (Esprit.FeatureSet set in Document.FeatureSets)
            {
                if (set.Name == "3 Pocket")
                {
                    Document.Group.Add(set);
                    Document.PartOperations.Add(millPocketOperation, set);
                    break;
                }
            }
        }

        //! [Code snippet pocket operation]

        //! [Code snippet facing operation]

        private void AddFaceMill()
        {
            var projectDirectoryPath = Path.GetDirectoryName(Document.DocumentProperties.Path);
            var adaptiveItemfilePath = Path.Combine(projectDirectoryPath, "MillToolHolder_CAT-40.gdml");

            var toolAssemblies = Document.ToolAssemblies;
            var machineSetup = Document.InitialMachineSetup;
            var targetStation = machineSetup.MachineDefinition.Turrets[1].Stations[1];
            var newMillToolAssembly = toolAssemblies.Add(adaptiveItemfilePath, targetStation);

            var mountedAdaptiveItem = newMillToolAssembly.RootAdaptiveItem;

            var technologyUtility = Document.TechnologyUtility as EspritTechnology.TechnologyUtility;
            var faceMillTool = technologyUtility.CreateTechnology(EspritConstants.espTechnologyType.espToolMillFaceMill, Document.SystemUnit);

            SetParameter(faceMillTool, "ToolID", "Facemill");
            SetParameter(faceMillTool, "SpindleDirection", EspritConstants.espSpindleDirection.espSpindleCW);
            SetParameter(faceMillTool, "InitialClearance", 50);
            SetParameter(faceMillTool, "Coolant", EspritConstants.espCoolantType.espCoolantOn);
            SetParameter(faceMillTool, "ExtensionImageDiameter", 35);
            SetParameter(faceMillTool, "OverallLength", 80);
            SetParameter(faceMillTool, "ToolLength", 80);
            SetParameter(faceMillTool, "ShankType", EspritConstants.espToolMillFaceShankType.espToolMillFaceShankReverseConical);
            SetParameter(faceMillTool, "ShankDiameter", 73);
            SetParameter(faceMillTool, "CuttingLength", 10);
            SetParameter(faceMillTool, "ShankTopDiameter", 30);
            SetParameter(faceMillTool, "ShankBottomLength", 20);
            SetParameter(faceMillTool, "ShankAngle", 75);
            SetParameter(faceMillTool, "ToolDiameter", 75);
            SetParameter(faceMillTool, "BottomClearance", 1);
            SetParameter(faceMillTool, "InsertType", EspritConstants.espToolMillInsertType.espToolMillInsertSquare);
            SetParameter(faceMillTool, "CornerRadius", 0.5);
            SetParameter(faceMillTool, "ICDiameter", 10);
            SetParameter(faceMillTool, "OrientationAngle", 0);
            SetParameter(faceMillTool, "NumberOfInserts", 6);

            mountedAdaptiveItem.ToolAdapters[mountedAdaptiveItem.ToolAdapters.Count].MountTool(faceMillTool);
        }

        private void AddFacingOperation()
        {
            Document.Group.Clear();

            EspritTechnology.TechMillFace millFaceOperation = new EspritTechnology.TechMillFace();

            millFaceOperation.OperationName = "Facing";
            millFaceOperation.ToolID = "Facemill";
            millFaceOperation.XYFeedratePM = 2000;
            millFaceOperation.ZFeedratePM = 2000;
            millFaceOperation.SpindleSpeedRPM = 10000;
            millFaceOperation.FeedUnit = EspritConstants.espFeedUnit.espFeedPerMinute;
            millFaceOperation.CalculateStock = true;

            millFaceOperation.OptimalCuttingAngle = true;
            millFaceOperation.StepOver = 37.5;
            millFaceOperation.StepPercentOfDiameter = 50;
            millFaceOperation.OverhangDirection = EspritConstants.espMillFaceOverhangDirection.espMillFaceOverhangDirectionOneWay;
            millFaceOperation.Overhang = 37.5;
            millFaceOperation.PercentOfToolOverhang = 50;
            millFaceOperation.IncrDepthCalculation = EspritConstants.espFaceIncrDepthCalculation.espFaceIncrDepthCalculationVarying;

            millFaceOperation.Clearance = 2;
            millFaceOperation.EntryMode = EspritConstants.espMillFaceEntryMode.espMillFaceEntryDownThenOver;
            millFaceOperation.LeadInDistance = 39.5;
            millFaceOperation.ExitMode = EspritConstants.espMillFaceExitMode.espMillFaceExitOverThenUp;
            millFaceOperation.LeadOutDistance = 39.5;

            foreach (Esprit.FeatureChain chain in Document.FeatureChains)
            {
                if (chain.Name == "1 Boundary")
                {
                    Document.Group.Add(chain);
                    
                    Document.PartOperations.Add(millFaceOperation, chain);
                    break;
                }
            }

            var operations = new List<Esprit.PartOperation>();
            operations.Add(Document.PartOperations[2]);
            Document.PartOperations.Reorder(operations.ToArray(), 1, true);
        }

        //! [Code snippet facing operation]


        //! [Code snippet drilling]

        private void AddDrillingTools()
        {
            var toolAssemblies = Document.ToolAssemblies;
            var projectDirectoryPath = Path.GetDirectoryName(Document.DocumentProperties.Path);
            var drillingToolsFilePath = Path.Combine(projectDirectoryPath, "DrillingTools.gdml");
            toolAssemblies.Import(drillingToolsFilePath);
        }

        private void RecognizeHoles()
        {
            var solidArray = new List<Esprit.IGraphicObject>();

            solidArray.Add(Document.Solids[1]);

            try
            {
                var conf = EspritApplication.Configuration.ConfigurationHoleRecognition2;
                conf.MaxDiameterInch = 20.49579;
                conf.MaxDiameterMetric = 20.49579;
                conf.MinDiameterInch = 0;
                conf.MinDiameterMetric = 0;
                conf.MinimumHoleOpeningInch = 180;
                conf.MinimumHoleOpeningMetric = 180;

                conf.CombineCoaxialHoles = true;
                conf.PropagateHoleFace = true;
                conf.ActiveWorkplaneOnly = false;
                conf.SplitCustomHole = false;

                conf.GroupByCadFeature = false;
                conf.GroupBySameHoleType = true;
                conf.GroupBySameDiameters = true;
                conf.GroupBySameDepths = false;

                conf.GroupBySameDirection = false;
                conf.GroupBySameAltitude = false;

                Document.FeatureRecognition.CreateHoleFeatures2(solidArray.ToArray(), out var comFaults);

                for (var i = 1; i <= comFaults.Count; i++)
                {
                    var msgType = comFaults[i].Severity == EspritComBase.espFaultSeverity.espFaultWarning ? EspritConstants.espMessageType.espMessageTypeWarning : EspritConstants.espMessageType.espMessageTypeError;
                    EspritApplication.EventWindow.AddMessage(msgType, "TraditionalMillingTutorial", comFaults[i].Description);
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Error on Hole Features recognition");
            }
        }

        private void AddDrillingOperations()
        {
            EspritTechnology.TechMillHoleMachining centerDrillingOperation = new EspritTechnology.TechMillHoleMachining();

            centerDrillingOperation.OperationName = "Pilot Drilling";
            centerDrillingOperation.ToolID = "Center Drill";
            centerDrillingOperation.SpindleSpeedRPM = 5000;
            centerDrillingOperation.ZFeedratePM = 500;
            centerDrillingOperation.CalculateStock = true;
            centerDrillingOperation.MachiningStrategy = EspritConstants.espCenterHoleMachiningStrategy.espCenterHoleMachiningStrategyToolSpinning;
            centerDrillingOperation.TotalDepth = 2;
            centerDrillingOperation.DepthFromFeature = false;
            centerDrillingOperation.DwellTime = 1;
            centerDrillingOperation.Clearance = 2;
            centerDrillingOperation.CycleType = EspritConstants.espHoleCycleType.espHoleCycleTypeSpotDrilling;

            Document.Group.Clear();

            foreach (Esprit.HolesFeature hole in Document.HolesFeatures)
            {
                if ((hole.Diameter == 8 && hole.Name == "1 Simple Hole Ø8. ↧17.7") || hole.Name == "4 Countersink Ø6.6 ↧25.4" || hole.Name == "3 Simple Hole Ø20.49579 ↧10.")
                {
                    Document.Group.Add(hole);
                    Document.PartOperations.Add(centerDrillingOperation, hole);
                }
            }

            EspritTechnology.TechMillHoleMachining drill8mmOperation = new EspritTechnology.TechMillHoleMachining();

            drill8mmOperation.OperationName = "Drilling";
            drill8mmOperation.ToolID = "8mm Drill";
            drill8mmOperation.SpindleSpeedRPM = 5000;
            drill8mmOperation.ZFeedratePM = 500;
            drill8mmOperation.CalculateStock = true;
            drill8mmOperation.MachiningStrategy = EspritConstants.espCenterHoleMachiningStrategy.espCenterHoleMachiningStrategyToolSpinning;
            drill8mmOperation.DepthFromFeature = true;
            drill8mmOperation.DwellTime = 1;
            drill8mmOperation.Clearance = 2;
            drill8mmOperation.CycleType = EspritConstants.espHoleCycleType.espHoleCycleTypeDrillingWithChipRemoval;
            drill8mmOperation.PeckingMode = EspritConstants.espHolePeckingMode.espHolePeckingModeConstant;
            drill8mmOperation.PeckIncrement = 10;
            drill8mmOperation.PeckClearance = 1;

            Document.Group.Clear();

            foreach (Esprit.HolesFeature hole in Document.HolesFeatures)
            {
                if ((hole.Diameter == 8 && hole.Name == "1 Simple Hole Ø8. ↧17.7") || hole.Name == "4 Countersink Ø6.6 ↧25.4" )
                {
                    Document.Group.Add(hole);
                    Document.PartOperations.Add(drill8mmOperation, hole);
                }
            }

            EspritTechnology.TechMillHoleMachining chamferMillOperation = new EspritTechnology.TechMillHoleMachining();

            chamferMillOperation.OperationName = "Countersink";
            chamferMillOperation.ToolID = "Chamfer Mill";
            chamferMillOperation.SpindleSpeedRPM = 5000;
            chamferMillOperation.ZFeedratePM = 500;
            chamferMillOperation.CalculateStock = true;
            chamferMillOperation.MachiningStrategy = EspritConstants.espCenterHoleMachiningStrategy.espCenterHoleMachiningStrategyToolSpinning;
            chamferMillOperation.DepthFromFeature = true;
            chamferMillOperation.DwellTime = 1;
            chamferMillOperation.Clearance = 2;
            chamferMillOperation.CycleType = EspritConstants.espHoleCycleType.espHoleCycleTypeSpotDrilling;

            Document.Group.Clear();

            foreach (Esprit.HolesFeature hole in Document.HolesFeatures)
            {
                if (hole.Name == "4 Countersink Ø6.6 ↧25.4")
                {
                    Document.Group.Add(hole);
                    Document.PartOperations.Add(chamferMillOperation, hole);
                }
            }

            EspritTechnology.TechMillHoleMachining drill20mmOperation = new EspritTechnology.TechMillHoleMachining();

            drill20mmOperation.OperationName = "Large Hole Drilling";
            drill20mmOperation.ToolID = "20mm Drill";
            drill20mmOperation.SpindleSpeedRPM = 1500;
            drill20mmOperation.ZFeedratePM = 200;
            drill20mmOperation.CalculateStock = true;
            drill20mmOperation.MachiningStrategy = EspritConstants.espCenterHoleMachiningStrategy.espCenterHoleMachiningStrategyToolSpinning;
            drill20mmOperation.DepthFromFeature = true;
            drill20mmOperation.ThroughDepth = 2;
            drill20mmOperation.DwellTime = 1;
            drill20mmOperation.Clearance = 2;
            drill20mmOperation.CycleType = EspritConstants.espHoleCycleType.espHoleCycleTypeDrillingWithChipBreaking;
            drill20mmOperation.ReturnPlane = EspritConstants.espHoleReturnPlane.espHoleReturnPlaneFullClearance;
            Document.Group.Clear();

            foreach (Esprit.HolesFeature hole in Document.HolesFeatures)
            {
                if (hole.Name == "3 Simple Hole Ø20.49579 ↧10.")
                {
                    Document.Group.Add(hole);
                    Document.PartOperations.Add(drill20mmOperation, hole);
                }
            }
        }

        //! [Code snippet drilling]

        public override void Execute()
        {
            CreateDialog();
        }

        public override string Name => "Traditional Milling";
        public override string HtmlPath => "html/traditional_milling_tutorial.html";
    }
}
