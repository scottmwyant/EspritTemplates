using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class ListToolbarsTutorial : BaseTutorial
    {
        public ListToolbarsTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            MessageBox.Show($"There is no Application.ToolBars API", "ListToolbarsTutorial");
        }

        public override string Name => "List Toolbars";
        public override string HtmlPath => "html/list_toolbars_tutorial.html";

    }
}
