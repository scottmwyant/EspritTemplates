namespace TutorialCSharp.Tutorials
{
    class MoveP0PointArcCircleCenterTutorial : BaseTutorial
    {
        public MoveP0PointArcCircleCenterTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void MoveP0ToArcOrCircleCenterOrPoint()
        {
            var typeMask = new EspritConstants.espGraphicObjectType[3]
            {
                EspritConstants.espGraphicObjectType.espArc,
                EspritConstants.espGraphicObjectType.espCircle,
                EspritConstants.espGraphicObjectType.espPoint
            };

            Esprit.IGraphicObject element = null;

            try
            {
                element = Document.GetAnyElement("Pick a Circle or Arc or Point", typeMask);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }
            
            if (element == null)
            {
                return;
            }

            switch (element.GraphicObjectType)
            {
                case EspritConstants.espGraphicObjectType.espArc:
                    var arc = element as Esprit.Arc;
                    Document.MoveP0(arc.CenterPoint);
                    break;
                case EspritConstants.espGraphicObjectType.espCircle:
                    var circle = element as Esprit.Circle;
                    Document.MoveP0(circle.CenterPoint);
                    break;
                case EspritConstants.espGraphicObjectType.espPoint:
                    var point = element as Esprit.Point;
                    Document.MoveP0(point);
                    break;
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            MoveP0ToArcOrCircleCenterOrPoint();
        }

        public override string Name => "Move P0 to Arc or Circle Center or Point";
        public override string HtmlPath => "html/movep0_point_arc_circle_center_tutorial.html";

    }
}
