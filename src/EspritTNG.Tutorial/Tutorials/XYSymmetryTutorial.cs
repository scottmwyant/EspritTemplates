using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class XYSymmetryTutorial : BaseTutorial
    {
        public XYSymmetryTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void XYSymmetry()
        {
            var groupName = "Temp";
            var set = SelectionSetHelper.GetSelectionSet(Document, groupName);

            set.RemoveAll();
            for(var i = 1; i <= Document.Group.Count; i++)
            {
                set.Add(Document.Group[i]);
            }

            set.AddCopiesToSelectionSet = true;
            set.Symmetry(Document.GetLine(Document.GetPoint(0, 0, 0), 1, 0, 0), true, true);
            set.Symmetry(Document.GetLine(Document.GetPoint(0, 0, 0), 0, 1, 0), true, true);
            Document.Refresh();

            Document.SelectionSets.Remove(groupName);
        }

        //! [Code snippet]

        public override void Execute()
        {
            XYSymmetry();
        }

        public override string Name => "Introduction to SelectionSet Manipulations: XY Symmetry";
        public override string HtmlPath => "html/x_y_symmetry_tutorial.html";

    }
}
