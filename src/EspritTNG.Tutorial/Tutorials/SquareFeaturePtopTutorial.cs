using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class SquareFeaturePtopTutorial : BaseTutorial
    {
        public SquareFeaturePtopTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void SquareFeaturePtop()
        {
            var length = 10.0;

            if (RequestUserInput("Enter Length", "Square Length", ref length))
            {
                var point = Document.Points.Add(0, 0, 0);

                Esprit.FeaturePtop featurePtop = Document.FeaturePtops.Add(point);
                featurePtop.Color = ColorHelper.ColorToUInt(System.Drawing.Color.Blue);
                featurePtop.LineType = EspritConstants.espLineType.espPhantom;

                point = Document.Points.Add(0, length, 0);
                featurePtop.Add(point);

                point = Document.Points.Add(length, length, 0);
                featurePtop.Add(point);

                point = Document.Points.Add(length, 0, 0);
                featurePtop.Add(point);

                Document.Refresh();
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            SquareFeaturePtop();
        }

        public override string Name => "Square FeaturePtop";
        public override string HtmlPath => "html/square_featureptop_tutorial.html";

    }
}
