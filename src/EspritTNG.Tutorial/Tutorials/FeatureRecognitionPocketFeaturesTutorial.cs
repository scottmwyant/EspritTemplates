using System;
using System.Windows;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class FeatureRecognitionPocketFeaturesTutorial : BaseTutorial
    {
        public FeatureRecognitionPocketFeaturesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void FindAllXYZPockets()
        {
            Esprit.Solid solid = null;
            try
            {
                solid = Document.GetAnyElement("Select Reference Solid", EspritConstants.espGraphicObjectType.espSolidModel) as Esprit.Solid;
            }
            catch(System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if(solid == null)
            {
                return;
            }

            var set = SelectionSetHelper.GetSelectionSet(Document, "Temp");
            set.RemoveAll();

            set.Add(solid);

            var plane = Document.Planes["XYZ"];
            if (plane == null)
            {
                plane = PlaneHelper.GetPlane(Document, "Temp XYZ");
                PlaneHelper.SetPlaneVectors(ref plane, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1);
            }

            EspritApplication.Configuration.ConfigurationFeatureRecognition.Tolerance = (Document.SystemUnit == EspritConstants.espUnitType.espInch)
                ? 0.0001
                : 0.0025;

            Array boundaries = null;
            try
            {
                boundaries = Document.FeatureRecognition.CreatePartProfileShadow(set, plane, EspritConstants.espGraphicObjectReturnType.espFeatureChains);
            }
            catch (Exception)
            {
                MessageBox.Show("Fail on creating part profile shadow.", "FeatureRecognitionPocketFeaturesTutorial", MessageBoxButton.OK);
                return;
            }

            Esprit.FeatureChain boundary = null;
            for (var i = boundaries.GetLowerBound(0); i <= boundaries.GetUpperBound(0); i++)
            {
                var graphicObject = boundaries.GetValue(i) as Esprit.GraphicObject;
                if (graphicObject.GraphicObjectType != EspritConstants.espGraphicObjectType.espFeatureChain)
                {
                    continue;
                }
                var featureChain = graphicObject as Esprit.FeatureChain;
                if (boundary == null)
                {
                    boundary = featureChain;
                }
                else if (featureChain.Area > boundary.Area)
                {
                    boundary = featureChain;
                }
            }

            if (boundary == null)
            {
                MessageBox.Show("Unable to determine boundary. Aborting.", "FeatureRecognitionPocketFeaturesTutorial", MessageBoxButton.OK);
                return;
            }

            set.RemoveAll();
            set.Add(boundary);

            try
            {
                Document.FeatureRecognition.CreatePocketFeatures(set, EspritConstants.espPocketType.espPocketTypeMultiple, solid, new Esprit.Plane[1] { plane });
            }
            catch (Exception)
            {
                MessageBox.Show("Fail on creating pocket features.", "FeatureRecognitionPocketFeaturesTutorial", MessageBoxButton.OK);
                return;
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            FindAllXYZPockets();
        }

        public override string Name => "Feature Recognition: Pocket Features";
        public override string HtmlPath => "html/feature_recognition_pocket_features_tutorial.html";

    }
}
