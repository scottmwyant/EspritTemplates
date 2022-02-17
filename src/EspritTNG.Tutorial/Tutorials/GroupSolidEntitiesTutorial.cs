using System.Text;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class GroupSolidEntitiesTutorial : BaseTutorial
    {
        public GroupSolidEntitiesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void GroupSolidLoop()
        {
            var selectedLoop = Document.GetAnyElementObject("Select a SolidLoop", EspritSolids.SolidEntityType.geoSolidLoop) as EspritSolids.ISolidLoop;
            if (selectedLoop != null)
            {
                var solidFaces = Document.Propagate(selectedLoop);

                for (var i = solidFaces.GetLowerBound(0); i <= solidFaces.GetUpperBound(0); i++)
                {
                    var face = solidFaces.GetValue(i) as EspritSolids.ISolidFace;
                    Document.Group.Add(face);
                }
            }
        }

        public void GroupSolidFace()
        {
            var selectedFace = Document.GetAnyElementObject("Select a SolidFace", EspritSolids.SolidEntityType.geoSolidFace) as EspritSolids.ISolidFace;
            if (selectedFace != null)
            {
                Document.Group.Add(selectedFace);
            }
        }

        public void GroupSolidEdge()
        {
            var selectedEdge = Document.GetAnyElementObject("Select a SolidEdge", EspritSolids.SolidEntityType.geoSolidEdge) as EspritSolids.ISolidEdge;
            if (selectedEdge != null)
            {
                Document.Group.Add(selectedEdge);
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            GroupSolidLoop();
            GroupSolidFace();
            GroupSolidEdge();
        }

        public override string Name => "Group Solid Entities";
        public override string HtmlPath => "html/group_solid_entities_tutorial.html";

    }
}
