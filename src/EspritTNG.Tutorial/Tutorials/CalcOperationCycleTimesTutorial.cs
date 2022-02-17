using System.Windows;
using EspritConstants;

namespace TutorialCSharp.Tutorials
{
    class CalcOperationCycleTimesTutorial : BaseTutorial
    {
        public CalcOperationCycleTimesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void CalculateCycleTimes()
        {
            var rapidrate = Document.SystemUnit == espUnitType.espMetric
                ? 2500
                : 100;

            var totalTime = 0.0;
            var time = 0.0;
            var feedrate = 0.0;
            var elapseTimeString = string.Empty;

            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            foreach (Esprit.PartOperation partOperation in Document.PartOperations)
            {
                var technology = partOperation.Technology as EspritTechnology.Technology;
                switch (technology.TechnologyType)
                {
                    case espTechnologyType.espTechLatheBalanceRough:
                        var lbr = partOperation.Technology as EspritTechnology.TechLatheBalanceRough;
                        feedrate = lbr.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheBarfeed:
                        var lbe = partOperation.Technology as EspritTechnology.TechLatheBarfeed;
                        feedrate = lbe.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheContour:
                        var lcn = partOperation.Technology as EspritTechnology.TechLatheContour;
                        feedrate = lcn.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheCopyRough:
                        var lcr = partOperation.Technology as EspritTechnology.TechLatheCopyRough;
                        feedrate = lcr.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheCutoff:
                        var lco = partOperation.Technology as EspritTechnology.TechLatheCutoff;
                        feedrate = lco.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheDrill:
                        var ldr = partOperation.Technology as EspritTechnology.TechLatheDrill;
                        feedrate = ldr.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheGroove:
                        var lgr = partOperation.Technology as EspritTechnology.TechLatheGroove;
                        feedrate = lgr.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheManual:
                        var lmn = partOperation.Technology as EspritTechnology.TechLatheManual;
                        feedrate = lmn.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheMillContour:
                        var lmc = partOperation.Technology as EspritTechnology.TechLatheMillContour;
                        feedrate = lmc.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheMillDrill:
                        var lmd = partOperation.Technology as EspritTechnology.TechLatheMillDrill;
                        feedrate = lmd.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheMillPocket:
                        var lmp = partOperation.Technology as EspritTechnology.TechLatheMillPocket;
                        feedrate = lmp.FeedratePM;
                        break;
                    case espTechnologyType.espTechLathePark:
                        feedrate = 0;
                        time = 2;
                        break;
                    case espTechnologyType.espTechLathePickup:
                        var lpu = partOperation.Technology as EspritTechnology.TechLathePickup;
                        feedrate = lpu.FeedratePM;
                        break;
                    case espTechnologyType.espTechLathePocket:
                        var lpo = partOperation.Technology as EspritTechnology.TechLathePocket;
                        feedrate = lpo.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheRelease:
                        var lrl = partOperation.Technology as EspritTechnology.TechLatheRelease;
                        feedrate = lrl.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheRough:
                        var lrg = partOperation.Technology as EspritTechnology.TechLatheRough;
                        feedrate = lrg.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheTailstock:
                        var lts = partOperation.Technology as EspritTechnology.TechLatheTailstock;
                        feedrate = lts.FeedratePM;
                        break;
                    case espTechnologyType.espTechLatheThread:
                        var lth = partOperation.Technology as EspritTechnology.TechLatheThread;
                        feedrate = lth.FeedratePM;
                        break;
                    case espTechnologyType.espTechMill3DFinish:
                        var l3f = partOperation.Technology as EspritTechnology.TechMill3DFinish;
                        feedrate = l3f.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMill3DProject:
                        var m3p = partOperation.Technology as EspritTechnology.TechMill3DProject;
                        feedrate = m3p.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMill3DRemachining:
                        var m3rem = partOperation.Technology as EspritTechnology.TechMill3DRemachining;
                        feedrate = m3rem.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMill3DRough:
                        var m3rgh = partOperation.Technology as EspritTechnology.TechMill3DRough;
                        feedrate = m3rgh.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMill3DZLevelFinish:
                        var m3zl = partOperation.Technology as EspritTechnology.TechMill3DZLevelFinish;
                        feedrate = m3zl.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMillContour:
                        var mcn = partOperation.Technology as EspritTechnology.TechMillContour;
                        feedrate = mcn.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMillContour1:
                        var mcn1 = partOperation.Technology as EspritTechnology.TechMillContour1;
                        feedrate = mcn1.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMillContour4x:
                        var mc4 = partOperation.Technology as EspritTechnology.TechMillContour4x;
                        feedrate = mc4.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMillDrill:
                        var mdr = partOperation.Technology as EspritTechnology.TechMillDrill;
                        feedrate = mdr.ZFeedratePM;
                        break;
                    case espTechnologyType.espTechMillDrill4x:
                        var md4 = partOperation.Technology as EspritTechnology.TechMillDrill4x;
                        feedrate = md4.ZFeedratePM;
                        break;
                    case espTechnologyType.espTechMillFace:
                        var mfc = partOperation.Technology as EspritTechnology.TechMillFace;
                        feedrate = mfc.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMillManual:
                        var mmn = partOperation.Technology as EspritTechnology.TechMillManual;
                        feedrate = mmn.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMillPark:
                        feedrate = 0;
                        time = 2;
                        break;
                    case espTechnologyType.espTechMillPocket:
                        var mpo = partOperation.Technology as EspritTechnology.TechMillPocket;
                        feedrate = mpo.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMillPocket1:
                        var mpo1 = partOperation.Technology as EspritTechnology.TechMillPocket1;
                        feedrate = mpo1.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMillPocket2:
                        var mpo2 = partOperation.Technology as EspritTechnology.TechMillPocket2;
                        feedrate = mpo2.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMillPocket4x:
                        var mpo4 = partOperation.Technology as EspritTechnology.TechMillPocket4x;
                        feedrate = mpo4.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMillSpiral:
                        var msp = partOperation.Technology as EspritTechnology.TechMillSpiral;
                        feedrate = msp.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechMillThread:
                        var mth = partOperation.Technology as EspritTechnology.TechMillThread;
                        feedrate = mth.XYFeedratePM;
                        break;
                    case espTechnologyType.espTechUnknown:
                        feedrate = 0;
                        time = 0;
                        break;
                }

                if (feedrate != 0)
                    time = partOperation.LengthOfFeed / feedrate + partOperation.LengthOfRapid / rapidrate;

                elapseTimeString += $"OP {partOperation.Key} time is {time.ToString("F")} minutes\n";

                totalTime += time;
            }

            if (elapseTimeString != string.Empty)
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "CalcOperationCycleTimesTutorial", elapseTimeString);

            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "CalcOperationCycleTimesTutorial", $"Total program time is {totalTime.ToString("F")} minutes");
        }

        //! [Code snippet]

        public override void Execute()
        {
            CalculateCycleTimes();
        }

        public override string Name => "Calculate Operation Cycle Times";
        public override string HtmlPath => "html/calc_operation_cycle_times_tutorial.html";

    }
}
