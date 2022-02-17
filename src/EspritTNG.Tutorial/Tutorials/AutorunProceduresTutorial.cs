using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class AutoRunProceduresTutorial : BaseTutorial
    {
        private bool _isSubscribed = false;

        public AutoRunProceduresTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public override void Execute()
        {
            if (!_isSubscribed)
            {
                _isSubscribed = true;
                EspritApplication.BeforeQuit += OnBeforeApplicationQuit;
                EspritApplication.AfterDocumentOpen += OnAfterDocumentOpen;
                EspritApplication.AfterDocumentClose += OnAfterDocumentClose;
                EspritApplication.AfterNewDocumentOpen += OnAfterNewDocumentOpen;
            }
        }

        private void OnBeforeApplicationQuit()
        {
            MessageBox.Show("OnBeforeApplicationQuit() runs before ESPRIT shutdown...", "AutoRunProceduresTutorial");
        }

        private void OnAfterDocumentClose()
        {
            MessageBox.Show("OnAfterDocumentClose() runs after Document Close", "AutoRunProceduresTutorial");
        }

        private void OnAfterDocumentOpen(string FileName)
        {
            MessageBox.Show("OnAfterDocumentOpen() runs after Document Open", "AutoRunProceduresTutorial");
        }

        private void OnAfterNewDocumentOpen()
        {
            MessageBox.Show("OnAfterNewDocumentOpen() runs after New Document Open", "AutoRunProceduresTutorial");
        }

        //! [Code snippet]

        public override string Name => "Introduction to Auto Run Procedures";
        public override string HtmlPath => "html/autorun_procedures_tutorial.html";

    }
}
