using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class FeatureRecognitionIntroTutorial : BaseTutorial
    {
        public FeatureRecognitionIntroTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void ChainAll()
        {
            var set = SelectionSetHelper.GetOrAddSelectionSet(Document.SelectionSets, "Temp");
            set.RemoveAll();

            foreach (Esprit.IGraphicObject graphicObject in Document.GraphicsCollection)
            {
                if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espArc || graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espSegment)
                {
                    set.Add(graphicObject);
                }
            }

            if (set.Count > 0)
            {
                try
                {
                    Document.FeatureRecognition.CreateAutoChains(set);
                }
                catch(System.Runtime.InteropServices.COMException)
                { }
            }

            Document.SelectionSets.Remove(set.Name);
        }

        //! [Code snippet]

        public override void Execute()
        {
            ChainAll();
        }

        public override string Name => "Introduction to Feature Recognition";
        public override string HtmlPath => "html/feature_recognition_intro_tutorial.html";

    }
}
