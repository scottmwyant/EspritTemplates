using System.Text;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class ScanFeatureHolesTutorial : BaseTutorial
    {
        public ScanFeatureHolesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void ScanFeatureHoles()
        {
            foreach(Esprit.IHolesFeature feature in Document.HolesFeatures)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Common data:");
                stringBuilder.AppendLine($"\tFeature Hole name: {feature.Name}");
                stringBuilder.AppendLine($"\tFeature Hole depth: {feature.Depth}");
                stringBuilder.AppendLine($"\tFeature Hole diameter: {feature.Diameter}");
                stringBuilder.AppendLine($"\tFeature Hole throughAll: {feature.ThroughAll}");
                stringBuilder.AppendLine($"\tFeature Hole Plane Name: {feature.Plane.Name}");
                stringBuilder.AppendLine($"\tFeature Hole WorkCoordinate Name: {feature.WorkCoordinate.Name}");

                var comFeature = feature.Object as EspritFeatures.ComFeatureHoles;
                stringBuilder.AppendLine($"\tFeature Holes count: {comFeature.Count}");

                var holes = comFeature.GetHoles();
                for (var i = holes.GetLowerBound(0); i <= holes.GetUpperBound(0); i++)
                {
                    var hole = holes.GetValue(i) as EspritFeatures.ComFeatureHole;
                    stringBuilder.AppendLine($"Hole {i+1} data:");
                    stringBuilder.AppendLine($"\tHole diameter: {hole.HoleDiameter}");
                    stringBuilder.AppendLine($"\tHole depth: {hole.HoleDepth}");
                    stringBuilder.AppendLine($"\tHole throughAll: {hole.ThroughAll}");
                }

                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "HolesFeature", stringBuilder.ToString());
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            ScanFeatureHoles();
        }

        public override string Name => "Scan Feature Holes";
        public override string HtmlPath => "html/scan_feature_holes_tutorial.html";

    }
}
