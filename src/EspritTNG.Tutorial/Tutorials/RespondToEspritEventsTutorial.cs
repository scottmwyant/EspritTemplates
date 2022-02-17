namespace TutorialCSharp.Tutorials
{
    //! [Code snippet declare]

    class RespondToEspritEventsHandler
    {
        private readonly Esprit.Application _espritApplication;
        public RespondToEspritEventsHandler(Esprit.Application app)
        {
            _espritApplication = app;
            app.AfterCommandRun += AfterCommandRun;
            app.AfterDocumentClose += AfterDocumentClose;
            app.AfterDocumentOpen += AfterDocumentOpen;
            app.AfterNewDocumentOpen += AfterNewDocumentOpen;
            app.AfterTemplateOpen += AfterTemplateOpen;
            app.BeforeCommandRun += BeforeCommandRun;
            app.BeforeQuit += BeforeQuit;
        }

        private void BeforeQuit()
        {
            _espritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "RespondToEspritEventsTutorial", "BeforeQuit Event");
        }

        private void BeforeCommandRun(int nCmdId, int nAuxId, ref bool bOverride)
        {
            _espritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "RespondToEspritEventsTutorial", $"BeforeCommandRun Event, nCmdID = {nCmdId}, nAuxId = {nAuxId}");
        }

        private void AfterNewDocumentOpen()
        {
            _espritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "RespondToEspritEventsTutorial", "AfterNewDocumentOpen Event");
        }

        private void AfterDocumentOpen(string fileName)
        {
            _espritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "RespondToEspritEventsTutorial", $"AfterDocumentOpen Event, FileName = {fileName}");
        }

        private void AfterTemplateOpen(string fileName)
        {
            _espritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "RespondToEspritEventsTutorial", $"AfterTemplateOpen Event, FileName = {fileName}");
        }

        private void AfterDocumentClose()
        {
            _espritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "RespondToEspritEventsTutorial", $"AfterDocumentClose Event");
        }

        private void AfterCommandRun(int nCmdId, int nAuxId)
        {
            _espritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "RespondToEspritEventsTutorial", $"AfterCommandRun Event, nCmdID = {nCmdId}, nAuxId = {nAuxId}");
        }
    }

    //! [Code snippet declare]

    class RespondToEspritEventsTutorial : BaseTutorial
    {
        public RespondToEspritEventsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet use]

        private RespondToEspritEventsHandler _testObject;

        public void ApplicationEventExample()
        {
            if (_testObject == null)
            {
                EspritApplication.EventWindow.Visible = true;
                _testObject = new RespondToEspritEventsHandler(EspritApplication);
            }
        }

        //! [Code snippet use]

        public override void Execute()
        {
            ApplicationEventExample();
        }

        public override string Name => "Using Class Modules to Respond to ESPRIT Events";
        public override string HtmlPath => "html/respond_to_esprit_events_tutorial.html";

    }
}
