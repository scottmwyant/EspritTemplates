using System;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    //! [Code snippet declare]

    class ClassModulesHandler : IDisposable
    {
        public ClassModulesHandler()
        {
            MessageBox.Show("This is the Constructor procedure.", "ClassModulesHandler", MessageBoxButton.OK);
        }

        public void Dispose()
        {
            MessageBox.Show("This is the Terminate procedure.", "ClassModulesHandler", MessageBoxButton.OK);
        }
    }

    //! [Code snippet declare]

    class ClassModulesTutorial : BaseTutorial
    {
        public ClassModulesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet use]

        public void ClassExample1()
        {
            ClassModulesHandler testObject = new ClassModulesHandler();
            testObject.Dispose();
        }

        //! [Code snippet use]

        public override void Execute()
        {
            ClassExample1();
        }

        public override string Name => "Introduction to Class Modules";
        public override string HtmlPath => "html/class_modules_tutorial.html";

    }
}
