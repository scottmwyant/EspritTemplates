using System.Text;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class GroupTutorial : BaseTutorial
    {
        public GroupTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        private Esprit.ISolid GetSolidGraphicObject(EspritSolids.ISolidBody body)
        {
            foreach(Esprit.ISolid solid in Document.Solids)
            {
                var currentBody = solid.SolidBody as EspritSolids.ISolidBody;
                if (body == currentBody)
                {
                    return solid;
                }

            }

            return null;
        }

        public void ScanGroup()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "GroupTutorial", $"Group contains {Document.Group.Count} items:");

            for (var i = 1; i <= Document.Group.Count; i++)
            {
                var graphicObject = Document.Group[i] as Esprit.GraphicObject;
                var solidFace = Document.Group[i] as EspritSolids.ISolidFace;
                var solidEdge = Document.Group[i] as EspritSolids.ISolidEdge;

                if (graphicObject != null && graphicObject.GraphicObjectType != EspritConstants.espGraphicObjectType.espUnknown)
                {
                    EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "GroupTutorial", $"Item {i} is {graphicObject.TypeName} {graphicObject.Key}");
                }
                else if (solidFace != null)
                {
                    var solid = GetSolidGraphicObject(solidFace.SolidBody);
                    var solidName = (solid != null) ? solid.Key : "...";
                    EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "GroupTutorial", $"Item {i} is a SolidFace of {solidName}");
                }
                else if (solidEdge != null)
                {
                    var solid = GetSolidGraphicObject(solidEdge.SolidBody);
                    var solidName = (solid != null) ? solid.Key : "...";
                    EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "GroupTutorial", $"Item {i} is a SolidEdge of {solidName}");
                }
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            ScanGroup();
        }

        public override string Name => "The Group";
        public override string HtmlPath => "html/group_tutorial.html";

    }
}
