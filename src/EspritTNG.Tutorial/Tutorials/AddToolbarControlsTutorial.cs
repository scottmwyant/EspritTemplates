using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class AddToolbarControlsTutorial : BaseTutorial
    {
        public AddToolbarControlsTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            MessageBox.Show($"There is no Application.ToolBars API", "AddToolbarControlsTutorial");
        }

        public override string Name => "Add Toolbar Controls and Add a Toolbar";
        public override string HtmlPath => "html/add_toolbar_controls_tutorial.html";

    }
}
