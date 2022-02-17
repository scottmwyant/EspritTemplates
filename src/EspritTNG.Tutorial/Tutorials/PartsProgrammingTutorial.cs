using System;
using System.Windows.Forms;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{

    //! [Code snippet declare]

    class PartsProgrammingHandler
    {
        private readonly Esprit.Application _espritApplication;
        private double _localGrooveWidth;
        private double _localThreadedDiameter;

        public double FilletRadius;
        public double Length;
        public double OutsideDiameter;
        public double GrooveWidth
        {
            get => _localGrooveWidth;
            set
            {
                _localGrooveWidth = (value > Length / 4.0)
                     ? Length / 4.0
                     : value;
            }
        }
        public double ThreadedDiameter
        {
            get => _localThreadedDiameter;
            set
            {
                _localThreadedDiameter = (value > OutsideDiameter / 2.0)
                     ? OutsideDiameter / 2.0
                     : value;
            }
        }


        public PartsProgrammingHandler(Esprit.Application app)
        {
            _espritApplication = app;
            if (_espritApplication.Document.SystemUnit == EspritConstants.espUnitType.espInch)
            {
                FilletRadius = 0.25;
                Length = 8;
                OutsideDiameter = 5;
                GrooveWidth = 1.625;
                ThreadedDiameter = 1;
            }
            else
            {
                FilletRadius = 5;
                Length = 200;
                OutsideDiameter = 125;
                GrooveWidth = 40;
                ThreadedDiameter = 25;
            }
        }

        public Esprit.SelectionSet DrawPart()
        {
            var document = _espritApplication.Document;
            var set = SelectionSetHelper.AddUniqueSelectionSet(document.SelectionSets, "LatheFamilyPart");

            var point1 = document.GetPoint(0, 0, 0);
            var point2 = document.GetPoint(0, ThreadedDiameter / 2.0, 0);
            var segment = document.Segments.Add(point1, point2);
            set.Add(segment);

            var localFaceContourFeature = document.FeatureChains.Add(point2);
            localFaceContourFeature.Add(point1);

            point1 = point2;
            var radialDifference = (OutsideDiameter - ThreadedDiameter) / 2.0;
            point2 = document.GetPoint(-(Length / 2) + radialDifference + FilletRadius * (1 - 2 * (Math.Sqrt(2) - 1) * Math.Cos(Geometry.ToRadians(45))), point1.Y, 0);
            segment = document.Segments.Add(point1, point2);
            set.Add(segment);

            var localODRoughFeature = document.FeatureChains.Add(point1);
            localODRoughFeature.Add(segment);
            var localThreadFeature = document.FeatureChains.Add(point1);
            localThreadFeature.Add(segment);

            var point3 = document.GetPoint(point2.X, point2.Y + FilletRadius, 0);
            var arc = document.Arcs.Add(point3, FilletRadius, Geometry.ToRadians(225), Geometry.ToRadians(270));
            set.Add(arc);
            arc.Reverse();
            localODRoughFeature.Add(arc);

            point1 = arc.Extremity(EspritConstants.espExtremityType.espExtremityEnd);
            point2 = document.GetPoint(point1.X - ((OutsideDiameter / 2.0) - point1.Y), OutsideDiameter / 2.0, 0);
            segment = document.Segments.Add(point1, point2);
            set.Add(segment);
            localODRoughFeature.Add(segment);

            point1 = point2;
            point2 = document.GetPoint(-(3 * Length / 4) + (GrooveWidth / 2), point1.Y, 0);
            segment = document.Segments.Add(point1, point2);
            set.Add(segment);

            point1 = point2;
            point2 = document.GetPoint(point1.X, point1.Y - radialDifference / 2.0, 0);
            segment = document.Segments.Add(point1, point2);
            set.Add(segment);
            var localGrooveFeature = document.FeatureChains.Add(point1);
            localGrooveFeature.Add(segment);

            point1 = point2;
            point2 = document.GetPoint(point1.X - GrooveWidth, point1.Y, 0);
            segment = document.Segments.Add(point1, point2);
            set.Add(segment);
            localGrooveFeature.Add(segment);

            point1 = point2;
            point2 = document.GetPoint(point1.X, OutsideDiameter / 2.0, 0);
            segment = document.Segments.Add(point1, point2);
            set.Add(segment);
            localGrooveFeature.Add(segment);

            point1 = point2;
            point2 = document.GetPoint(-Length + FilletRadius, point1.Y, 0);
            segment = document.Segments.Add(point1, point2);
            set.Add(segment);

            point3 = document.GetPoint(point2.X, point2.Y - FilletRadius, 0);
            arc = document.Arcs.Add(point3, FilletRadius, Geometry.ToRadians(90), Geometry.ToRadians(180));
            set.Add(arc);

            localODRoughFeature.Add(document.GetPoint(-Length, point1.Y, 0));
            var localCutoffFeature = document.FeatureChains.Add(arc.Extremity(EspritConstants.espExtremityType.espExtremityStart));
            localCutoffFeature.Add(arc);

            point1 = arc.Extremity(EspritConstants.espExtremityType.espExtremityEnd);
            point2 = document.GetPoint(-Length, 0, 0);
            segment = document.Segments.Add(point1, point2);
            set.Add(segment);
            localCutoffFeature.Add(segment);

            localODRoughFeature.Name = "1 - OD Rough";
            localFaceContourFeature.Name = "2 - Face Contour";
            localThreadFeature.Name = "3 - Thread";
            localGrooveFeature.Name = "4 - Groove";
            localCutoffFeature.Name = "5 - Cutoff";

            set.Add(localODRoughFeature);
            set.Add(localFaceContourFeature);
            set.Add(localGrooveFeature);
            set.Add(localThreadFeature);
            set.Add(localCutoffFeature);
            SelectionSetHelper.DefaultAttributesSet(set, _espritApplication.Configuration);

            document.Refresh();

            return set;
        }
    }

    //! [Code snippet declare]

    class PartsProgrammingTutorial : BaseTutorial
    {
        public PartsProgrammingTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet use]

        private PartsProgrammingHandler _testObject = null;

        public void LatheFamilyPartExample()
        {
            if (_testObject == null)
            {
                _testObject = new PartsProgrammingHandler(EspritApplication);
            }

            var filletRadius = _testObject.FilletRadius;
            var length = _testObject.Length;
            var outsideDiameter = _testObject.OutsideDiameter;
            var grooveWidth = _testObject.GrooveWidth;
            var threadedDiameter = _testObject.ThreadedDiameter;

            if (RequestUserInput("Enter Part Length", "Length", ref length) &&
                RequestUserInput("Enter Outside Diameter", "Diameter", ref outsideDiameter) &&
                RequestUserInput("Enter Fillet Radius", "Fillet Radius", ref filletRadius) &&
                RequestUserInput("Enter GrooveWidth", "Groove Width", ref grooveWidth) &&
                RequestUserInput("Enter Threaded Diameter", "Threaded Diameter", ref threadedDiameter))
            {
                if (length > 0 && outsideDiameter > 0 && filletRadius > 0 && grooveWidth > 0 && threadedDiameter > 0)
                {
                    _testObject.FilletRadius = filletRadius;
                    _testObject.Length = length;
                    _testObject.OutsideDiameter = outsideDiameter;
                    _testObject.GrooveWidth = grooveWidth;
                    _testObject.ThreadedDiameter = threadedDiameter;
                    _testObject.DrawPart();
                }
            }
        }

        //! [Code snippet use]

        public override void Execute()
        {
            LatheFamilyPartExample();
        }

        public override string Name => "Introduction to Family of Parts Programming";
        public override string HtmlPath => "html/parts_programming_tutorial.html";

    }
}
