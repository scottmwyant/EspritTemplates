using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    //! [Code snippet declare]

    class GraphicsEventsHandler
    {
        public GraphicsEventsHandler(Esprit.Application app)
        {
            app.Document.GraphicsCollection.OnCreate += GraphicsCollectionOnCreate;
            app.Document.GraphicsCollection.OnModify += GraphicsCollectionOnModify;

            foreach (Esprit.GraphicObject circle in app.Document.Circles)
            {
                var property = GraphicObjectHelper.GetOrAddCustomProperty(circle, "Diameter", EspritConstants.espPropertyType.espPropertyTypeDouble);
                property.Caption = "Diameter";
                property.ReadOnly = true;
                property.Value = 2 * (circle as Esprit.Circle).Radius;
            }
        }

        private void GraphicsCollectionOnCreate(object graphicsArray)
        {
            var data = graphicsArray as System.Collections.IEnumerable;
            foreach (Esprit.GraphicObject graphicObject in data)
            {
                if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espCircle)
                {
                    var property = GraphicObjectHelper.GetOrAddCustomProperty(graphicObject, "Diameter", EspritConstants.espPropertyType.espPropertyTypeDouble);
                    property.Caption = "Diameter";
                    property.ReadOnly = true;
                    property.Value = 2 * (graphicObject as Esprit.Circle).Radius;
                }
            }
        }

        private void GraphicsCollectionOnModify(object graphicsArray)
        {
            var data = graphicsArray as System.Collections.IEnumerable;
            foreach(Esprit.GraphicObject graphicObject in data)
            {
                if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espCircle)
                {
                    var property = GraphicObjectHelper.GetOrAddCustomProperty(graphicObject, "Diameter", EspritConstants.espPropertyType.espPropertyTypeDouble);
                    property.Value = 2 * (graphicObject as Esprit.Circle).Radius;
                }
            }
        }
    }

    //! [Code snippet declare]

    class GraphicsEventsTutorial : BaseTutorial
    {
        public GraphicsEventsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet use]

        private GraphicsEventsHandler _testObject = null;

        public void GraphicsEventExample()
        {
            if (_testObject == null)
            {
                _testObject = new GraphicsEventsHandler(EspritApplication);
            }
        }

        //! [Code snippet use]

        public override void Execute()
        {
            GraphicsEventExample();
        }

        public override string Name => "Graphics Events";
        public override string HtmlPath => "html/graphics_events_tutorial.html";

    }
}
