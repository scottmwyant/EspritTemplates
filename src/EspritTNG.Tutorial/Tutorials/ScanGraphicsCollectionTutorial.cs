using System.Windows;
using EspritConstants;

namespace TutorialCSharp.Tutorials
{
    class ScanGraphicsCollectionTutorial : BaseTutorial
    {
        public ScanGraphicsCollectionTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void ScanGraphicsCollection()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanGraphicsCollectionTutorial", $"Database contains up to {Document.GraphicsCollection.Count} GraphicObject items");

            var validObjectsCount = 0;

            foreach (Esprit.GraphicObject graphicObject in Document.GraphicsCollection)
            {
                if (graphicObject.GraphicObjectType != espGraphicObjectType.espUnknown)
                {
                    validObjectsCount++;
                    EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanGraphicsCollectionTutorial", $"Element {validObjectsCount} is {graphicObject.TypeName}  {graphicObject.Key}");
                }
            }

            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanGraphicsCollectionTutorial", $"Total Number of known GraphicObject items is {validObjectsCount}");
        }

        //! [Code snippet]

        public override void Execute()
        {
            ScanGraphicsCollection();
        }

        public override string Name => "Scan the Graphics Collection";
        public override string HtmlPath => "html/scan_graphics_collection_tutorial.html";

    }
}
