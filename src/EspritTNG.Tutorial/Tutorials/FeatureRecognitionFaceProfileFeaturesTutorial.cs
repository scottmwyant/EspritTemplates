using System;
using System.Windows;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class FeatureRecognitionFaceProfileFeaturesTutorial : BaseTutorial
    {
        public FeatureRecognitionFaceProfileFeaturesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void FindAllFaceProfilesOnXYZ()
        {
            var set = SelectionSetHelper.GetSelectionSet(Document, "Temp");
            set.RemoveAll();

            foreach (Esprit.Solid solid in Document.Solids)
            {
                set.Add(solid);
            }

            if (set.Count == 0)
            {
                MessageBox.Show("There are no solids in this file.", "FeatureRecognitionFaceProfileFeaturesTutorial", MessageBoxButton.OK);
                return;
            }

            var plane = Document.Planes["XYZ"];
            if (plane == null)
            {
                plane = PlaneHelper.GetPlane(Document, "Temp XYZ");
                PlaneHelper.SetPlaneVectors(ref plane, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1);
            }

            EspritApplication.Configuration.ConfigurationFeatureRecognition.Tolerance = (Document.SystemUnit == EspritConstants.espUnitType.espInch)
                ? 0.0001
                : 0.0025;

            try
            {
                Document.FeatureRecognition.CreateFaceProfileFeatures(set, new Esprit.Plane[1] { plane });
            }
            catch (Exception)
            {
                MessageBox.Show("Fail on creationg face profile features.", "FeatureRecognitionFaceProfileFeaturesTutorial", MessageBoxButton.OK);
                return;
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            FindAllFaceProfilesOnXYZ();
        }

        public override string Name => "Feature Recognition: Face Profile Features";
        public override string HtmlPath => "html/feature_recognition_face_profile_features_tutorial.html";

    }
}
