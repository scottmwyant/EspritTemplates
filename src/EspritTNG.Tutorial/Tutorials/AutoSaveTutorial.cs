using System.IO;

namespace TutorialCSharp.Tutorials
{

    //! [Code snippet declare]

    class AutoSaveHandler
    {
        private readonly Esprit.Application _espritApplication;
        private int _commandCounter;

        public int AutoSaveInterval;
        public string AutoSaveName;

        public AutoSaveHandler(Esprit.Application app)
        {
            _espritApplication = app;
            AutoSaveInterval = 100;
            _commandCounter = 0;
            AutoSaveName = GetAutoSaveName();

            _espritApplication.Document.AfterDocumentSave += AfterDocumentSave;
            _espritApplication.AfterCommandRun += AfterCommandRun;
        }

        private void AfterCommandRun(int nCmdId, int nAuxId)
        {
            _commandCounter++;

            if (_commandCounter >= AutoSaveInterval)
            {
                _espritApplication.Document.SaveCopyAs(AutoSaveName);
                _commandCounter = 0;
            }
        }

        private void AfterDocumentSave()
        {
            if (_commandCounter < AutoSaveInterval)
            {
                _commandCounter = 0;
                AutoSaveName = GetAutoSaveName();
            }
        }

        public string GetAutoSaveName()
        {
            var path = (_espritApplication.Document.FileName != string.Empty)
                ? Path.Combine(Path.GetDirectoryName(_espritApplication.Document.FileName), Path.GetFileNameWithoutExtension(_espritApplication.Document.FileName) + "_AutoSave.esprit")
                : _espritApplication.TempDir + "AutoSave.esprit";

            return path;
        }
    }

    //! [Code snippet declare]

    class AutoSaveTutorial : BaseTutorial
    {
        public AutoSaveTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet use]

        private AutoSaveHandler _testObject;

        public override void Execute()
        {
            _testObject = new AutoSaveHandler(EspritApplication)
            {
                AutoSaveInterval = 10
            };
        }

        //! [Code snippet use]

        public override string Name => "Auto Save";
        public override string HtmlPath => "html/auto_save_tutorial.html";

    }
}
