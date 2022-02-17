namespace TutorialCSharp.Tutorials
{
    class UndoTutorial : BaseTutorial
    {
        //! [Code snippet smashall]

        private void SmashAllUnsuppressedOperationsWithUndoAll()
        {
            Document.OpenUndoTransaction();
            foreach (Esprit.PartOperation operation in Document.PartOperations)
            {
                if (!operation.Suppress)
                {
                    SmashingToolpathToGeometryTutorial.SmashOperation(operation, EspritApplication);
                }
            }

            Document.Refresh();
            Document.CloseUndoTransaction(true);
        }

        //! [Code snippet smashall]

        //! [Code snippet smasheach]

        private void SmashAllUnsuppressedOperationsWithUndoEach()
        {
            foreach (Esprit.PartOperation operation in Document.PartOperations)
            {
                if (!operation.Suppress)
                {
                    Document.OpenUndoTransaction();
                    SmashingToolpathToGeometryTutorial.SmashOperation(operation, EspritApplication);
                    Document.CloseUndoTransaction(true);
                }
            }

            Document.Refresh();
        }

        //! [Code snippet smasheach]

        //! [Code snippet undo two]

        private void UndoTwo()
        {
            EspritApplication.RunCommand(EspritConstants.espCommands.espCmdUndo);
            EspritApplication.RunCommand(EspritConstants.espCommands.espCmdUndo);
        }

        //! [Code snippet undo two]

        public UndoTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            SmashAllUnsuppressedOperationsWithUndoEach();
            Document.Refresh();
        }

        public override string Name => "Undo";
        public override string HtmlPath => "html/undo_tutorial.html";

    }
}
