namespace TutorialCSharp.Tutorials
{
    class MoveP0AnyElementCenterTutorial : BaseTutorial
    {
        public MoveP0AnyElementCenterTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void MoveP0ToAnyElementCenter()
        {
            Esprit.IGraphicObject element = null;

            try
            {
                element = Document.GetAnyElement("Pick a Circle or Arc");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if(element == null)
            {
                return;
            }

            switch (element.TypeName)
            {
                case "Arc":
                    var arc = element as Esprit.Arc;
                    Document.MoveP0(arc.CenterPoint);
                    break;
                case "Circle":
                    var circle = element as Esprit.Circle;
                    Document.MoveP0(circle.CenterPoint);
                    break;
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            MoveP0ToAnyElementCenter();
        }

        public override string Name => "Move P0 to Any Element Center";
        public override string HtmlPath => "html/movep0_anyelement_center_tutorial.html";

    }
}
