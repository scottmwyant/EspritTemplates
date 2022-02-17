using System;
using System.Collections.Generic;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class CreateCenterLinesAsLeadersTutorial : BaseTutorial
    {
        public CreateCenterLinesAsLeadersTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void CenterLinesAsLeaders()
        {
            if (Document.Circles.Count == 0 && Document.Arcs.Count == 0)
            {
                MessageBox.Show("There is no Arc or Circle", "CreateCenterLinesAsLeadersTutorial");
                return;
            }

            var ratio1 = 1.15;
            var ratio2 = 0.2;
            var ratio3 = 0.4;

            foreach (Esprit.Circle circle in Document.Circles)
            {
                if (!circle.Grouped)
                {
                    continue;
                }

                var leaders = new List<Esprit.Leader>();
                leaders.Add(Document.Annotations.AddLeader(
                    Document.GetPoint(circle.X - ratio1 * circle.Radius, circle.Y, circle.Z),
                    Document.GetPoint(circle.X - ratio3 * circle.Radius, circle.Y, circle.Z))
                );

                leaders.Add(Document.Annotations.AddLeader(
                    Document.GetPoint(circle.X - ratio2 * circle.Radius, circle.Y, circle.Z),
                    Document.GetPoint(circle.X + ratio2 * circle.Radius, circle.Y, circle.Z))
                );

                leaders.Add(Document.Annotations.AddLeader(
                    Document.GetPoint(circle.X + ratio3 * circle.Radius, circle.Y, circle.Z),
                    Document.GetPoint(circle.X + ratio1 * circle.Radius, circle.Y, circle.Z))
                );

                leaders.Add(Document.Annotations.AddLeader(
                    Document.GetPoint(circle.X, circle.Y - ratio1 * circle.Radius, circle.Z),
                    Document.GetPoint(circle.X, circle.Y - ratio3 * circle.Radius, circle.Z))
                );

                leaders.Add(Document.Annotations.AddLeader(
                    Document.GetPoint(circle.X, circle.Y - ratio2 * circle.Radius, circle.Z),
                    Document.GetPoint(circle.X, circle.Y + ratio2 * circle.Radius, circle.Z))
                );

                leaders.Add(Document.Annotations.AddLeader(
                    Document.GetPoint(circle.X, circle.Y + ratio3 * circle.Radius, circle.Z),
                    Document.GetPoint(circle.X, circle.Y + ratio1 * circle.Radius, circle.Z))
                );

                foreach(var leader in leaders)
                {
                    leader.ArrowType = EspritConstants.espArrowType.espArrowNone;
                    leader.Layer = circle.Layer;

                    leader.Color = EspritApplication.Configuration.ConfigurationDimensions.Color;
                    leader.LineType = EspritApplication.Configuration.ConfigurationDimensions.LineType;
                    leader.LineWeight = EspritApplication.Configuration.ConfigurationDimensions.LineWeight;
                }
            }

            foreach (Esprit.Arc arc in Document.Arcs)
            {
                if (!arc.Grouped || Math.Abs(arc.EndAngle - arc.StartAngle) != 2 * Math.PI)
                {
                    continue;
                }

                var leaders = new List<Esprit.Leader>();
                leaders.Add(Document.Annotations.AddLeader(
                    Document.GetPoint(arc.X - ratio1 * arc.Radius, arc.Y, arc.Z),
                    Document.GetPoint(arc.X - ratio3 * arc.Radius, arc.Y, arc.Z))
                );

                leaders.Add(Document.Annotations.AddLeader(
                    Document.GetPoint(arc.X - ratio2 * arc.Radius, arc.Y, arc.Z),
                    Document.GetPoint(arc.X + ratio2 * arc.Radius, arc.Y, arc.Z))
                );

                leaders.Add(Document.Annotations.AddLeader(
                    Document.GetPoint(arc.X + ratio3 * arc.Radius, arc.Y, arc.Z),
                    Document.GetPoint(arc.X + ratio1 * arc.Radius, arc.Y, arc.Z))
                );

                leaders.Add(Document.Annotations.AddLeader(
                    Document.GetPoint(arc.X, arc.Y - ratio1 * arc.Radius, arc.Z),
                    Document.GetPoint(arc.X, arc.Y - ratio3 * arc.Radius, arc.Z))
                );

                leaders.Add(Document.Annotations.AddLeader(
                    Document.GetPoint(arc.X, arc.Y - ratio2 * arc.Radius, arc.Z),
                    Document.GetPoint(arc.X, arc.Y + ratio2 * arc.Radius, arc.Z))
                );

                leaders.Add(Document.Annotations.AddLeader(
                    Document.GetPoint(arc.X, arc.Y + ratio3 * arc.Radius, arc.Z),
                    Document.GetPoint(arc.X, arc.Y + ratio1 * arc.Radius, arc.Z))
                );

                foreach (var leader in leaders)
                {
                    leader.ArrowType = EspritConstants.espArrowType.espArrowNone;
                    leader.Layer = arc.Layer;

                    leader.Color = EspritApplication.Configuration.ConfigurationDimensions.Color;
                    leader.LineType = EspritApplication.Configuration.ConfigurationDimensions.LineType;
                    leader.LineWeight = EspritApplication.Configuration.ConfigurationDimensions.LineWeight;
                }
            }

            Document.Refresh();
        }

        //! [Code snippet]

        public override void Execute()
        {
            CenterLinesAsLeaders();
        }

        public override string Name => "Create Center Lines as Leaders";
        public override string HtmlPath => "html/create_center_lines_as_leaders_tutorial.html";

    }
}
