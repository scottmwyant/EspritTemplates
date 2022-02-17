using System;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class FeatureChainCirclesTutorial : BaseTutorial
    {
        public FeatureChainCirclesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void FeatureChainCircles()
        {
            var previousActiveColor = Document.ActiveColor;
            Document.ActiveColor = ColorHelper.ColorToUInt(System.Drawing.Color.Blue);

            foreach (Esprit.Circle circul in Document.Circles)
            {
                var featureChain = Document.FeatureChains.Add(Document.GetPoint(circul.X, circul.Y + circul.Radius, circul.Z));
                featureChain.Add(Document.GetArc(circul.CenterPoint, circul.Radius, 90 * Math.PI / 180, (90 + 360) * Math.PI / 180));
            }

            Document.Windows.ActiveWindow.Fit();
            Document.Refresh();

            Document.ActiveColor = previousActiveColor;
        }

        //! [Code snippet]

        public override void Execute()
        {
            FeatureChainCircles();
        }

        public override string Name => "FeatureChain Circles";
        public override string HtmlPath => "html/featurechain_circles_tutorial.html";

    }
}
