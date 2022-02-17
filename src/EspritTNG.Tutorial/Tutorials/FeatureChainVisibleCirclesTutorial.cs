using System;
using System.Drawing;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class FeatureChainVisibleCirclesTutorial : BaseTutorial
    {
        public FeatureChainVisibleCirclesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void FeatureChainVisibleCircles()
        {
            double angleDegree = 90.0;
            if (!RequestUserInput("Enter Starting Angle", "Starting Angle", ref angleDegree))
            {
                return;
            }

            var angleRad = angleDegree * Math.PI / 180;
            var previousActiveColor = Document.ActiveColor;
            Document.ActiveColor = ColorHelper.ColorToUInt(Color.Blue);

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

            Document.ActiveColor = previousActiveColor;
            WindowHelper.FitAllWindows(Document.Windows);
        }

        //! [Code snippet]

        public override void Execute()
        {
            FeatureChainVisibleCircles();
        }

        public override string Name => "FeatureChain Visible Circles";
        public override string HtmlPath => "html/featurechain_visible_circles_tutorial.html";

    }
}
