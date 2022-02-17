using System;
using System.Windows.Forms;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class ExponentialAndSineWaveCurvesTutorial : BaseTutorial
    {
        public ExponentialAndSineWaveCurvesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet exp]

        private void CreateExponentialCurve()
        {
            var coefA = 1.0;
            var coefB = 1.0;
            if (!RequestUserInput("For the equation y = Ae^Bx, enter the coefficient A.", "A", ref coefA) ||
                !RequestUserInput("For the equation y = Ae^Bx, enter the coefficient B.", "B", ref coefB) || 
                (coefA == 0 && coefB == 0))
            {
                return;
            }

            var startX = 0.0;
            var endX = 1.0;
            var stepX = 0.1;
            if (!RequestUserInput("Enter the starting X value.", "Start X", ref startX) ||
                !RequestUserInput("Enter the ending X value.", "End X", ref endX) ||
                !RequestUserInput("Enter the X step value.", "Step X", ref stepX) || 
                startX == endX)
            {
                return;
            }

            stepX = Math.Abs(stepX) * Math.Sign(endX - startX);
            Esprit.FeaturePtop featurePtop = null;
            double x = 0;

            for (x = startX; x <= endX; x += stepX)
            {
                var y = coefA * Math.Exp(coefB * x);
                var point = Document.GetPoint(x, y, 0);
                if (featurePtop == null)
                {
                    featurePtop = Document.FeaturePtops.Add(point);
                }
                else
                {
                    featurePtop.Add(point);
                }
            }

            if (featurePtop == null)
            {
                System.Windows.MessageBox.Show("There is no featurePtop");
                return;
            }

            if (x != endX)
            {
                var y = coefA * Math.Exp(coefB * endX);
                var point = Document.GetPoint(endX, y, 0);
                featurePtop.Add(point);
            }

            try
            {
                var curve = Document.Curves.AddFromElement(featurePtop);
                if (curve != null)
                {
                    GraphicObjectHelper.SetDefaultAttributes(curve as Esprit.GraphicObject, EspritApplication.Configuration);
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            { }

            Document.FeaturePtops.Remove(featurePtop.Key);
            Document.Refresh();
        }

        //! [Code snippet exp]

        //! [Code snippet sine]

        private void CreateSineWaveCurve()
        {
            var coefA = 1.0;
            var coefB = 1.0;
            if (!RequestUserInput("For the equation y = Asin(x) + Bcos(x), enter the coefficient A.", "A", ref coefA) ||
                !RequestUserInput("For the equation y = Asin(x) + Bcos(x), enter the coefficient B.", "B", ref coefB) ||
                (coefA == 0 && coefB == 0))
            {
                return;
            }

            var startX = 0.0;
            var endX = 1.0;
            var stepX = 0.1;
            if (!RequestUserInput("Enter the starting X value.", "Start X", ref startX) ||
                !RequestUserInput("Enter the ending X value.", "End X", ref endX) ||
                !RequestUserInput("Enter the X step value.", "Step X", ref stepX) ||
                startX == endX)
            {
                return;
            }

            stepX = Math.Abs(stepX) * Math.Sign(endX - startX);
            Esprit.FeaturePtop featurePtop = null;
            double x = 0;
            for (x = startX; x <= endX; x += stepX)
            {
                var y = coefA * Math.Sin(x) + coefB * Math.Cos(x);
                var point = Document.GetPoint(x, y, 0);
                if (featurePtop == null)
                {
                    featurePtop = Document.FeaturePtops.Add(point);
                }
                else
                {
                    featurePtop.Add(point);
                }
            }

            if (featurePtop == null)
            {
                System.Windows.MessageBox.Show("There is no featurePtop");
                return;
            }

            if (x != endX)
            {
                var y = coefA * Math.Sin(endX) + coefB * Math.Cos(endX);
                var point = Document.GetPoint(endX, y, 0);
                featurePtop.Add(point);
            }

            Esprit.Curve curve = null;
            try
            {
                curve = Document.Curves.AddFromElement(featurePtop);
                if (curve != null)
                {
                    GraphicObjectHelper.SetDefaultAttributes(curve as Esprit.GraphicObject, EspritApplication.Configuration);
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            { }

            Document.FeaturePtops.Remove(featurePtop.Key);
            Document.Refresh();
        }

        //! [Code snippet sine]

        public override void Execute()
        {
            CreateExponentialCurve();
            // CreateSineWaveCurve can be used alternatively
        }

        public override string Name => "Exponential and Sine Wave Curves";
        public override string HtmlPath => "html/exponential_and_sine_curves_tutorial.html";

    }
}
