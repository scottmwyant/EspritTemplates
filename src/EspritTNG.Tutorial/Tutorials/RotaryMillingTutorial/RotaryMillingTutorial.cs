using System;
using System.Collections.Generic;
using System.IO;
using TutorialCSharp.Tutorials.Helpers;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class RotaryMillingTutorial : BaseTutorial
    {
        public RotaryMillingTutorial(Esprit.Application app) : base(app)
        {
        }

        private Tutorial_RotaryMilling.RotaryMillingDialog _dialog;

        private void CreateDialog()
        {
            _dialog = new Tutorial_RotaryMilling.RotaryMillingDialog();
            _dialog.ImportCADmodelButton.Click += OnButtonOpenCADClick;
            _dialog.PartSetupButton.Click += OnButtonCreateBlockStockClick;
            _dialog.MachineButton.Click += OnButtonMachineSetupClick;
            _dialog.RoughingButton.Click += OnButtonRoughingOperationClick;
            _dialog.ConcentricFButton.Click += OnButtonConcentricFinishingOperationClick;
            _dialog.WrappedButton.Click += OnButtonWrappedMillingOperationClick;
            _dialog.DrillingButton.Click += OnButtonDrillingOperationClick;
            _dialog.ContouringButton.Click += OnButtonContouringOperationClick;
            _dialog.CenterDrillingButton.Click += OnButtonCenterDrillingOperationClick;
            _dialog.SpiralingButton.Click += OnButtonSpirallingOperationClick;
            _dialog.EngravingButton.Click += OnButtonEngravingOperationClick;
            _dialog.SimulateButton.Click += OnButtonSimulateClick;

            _dialog.ImportCADmodelButton.IsEnabled = true;
            _dialog.PartSetupButton.IsEnabled = false;
            _dialog.MachineButton.IsEnabled = false;
            _dialog.RoughingButton.IsEnabled = false;
            _dialog.ConcentricFButton.IsEnabled = false;
            _dialog.WrappedButton.IsEnabled = false;
            _dialog.DrillingButton.IsEnabled = false;
            _dialog.ContouringButton.IsEnabled = false;
            _dialog.CenterDrillingButton.IsEnabled = false;
            _dialog.SpiralingButton.IsEnabled = false;
            _dialog.EngravingButton.IsEnabled = false;
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
                System.Windows.MessageBox.Show("Load right project for demonstrating this tutorial", "RotaryMillingTutorial", MessageBoxButton.OK);
                _dialog.Close();
                return;
            }

            _dialog.PartSetupButton.IsEnabled = true;
        }

        private void OnButtonCreateBlockStockClick(object sender, RoutedEventArgs e)
        {
            _dialog.PartSetupButton.IsEnabled = false;

            AddBarStock();

            _dialog.MachineButton.IsEnabled = true;

            WindowHelper.FitAllWindows(Document.Windows);
        }

        private void OnButtonMachineSetupClick(object sender, RoutedEventArgs e)
        {
            _dialog.MachineButton.IsEnabled = false;

            CreateMachineSetup();

            _dialog.RoughingButton.IsEnabled = true;
        }

        private void OnButtonRoughingOperationClick(object sender, RoutedEventArgs e)
        {
            _dialog.RoughingButton.IsEnabled = false;

            AddRoughingOperation();

            _dialog.ConcentricFButton.IsEnabled = true;
        }

        private void OnButtonConcentricFinishingOperationClick(object sender, RoutedEventArgs e)
        {
            _dialog.ConcentricFButton.IsEnabled = false;

            AddConcentricFinishingOperation();

            _dialog.WrappedButton.IsEnabled = true;
        }

        private void OnButtonWrappedMillingOperationClick(object sender, RoutedEventArgs e)
        {
            _dialog.WrappedButton.IsEnabled = false;

            WrappedMilling();

            _dialog.DrillingButton.IsEnabled = true;
        }

        private void OnButtonDrillingOperationClick(object sender, RoutedEventArgs e)
        {
            _dialog.DrillingButton.IsEnabled = false; 

            RecognizeHoles();
            AddDrillingOperation();

            _dialog.ContouringButton.IsEnabled = true;
        }

        private void OnButtonContouringOperationClick(object sender, RoutedEventArgs e)
        {
            _dialog.ContouringButton.IsEnabled = false;

            AddContouringOperation();

            _dialog.CenterDrillingButton.IsEnabled = true;
        }

        private void OnButtonCenterDrillingOperationClick(object sender, RoutedEventArgs e)
        {
            _dialog.CenterDrillingButton.IsEnabled = false;

            AddCenterDrillingOperation();

            _dialog.SpiralingButton.IsEnabled = true;
        }

        private void OnButtonSpirallingOperationClick(object sender, RoutedEventArgs e)
        {
            _dialog.SpiralingButton.IsEnabled = false;

            AddSpiralingOperation();

            _dialog.EngravingButton.IsEnabled = true;
        }

        private void OnButtonEngravingOperationClick(object sender, RoutedEventArgs e)
        {
            _dialog.EngravingButton.IsEnabled = false;

            AddEngravingOperation();

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

            var cadModelFilePath = Path.Combine(projectDirectoryPath, "RotorVane.SLDPRT");

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

            Document.Refresh();
        }

        //! [Code snippet import]

        //! [Code snippet bar]

        private void AddBarStock()
        {
            Document.PartSetup.BeginEdit();

            var wp = Document.Workpieces[1];
            var bs = wp.Stocks.AddBarStock();
            bs.AutoSize();

            bs.Tolerance = 0.1;

            Document.PartSetup.EndEdit();
        }

        //! [Code snippet bar]

        //! [Code snippet machine]

        private void CreateMachineSetup()
        {
            Document.InitialMachineSetup.BeginEdit();

            var workpieceItem = Document.InitialMachineSetup.MachineItems[1].MachineItems.AddWorkpieceInstance();
            workpieceItem.Name = "Workpiece 1";
            workpieceItem.WorkOffsets[1].WorkOffsetType = EspritConstants.espWorkOffsetType.espWorkOffsetTypeCustom;
            workpieceItem.ZTranslation = -10;
            Document.InitialMachineSetup.EndEdit();
        }

        //! [Code snippet machine]

        //! [Code snippet roughing]

        private Esprit.FreeFormFeature CreateFreeFormFeature(string name, EspritConstants.espFreeFormElementType type)
        {
            Esprit.FreeFormFeature feature = Document.FreeFormFeatures.Add();
            feature.Name = name;
            feature.Add(Document.Solids[1], type);
            return feature;
        }

        private Esprit.FreeFormFeature CreateFreeFormFeature(string name, EspritConstants.espFreeFormElementType type, int faceIdentity)
        {
            var feature = CreateFreeFormFeature(name, type);

            EspritSolids.SolidBody body = Document.Solids[1].SolidBody;

            foreach (EspritSolids.SolidFace face in body.SolidFaces)
            {
                if (face.Identity == faceIdentity)
                {
                    feature.Add(face, EspritConstants.espFreeFormElementType.espFreeFormPartSurfaceItem);
                    break;
                }
            }
            return feature;
        }

        private void AddFeatureOperation(Esprit.IGraphicObject feature, Object technology)
        {
            Document.Group.Clear();
            Document.Group.Add(feature);
            Document.PartOperations.Add(technology, feature);
            Document.Group.Clear();
        }

        private void AddRoughingOperation()
        {
            Esprit.FreeFormFeature freeFormFeature1 = CreateFreeFormFeature("1 FreeForm", EspritConstants.espFreeFormElementType.espFreeFormPartSurfaceItem);

            EspritTechnology.TechMoldRoughing roughing = new EspritTechnology.TechMoldRoughing()
            {
                OperationName = "Roughing",
                ToolID = "EM2 (25mm EM)",
                SpindleSpeedRPM = 5000,
                SpindleSpeedSPM = 393,
                FeedratePM = 2000,
                FeedratePT = 0.2,
                RotaryMachining = true,
                StockAllowanceWalls = 1,
                StockAllowanceFloors = 1,
                DepthCalculation = EspritConstants.espMoldRoughingDepthCalculation.espMoldRoughingDepthCalculationAdaptive,
                IncrementalDepth = 10,
                PlungeFeedRatePercent = 100,
                LateralFeedRatePercent = 100,
                FeedUnit = EspritConstants.espFeedUnit.espFeedPerMinute,
                CalculateStock = true,
                Tolerance = 0.03,
                DepthStrategy = EspritConstants.espMoldRoughingDepthStrategy.espMoldRoughingDepthStrategyTopDown,
                CuttingStrategy = EspritConstants.espMoldRoughingMachiningStrategy.espMoldRoughingMachiningStrategyClimbOutsideIn,
                MoldCutPassesMode = EspritConstants.espMoldCutPassesMode.espMoldCutPassesClosed,
                RoundAllCorners = EspritConstants.espMoldRoundAllCorners.espMoldRoundAllCornersYes,
                CornerRoundingTolerance = 3.125,
                StepOver = 12.5,
                StepPercentOfDiameter = 50,
                PositionOnModelLimit = EspritConstants.espMoldPositionOnModelLimit.espMoldPositionOnModelLimitLimitedByStock,
                PositionOnBoundaryProfile = EspritConstants.espMoldPositionOnBoundaryProfile.espMoldPositionOnBoundaryProfileInside,
                RetractOptimization = EspritConstants.espMoldRetractOptimization.espMoldRetractOptimizationWithin,
                Clearance = 2,
                FullClearance = 70,
                OptimizationMode = EspritConstants.espRetractOptimizationMode.espRetractOptimizationStock,
                ClippingEntity = "1003,1",
                FullClearanceReference = "1003,1" // identifies 1 FreeForm feature
            };

            EspritTechnology.TechMoldApproach entry = new EspritTechnology.TechMoldApproach()
            {
                EntryMovesType = EspritConstants.espMoldEntryMovesType.espMoldEntryMovesVerticalThenLateralWithLimits,
                LateralDistance = 20,
                VerticalDistance = 0
            };
            var entries = new List<EspritTechnology.TechMoldApproach>()
            {
                 entry
            };

            roughing.set_EntryMovesStrategies(entries.ToArray());

            EspritTechnology.TechMoldDetach exit1 = new EspritTechnology.TechMoldDetach()
            {
                ExitMovesType = EspritConstants.espMoldExitMovesType.espMoldExitMovesVerticalThenRadiusWithLimits,
                LateralDistance = 15,
                ArcRadius = 6.25,
                VerticalDistance = 2
            };
            EspritTechnology.TechMoldDetach exit2 = new EspritTechnology.TechMoldDetach()
            {
                ExitMovesType = EspritConstants.espMoldExitMovesType.espMoldExitMovesVerticalThenLateralWithLimits,
                LateralDistance = 15,
                VerticalDistance = 2
            };

            var exitModes = new List<EspritTechnology.TechMoldDetach>()
            {
                 exit1,
                 exit2
            };
            roughing.set_ExitMovesStrategies(exitModes.ToArray());

            EspritTechnology.TechMoldFeedLink feedLink1 = new EspritTechnology.TechMoldFeedLink()
            {
                FeedLinkType = EspritConstants.espMoldFeedLinkType.espMoldFeedLinkSmoothArcWithDetach,
                MaximumLinkDistance = 50,
                RampAngle = 2
            };
            EspritTechnology.TechMoldFeedLink feedLink2 = new EspritTechnology.TechMoldFeedLink()
            {
                FeedLinkType = EspritConstants.espMoldFeedLinkType.espMoldFeedLinkSmoothArcWithDetach,
                MaximumLinkDistance = 50,
                RampAngle = 2,
                LateralDistance = 0.25
            };
            EspritTechnology.TechMoldFeedLink feedLink3 = new EspritTechnology.TechMoldFeedLink()
            {
                FeedLinkType = EspritConstants.espMoldFeedLinkType.espMoldFeedLinkLinearWithDetach,
                MaximumLinkDistance = 50,
                LateralDistance = 6.25,
                RampAngle = 2
            };
            EspritTechnology.TechMoldFeedLink feedLink4 = new EspritTechnology.TechMoldFeedLink()
            {
                FeedLinkType = EspritConstants.espMoldFeedLinkType.espMoldFeedLinkRamp,
                MaximumLinkDistance = 100,
                MinimumRampWidth = 25,
                RampAngle = 2
            };
            var feedLinks = new List<EspritTechnology.TechMoldFeedLink>()
            {
                 feedLink1,
                 feedLink2,
                 feedLink3,
                 feedLink4
            };
            roughing.set_FeedLinks(feedLinks.ToArray());

            // create auto chain on elements in Geometry Layer
            var set = SelectionSetHelper.GetOrAddSelectionSet(Document.SelectionSets, "Temp");
            set.RemoveAll();

            foreach (Esprit.IGraphicObject graphicObject in Document.GraphicsCollection)
            {
                if(graphicObject.Layer.Name == "Geometry")
                {
                    set.Add(graphicObject);
                }
            }

            if (set.Count > 0)
            {
                try
                {
                    Document.FeatureRecognition.CreateAutoChains(set);
                    roughing.BoundaryProfiles = "6,1"; // identifies 1 Chain
                    AddFeatureOperation(freeFormFeature1, roughing);
                }
                catch (System.Runtime.InteropServices.COMException)
                { }
            }
            Document.SelectionSets.Remove(set.Name);
        }

        //! [Code snippet roughing]

        //! [Code snippet finishing]

        private EspritTechnology.TechMoldConcentricFinishing FinishingOperation(double angle, string fullClearanceReference)
        {
            EspritTechnology.TechMoldConcentricFinishing finishing = new EspritTechnology.TechMoldConcentricFinishing()
            {
                OperationName = "Concentric Finishing",
                SpindleSpeedRPM = 8000,
                SpindleSpeedSPM = 452,
                FeedratePM = 1000,
                FeedratePT = 0.0625,
                ToolID = "EM1 (18mm BNEM)",
                FeedUnit = EspritConstants.espFeedUnit.espFeedPerMinute,
                PlungeFeedRatePercent = 100,
                LateralFeedRatePercent = 100,
                CalculateStock = true,
                RotaryMachining = true,
                ToolPositionAngle = angle,
                Tolerance = 0.012,
                StepPercentOfDiameter = 25,
                MaximumSlopeAngle = 90,
                StepOver = 4.5,
                OffsetSide = EspritConstants.espMoldConcSideOffset.espMoldConcSideOffsetLeft,
                MachiningDirection = EspritConstants.espMoldConcentricMachiningDirection.espMoldConcentricMachiningDirectionInsideOut,
                PassOrder = EspritConstants.espMoldConcentricPassOrder.espMoldConcentricPassOrderPerRegionShortestDistance,
                PositionOnBoundaryProfile = EspritConstants.espMoldPositionOnBoundaryProfile.espMoldPositionOnBoundaryProfileInside,
                ApproachInsideBoundary = true,
                RetractOptimization = EspritConstants.espMoldRetractOptimization.espMoldRetractOptimizationWithin,
                Clearance = 2,
                FullClearance = 70,
                FullClearanceReference = fullClearanceReference, // identifies 2 or 3 FreeForm feature
                ExtraMoves = EspritConstants.espMoldConcentricFullFilled.espMoldConcentricFullFilled_No,
                PositionOnModelLimit = EspritConstants.espMoldPositionOnModelLimit.espMoldPositionOnModelLimitOutsideOffset,
                ModelLimitOffset = 6.0
            };

            EspritTechnology.TechMoldApproach entry1 = new EspritTechnology.TechMoldApproach()
            {
                EntryMovesType = EspritConstants.espMoldEntryMovesType.espMoldEntryMovesHelical,
                VerticalDistance = 12,
                HelixAngle = 10,
                HelixDiameter = 10
            };
            EspritTechnology.TechMoldApproach entry2 = new EspritTechnology.TechMoldApproach()
            {
                EntryMovesType = EspritConstants.espMoldEntryMovesType.espMoldEntryMovesVerticalThenRadius,
                ArcAngle = 30,
                ArcRadius = 9,
                VerticalDistance = 3
            };
            EspritTechnology.TechMoldApproach entry3 = new EspritTechnology.TechMoldApproach()
            {
                EntryMovesType = EspritConstants.espMoldEntryMovesType.espMoldEntryMovesVertical,
                VerticalDistance = 3
            };
            var entries = new List<EspritTechnology.TechMoldApproach>()
            {
                 entry1,
                 entry2,
                 entry3
            };

            finishing.set_EntryMovesStrategies(entries.ToArray());

            EspritTechnology.TechMoldDetach exit1 = new EspritTechnology.TechMoldDetach()
            {
                ExitMovesType = EspritConstants.espMoldExitMovesType.espMoldExitMovesVerticalThenRadius,
                ArcAngle = 30,
                ArcRadius = 9,
                VerticalDistance = 1,
                EntryMovesAngle = 60
            };
            EspritTechnology.TechMoldDetach exit2 = new EspritTechnology.TechMoldDetach()
            {
                ExitMovesType = EspritConstants.espMoldExitMovesType.espMoldExitMovesVertical,
                VerticalDistance = 3
            };

            var exitModes = new List<EspritTechnology.TechMoldDetach>()
            {
                 exit1,
                 exit2
            };
            finishing.set_ExitMovesStrategies(exitModes.ToArray());

            return finishing;
        }

        private void AddConcentricFinishingOperation()
        {
            var freeFormFeature2 = CreateFreeFormFeature("2 FreeForm", EspritConstants.espFreeFormElementType.espFreeFormCheckSurfaceItem, 2154);
            var freeFormFeature3 = CreateFreeFormFeature("3 FreeForm", EspritConstants.espFreeFormElementType.espFreeFormCheckSurfaceItem, 1853);

            var finishing1 = FinishingOperation(0, "1003,2");
            var finishing2 = FinishingOperation(30, "1003,3");

            AddFeatureOperation(freeFormFeature2, finishing1);
            AddFeatureOperation(freeFormFeature3, finishing2);
        }

        //! [Code snippet finishing]

        //! [Code snippet wrapped]

        private void CreateAutoChainOnLoop(int loopIdentity)
        {
            EspritSolids.SolidBody body = Document.Solids[1].SolidBody;

            var set = SelectionSetHelper.GetOrAddSelectionSet(Document.SelectionSets, "Temp");
            set.RemoveAll();

            foreach (EspritSolids.SolidLoop loop in body.SolidLoops)
            {
                if (loop.Identity == loopIdentity)
                {
                    set.Add(loop);
                    break;
                }
            }

            if (set.Count > 0)
            {
                try
                {
                    Document.FeatureRecognition.CreateAutoChains(set);
                }
                catch (System.Runtime.InteropServices.COMException)
                { }
            }
            Document.SelectionSets.Remove(set.Name);
        }

        private void WrappedMilling()
        {
            CreateAutoChainOnLoop(3971);

            EspritTechnology.TechMillWrapPocket wrapPocketing = new EspritTechnology.TechMillWrapPocket()
            {
                OperationName = "Wrap Pocketing",
                ToolID = "EM3 (20mm BM)",
                FinishToolID = "EM3 (20mm BM)",
                FloorFinishToolID = "EM3 (20mm BM)",
                SpindleSpeedRPM = 6000,
                SpindleSpeedSPM = 377,
                XYFeedratePM = 1000,
                XYFeedratePT = 0.08333,
                ZFeedratePM = 1000,
                ZFeedratePT = 0.083333,
                FeedUnit = EspritConstants.espFeedUnit.espFeedPerRevolution,
                CalculateStock = true,
                WrapFeature = true,
                WorkingDiameter = 25,
                TypeOfWork = EspritConstants.espWrapDiameterSide.espWrapOutsideDiameter,
                WallType = EspritConstants.espMillWrapPocketMoveType.espMillWrapPocketMoveTypeRadialWall,
                Tolerance = 0.1,
                TotalDepth = 6,
                IncrementalDepth = 6,
                IncrDepthCalculation = EspritConstants.espPocketIncrDepthCalculation.espPocketIncrDepthCalculationVarying,
                RetractForIdepth = EspritConstants.espRetractMode.espRetractClearanceAboveSurface,
                StepOver = 10, 
                StepPercentOfDiameter = 50,
                EdgeClearance = 1,
                CylindricalFeatureTolerance = 1,
                OutputRoughPasses = true,
                ToolMotionPattern = EspritConstants.espMillPocketMovement.espMillPocketConcentricOut,
                FullEngageFeedPercent = 100,
                EntryMode = EspritConstants.espMillPocketEntryMode.espMillPocketEntryModePlunge,
                ExitMode = EspritConstants.espMillPocketExitMode.espMillPocketExitModeUp,
                Clearance = 1,
                FullClearance = 35,
                RapidPartialDepth = true,
                FullClearanceFrom = "6,2",
                MachiningPriority = EspritConstants.espMillPocketMachiningPriority.espMillPocketMachiningPriorityRegion,
                FinishPassesAfter = EspritConstants.espMillPocketPassesAfter.espMillPocketPassesAfter_REWAF
            };

            foreach (Esprit.FeatureChain chain in Document.FeatureChains)
            {
                if (chain.Name == "2 Chain")
                {
                    AddFeatureOperation(chain, wrapPocketing);
                    break;
                }
            }
        }

        //! [Code snippet wrapped]

        //! [Code snippet drilling]

        private void RecognizeHoles()
        {
            var solidArray = new List<Esprit.IGraphicObject>();

            solidArray.Add(Document.Solids[1]);

            try
            {
                var conf = EspritApplication.Configuration.ConfigurationHoleRecognition2;
                conf.MaxDiameterInch = 30;
                conf.MaxDiameterMetric = 30;
                conf.MinDiameterInch = 0;
                conf.MinDiameterMetric = 0;

                conf.CombineCoaxialHoles = true;
                conf.PropagateHoleFace = true;
                conf.ActiveWorkplaneOnly = false;
                conf.SplitCustomHole = false;

                conf.GroupBySameDiameters = true;

                conf.GroupBySameDirection = false;
                conf.GroupBySameAltitude = false;
                conf.GroupByCadFeature = false;

                Document.FeatureRecognition.CreateHoleFeatures2(solidArray.ToArray(), out var comFaults);

                for (var i = 1; i <= comFaults.Count; i++)
                {
                    var msgType = comFaults[i].Severity == EspritComBase.espFaultSeverity.espFaultWarning ? EspritConstants.espMessageType.espMessageTypeWarning : EspritConstants.espMessageType.espMessageTypeError;
                    EspritApplication.EventWindow.AddMessage(msgType, "RotaryMillingTutorial", comFaults[i].Description);
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Error on Hole Features recognition");
            }
        }

        private void AddDrillingOperation()
        {
            EspritTechnology.TechMillHoleMachining drillingOperation = new EspritTechnology.TechMillHoleMachining()
            {
                OperationName = "Drilling",
                ToolID = "DR4 (12mm Drill)",
                SpindleSpeedRPM = 800,
                SpindleSpeedSPM = 30,
                ZFeedratePM = 400,
                ZFeedratePR = 0.5,
                FeedUnit = EspritConstants.espFeedUnit.espFeedPerMinute,
                CalculateStock = true,
                MachiningStrategy = EspritConstants.espCenterHoleMachiningStrategy.espCenterHoleMachiningStrategyToolSpinning,
                DepthFromFeature = true,
                DwellTime = 1,
                DwellRevolution = 0,
                CannedCycleNCOutput = true,
                PeckingMode = EspritConstants.espHolePeckingMode.espHolePeckingModeConstant,
                PeckIncrement = 10,
                PeckClearance = 1,
                Clearance = 10,
                CycleType = EspritConstants.espHoleCycleType.espHoleCycleTypeDrillingWithChipRemoval
            };

            foreach (Esprit.HolesFeature hole in Document.HolesFeatures)
            {
                if (hole.Diameter < 30)
                {
                    AddFeatureOperation(hole, drillingOperation);
                    break;
                }
            }
        }

        private void AddCenterDrillingOperation()
        {
            EspritTechnology.TechMillHoleMachining drillingOperation = new EspritTechnology.TechMillHoleMachining()
            {
                OperationName = "Center Drilling",
                ToolID = "DR6 (30mm Drill)",
                SpindleSpeedRPM = 600,
                SpindleRPM = 600,
                RetractSpindleSpeedRPM = 50,
                RetractSpindleSpeedSPM = 5,
                FeedUnit = EspritConstants.espFeedUnit.espFeedPerMinute,
                SpindleSpeedSPM = 57,
                ZFeedratePM = 80,
                ZFeedratePR = 0.13333,
                CalculateStock = true,
                MachiningStrategy = EspritConstants.espCenterHoleMachiningStrategy.espCenterHoleMachiningStrategyPartSpinning,
                DepthFromFeature = true,
                DwellTime = 1,
                DwellRevolution = 0,
                CannedCycleNCOutput = true,
                PeckingMode = EspritConstants.espHolePeckingMode.espHolePeckingModeConstant,
                PeckIncrement = 10,
                PeckClearance = 1,
                Clearance = 15,
                CycleType = EspritConstants.espHoleCycleType.espHoleCycleTypeDrilling,
                ThroughDepth = 5,
                AutomaticWorkOffsetRotation = true,
                RigidTapping = true,
                ApproachSpindleDirection = EspritConstants.espSpindleDirection.espSpindleCW,
                ApproachSpindleSpeedRPM = 50,
                ApproachSpindleSpeedSPM = 5,
                EntryFeedratePercent = 100,
                IntersectionFeedratePercent = 200,
                PartialContactFeedratePercent = 25,
                IntersectionClearance = 0.04,
                FullRetractBetweenHoles = true,
                FullClearance = 10,
                FullClearanceFrom = "1006,5",
                LiveStockUpdating = true,
                StockAutomation = EspritConstants.espStockAutomationDrilling.espStockAutomationDrillingStartDepth
            };

            foreach (Esprit.HolesFeature hole in Document.HolesFeatures)
            {
                if (hole.Diameter == 30)
                {
                    AddFeatureOperation(hole, drillingOperation);
                    break;
                }
            }

        }

        //! [Code snippet drilling]

        //! [Code snippet contouring]

        private void AddContouringOperation()
        {
            EspritSolids.SolidBody body = Document.Solids[1].SolidBody;

            var set = SelectionSetHelper.GetOrAddSelectionSet(Document.SelectionSets, "Temp");
            set.RemoveAll();

            foreach (EspritSolids.SolidLoop loop in body.SolidLoops)
            {
                if (set.Count > 0)
                {
                    break;
                }
                foreach (EspritSolids.SolidEdge edge in loop.SolidEdges)
                {
                    if (edge.Identity == 3486)
                    {
                        set.Add(edge);
                        break;
                    }
                }
            }

            if (set.Count > 0)
            {
                try
                {
                    Document.FeatureRecognition.CreateAutoChains(set);
                }
                catch (System.Runtime.InteropServices.COMException)
                { }
            }

            Document.SelectionSets.Remove(set.Name);

            EspritTechnology.TechMillContour1 contouringOperation = new EspritTechnology.TechMillContour1()
            {
                OperationName = "Contouring",
                ToolID = "EM1 (18mm BNEM)",
                SpindleSpeedRPM = 8000,
                SpindleSpeedSPM = 452,
                XYFeedratePM = 1000,
                XYFeedratePT = 0.0625,
                ZFeedratePM = 1000,
                ZFeedratePT = 0.0625,
                FeedUnit = EspritConstants.espFeedUnit.espFeedPerMinute,
                CalculateStock = true,
                RotaryMachining = true,
                FinishStockAllowanceWalls = -6.0,
                StockAllowanceFloors = 1.0,
                ProcessOrder = EspritConstants.espMillContourProcessOrder.espMillContourProcessOrderWidthFirst,
                SpiralTolerance = 0.005,
                IncrementalDepth = 18,
                RetractForIdepth = EspritConstants.espRetractMode.espRetractClearanceAboveSurface,
                ReadTaperFromFeature = true,
                OffsetSideComputer = EspritConstants.espMillContourOffsetSide.espMillContourOffsetLeft,
                OffsetToolRadius = true,
                CutterCompNC = EspritConstants.espOffsetSide.espOffsetLeft,
                OffsetRegisterNC = 1,
                Trimming = EspritConstants.espMillContourTrimming.espMillContourTrimCollapse,
                ToolBlend = true,
                LookAhead = EspritConstants.espLookAheadMode.espLookAheadOn,
                IslandCollisionCheck = EspritConstants.espIslandCollisionDetection.espIslandCollisionSkip,
                Clearance = 2,
                RetractBetweenCuts = EspritConstants.espRetractMode.espRetractClearanceAboveSurface,
                EntryMode = EspritConstants.espMillEntryMode.espMillEntryDownThenOver,
                ExitMode = EspritConstants.espExitMode.espExitOverThenUp,
                LeadInType = EspritConstants.espMillLeadinType.espMillLeadinTangent,
                LeadOutType = EspritConstants.espMillLeadoutType.espMillLeadoutTangent,
                TangentLeadInDistance = 11,
                TangentLeadOutDistance = 11,
                StartOvercut = 9,
                StartOvercutPercentage = 50,
                EndOvercut = 9,
                EndOvercutPercentage = 50,
                RoughPasses = 1
            };

            Document.Refresh();

            foreach (Esprit.FeatureChain chain in Document.FeatureChains)
            {
                if (chain.Name == "3 Chain")
                {
                    AddFeatureOperation(chain, contouringOperation);
                    break;
                }
            }
        }

        //! [Code snippet contouring]

        //! [Code snippet spiraling]

        private void AddSpiralingOperation()
        {
            CreateAutoChainOnLoop(45);

            EspritTechnology.TechMillSpiral spiralingOperation = new EspritTechnology.TechMillSpiral()
            {
                OperationName = "Spiraling",
                ToolID = "EM4 (10mm EM)",
                SpindleSpeedRPM = 4000,
                SpindleSpeedSPM = 126,
                XYFeedratePM = 1000,
                XYFeedratePT = 0.125,
                ZFeedratePM = 1000,
                ZFeedratePT = 0.125,
                FeedUnit = EspritConstants.espFeedUnit.espFeedPerMinute,
                CalculateStock = true,
                HoleOrder = EspritConstants.espHoleMachiningOrder.espHoleMachiningOrderFeatureOrder,
                RotaryMachining = true,
                Strategy = EspritConstants.espMillSpiralStrategy.espMillSpiralStrategyTangentArcs,
                Direction = EspritConstants.espClockDirection.espCCW,
                ClearingStep = 2.5,
                StepPercentOfDiameter = 25,
                DrillHoleDiameter = 30,
                Tolerance = 0.005,
                TotalDepth = 0,
                IncrementalDepth = 10,
                RetractForIdepth = EspritConstants.espRetractMode.espRetractClearanceAboveDepth,
                OffsetToolRadius = true,
                CutterCompNC = EspritConstants.espOffsetSide.espOffsetOff,
                Clearance = 2,
                ExitMode = EspritConstants.espExitMode.espExitUpThenOver,
                EntryMode = EspritConstants.espMillEntryMode.espMillEntryOverThenDown,
                LeadInType = EspritConstants.espMillLeadinType.espMillLeadinRadius,
                LeadInDistance = 25,
                LeadInRadius = 10,
                LeadOutType = EspritConstants.espMillLeadoutType.espMillLeadoutTangent,
                LeadOutDistance = 5
            };

            foreach (Esprit.FeatureChain chain in Document.FeatureChains)
            {
                if (chain.Name == "4 Chain")
                {
                    AddFeatureOperation(chain, spiralingOperation);
                    break;
                }
            }
        }

        //! [Code snippet spiraling]

        //! [Code snippet engraving]

        private void AddEngravingOperation()
        {
            EspritTechnology.TechMillEngraving engravingOperation = new EspritTechnology.TechMillEngraving()
            {
                OperationName = "Engraving",
                ToolID = "EM5 (CM)",
                SpindleSpeedRPM = 8000,
                SpindleSpeedSPM = 251,
                XYFeedratePM = 1000,
                XYFeedratePT = 0.0625,
                ZFeedratePM = 1000,
                ZFeedratePT = 0.0625,
                AutomaticWorkOffsetRotation = true,
                FeedUnit = EspritConstants.espFeedUnit.espFeedPerMinute,
                CalculateStock = true,
                FontType = EspritConstants.espMillEngraveFontType.espMillEngraveFontTypeSingleStroke,
                SingleStrokeFontName = "0-DIN 17 F.csv",
                Text = "ESPRIT",
                CharacterHeight = 8,
                CharacterWidth = 8,
                TextPattern = EspritConstants.espMillEngraveTextPattern.espMillEngraveTextPatternCircular,
                CenterPointX = 0,
                CenterPointY = 0,
                CenterPointZ = 51.562,
                Radius = 22,
                TextDirection = EspritConstants.espMillEngraveTextDirection.espMillEngraveTextDirectionCW,
                StartAngle = 90,
                HorizontalAlignment = EspritConstants.espMillEngraveHorizontalAlignment.espMillEngraveHorizontalAlignmentCenter,
                VerticalAlignment = EspritConstants.espMillEngraveVerticalAlignment.espMillEngraveVerticalAlignmentCenter,
                SpiralTolerance = 0.1,
                TotalDepth = 0.5,
                TypeOfWork = EspritConstants.espWrapDiameterSide.espWrapOutsideDiameter,
                WorkingDiameter = EspritConstants.espWrapDiameterSide.espWrapOutsideDiameter,
                RotaryMachining = true,
                RetractForIdepth = EspritConstants.espRetractMode.espRetractClearanceAboveSurface,
                PolarInterpolation = false,
                LengthCompOverride = false,
                FullClearance = 10,
                Clearance = 2,
                EntryMode = EspritConstants.espMillPocketEntryMode.espMillPocketEntryModePlunge,
                StepOver = 5,
                StepPercentOfDiameter = 50,
                CharacterWidthPercent = 100,
                EngraveStrategy = EspritConstants.espMillEngraveStrategy.espMillEngraveStrategyContour,
                SecondaryAxisClamp = EspritConstants.espAxisClamp.espAxisClampOff,
                PrimaryAxisClamp = EspritConstants.espAxisClamp.espAxisClampOff,
                TertiaryAxisClamp = EspritConstants.espAxisClamp.espAxisClampOff
            };

            foreach (Esprit.FeatureChain chain in Document.FeatureChains)
            {
                if (chain.Name == "4 Chain")
                {
                    AddFeatureOperation(chain, engravingOperation);
                    break;
                }
            }

            Document.PartOperations.RebuildAll();
        }

        //! [Code snippet engraving]

        public override void Execute()
        {
            CreateDialog();
        }

        public override string Name => "Rotary Milling";
        public override string HtmlPath => "html/rotary_milling_tutorial.html";
    }
}
