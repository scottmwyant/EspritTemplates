using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    //! [Code snippet declare]

    class RespondingToCustomPropertyChangesHandler
    {
        private readonly Esprit.Application _espritApplication;

        public RespondingToCustomPropertyChangesHandler(Esprit.Application app)
        {
            _espritApplication = app;
            app.Document.GraphicsCollection.OnCreate += GraphicsCollectionOnCreate;
            app.Document.GraphicsCollection.OnModify += GraphicsCollectionOnModify;

            foreach (Esprit.GraphicObject circle in app.Document.Circles)
            {
                var property = GraphicObjectHelper.GetOrAddCustomProperty(circle, "Diameter", EspritConstants.espPropertyType.espPropertyTypeDouble);
                property.Caption = "Diameter";
                property.ReadOnly = false;
                property.Value = 2 * (circle as Esprit.Circle).Radius;

                var previousProperty = GraphicObjectHelper.GetOrAddCustomProperty(circle, "PreviousDiameter", EspritConstants.espPropertyType.espPropertyTypeDouble);
                previousProperty.Caption = "PreviousDiameter";
                previousProperty.Visible = false;
                previousProperty.Value = 2 * (circle as Esprit.Circle).Radius;
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
                    property.ReadOnly = false;
                    property.Value = 2 * (graphicObject as Esprit.Circle).Radius;

                    var previousProperty = GraphicObjectHelper.GetOrAddCustomProperty(graphicObject, "PreviousDiameter", EspritConstants.espPropertyType.espPropertyTypeDouble);
                    previousProperty.Caption = "PreviousDiameter";
                    previousProperty.Visible = false;
                    previousProperty.Value = 2 * (graphicObject as Esprit.Circle).Radius;
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
                    var circle = graphicObject as Esprit.Circle;
                    var property = GraphicObjectHelper.GetOrAddCustomProperty(graphicObject, "Diameter", EspritConstants.espPropertyType.espPropertyTypeDouble);
                    var previousProperty = GraphicObjectHelper.GetOrAddCustomProperty(graphicObject, "PreviousDiameter", EspritConstants.espPropertyType.espPropertyTypeDouble);

                    if (property.Value != previousProperty.Value)
                    {
                        circle.Radius = property.Value / 2.0;
                        _espritApplication.Document.Refresh();
                    }
                    else
                    {
                        property.Value = 2 * circle.Radius;
                    }

                    previousProperty.Value = 2 * circle.Radius;
                }
            }
        }
    }

    //! [Code snippet declare]

    class RespondingToCustomPropertyChangesTutorial : BaseTutorial
    {
        public RespondingToCustomPropertyChangesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet use]

        private RespondingToCustomPropertyChangesHandler _testObject = null;

        public void GraphicsEventExample2()
        {
            if (_testObject == null)
            {
                _testObject = new RespondingToCustomPropertyChangesHandler(EspritApplication);
            }
        }

        //! [Code snippet use]

        public override void Execute()
        {
            GraphicsEventExample2();
        }

        public override string Name => "Responding to Custom Property Changes";
        public override string HtmlPath => "html/respond_to_custom_property_changes_tutorial.html";

    }
}
