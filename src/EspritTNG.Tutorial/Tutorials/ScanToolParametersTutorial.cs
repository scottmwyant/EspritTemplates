using System.Text;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class ScanToolParametersTutorial : BaseTutorial
    {
        public ScanToolParametersTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void ScanAllToolParameters()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            var tools = Document.Tools as EspritTools.Tools;

            foreach (EspritTechnology.ITechnology technology in tools)
            {
                foreach (EspritTechnology.Parameter parameter in technology)
                {
                    EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanToolParametersTutorial", $"{parameter.Name} = {parameter.Value}");
                }
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            ScanAllToolParameters();
        }

        public override string Name => "Scan Tool Parameters";
        public override string HtmlPath => "html/scan_tool_param_tutorial.html";

    }
}
