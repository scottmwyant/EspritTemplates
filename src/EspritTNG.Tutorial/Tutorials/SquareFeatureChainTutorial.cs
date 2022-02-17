using System.Collections.Generic;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class SquareFeatureChainTutorial : BaseTutorial
    {
        public SquareFeatureChainTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void SquareFeatureChain()
        {
            var length = 10.0;

            if (RequestUserInput("Enter Length", "Square Length", ref length))
            {
                Esprit.Point point = null;
                try
                {
                    point = Document.GetPoint("Pick Corner Point for Square");
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // Element was not set
                    return;
                }

                if (point == null)
                {
                    return;
                }

                var segments = new List<Esprit.Segment>
                {
                    Document.Segments.Add(point, Document.GetPoint(point.X, point.Y + length, 0)),
                    Document.Segments.Add(Document.GetPoint(point.X, point.Y + length, 0), Document.GetPoint(point.X + length, point.Y + length, 0)),
                    Document.Segments.Add(Document.GetPoint(point.X + length, point.Y + length, 0), Document.GetPoint(point.X + length, point.Y, 0)),
                    Document.Segments.Add(Document.GetPoint(point.X + length, point.Y, 0), point)
                };

                var featureChain = Document.FeatureChains.Add(point);
                featureChain.Color = ColorHelper.ColorToUInt(System.Drawing.Color.Blue);

                foreach (var segment in segments)
                {
                    featureChain.Add(segment);
                }

                Document.Refresh();
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            SquareFeatureChain();
        }

        public override string Name => "Square FeatureChain";
        public override string HtmlPath => "html/square_feature_chain_tutorial.html";

    }
}
