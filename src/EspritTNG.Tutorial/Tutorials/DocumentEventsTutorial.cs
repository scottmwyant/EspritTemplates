using System.Windows;

namespace TutorialCSharp.Tutorials
{

    //! [Code snippet declare]

    class DocumentEventsHandler
    {
        private readonly Esprit.Application _espritApplication;
        public DocumentEventsHandler(Esprit.Application app)
        {
            _espritApplication = app;
            _espritApplication.Document.AfterDocumentPrint += AfterDocumentPrint;
            _espritApplication.Document.BeforeDocumentPrint += BeforeDocumentPrint;
            _espritApplication.Document.BeforeDocumentSave += BeforeDocumentSave;
            _espritApplication.Document.AfterDocumentSave += AfterDocumentSave;
            _espritApplication.Document.BeforeDocumentClose += BeforeDocumentClose;
            _espritApplication.Document.OnActiveWorkCoordinateChanged += OnActiveWorkCoordinateChanged;
            _espritApplication.Document.OnDocumentMerge += OnDocumentMerge;
            _espritApplication.Document.OnLeftMouseClick += OnLeftMouseClick;
        }

        private void OnLeftMouseClick(double x, double y, double z)
        {
            MessageBox.Show("OnLeftMouseClick Event", "DocumentEventsTutorial", MessageBoxButton.OK);
        }

        private void OnDocumentMerge(string mergeFileName)
        {
            MessageBox.Show("OnDocumentMerge Event", "DocumentEventsTutorial", MessageBoxButton.OK);
        }

        private void OnActiveWorkCoordinateChanged(object workCoordinate)
        {
            MessageBox.Show("OnActiveWorkCoordinateChanged Event", "DocumentEventsTutorial", MessageBoxButton.OK);
        }

        private void AfterDocumentSave()
        {
            MessageBox.Show("AfterDocumentSave Event", "DocumentEventsTutorial", MessageBoxButton.OK);
        }

        private void BeforeDocumentClose()
        {
            MessageBox.Show("BeforeDocumentClose Event", "DocumentEventsTutorial", MessageBoxButton.OK);
        }

        private void BeforeDocumentSave()
        {
            MessageBox.Show("BeforeDocumentSave Event", "DocumentEventsTutorial", MessageBoxButton.OK);
        }

        private void BeforeDocumentPrint()
        {
            MessageBox.Show("BeforeDocumentPrint Event", "DocumentEventsTutorial", MessageBoxButton.OK);
        }

        private void AfterDocumentPrint()
        {
            MessageBox.Show("AfterDocumentPrint Event", "DocumentEventsTutorial", MessageBoxButton.OK);
        }
    }

    //! [Code snippet declare]

    class DocumentEventsTutorial : BaseTutorial
    {
        public DocumentEventsTutorial(Esprit.Application app): base(app)
        {
            EspritApplication.AfterDocumentClose += AfterDocumentClose;
        }

        //! [Code snippet use]

        private DocumentEventsHandler _testObject;

        public void DocumentEventExample()
        {
            if (_testObject == null)
            {
                _testObject = new DocumentEventsHandler(EspritApplication);
            }
        }

        //! [Code snippet use]

        public override void Execute()
        {
            DocumentEventExample();
        }

        private void AfterDocumentClose()
        {
            _testObject = null;
        }

        public override string Name => "Document Events";
        public override string HtmlPath => "html/document_events_tutorial.html";

    }
}
