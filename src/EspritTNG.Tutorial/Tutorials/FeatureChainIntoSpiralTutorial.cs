using System;
using System.Windows;
using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    class FeatureChainIntoSpiralTutorial : BaseTutorial
    {
        public FeatureChainIntoSpiralTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void SpiralClosedFeature()
        {
            Esprit.FeatureChain featureChain = null;
            try
            {
                featureChain = Document.GetAnyElement("Pick Feature Chain to Spiral", EspritConstants.espGraphicObjectType.espFeatureChain) as Esprit.FeatureChain;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if (featureChain == null)
            {
                return;
            }

            if (!featureChain.IsClosed)
            {
                System.Windows.MessageBox.Show("Feature chain must be closed!", "FeatureChainIntoSpiralTutorial", MessageBoxButton.OK);
                return;
            }
            var totalSegmentLength = 0.0;

            for (var i = 1; i <= featureChain.Count; i++)
            {
                if (featureChain.Item[i].GraphicObjectType == EspritConstants.espGraphicObjectType.espSegment)
                {
                    var segment = featureChain.Item[i] as Esprit.Segment;
                    totalSegmentLength += segment.Length;
                }
            }

            if (totalSegmentLength == 0)
            {
                System.Windows.MessageBox.Show("Cannot spiral feature containing only arcs!", "FeatureChainIntoSpiralTutorial", MessageBoxButton.OK);
                return;
            }

            var totalDepth = (Document.SystemUnit == EspritConstants.espUnitType.espInch)
                ? 1.0
                : 25.0;

            var valueEntered = true;
            do
            {
                valueEntered = RequestUserInput("Enter Total Depth", "Total Depth", ref totalDepth);
            } while (!valueEntered);

            totalDepth = Math.Abs(totalDepth);
            var maxIncrementalDepth = totalDepth / 10;

            if (!RequestUserInput("Enter Maximum Incremental Depth", "Max Increment", ref maxIncrementalDepth))
            {
                maxIncrementalDepth = totalDepth;
            }
            var numberOfPasses = (int)(totalDepth / maxIncrementalDepth);
            if (totalDepth / maxIncrementalDepth > numberOfPasses)
            {
                numberOfPasses++;
            }

            maxIncrementalDepth = totalDepth / numberOfPasses;

            var spiral = Document.FeatureChains.Add(featureChain.Extremity(EspritConstants.espExtremityType.espExtremityStart));
            spiral.Color = featureChain.Color;
            spiral.LineType = featureChain.LineType;
            spiral.LineWeight = featureChain.LineWeight;

            var currentDepth = spiral.Extremity(EspritConstants.espExtremityType.espExtremityStart).Z;
            numberOfPasses++;

            for (var pass = 1; pass <= numberOfPasses; pass++)
            {
                for (var i = 1; i <= featureChain.Count; i++)
                {
                    switch (featureChain.Item[i].GraphicObjectType)
                    {
                        case EspritConstants.espGraphicObjectType.espArc:
                            var arc = featureChain.Item[i] as Esprit.Arc;
                            var centralPoint = arc.CenterPoint;
                            centralPoint = Document.GetPoint(centralPoint.X, centralPoint.Y, currentDepth);

                            try
                            {
                                arc = Document.GetArc(centralPoint, arc.Radius, arc.StartAngle, arc.EndAngle);
                                spiral.Add(arc);
                            }
                            catch (System.Runtime.InteropServices.COMException)
                            {
                            }
                            break;
                        case EspritConstants.espGraphicObjectType.espSegment:
                            var segment = featureChain.Item[i] as Esprit.Segment;
                            if (pass < numberOfPasses)
                            {
                                currentDepth -= maxIncrementalDepth * segment.Length / totalSegmentLength;
                            }
                            spiral.Add(Document.GetPoint(segment.XEnd, segment.YEnd, currentDepth));
                            break;
                    }
                }
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            SpiralClosedFeature();
        }

        public override string Name => "Making a FeatureChain into a Spiral";
        public override string HtmlPath => "html/featurechain_into_spiral_tutorial.html";

    }
}
