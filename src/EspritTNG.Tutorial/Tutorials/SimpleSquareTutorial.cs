using System.Collections.Generic;

namespace TutorialCSharp.Tutorials 
{
    class SimpleSquareTutorial : BaseTutorial
    {
        public SimpleSquareTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void SimpleSquare()
        {
            var length = 10.0;

            if (RequestUserInput("Enter Length", "Square Length", ref length))
            {
                var points = new List<Esprit.Point>();
                points.Add(Document.GetPoint(0, 0, 0));             //Bottom left
                points.Add(Document.GetPoint(0, length, 0));        //Top left
                points.Add(Document.GetPoint(length, length, 0));   //Top right
                points.Add(Document.GetPoint(length, 0, 0));        //Bottom right

                Document.Segments.Add(points[0], points[1]);
                Document.Segments.Add(points[1], points[2]);
                Document.Segments.Add(points[2], points[3]);
                Document.Segments.Add(points[3], points[0]);

                Document.Refresh();
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            SimpleSquare();
        }

        public override string Name => "Simple Square";
        public override string HtmlPath => "html/simple_square_tutorial.html";

    }
}
