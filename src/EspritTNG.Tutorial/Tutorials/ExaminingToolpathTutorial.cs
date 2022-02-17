namespace TutorialCSharp.Tutorials
{
    class ExaminingToolpathTutorial : BaseTutorial
    {
        //! [Code snippet]

        private void ScanToolPath()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            foreach (Esprit.PartOperation operation in Document.PartOperations)
            {
                for (var i = 1; i <= operation.ToolPath.Count; i++)
                {
                    var toolPathItem = operation.ToolPath.ItemCopy(i);
                    EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ExaminingToolpathTutorial", $"Operation {operation.Key} ({operation.Name}) Item {i} is type {toolPathItem.Type} (AuxCode = {toolPathItem.AuxCode})");
                }
            }
        }

        //! [Code snippet]

        public ExaminingToolpathTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            ScanToolPath();
        }

        public override string Name => "Examining the Toolpath";
        public override string HtmlPath => "html/examining_toolpath_tutorial.html";

    }
}
