using System;

namespace TutorialCSharp.Tutorials
{
     class CreateHoleProcessingInfoTutorial : BaseTutorial
    {
        public CreateHoleProcessingInfoTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void HoleProcessingInfo()
        {
            var text = "Describe Center Drill / Drill / Tap or Any Other Info Here";

            if (!RequestUserInput("Enter Processing Info", "Processing Info", ref text))
            {
                return;
            }

            var configuration = EspritApplication.Configuration;

            foreach (Esprit.Circle circule in Document.Circles)
            {
                if (!circule.Grouped)
                {
                    continue;
                }

                var point1 = Document.GetPoint(circule.X + circule.Radius * Math.Cos(Math.PI / 4), circule.Y + circule.Radius * Math.Sin(Math.PI / 4), circule.Z);
                var point2 = Document.GetPoint(circule.X + 1.5 * circule.Radius, circule.Y + 1.5 * circule.Radius, circule.Z);
                var point3 = Document.GetPoint(circule.X + 1.75 * circule.Radius, circule.Y + 1.5 * circule.Radius, circule.Z);
                var leader = Document.Annotations.AddLeader(point1, point2);

                leader.LeaderPoints.Add(point3);
                leader.Color = configuration.ConfigurationDimensions.Color;
                leader.LineType = configuration.ConfigurationDimensions.LineType;

                var notes = Document.Annotations.AddNotes();
                notes.Color = configuration.ConfigurationNotes.Color;
                var note = notes.Add();
                note.Font.Name = "Arial";
                note.Font.Size = 6;
                note.Font.Bold = true;
                note.RotationAngle = configuration.ConfigurationAnnotation.RotationAngle;

                note.String = $"R{circule.Radius}";
                note.X = point3.X + 0.1 * circule.Radius;
                note.Y = point3.Y;
                note.Z = point3.Z;

                note = notes.Add();
                note.Font.Name = "Arial";
                note.Font.Size = 6;
                note.Font.Bold = true;
                note.RotationAngle = configuration.ConfigurationAnnotation.RotationAngle;
                note.String = text;
                note.X = point3.X + 0.1 * circule.Radius;
                note.Y = point3.Y - note.Height;
                note.Z = point3.Z;
            }

            foreach (Esprit.Arc arc in Document.Arcs)
            {
                if (!arc.Grouped || Math.Abs(arc.EndAngle - arc.StartAngle) != 2 * Math.PI)
                {
                    continue;
                }

                var point1 = Document.GetPoint(arc.X + arc.Radius * Math.Cos(Math.PI / 4), arc.Y + arc.Radius * Math.Sin(Math.PI / 4), arc.Z);
                var point2 = Document.GetPoint(arc.X + 1.5 * arc.Radius, arc.Y + 1.5 * arc.Radius, arc.Z);
                var point3 = Document.GetPoint(arc.X + 1.75 * arc.Radius, arc.Y + 1.5 * arc.Radius, arc.Z);
                var leader = Document.Annotations.AddLeader(point1, point2);

                leader.LeaderPoints.Add(point3);
                leader.Color = configuration.ConfigurationDimensions.Color;
                leader.LineType = configuration.ConfigurationDimensions.LineType;

                var notes = Document.Annotations.AddNotes();
                notes.Color = configuration.ConfigurationNotes.Color;
                var note = notes.Add();
                note.Font.Name = "Arial";
                note.Font.Size = 6;
                note.Font.Bold = true;
                note.RotationAngle = configuration.ConfigurationAnnotation.RotationAngle;

                note.String = $"R{arc.Radius}";
                note.X = point3.X + 0.1 * arc.Radius;
                note.Y = point3.Y + 0.5 * configuration.ConfigurationAnnotation.HeightSpacing * note.Height;
                note.Z = point3.Z;

                note = notes.Add();
                note.Font.Name = "Arial";
                note.Font.Size = 6;
                note.Font.Bold = true;
                note.RotationAngle = configuration.ConfigurationAnnotation.RotationAngle;
                note.String = text;
                note.X = point3.X + 0.1 * arc.Radius;
                note.Y = point3.Y - note.Height - 0.5 * configuration.ConfigurationAnnotation.HeightSpacing * note.Height;
                note.Z = point3.Z;
            }

        }

        //! [Code snippet]

        public override void Execute()
        {
            HoleProcessingInfo();
        }

        public override string Name => "Create Hole Processing Info";
        public override string HtmlPath => "html/create_hole_processing_info_tutorial.html";

    }
}
