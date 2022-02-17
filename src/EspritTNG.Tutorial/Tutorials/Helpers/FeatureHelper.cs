using System;
using Esprit;
using EspritConstants;

namespace TutorialCSharp.Tutorials.Helpers
{
    public static class FeatureHelper
    {

        //! [Code snippet bolthole helper]

        public static SelectionSet BoltHolePattern(Document document, Point center, double patternRadius, double holeRadius, int numberOfHoles)
        {
            var boltHolePattern = SelectionSetHelper.AddUniqueSelectionSet(document.SelectionSets, "BoltHole");
            FeaturePtop featurePtop = null;

            for (var i = 0; i < numberOfHoles; i++)
            {
                var angle = i * 2 * Math.PI / numberOfHoles;
                var x = patternRadius * Math.Cos(angle) + center.X;
                var y = patternRadius * Math.Sin(angle) + center.Y;

                var hole = document.Circles.Add(document.GetPoint(x, y, center.Z), holeRadius);
                boltHolePattern.Add(hole);
                if (featurePtop == null)
                {
                    featurePtop = document.FeaturePtops.Add(hole.CenterPoint);
                }
                else
                {
                    featurePtop.Add(hole.CenterPoint);
                }
            }

            if (featurePtop != null)
            {
                boltHolePattern.Add(featurePtop);
            }

            return boltHolePattern;
        }

        //! [Code snippet bolthole helper]

        //! [Code snippet chain helper]

        public static SelectionSet ChainCircles(Document document, SelectionSet selectionSet, double startAngle)
        {
            var chainCircles = SelectionSetHelper.AddUniqueSelectionSet(document.SelectionSets, "ChainCircles");
            for (var i = 1; i <= selectionSet.Count; i++)
            {
                var graphicObject = selectionSet[i] as IGraphicObject;

                if (graphicObject.GraphicObjectType != espGraphicObjectType.espCircle)
                {
                    continue;
                }

                var circle = graphicObject as Circle;
                var featureChain = document.FeatureChains.Add(document.GetPoint(circle.CenterPoint.X + circle.Radius * Math.Cos(startAngle), circle.CenterPoint.Y + circle.Radius * Math.Sin(startAngle), circle.CenterPoint.Z));
                featureChain.Add(document.GetArc(circle.CenterPoint, circle.Radius, startAngle, startAngle + 2 * Math.PI));
                chainCircles.Add(featureChain);
            }

            return chainCircles;
        }

        //! [Code snippet chain helper]

        //! [Code snippet mold helper]

        public static SelectionSet MoldHolePattern(Document document, Point center, double boundaryRadius, double holeRadius, double spacing)
        {
            var moldHolePattern = SelectionSetHelper.AddUniqueSelectionSet(document.SelectionSets, "MoldHole");
            if (boundaryRadius < holeRadius)
            {
                return moldHolePattern;
            }

            var hole = document.Circles.Add(center, holeRadius);
            moldHolePattern.Add(hole);

            var centerDistance = spacing + 2 * holeRadius;
            uint ring = 0;
            var holesOnRing = 0;

            do
            {
                ring++;
                holesOnRing = 0;
                var x = center.X + ring * centerDistance;
                var y = center.Y;

                for (var sideAngleInDegrees = 120; sideAngleInDegrees <= 420; sideAngleInDegrees += 60)
                {
                    for (var holeOnSide = 1; holeOnSide <= ring; holeOnSide++)
                    {
                        x = x + centerDistance * Math.Cos(sideAngleInDegrees * Math.PI / 180);
                        y = y + centerDistance * Math.Sin(sideAngleInDegrees * Math.PI / 180);

                        var outerTangent = Geometry.Distance(x, y, center.X, center.Y) + holeRadius + spacing;
                        if (outerTangent < boundaryRadius)
                        {
                            hole = document.Circles.Add(document.GetPoint(x, y, center.Z), holeRadius);
                            moldHolePattern.Add(hole);
                            holesOnRing++;
                        }
                    }

                }
            } while (holesOnRing != 0);

            return moldHolePattern;
        }

        //! [Code snippet mold helper]

        //! [Code snippet bolthole improved helper]

        public static SelectionSet BoltHoleImproved(Document document, Configuration config, Point center, double patternRadius, double holeRadius, int numberOfHoles)
        {
            var boltHolePattern = SelectionSetHelper.AddUniqueSelectionSet(document.SelectionSets, "BoltHole");
            FeaturePtop featurePtop = null;

            for (var i = 0; i < numberOfHoles; i++)
            {
                var angle = i * 2 * Math.PI / numberOfHoles;
                var x = patternRadius * Math.Cos(angle) + center.X;
                var y = patternRadius * Math.Sin(angle) + center.Y;

                var hole = document.Circles.Add(document.GetPoint(x, y, center.Z), holeRadius);
                hole.Color = config.ConfigurationCircles.Color;
                hole.LineType = config.ConfigurationCircles.LineType;
                hole.LineWeight = config.ConfigurationCircles.LineWeight;

                boltHolePattern.Add(hole);
                if (featurePtop == null)
                {
                    featurePtop = document.FeaturePtops.Add(hole.CenterPoint);
                }
                else
                {
                    featurePtop.Add(hole.CenterPoint);
                }
            }

            if (featurePtop != null)
            {
                featurePtop.Color = config.ConfigurationFeaturePtops.Color;
                featurePtop.LineType = config.ConfigurationFeaturePtops.LineType;
                featurePtop.LineWeight = config.ConfigurationFeaturePtops.LineWeight;
                boltHolePattern.Add(featurePtop);
            }

            return boltHolePattern;
        }

        //! [Code snippet bolthole improved helper]

        //! [Code snippet optimize fp]

        public static void FullyOptimizeFeaturePtop(ref Esprit.FeaturePtop featurePtop, Esprit.Document document, Esprit.EventWindow outputWindow)
        {
            var set = SelectionSetHelper.GetOrAddSelectionSet(document.SelectionSets, "Temp");

            set.RemoveAll();
            set.Add(featurePtop);
            set.AddCopiesToSelectionSet = true;

            var shortestItemIndex = 0;
            var length = 0.0;

            for (var i = 1; i <= featurePtop.Count; i++)
            {
                set.Translate(0, 0, 0, 1);
                var featurePtopCopy = set[2] as Esprit.FeaturePtop;
                featurePtopCopy.Optimize(EspritConstants.espPtopOptimizeType.espPtopOptimizeTypeExtended, i);
                outputWindow?.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FullyOptimizeFeaturePtop", $"Length starting at Item {i} is {Math.Round(featurePtopCopy.Length, 5)}");

                if (i == 1)
                {
                    shortestItemIndex = i;
                    length = featurePtopCopy.Length;
                }
                else if (featurePtopCopy.Length < length)
                {
                    length = featurePtopCopy.Length;
                    shortestItemIndex = i;
                }

                set.Remove(2);
                document.FeaturePtops.Remove(featurePtopCopy.Key);
            }

            if (shortestItemIndex > 0)
            {
                featurePtop.Optimize(EspritConstants.espPtopOptimizeType.espPtopOptimizeTypeExtended, shortestItemIndex);
                outputWindow?.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FullyOptimizeFeaturePtop", $"Shortest path is {Math.Round(featurePtop.Length, 5)} starting at what was item {shortestItemIndex}");
                document.Refresh();
            }
        }

        //! [Code snippet optimize fp]
    }
}
