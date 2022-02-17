using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class CustomPropertiesTutorial : BaseTutorial
    {
        public CustomPropertiesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void AddOrUpdateDiameterPropertyOnCircles()
        {
            foreach(Esprit.Circle circle in Document.Circles)
            {
                var property = GraphicObjectHelper.GetOrAddCustomProperty(circle as Esprit.IGraphicObject, "Diameter", EspritConstants.espPropertyType.espPropertyTypeDouble);
                property.Caption = "Diameter";
                property.ReadOnly = true;
                property.Value = 2 * circle.Radius;
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            AddOrUpdateDiameterPropertyOnCircles();
        }

        public override string Name => "Introduction to Custom Properties";
        public override string HtmlPath => "html/custom_properties_tutorial.html";

    }
}
