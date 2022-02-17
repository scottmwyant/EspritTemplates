using System;
using System.Collections.Generic;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class AutoRecognizeFeatureStartPointsTutorial : BaseTutorial
    {
        public AutoRecognizeFeatureStartPointsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet restart]

        private static Esprit.FeatureChain RestartFeatureChain(Esprit.Document document, Esprit.FeatureChain chainToRestart, Esprit.Point splitPoint)
        {
            var originalCount = chainToRestart.Count;
            try
            {
                chainToRestart.Split(splitPoint);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
            }

            var featureChain = chainToRestart;

            if (chainToRestart.Count > originalCount)
            {
                Esprit.Point endPoint = null;
                for (var i = 1; i <= chainToRestart.Count; i++)
                {
                    switch (chainToRestart.Item[i].GraphicObjectType)
                    {
                        case EspritConstants.espGraphicObjectType.espArc:
                            var arc = chainToRestart.Item[i] as Esprit.Arc;
                            endPoint = arc.Extremity(EspritConstants.espExtremityType.espExtremityEnd);
                            break;
                        case EspritConstants.espGraphicObjectType.espSegment:
                            var segment = chainToRestart.Item[i] as Esprit.Segment;
                            endPoint = segment.Extremity(EspritConstants.espExtremityType.espExtremityEnd);
                            break;
                    }

                    if (endPoint != null && SamePoint(endPoint, splitPoint))
                    {
                        featureChain = document.FeatureChains.Add(splitPoint);
                        for (var j = i+1; j<= chainToRestart.Count; j++)
                        {
                            try
                            {
                                featureChain.Add(chainToRestart.Item[j]);
                            }
                            catch (Exception)
                            { }
                        }
                        for (var j = 1; j <= i; j++)
                        {
                            try
                            {
                                featureChain.Add(chainToRestart.Item[j]);
                            }
                            catch (Exception)
                            { }
                        }

                        break;
                    }
                }
            }

            return featureChain;
        }

        private static bool SamePoint(Esprit.Point point1, Esprit.Point point2)
        {
            return ((point1.X == point2.X) && (point1.Y == point2.Y) && (point1.Z == point2.Z));
        }

        //! [Code snippet restart]

        //! [Code snippet chainall]

        public void ChainAllCWCloseGapsAndChangeStart()
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
                        catch(Exception)
                        { }
                    }

                    foreach(Esprit.Point point in Document.Points)
                    {
                        var newFC = RestartFeatureChain(Document, featureChain, point);
                    }
                }
            }

            Document.SelectionSets.Remove(set.Name);
            Document.SelectionSets.Remove(set2.Name);
        }

        //! [Code snippet chainall]

        public override void Execute()
        {
            ChainAllCWCloseGapsAndChangeStart();
        }

        public override string Name => "Automatically Recognizing Feature Start Points";
        public override string HtmlPath => "html/auto_recognize_feature_start_points_tutorial.html";

    }
}
