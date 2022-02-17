using System;
using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    //! [Code snippet declare]

    class BoltHolePatternHandler
    {
        private readonly Esprit.Application _espritApplication;
        public double HoleDiameter;
        public double PatternDiameter;
        public int NumberOfHoles;

        public BoltHolePatternHandler(Esprit.Application app)
        {
            _espritApplication = app;
            if (_espritApplication.Document.SystemUnit == EspritConstants.espUnitType.espInch)
            {
                HoleDiameter = 1;
                PatternDiameter = 10;
            }
            else
            {
                HoleDiameter = 25;
                PatternDiameter = 250;
            }
            NumberOfHoles = 10;
        }

        public void DrawPattern(Esprit.Point point)
        {
            if (point == null)
            {
                return;
            }

            for (var i = 0; i < NumberOfHoles; i++)
            {
                var angle = i * 2 * Math.PI / NumberOfHoles;
                var x = (PatternDiameter / 2) * Math.Cos(angle) + point.X;
                var y = (PatternDiameter / 2) * Math.Sin(angle) + point.Y;
                _espritApplication.Document.Circles.Add(_espritApplication.Document.GetPoint(x, y, 0), HoleDiameter / 2.0);
            }

            _espritApplication.Document.Refresh();
        }
    }

    //! [Code snippet declare]

    class BoltHolePatternTutorial : BaseTutorial
    {
        public BoltHolePatternTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet use]

        private BoltHolePatternHandler _boltHolePattern = null;

        public void MakeBoltHolePattern()
        {
            if (_boltHolePattern == null)
            {
                _boltHolePattern = new BoltHolePatternHandler(EspritApplication);
            }

            var holeDiameter = _boltHolePattern.HoleDiameter;
            var patternDiameter = _boltHolePattern.PatternDiameter;
            var numberOfHoles = _boltHolePattern.NumberOfHoles;
            
            if (RequestUserInput("Enter Hole Diameter", "Hole Diameter", ref holeDiameter) &&
                RequestUserInput("Enter Pattern Diameter", "Pattern Diameter", ref patternDiameter) &&
                RequestUserInput("Enter Number of Holes", "Holes", ref numberOfHoles))
            {
                if (holeDiameter > 0 && patternDiameter > 0 && numberOfHoles > 0)
                {
                    _boltHolePattern.HoleDiameter = holeDiameter;
                    _boltHolePattern.PatternDiameter = patternDiameter;
                    _boltHolePattern.NumberOfHoles = numberOfHoles;

                    Esprit.Point point = null;
                    try
                    {
                        point = Document.GetPoint("Pick The Pattern Center");
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        // Point was not set
                        return;
                    }

                    _boltHolePattern.DrawPattern(point);
                }
            }
        }

        //! [Code snippet use]

        public override void Execute()
        {
            MakeBoltHolePattern();
        }

        public override string Name => "Using Class Modules: Bolt Hole Pattern";
        public override string HtmlPath => "html/bolt_hole_pattern_tutorial.html";

    }
}
