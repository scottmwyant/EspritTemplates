using System.Collections.Generic;

namespace TutorialCSharp.Tutorials
{
    class SwarfCycleTutorial : BaseTutorial
    {
        public SwarfCycleTutorial(Esprit.Application app) : base(app)
        {
        }

        //! [Code snippet]

        private EspritTechnology.TechMill5xSwarfMerged CreateSwarfCycle(string operationName)
        {
            EspritTechnology.TechMill5xSwarfMerged swarfCycle = new EspritTechnology.TechMill5xSwarfMerged()
            {
                OperationName = operationName,
                ToolID = "D6R0.5_Swarf",
                SpindleSpeedRPM = 8000,
                SpindleSpeedSPM = 151,
                FeedratePM = 2000,
                FeedratePT = 0.125,
                PlungeFeedRatePercent = 100,
                LateralFeedRatePercent =  100,
                FeedUnit = EspritConstants.espFeedUnit.espFeedPerMinute,
                CalculateStock = true,
                WorkOffsetTransformation = EspritConstants.espWorkOffsetTransformation.espWorkOffsetTransformationRTCP,
                Tolerance = 0.01,
                SwarfInput = EspritConstants.espSwarfInput.espSwarfInputCurves,
                ToolReference = EspritConstants.espMoldSwarfToolReference.espMoldSwarfToolReferenceLowerProfile,
                CuttingSide = EspritConstants.espMillContourOffsetSide.espMillContourOffsetLeft,
                MachineNotRuledSurfaces = true,
                InitialAxisOrientation = EspritConstants.espMoldSwarfInitialAxisOrientation.espMoldSwarfInitialAxisOrientationJoin,
                FinalAxisOrientation = EspritConstants.espMoldSwarfFinalAxisOrientation.espMoldSwarfFinalAxisOrientationJoin,
                ProfilesSynchronization = EspritConstants.espMoldRuledSwarfSynchronization.espMoldRuledSwarfSynchronizationAdaptTAForBestFit,
                RetractOptimization = EspritConstants.espMoldRetractOptimization.espMoldRetractOptimizationWithin,
                Clearance = 2,
                FullClearance = 50,
                MoldLinearAxesMovement = EspritConstants.espMoldLinearAxesMovement.espMoldLinearAxesMovementRapid,
                SwarfIncrementStrategy = EspritConstants.espMoldSwarfIncrementStrategy.espMoldSwarfIncrementStrategyMorph,
                SwarfStartFromOffsetMorph = EspritConstants.espMoldSwarfStartFromOffsetMorph.espMoldSwarfStartFromOffsetMorphNo
            };

            EspritTechnology.TechMoldApproach entry = new EspritTechnology.TechMoldApproach()
            {
                EntryMovesType = EspritConstants.espMoldEntryMovesType.espMoldEntryMovesRadiusLateralPlane,
                ArcAngle = 30,
                ArcRadius = 3,
                VerticalDistance = 3,
                TangentDistance = 1,
                LeadReferenceDirection = EspritConstants.espLeadReferenceDirection.espLeadReferenceDirectionToolTip
            };

            var entries = new List<EspritTechnology.TechMoldApproach>()
            {
                 entry
            };

            swarfCycle.set_EntryMovesStrategies(entries.ToArray());

            return swarfCycle;
        }

        private void AddSwarfOperation(EspritTechnology.TechMill5xSwarfMerged technology, Esprit.IGraphicObject feature)
        {
            Document.Group.Clear();
            Document.Group.Add(feature);
            Document.PartOperations.Add(technology, feature);
            Document.Group.Clear();
        }

        private void SwarfLeft()
        {
            EspritTechnology.TechMill5xSwarfMerged swarfCycle = CreateSwarfCycle("Swarf Left");

            swarfCycle.UpperProfile = "6,6"; // identifies "LeftBlade_Top"
            swarfCycle.LowerProfile = "6,7"; // identifies "LeftBlade_Bottom"

            foreach (Esprit.FreeFormFeature feature in Document.FreeFormFeatures)
            {
                if (feature.Name == "LeftBlade")
                {
                    AddSwarfOperation(swarfCycle, feature);
                    break;
                }
            }
        }

        private void SwarfRight()
        {
            EspritTechnology.TechMill5xSwarfMerged swarfCycle = CreateSwarfCycle("Swarf Right");

            swarfCycle.UpperProfile = "6,8"; // identifies "RightBlade_Top"
            swarfCycle.LowerProfile = "6,9"; // identifies "RightBlade_Bottom"

            foreach (Esprit.FreeFormFeature feature in Document.FreeFormFeatures)
            {
                if (feature.Name == "RightBlade")
                {
                    AddSwarfOperation(swarfCycle, feature);
                    break;
                }
            }
        }

        //! [Code snippet]

        //! [Code snippet simulation]

        private void StartSimulation()
        {
            Document.Simulation.SetCollisions(true);
            Document.Simulation.Play();
        }

        //! [Code snippet simulation]

        public override void Execute()
        {
            SwarfLeft();
            SwarfRight();

            StartSimulation();
        }

        public override string Name => "Swarf Cycle";
        public override string HtmlPath => "html/swarf_cycle_tutorial.html";
    }
}
