using System;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class ExtractFeatureInfoTutorial : BaseTutorial
    {
        public ExtractFeatureInfoTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void FindFeaturePtopInfo()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;
            var shortestDistance = 0.0;
            var longestDistance = 0.0;

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

            for (var i = 1; i <= featurePtop.Count - 1; i++)
            {
                var distance = Geometry.Distance(featurePtop.Item[i] as Esprit.Point, featurePtop.Item[i + 1] as Esprit.Point);
                if (i == 1)
                {
                    shortestDistance = distance;
                    longestDistance = distance;
                }
                else
                {
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                    }
                    if (distance > longestDistance)
                    {
                        longestDistance = distance;
                    }
                }

                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ExtractFeatureInfoTutorial", $"The distance between items {i} and {i + 1} is {Math.Round(distance, 5)}");
            }

            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ExtractFeatureInfoTutorial", $"The shortest distance is {Math.Round(shortestDistance, 5)}");
            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ExtractFeatureInfoTutorial", $"The longest distance is {Math.Round(longestDistance, 5)}");
        }

        //! [Code snippet]

        public override void Execute()
        {
            FindFeaturePtopInfo();
        }

        public override string Name => "Extracting Feature Information";
        public override string HtmlPath => "html/extract_feature_info_tutorial.html";

    }
}
