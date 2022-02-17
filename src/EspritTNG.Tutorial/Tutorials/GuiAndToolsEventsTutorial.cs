using System.Windows;

namespace TutorialCSharp.Tutorials
{
    //! [Code snippet declare]

    class GuiAndToolsEventsHandler
    {
        public GuiAndToolsEventsHandler(Esprit.Application app)
        {
            app.Document.GUI.BeforeWorkCoordinateEdit += OnBeforeWorkCoordinateEdit;
            var tools = app.Document.Tools as EspritTools.Tools;
            tools.OnCreate += OnToolCreate;
            tools.OnDelete += OnToolDelete;
            tools.OnModify += OnToolModify;
        }

        private string GetToolId(EspritTechnology.ITechnology technology)
        {
            var parameter = technology.FindParameter("ToolID");
            return (parameter != null) ? parameter.Value.toString() : string.Empty;
        }

        private void OnToolModify(string oldName, int cLCode, object toolObject)
        {
            var tool = toolObject as EspritTechnology.ITechnology;
            MessageBox.Show($"Modify ToolID = {GetToolId(tool)}");
        }

        private void OnToolDelete(object toolObject)
        {
            var tool = toolObject as EspritTechnology.ITechnology;
            MessageBox.Show($"Delete ToolID = {GetToolId(tool)}");
        }

        private void OnToolCreate(object toolObject)
        {
            var tool = toolObject as EspritTechnology.ITechnology;
            MessageBox.Show($"Create ToolID = {GetToolId(tool)}");
        }

        private void OnBeforeWorkCoordinateEdit(object workCoordinateObject, ref bool bOverride)
        {
            var workCoordinate = workCoordinateObject as Esprit.WorkCoordinate;
            MessageBox.Show($"WorkCoordinate.Name = {workCoordinate.Name}");
        }
    }

    //! [Code snippet declare]

    class GuiAndToolsEventsTutorial : BaseTutorial
    {
        private GuiAndToolsEventsHandler _testObject = null;

        public GuiAndToolsEventsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet use]

        public override void Execute()
        {
            if (_testObject == null)
            {
                _testObject = new GuiAndToolsEventsHandler(EspritApplication);
            }
        }

        //! [Code snippet use]

        public override string Name => "Introduction to GUI and Tools Events";
        public override string HtmlPath => "html/gui_and_tools_events_tutorial.html";

    }
}
