using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class ScanToolsTutorial : BaseTutorial
    {
        public ScanToolsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void ScanTools()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            var tools = Document.Tools as EspritTools.Tools;

            foreach (EspritTechnology.ITechnology technology in tools)
            {
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanToolsTutorial", $"ToolId = {technology.FindParameter("ToolID").Value}");
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanToolsTutorial", $"ToolNumber = {technology.FindParameter("ToolNumber").Value}");
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanToolsTutorial", $"Comment = {technology.FindParameter("Comment").Value}");
                var toolStyle = (EspritConstants.espToolType)technology.FindParameter("ToolStyle").Value;
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanToolsTutorial", $"ToolStyle = {toolStyle}");

                switch (toolStyle)
                {
                    case EspritConstants.espToolType.espMillToolDrill:
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanToolsTutorial", $"ShankDiameter = {technology.FindParameter("ShankDiameter").Value}");
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanToolsTutorial", $"TipAngle = {technology.FindParameter("TipAngle").Value}");
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanToolsTutorial", $"ToolDiameter = {technology.FindParameter("ToolDiameter").Value}");
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanToolsTutorial", $"ToolLength = {technology.FindParameter("ToolLength").Value}");
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanToolsTutorial", $"ToolTip = {technology.FindParameter("ToolTip").Value}");
                        break;
                    case EspritConstants.espToolType.espMillToolEndMill:
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanToolsTutorial", $"ShankDiameter = {technology.FindParameter("ShankDiameter").Value}");
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanToolsTutorial", $"ToolDiameter = {technology.FindParameter("ToolDiameter").Value}");
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanToolsTutorial", $"ToolLength = {technology.FindParameter("ToolLength").Value}");
                        break;
                }
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            ScanTools();
        }

        public override string Name => "Scan Tools";
        public override string HtmlPath => "html/scan_tools_tutorial.html";

    }
}
