using System.Text;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class ScanOperationsTutorial : BaseTutorial
    {
        public ScanOperationsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void ScanOperations()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            foreach (Esprit.PartOperation partOperation in Document.PartOperations)
            {
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanOperationsTutorial", $"OP {partOperation.Key} is a {partOperation.TypeName}");
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanOperationsTutorial", $"OP {partOperation.Key} is on Layer {partOperation.Layer.Name}");

                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanOperationsTutorial", $"OP {partOperation.Key} Length of Feed is {partOperation.LengthOfFeed.ToString("F3")}");
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanOperationsTutorial", $"OP {partOperation.Key} Length of Rapid is {partOperation.LengthOfRapid.ToString("F3")}");
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            ScanOperations();
        }

        public override string Name => "Scan Operations";
        public override string HtmlPath => "html/scan_operations_tutorial.html";

    }
}
