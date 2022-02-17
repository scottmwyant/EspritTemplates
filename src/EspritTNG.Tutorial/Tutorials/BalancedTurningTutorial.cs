using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialCSharp.Tutorials
{
    class BalancedTurningTutorial : BaseTutorial
    {
        public BalancedTurningTutorial(Esprit.Application app) : base(app)
        {
        }

        //! [Code snippet]

        private EspritTechnology.TechLatheBalanceRough2 CreateBalancedTurningTechnology()
        {
            EspritTechnology.TechLatheBalanceRough2 balancedTurning = new EspritTechnology.TechLatheBalanceRough2()
            {
                SpindleSpeedRPM = 5000,
                FeedratePR = 0.05,
                SpeedUnit = EspritConstants.espSpindleSpeedUnit.espSpindleSpeedConstantSurface,
                FeedUnit = EspritConstants.espFeedUnit.espFeedPerRevolution,
                TransverseFeedratePercent = 50,
                MaximumRPM = 6000,
                ReferenceDiameter = 35,
                CalculateStock = true,

                TypeOfWork = EspritConstants.espGrooveWorkType.espGrooveWorkOD,
                MasterChannel = EspritConstants.espBalancedTurningMasterChannel.espBalancedTurningMasterChannelLeading,
                UndercuttingMode = EspritConstants.espLatheUndercuttingMode.espLatheUndercuttingYes,
                CollisionDetection = true,

                StockType = EspritConstants.espLatheRoughStockType.espLatheRoughStockLive,

                PullOutAngle = 45,
                FinishLeadInType = EspritConstants.espLatheFinishLeadType.espLatheFinishLeadTangent,
                FinishLeadOutType = EspritConstants.espLatheFinishLeadType.espLatheFinishLeadTangent,

                LeadOutAngle = 90,
                ChamferAngle = 45,
                ChamferDistance = 0.02,

                DepthOfCut = 1,
                RetractPercentOfDepth = 100,
                PullOutMode = EspritConstants.espPullOutType1.espPullOutAlongFeature,
                DepthClearance = 2,
                ClearanceAlongCut = 2,
                LeadInType = EspritConstants.espLeadType.espLeadNormal,
                LeadOutType = EspritConstants.espLeadType.espLeadNormal,

                EntryMode = EspritConstants.espLatheEntryMode1.espLatheEntry1None,
                ExitMode = EspritConstants.espLatheExitMode1.espLatheExit1None,
            };

            balancedTurning.TrailingToolDefinition["498"].Value = "OD Lower";

            var parameter = balancedTurning.TrailingToolDefinition.FindParameter("LatheToolOrientation");
            if (parameter != null)
            {
                parameter.Value = EspritConstants.espLatheToolOrientation.espLatheToolOrientationReverseAngle;
            }

            parameter = balancedTurning.TrailingToolDefinition.FindParameter("LatheToolOrientationAngle");
            if (parameter != null)
            {
                parameter.Value = -180.0;
            }

            parameter = balancedTurning.TrailingToolDefinition.FindParameter("CoolantOverride");
            if (parameter != null)
            {
                parameter.Value = false;
            }

            parameter = balancedTurning.TrailingToolDefinition.FindParameter("Coolant");
            if (parameter != null)
            {
                parameter.Value = 0;
            }

            parameter = balancedTurning.TrailingToolDefinition.FindParameter("CoolantPressure");
            if (parameter != null)
            {
                parameter.Value = 1;
            }

            parameter = balancedTurning.TrailingToolDefinition.FindParameter("WorkOffsetRotationPlane");
            if (parameter != null)
            {
                parameter.Value = "XYZ";
            }

            parameter = balancedTurning.TrailingToolDefinition.FindParameter("AutomaticWorkOffsetRotation");
            if (parameter != null)
            {
                parameter.Value = false;
            }

            balancedTurning.LeadingToolDefinition["498"].Value = "OD Upper";

            parameter = balancedTurning.LeadingToolDefinition.FindParameter("CoolantOverride");
            if (parameter != null)
            {
                parameter.Value = false;
            }

            parameter = balancedTurning.LeadingToolDefinition.FindParameter("LatheToolOrientation");
            if (parameter != null)
            {
                parameter.Value = EspritConstants.espLatheToolOrientation.espLatheToolOrientationAngle;
            }

            parameter = balancedTurning.LeadingToolDefinition.FindParameter("LatheToolOrientationAngle");
            if (parameter != null)
            {
                parameter.Value = 0;
            }

            parameter = balancedTurning.LeadingToolDefinition.FindParameter("Coolant");
            if (parameter != null)
            {
                parameter.Value = 0;
            }

            parameter = balancedTurning.LeadingToolDefinition.FindParameter("CoolantPressure");
            if (parameter != null)
            {
                parameter.Value = 1;
            }

            parameter = balancedTurning.LeadingToolDefinition.FindParameter("AutomaticWorkOffsetRotation");
            if (parameter != null)
            {
                parameter.Value = false;
            }

            parameter = balancedTurning.LeadingToolDefinition.FindParameter("WorkOffsetRotationPlane");
            if (parameter != null)
            {
                parameter.Value = "XYZ";
            }

            return balancedTurning;
        }

        private void BalancedTurning()
        {
            var balancedTurning = CreateBalancedTurningTechnology();
            balancedTurning.OperationName = "Balanced Roughing";
            balancedTurning.PinchTurningMode = EspritConstants.espPinchTurningMode.espPinchTurningModeTrailingTool;
            balancedTurning.TrailingDistance = 1;
            balancedTurning.ZStockForRough = 0.5;
            balancedTurning.XStockForRough = 0.5;
            balancedTurning.FinishUseFeedSpeedKB = true;

            Document.GUI.EditTechnology(balancedTurning);

            Document.Group.Clear();

            foreach (Esprit.FeatureChain feature in Document.FeatureChains)
            {
                if (feature.Name == "2 Chain")
                {
                    Document.Group.Add(feature);
                    Document.PartOperations.Add(balancedTurning, feature);
                    break;
                }
            }

            var syncPosition1 = Document.Program.Channels[1].ChannelItems[5].Before;
            var syncPosition2 = Document.Program.Channels[2].ChannelItems[1].Before;
            Document.Program.Syncs.Add(syncPosition1, syncPosition2);
        }

        private void BalancedFinishing()
        {
            var balancedTurning = CreateBalancedTurningTechnology();
            balancedTurning.OperationName = "Balanced Finishing";
            balancedTurning.PinchTurningMode = EspritConstants.espPinchTurningMode.espPinchTurningModeSimultaneous;
            balancedTurning.OutputRoughPasses = false;
            balancedTurning.FinishPass = true;

            balancedTurning.ZStockForRough = 0;
            balancedTurning.XStockForRough = 0;

            balancedTurning.FinishSpindleSpeedRPM = 6000;
            balancedTurning.FinishFeedratePR = 0.05;
            balancedTurning.FinishLeadInTangentDistance = 1;

            balancedTurning.FinishLeadInType = EspritConstants.espLatheFinishLeadType.espLatheFinishLeadTangent;

            balancedTurning.FinishUseFeedSpeedKB = false;
            balancedTurning.TrailingDistance = 1;

            Document.Group.Clear();

            Document.GUI.EditTechnology(balancedTurning);

            foreach (Esprit.FeatureChain feature in Document.FeatureChains)
            {
                if (feature.Name == "2 Chain")
                {
                    Document.Group.Add(feature);
                    Document.PartOperations.Add(balancedTurning, feature);
                    break;
                }
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            BalancedTurning();
            BalancedFinishing();
        }

        public override string Name => "Balanced Turning";
        public override string HtmlPath => "html/balanced_turning_tutorial.html";
    }
}
