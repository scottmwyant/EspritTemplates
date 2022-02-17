using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class RectFeatureChainTutorial : BaseTutorial
    {
        public RectFeatureChainTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void RectangularFeatureChain()
        {
            Esprit.Point point1 = null;
            Esprit.Point point2 = null;

            try
            {
                point1 = Document.GetPoint("Pick First Corner Point");
                point2 = Document.GetPoint("Pick Second Corner Point");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if (point1 == null || point2 == null)
            {
                return;
            }

            var featureChain = Document.FeatureChains.Add(point1);

            featureChain.Add(Document.GetSegment(point1, Document.GetPoint(point1.X, point2.Y, 0)));
            featureChain.Add(Document.GetSegment(Document.GetPoint(point1.X, point2.Y, 0), point2));
            featureChain.Add(Document.GetSegment(point2, Document.GetPoint(point2.X, point1.Y, 0)));
            featureChain.Add(Document.GetSegment(Document.GetPoint(point2.X, point1.Y, 0), point1));

            featureChain.Color = ColorHelper.ColorToUInt(System.Drawing.Color.Blue);
            Document.Refresh();
        }

        //! [Code snippet]

        public override void Execute()
        {
            RectangularFeatureChain();
        }

        public override string Name => "Rectangular FeatureChain";
        public override string HtmlPath => "html/rect_featurechain_tutorial.html";

    }
}
