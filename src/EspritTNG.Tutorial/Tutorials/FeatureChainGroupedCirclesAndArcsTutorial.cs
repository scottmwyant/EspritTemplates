using System;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class FeatureChainGroupedCirclesAndArcsTutorial : BaseTutorial    {
        public FeatureChainGroupedCirclesAndArcsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void FeatureChainGroupedCirclesAndArcs()
        {
            var angle = 90.0;
            if (!RequestUserInput("Enter Starting Angle", "Starting Angle", ref angle))
            {
                return;
            }

            angle = angle * Math.PI / 180;
            var previousActiveColor = Document.ActiveColor;
            Document.ActiveColor = ColorHelper.ColorToUInt(System.Drawing.Color.Blue);

            foreach (Esprit.Circle circul in Document.Circles)
            {
                if (circul.Grouped)
                {
                    var featureChain = Document.FeatureChains.Add(Document.GetPoint(circul.X + circul.Radius * Math.Cos(angle), circul.Y + circul.Radius * Math.Sin(angle), circul.Z));
                    featureChain.Layer = circul.Layer;
                    featureChain.Add(Document.GetArc(circul.CenterPoint, circul.Radius, angle, 2 * Math.PI + angle));
                }
            }

            foreach (Esprit.Arc arc in Document.Arcs)
            {
                if (arc.Grouped && (Math.Abs(arc.EndAngle - arc.StartAngle) == 2 * Math.PI))
                {
                    var featureChain = Document.FeatureChains.Add(arc.Extremity(EspritConstants.espExtremityType.espExtremityStart));
                    featureChain.Layer = arc.Layer;
                    featureChain.Add(arc);
                }
            }

            Document.ActiveColor = previousActiveColor;

            WindowHelper.FitAllWindows(Document.Windows);
        }

        //! [Code snippet]

        public override void Execute()
        {
            FeatureChainGroupedCirclesAndArcs();
        }

        public override string Name => "FeatureChain Grouped Circles and Arcs";
        public override string HtmlPath => "html/featurechain_grouped_circles_arcs_tutorial.html";

    }
}
