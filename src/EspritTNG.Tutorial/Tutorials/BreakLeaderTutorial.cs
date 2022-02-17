using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class BreakLeaderTutorial : BaseTutorial
    {
        public BreakLeaderTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void BreakLeader()
        {
            Esprit.Leader leader = null;
            try
            {
                leader = Document.GetAnyElement("Select Leader to Break", EspritConstants.espGraphicObjectType.espLeader) as Esprit.Leader;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                MessageBox.Show("No leader was selected", "BreakLeaderTutorial");
                return;
            }

            if (leader == null)
            {
                return;
            }

            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "BreakLeader", $"Selected Leader contains {leader.LeaderPoints.Count} points.");

            var leaderPoints = new List<Esprit.Point>();
            for (var i = 1; i <= leader.LeaderPoints.Count; i++)
            {
                leaderPoints.Add(leader.LeaderPoints.Item[i]);
            }

            var index = 2;
            var mask = new EspritConstants.espGraphicObjectType[2]
                {
                    EspritConstants.espGraphicObjectType.espLeader,
                    EspritConstants.espGraphicObjectType.espPoint
                };

            do
            {
                Esprit.IGraphicObject graphicObject = null;
                try
                {
                    graphicObject = Document.GetAnyElement($"Select New Leader Vertex {index} Reselect Leader To Move to Next Vertex", mask);
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    return;
                }

                if(graphicObject == null)
                {
                    return;
                }

                if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espPoint)
                {
                    leaderPoints.Add(leaderPoints.Last());

                    for (var j = leader.LeaderPoints.Count; j >= index; j--)
                    {
                        leaderPoints[j] = leaderPoints[j - 1];
                        leader.LeaderPoints.Remove(j);
                    }

                    leaderPoints[index - 1] = graphicObject as Esprit.Point;

                    for (var j = index; j <= leaderPoints.Count; j++)
                    {
                        leader.LeaderPoints.Add(leaderPoints[j - 1]);
                    }

                    Document.Refresh();

                    if (MessageBox.Show("Continue to Break?", "Continue", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        break;
                    }
                }

                index++;

            } while (index <= leader.LeaderPoints.Count);

            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "BreakLeader", $"After operation the Leader contains {leader.LeaderPoints.Count} points.");
        }

        //! [Code snippet]

        public override void Execute()
        {
            BreakLeader();
        }

        public override string Name => "Break Leader";
        public override string HtmlPath => "html/break_leader_tutorial.html";

    }
}
