using System;
using System.Drawing;
using EspritConstants;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class FeatureChainVisibleCirclesArcsTutorial : BaseTutorial
    {
        public FeatureChainVisibleCirclesArcsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void FeatureChainVisibleCirclesAndArcs()
        {
            var angleDegree = 90.0;

            if (!RequestUserInput("Enter Starting Angle", "Starting Angle", ref angleDegree))
            {
                return;
            }

            var angleRad = angleDegree * Math.PI / 180;
            var previousActiveColor = Document.ActiveColor;
            Document.ActiveColor = ColorHelper.ColorToUInt(Color.Blue);

            //Because you cannot add a circle to a FeatureChain, make temporary arc instead
            foreach (Esprit.Circle circle in Document.Circles)
            {
                if (circle.Layer.Visible)
                {
                    var featureChain = Document.FeatureChains.Add(
                        Document.GetPoint(circle.X + circle.Radius * Math.Cos(angleRad), circle.Y + circle.Radius * Math.Sin(angleRad), circle.Z));
                    featureChain.Layer = circle.Layer;
                    featureChain.Add(Document.GetArc(circle.CenterPoint, circle.Radius, angleRad, 2 * Math.PI + angleRad));
                }
            }

            foreach (Esprit.Arc arc in Document.Arcs)
            {
                if (arc.Layer.Visible)
                {
                    var tolerance = Document.SystemUnit == espUnitType.espInch
                        ? 0.00001
                        : 0.00025;

                    if (Math.Abs(arc.EndAngle - arc.StartAngle - 2 * Math.PI) < tolerance)
                    {
                        var featureChain = Document.FeatureChains.Add(arc.Extremity(EspritConstants.espExtremityType.espExtremityStart));
                        featureChain.Add(arc);
                        featureChain.Layer = arc.Layer;
                    }

                }
            }

            Document.ActiveColor = previousActiveColor;
            WindowHelper.FitAllWindows(Document.Windows);
        }

        //! [Code snippet]

        public override void Execute()
        {
            FeatureChainVisibleCirclesAndArcs();
        }

        public override string Name => "FeatureChain Visible Circles and Arcs";
        public override string HtmlPath => "html/featurechain_visible_circles_arcs_tutorial.html";

    }
}
