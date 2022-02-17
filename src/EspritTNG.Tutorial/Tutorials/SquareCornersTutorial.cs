using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    class SquareCornersTutorial : BaseTutorial
    {
        public SquareCornersTutorial(Esprit.Application application): base(application)
        {
        }

        //! [Code snippet]

        public void SquareCorners()
        {
            double length = 10;

            if (RequestUserInput("Enter Length", "Square Length", ref length))
            {
                Document.Points.Add(0, 0, 0);
                Document.Points.Add(0, length, 0);
                Document.Points.Add(length, length, 0);
                Document.Points.Add(length, 0, 0);

                Document.Refresh();
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            SquareCorners();
        }

        public override string Name => "Square Corners";
        public override string HtmlPath => "html/square_corners_tutorial.html";

    }
}
