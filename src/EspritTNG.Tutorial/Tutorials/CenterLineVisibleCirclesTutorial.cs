
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class CenterLineVisibleCirclesTutorial : BaseTutorial
    {
        public CenterLineVisibleCirclesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void CenterLineVisibleCircles()
        {
            var segments = Document.Segments;
            Document.ActiveColor = ColorHelper.ColorToUInt(System.Drawing.Color.Green);
            var clRatio = new double[3]
            {
                1.15, 0.2, 0.4
            };

            foreach (Esprit.Circle circul in Document.Circles)
            {
                if (circul.Layer.Visible)
                {
                    segments.Add(Document.GetPoint(circul.X - clRatio[0] * circul.Radius, circul.Y, circul.Z), Document.GetPoint(circul.X - clRatio[2] * circul.Radius, circul.Y, circul.Z));
                    segments.Add(Document.GetPoint(circul.X - clRatio[1] * circul.Radius, circul.Y, circul.Z), Document.GetPoint(circul.X + clRatio[1] * circul.Radius, circul.Y, circul.Z));
                    segments.Add(Document.GetPoint(circul.X + clRatio[2] * circul.Radius, circul.Y, circul.Z), Document.GetPoint(circul.X + clRatio[0] * circul.Radius, circul.Y, circul.Z));
                    segments.Add(Document.GetPoint(circul.X, circul.Y - clRatio[0] * circul.Radius, circul.Z), Document.GetPoint(circul.X, circul.Y - clRatio[2] * circul.Radius, circul.Z));
                    segments.Add(Document.GetPoint(circul.X, circul.Y - clRatio[1] * circul.Radius, circul.Z), Document.GetPoint(circul.X, circul.Y + clRatio[1] * circul.Radius, circul.Z));
                    segments.Add(Document.GetPoint(circul.X, circul.Y + clRatio[2] * circul.Radius, circul.Z), Document.GetPoint(circul.X, circul.Y + clRatio[0] * circul.Radius, circul.Z));
                }
            }

            WindowHelper.FitAllWindows(Document.Windows);
        }

        //! [Code snippet]

        public override void Execute()
        {
            CenterLineVisibleCircles();
        }

        public override string Name => "Center Line Visible Circles";
        public override string HtmlPath => "html/centerline_visible_circles_tutorial.html";

    }
}
