using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class FitAllWindowsTutorial : BaseTutorial
    {
        public FitAllWindowsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void FitAllWindowsDemo()
        {
            WindowHelper.FitAllWindows(Document.Windows);
        }

        //! [Code snippet]

        public override void Execute()
        {
            FitAllWindowsDemo();
        }

        public override string Name => "Fit All Windows";
        public override string HtmlPath => "html/fit_all_windows_tutorial.html";

    }
}
