using System;

namespace TutorialCSharp.Tutorials
{
    class SimpleSquareAnywhereTutorial : BaseTutorial
    {
        public SimpleSquareAnywhereTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void SimpleSquareAnywhere()
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
                    // Point was not set
                    return;
                }

                if (point == null)
                {
                    return;
                }

                Document.Segments.Add(point, Document.GetPoint(point.X, point.Y + length, 0));
                Document.Segments.Add(Document.GetPoint(point.X, point.Y + length, 0), Document.GetPoint(point.X + length, point.Y + length, 0));
                Document.Segments.Add(Document.GetPoint(point.X + length, point.Y + length, 0), Document.GetPoint(point.X + length, point.Y, 0));
                Document.Segments.Add(Document.GetPoint(point.X + length, point.Y, 0), point);

                Document.Refresh();
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            SimpleSquareAnywhere();
        }

        public override string Name => "Simple Square Anywhere";
        public override string HtmlPath => "html/simple_square_anywhere_tutorial.html";

    }
}
