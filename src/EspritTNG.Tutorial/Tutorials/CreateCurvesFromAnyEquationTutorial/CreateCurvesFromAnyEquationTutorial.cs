using System;
using System.Windows;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class CreateCurvesFromAnyEquationTutorial : BaseTutorial
    {
        private Tutorial_CreateCurvesFromAnyEquation.EvaluateDialog _dialog;

        public CreateCurvesFromAnyEquationTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            CreateMangerDialog();
        }

        private void CreateMangerDialog()
        {
            _dialog = new Tutorial_CreateCurvesFromAnyEquation.EvaluateDialog();
            _dialog.ApplyButton.Click += OnApplyButtonClick;

            _dialog.ShowDialog();
        }

        //! [Code snippet apply]

        private void OnApplyButtonClick(object sender, RoutedEventArgs e)
        {
            double startValue = 0.0;
            double endValue = 0.0;
            double stepValue = 0.0;

            if (!double.TryParse(_dialog.StartValue.Text, out startValue) ||
                !double.TryParse(_dialog.EndValue.Text, out endValue) || 
                !double.TryParse(_dialog.StepValue.Text, out stepValue))
            {
                MessageBox.Show("Incorrect input values.", "CreateCurvesFromAnyEquationTutorial", MessageBoxButton.OK);
                return;
            }

            _dialog.Close();

            if (_dialog.Cam.IsChecked.HasValue && _dialog.Cam.IsChecked.Value)
            {
                startValue *= (Math.PI / 180.0);
                endValue *= (Math.PI / 180.0);
                stepValue *= (Math.PI / 180.0);
            }

            if (startValue == endValue)
            {
                MessageBox.Show("Start Value and End Value cannot be the same.", "CreateCurvesFromAnyEquationTutorial", MessageBoxButton.OK);
                return;
            }
            if (stepValue == 0)
            {
                MessageBox.Show("Step Value cannot be zero.", "CreateCurvesFromAnyEquationTutorial", MessageBoxButton.OK);
                return;
            }

            if (_dialog.Cam.IsChecked.HasValue && _dialog.Cam.IsChecked.Value)
            {
                CamCurve(startValue, endValue, stepValue);
            }
            else
            {
                XYCurve(startValue, endValue, stepValue);
            }
                
            Document.Refresh();
        }

        //! [Code snippet apply]

        //! [Code snippet curves]

        private static double EvaluateY(double x)
        {
            return x * x + 2 * x + 3;
        }

        private void XYCurve(double startX, double endX, double stepX)
        {
            stepX = Math.Abs(stepX) * Math.Sign(endX - startX);
            Esprit.FeaturePtop featurePtop = null;
            var x = startX;

            for (x = startX; x <= endX; x+=stepX)
            {
                var y = EvaluateY(x);
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

            if (x != endX)
            {
                var y = EvaluateY(endX);
                var point = Document.GetPoint(endX, y, 0);

                if (featurePtop == null)
                {
                    featurePtop = Document.FeaturePtops.Add(point);
                }
                else
                {
                    featurePtop.Add(point);
                }
            }

            if (featurePtop != null)
            {
                try
                {
                    Esprit.Curve curve = Document.Curves.AddFromElement(featurePtop);
                    GraphicObjectHelper.SetDefaultAttributes(curve as Esprit.GraphicObject, EspritApplication.Configuration);
                }
                catch (System.Runtime.InteropServices.COMException)
                { }

                Document.FeaturePtops.Remove(featurePtop.Key);
            }
        }

        private void CamCurve(double startAngle, double endAngle, double stepAngle)
        {
            stepAngle = Math.Abs(stepAngle) * Math.Sign(endAngle - startAngle);
            Esprit.FeaturePtop featurePtop = null;
            var a = startAngle;

            for (a = startAngle; a <= endAngle; a += stepAngle)
            {
                var r = EvaluateY(a);
                var point = Document.GetPoint(r * Math.Cos(a), r * Math.Sin(a), 0);

                if (featurePtop == null)
                {
                    featurePtop = Document.FeaturePtops.Add(point);
                }
                else
                {
                    featurePtop.Add(point);
                }
            }

            if (a != endAngle)
            {
                var r = EvaluateY(endAngle);
                var point = Document.GetPoint(r * Math.Cos(endAngle), r * Math.Sin(endAngle), 0);

                if (featurePtop == null)
                {
                    featurePtop = Document.FeaturePtops.Add(point);
                }
                else
                {
                    featurePtop.Add(point);
                }
            }

            if (featurePtop != null)
            {
                try
                {
                    var curve = Document.Curves.AddFromElement(featurePtop);
                    GraphicObjectHelper.SetDefaultAttributes(curve as Esprit.GraphicObject, EspritApplication.Configuration);
                }
                catch (System.Runtime.InteropServices.COMException)
                { }

                Document.FeaturePtops.Remove(featurePtop.Key);
            }
        }

        //! [Code snippet curves]

        public override string Name => "Creating Curves from Any Equation";
        public override string HtmlPath => "html/create_curves_from_any_equation_tutorial.html";

    }
}
