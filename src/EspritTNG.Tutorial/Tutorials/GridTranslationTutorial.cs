using System;
using System.Windows.Forms;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class GridTranslationTutorial : BaseTutorial
    {
        public GridTranslationTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void GridTranslation()
        {
            var groupName = "Temp";
            var set = SelectionSetHelper.GetSelectionSet(Document, groupName);

            set.RemoveAll();
            var originalCount = Document.Group.Count;
            for (var i = 1; i <= originalCount; i++)
            {
                set.Add(Document.Group[i]);
            }

            set.AddCopiesToSelectionSet = true;
            var firstGridNumber = 10;
            var secondGridNumber = 10;
            var firstDistance = 1.0;
            var secondDistance = 1.0;
            var firstAngle = 0.0;
            var secondAngle = 90.0;

            if (RequestUserInput("Enter First Number of Grid Items", "First Number", ref firstGridNumber) &&
                RequestUserInput("Enter Second Number of Grid Items", "Second Number", ref secondGridNumber) &&
                RequestUserInput("Enter First Distance", "First Distance", ref firstDistance) &&
                RequestUserInput("Enter Second Distance", "Second Distance", ref secondDistance) &&
                RequestUserInput("Enter First Angle", "First Angle", ref firstAngle) &&
                RequestUserInput("Enter Second Angle", "Second Angle", ref secondAngle))
            {
                firstAngle *= Math.PI / 180;
                secondAngle *= Math.PI / 180;
                set.Translate(firstDistance * Math.Cos(firstAngle), firstDistance * Math.Sin(firstAngle), 0, firstGridNumber - 1);
                set.Translate(secondDistance * Math.Cos(secondAngle), secondDistance * Math.Sin(secondAngle), 0, secondGridNumber - 1);
            }

            Document.Refresh();

            for (var i = 1; i <= originalCount; i++)
            {
                var graphicObject = set[i] as Esprit.IGraphicObject;
                if (graphicObject != null)
                    graphicObject.Grouped = false;
            }

            Document.SelectionSets.Remove(groupName);
        }

        //! [Code snippet]

        public override void Execute()
        {
            GridTranslation();
        }

        public override string Name => "More SelectionSet Manipulations: Grid Translation";
        public override string HtmlPath => "html/grid_translation_tutorial.html";

    }
}
