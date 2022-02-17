using System;
using System.Windows;
using Esprit;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class FeatureRecognitionPartProfilesTutorial : BaseTutorial
    {
        public FeatureRecognitionPartProfilesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void FindMaximumDiameter()
        {
            if (Document.Solids.Count == 0)
            {
                MessageBox.Show("There are no solids in this file.", "FeatureRecognitionPartProfilesTutorial", MessageBoxButton.OK);
                return;
            }

            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            var plane = PlaneHelper.GetPlane(Document, "Temp XYZ");
            PlaneHelper.SetPlaneVectors(ref plane, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1);

            var set = SelectionSetHelper.GetSelectionSet(Document, "Temp");

            EspritApplication.Configuration.ConfigurationFeatureRecognition.Tolerance = Document.SystemUnit == EspritConstants.espUnitType.espInch ? 0.001 : 0.025;

            foreach (Esprit.Solid solid in Document.Solids)
            {
                set.RemoveAll();
                set.Add(solid);

                Array shadows = null;
                try
                {
                    shadows = Document.FeatureRecognition.CreatePartProfileShadow(set, plane, EspritConstants.espGraphicObjectReturnType.espFeatureChains);
                }
                catch(Exception)
                {
                    MessageBox.Show("Fail on creating part profile shadow.", "FeatureRecognitionPartProfilesTutorial", MessageBoxButton.OK);
                    return;
                }
                
                var maxDiameter = 0.0;
                for (var i = shadows.GetLowerBound(0); i <= shadows.GetUpperBound(0); i++)
                {
                    var graphicObject = shadows.GetValue(i) as Esprit.IGraphicObject;
                    if (graphicObject.GraphicObjectType != EspritConstants.espGraphicObjectType.espFeatureChain)
                    {
                        continue;
                    }

                    var featureChain = graphicObject as Esprit.FeatureChain;
                    for (var j = 1; j <= featureChain.Count; j++)
                    {
                        Esprit.Point point1 = null;
                        Esprit.Point point2 = null;

                        switch (featureChain.Item[j].GraphicObjectType)
                        {
                            case EspritConstants.espGraphicObjectType.espArc:
                                var arc = featureChain.Item[j] as Esprit.Arc;
                                point1 = arc.Extremity(EspritConstants.espExtremityType.espExtremityStart);
                                point2 = arc.Extremity(EspritConstants.espExtremityType.espExtremityEnd);
                                break;

                            case EspritConstants.espGraphicObjectType.espSegment:
                                var segment = featureChain.Item[j] as Esprit.Segment;
                                point1 = segment.Extremity(EspritConstants.espExtremityType.espExtremityStart);
                                point2 = segment.Extremity(EspritConstants.espExtremityType.espExtremityEnd);
                                break;
                        }

                        var distance = Math.Sqrt(point1.Y * point1.Y + point1.Z * point1.Z);

                        if (distance > maxDiameter / 2)
                        {
                            maxDiameter = distance * 2;
                        }

                        distance = Math.Sqrt(point2.Y * point2.Y + point2.Z * point2.Z);

                        if (distance > maxDiameter / 2)
                        {
                            maxDiameter = distance * 2;
                        }
                    }

                    Document.FeatureChains.Remove(featureChain.Key);
                }

                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FeatureRecognitionPartProfilesTutorial", $"The Max YZ Diameter of Solid {solid.Key} is {maxDiameter}");
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            FindMaximumDiameter();
        }

        public override string Name => "Feature Recognition: Part Profiles";
        public override string HtmlPath => "html/feature_recognition_part_profiles_tutorial.html";

    }
}
