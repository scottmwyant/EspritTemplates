using System;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class FeatureRecognitionAccessingResultsTutorial : BaseTutorial
    {
        public FeatureRecognitionAccessingResultsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void ChainAllCWAndCloseGaps()
        {
            var set = SelectionSetHelper.GetOrAddSelectionSet(Document.SelectionSets, "Temp");
            set.RemoveAll();

            var set2 = SelectionSetHelper.GetOrAddSelectionSet(Document.SelectionSets, "Temp2");
            set2.RemoveAll();

            foreach (Esprit.IGraphicObject graphicObject in Document.GraphicsCollection)
            {
                if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espArc || graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espSegment)
                {
                    set.Add(graphicObject);
                }
            }

            if (set.Count == 0)
            {
                return;
            }

            var objects = Document.FeatureRecognition.CreateAutoChains(set);

            for (var i = objects.GetLowerBound(0); i <= objects.GetUpperBound(0); i++)
            {
                var graphicObject = objects.GetValue(i) as Esprit.GraphicObject;

                if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espFeatureChain)
                {
                    var featureChain = graphicObject as Esprit.FeatureChain;
                    if (!featureChain.IsClosed)
                    {
                        var startPoint = featureChain.Extremity(EspritConstants.espExtremityType.espExtremityStart);
                        var endPoint = featureChain.Extremity(EspritConstants.espExtremityType.espExtremityEnd);
                        if (Geometry.Distance(startPoint, endPoint) < EspritApplication.Configuration.ConfigurationFeatureRecognition.Tolerance)
                        {
                            featureChain.Add(startPoint);
                        }
                    }
                    if (featureChain.IsClosed) // check again
                    {
                        set2.AddCopiesToSelectionSet = true;
                        set2.RemoveAll();

                        set2.Add(featureChain);

                        try
                        {
                            set2.Offset(0.001, EspritConstants.espOffsetSide.espOffsetRight, false, EspritConstants.espLookAheadMode.espLookAheadOn, true);

                            var offsetFeatureChain = set2[2] as Esprit.FeatureChain;
                            if (offsetFeatureChain.Area > featureChain.Area)
                            {
                                featureChain.Reverse();
                            }
                            Document.FeatureChains.Remove(offsetFeatureChain.Key);
                        }
                        catch (Exception)
                        { }
                    }
                }
            }


            Document.SelectionSets.Remove(set.Name);
            Document.SelectionSets.Remove(set2.Name);
        }

        //! [Code snippet]

        public override void Execute()
        {
            ChainAllCWAndCloseGaps();
        }

        public override string Name => "Accessing the Features Resulting from FeatureRecognition";
        public override string HtmlPath => "html/feature_recognition_accessing_results_tutorial.html";

    }
}
