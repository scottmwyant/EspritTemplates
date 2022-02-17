using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class OptimizeFeaturePtopObjectsTutorial : BaseTutorial
    {
        public OptimizeFeaturePtopObjectsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void FullyOptimizeSelectedFeaturePtop()
        {
            Esprit.FeaturePtop featurePtop = null;

            try
            {
                featurePtop = Document.GetAnyElement("Pick Feature Ptop", EspritConstants.espGraphicObjectType.espFeaturePtop) as Esprit.FeaturePtop;
            }
            catch(System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if (featurePtop == null)
            {
                return;
            }

            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            FeatureHelper.FullyOptimizeFeaturePtop(ref featurePtop, Document, EspritApplication.EventWindow);
        }

        //! [Code snippet]

        public override void Execute()
        {
            FullyOptimizeSelectedFeaturePtop();
        }

        public override string Name => "Optimizing FeaturePtop Objects";
        public override string HtmlPath => "html/optimize_featureptop_objects_tutorial.html";

    }
}
