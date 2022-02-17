using System;
using System.Collections.Generic;
using System.Windows;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class FeatureHolesRecognitionTutorial : BaseTutorial
    {
        public FeatureHolesRecognitionTutorial(Esprit.Application app): base(app)
        {
        }

        private void PrepareGeometry()
        {
            var pointCount = Document.Points.Count >= 3 ? 0 : 3 - Document.Points.Count;
            
            for (var i = 0; i < pointCount; i++)
            {
                Document.Points.Add(10 * (i + 1), 5, 3);
            }

            var circuleCount = Document.Circles.Count >= 2 ? 0 : 2 - Document.Circles.Count;

            for (var i = 0; i < circuleCount; i++)
            {
                Document.Circles.Add(Document.GetPoint(-10 * (i + 1), 5, 3), 5 * (i + 1));
            }
        }

        //! [Code snippet]

        private void RecognizeHoles()
        {
            EspritApplication.Configuration.ConfigurationHoleRecognition2.MinDiameterMetric = 1;
            EspritApplication.Configuration.ConfigurationHoleRecognition2.MaxDiameterMetric = 15;

            var points = new List<Esprit.Point>()
            {
                Document.Points[1],
                Document.Points[2],
                Document.Points[3],
            };

            var pointFeatureHoles = Document.FeatureRecognition.CreateHoleFeatures2(points.ToArray(), out var espPointFaults);

            if (pointFeatureHoles != null)
            {
                for (var i = pointFeatureHoles.GetLowerBound(0); i <= pointFeatureHoles.GetUpperBound(0); i++)
                {
                    var featureHole = pointFeatureHoles.GetValue(i) as Esprit.HolesFeature;
                    var hole = featureHole.Object as EspritFeatures.ComFeatureHoles;

                    MessageBox.Show($"{hole.Count} point hole(s) recognized", "FeatureHolesRecognitionTutorial");
                }
            }

            var circles = new List<Esprit.Circle>()
            {
                Document.Circles[1],
                Document.Circles[2],
            };

            var circleFeatureHoles = Document.FeatureRecognition.CreateHoleFeatures2(circles.ToArray(), out var espCircleFaults);

            if (circleFeatureHoles != null)
            {
                for (var i = circleFeatureHoles.GetLowerBound(0); i <= circleFeatureHoles.GetUpperBound(0); i++)
                {
                    var featureHole = circleFeatureHoles.GetValue(i) as Esprit.HolesFeature;
                    var hole = featureHole.Object as EspritFeatures.ComFeatureHoles;

                    MessageBox.Show($"{hole.Count} circle hole(s) recognized", "FeatureHolesRecognitionTutorial");
                }
            }
        }

        private void RecognizeFeatureHolesOnSolid()
        {
            if (Document.Solids.Count == 0)
            {
                return;
            }

            var solidArray = new List<Esprit.IGraphicObject>();

            solidArray.Add(Document.Solids[1]);

            try
            {
                Document.FeatureRecognition.CreateHoleFeatures2(solidArray.ToArray(), out var comFaults);

                for (var i = 1; i <= comFaults.Count; i++)
                {
                    var msgType = comFaults[i].Severity == EspritComBase.espFaultSeverity.espFaultWarning ? EspritConstants.espMessageType.espMessageTypeWarning : EspritConstants.espMessageType.espMessageTypeError;
                    EspritApplication.EventWindow.AddMessage(msgType, "FeatureHolesRecognitionTutorial", comFaults[i].Description);
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Error on Hole Features recognition", "FeatureHolesRecognitionTutorial");
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            PrepareGeometry();
            RecognizeHoles();
            RecognizeFeatureHolesOnSolid();
        }

        public override string Name => "Recognize Feature Holes";
        public override string HtmlPath => "html/recognize_feature_holes_tutorial.html";

    }
}
