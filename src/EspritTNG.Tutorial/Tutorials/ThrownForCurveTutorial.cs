using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class ThrownForCurveTutorial : BaseTutorial
    {
        private Form _form;

        private void Show()
        {
            _form = new Form();
            var scanButton = new Button();
            var groupedPointsToCurveButton = new Button();
            var convertEllipsesToCurvesButton = new Button();
            var groupedPointsToClosedCurveButton = new Button();
            var oneRevolutionButton = new Button();
            var constantRadiusButton = new Button();

            _form.Text = "ThrownForCurveTutorial";
            scanButton.Text = "Scan curves";
            groupedPointsToCurveButton.Text = "Grouped points to curve";
            groupedPointsToClosedCurveButton.Text = "Grouped points to closed curve";
            convertEllipsesToCurvesButton.Text = "Convert ellipses to curves";
            oneRevolutionButton.Text = "One revolution helix from center and depth";
            constantRadiusButton.Text = "Constant radius helix from center axis segment";

            scanButton.AutoSize = groupedPointsToCurveButton.AutoSize = convertEllipsesToCurvesButton.AutoSize = groupedPointsToClosedCurveButton.AutoSize = constantRadiusButton.AutoSize = oneRevolutionButton.AutoSize = true;
             scanButton.Anchor = groupedPointsToCurveButton.Anchor = convertEllipsesToCurvesButton.Anchor = groupedPointsToClosedCurveButton.Anchor = oneRevolutionButton.Anchor = constantRadiusButton.Anchor = oneRevolutionButton.Anchor | AnchorStyles.Right;

            scanButton.SetBounds(5, 5, 172, 13);
            groupedPointsToCurveButton.SetBounds(5, 35, 172, 13);
            convertEllipsesToCurvesButton.SetBounds(5, 65, 172, 13);
            groupedPointsToClosedCurveButton.SetBounds(5, 95, 172, 13);
            oneRevolutionButton.SetBounds(5, 125, 172, 13);
            constantRadiusButton.SetBounds(5, 155, 172, 13);

            _form.ClientSize = new System.Drawing.Size(596, 187);
            _form.Controls.AddRange(new Control[] { scanButton, groupedPointsToCurveButton, convertEllipsesToCurvesButton, groupedPointsToClosedCurveButton, oneRevolutionButton, constantRadiusButton });
            _form.ClientSize = new System.Drawing.Size(Math.Max(350, oneRevolutionButton.Right + 10), _form.ClientSize.Height);
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            _form.FormClosed += OnFormClosed;
            scanButton.Click += OnScanButtonClick;
            groupedPointsToCurveButton.Click += OnGroupedPointsToCurveButtonClick;
            convertEllipsesToCurvesButton.Click += OnConvertEllipsesToCurvesButtonClick;
            groupedPointsToClosedCurveButton.Click += OnGroupedPointsToClosedCurveButtonClick;
            oneRevolutionButton.Click += OnOneRevolutionButtonClick;
            constantRadiusButton.Click += OnConstantRadiusButtonClick;

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet revolution]

        private void OneRevolutionHelixFromCenterAndDepth()
        {
            Esprit.Point centerPoint = null;
            try
            {
                centerPoint = Document.GetPoint("Select Top Center Point");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Point was not set
                return;
            }

            if (centerPoint == null)
            {
                return;
            }

            var depth = 2.0;
            if (!RequestUserInput("Enter Depth", "Depth", ref depth) || depth == 0)
            {
                return;
            }

            var startRadius = 1.0;
            if (!RequestUserInput("Enter Starting Radius", "Starting Radius", ref startRadius) || startRadius == 0)
            {
                return;
            }

            var endRadius = 10.0;
            if (!RequestUserInput("Enter Ending Radius", "Ending Radius", ref endRadius) || endRadius == 0)
            {
                return;
            }

            var point = Document.GetPoint(centerPoint.X, centerPoint.Y, centerPoint.Z - depth);
            var sPoint = Document.GetPoint(centerPoint.X + startRadius, centerPoint.Y, centerPoint.Z);
            var taper = Math.Atan((endRadius - startRadius) / depth);

            try
            {
                GraphicObjectHelper.SetDefaultAttributes(Document.Curves.AddHelix(centerPoint, point, sPoint, depth, taper) as Esprit.GraphicObject, EspritApplication.Configuration);
            }
            catch (System.Runtime.InteropServices.COMException)
            { }
        }

        //! [Code snippet revolution]

        private void OnOneRevolutionButtonClick(object sender, EventArgs e)
        {
            OneRevolutionHelixFromCenterAndDepth();
        }

        //! [Code snippet grouped to closed curve]

        private void GroupedPointsToClosedCurve()
        {
            var points = new List<Esprit.Point>();
            for (var i = 1; i <= Document.Group.Count; i++)
            {
                var graphicObject = Document.Group[i] as Esprit.IGraphicObject;
                if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espPoint)
                {
                    points.Add(graphicObject as Esprit.Point);
                }
            }

            if (points.Count != 0)
            {
                points.Add(points[0]);

                Document.Curves.Add(points.ToArray());
                Document.Refresh();
            }
        }

        //! [Code snippet grouped to closed curve]

        private void OnGroupedPointsToClosedCurveButtonClick(object sender, EventArgs e)
        {
            GroupedPointsToClosedCurve();
        }

        //! [Code snippet ellipses]

        private Esprit.Curve ConvertEllipseToCurve(Esprit.IGraphicObject graphicObject)
        {
            Esprit.Curve curve = null;
            if (graphicObject == null)
            {
                return curve;
            }

            try
            {
                curve = Document.Curves.AddFromElement(graphicObject);
            }
            catch (System.Runtime.InteropServices.COMException)
            { }

            if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espArc)
            {
                Document.Arcs.Remove(graphicObject.Key);
            }
            else if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espCircle)
            {
                Document.Circles.Remove(graphicObject.Key);
            }

            return curve;
        }

        private void ConvertEllipsesToCurves()
        {
            if (Document.Group.Count == 0)
            {
                var mask = new EspritConstants.espGraphicObjectType[2] { EspritConstants.espGraphicObjectType.espArc, EspritConstants.espGraphicObjectType.espCircle };
                Esprit.IGraphicObject graphicObject = null;
                try
                {
                    graphicObject = Document.GetAnyElement("Pick the Ellipse", mask);
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // Element was not set
                    return;
                }

                if (graphicObject == null)
                {
                    return;
                }

                GraphicObjectHelper.SetDefaultAttributes(ConvertEllipseToCurve(graphicObject) as Esprit.GraphicObject, EspritApplication.Configuration);
            }
            else
            {
                for (var i = Document.Group.Count; i > 0 ; i--)
                {
                    var graphicObject = Document.Group[i] as Esprit.IGraphicObject;
                    if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espArc || graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espCircle)
                    {
                        GraphicObjectHelper.SetDefaultAttributes(ConvertEllipseToCurve(graphicObject) as Esprit.GraphicObject, EspritApplication.Configuration);
                    }
                }
            }

            Document.Refresh();
        }

        //! [Code snippet ellipses]

        private void OnConvertEllipsesToCurvesButtonClick(object sender, EventArgs e)
        {
            ConvertEllipsesToCurves();
        }

        //! [Code snippet grouped to curve]

        private void GroupedPointsToCurve()
        {
            var points = new List<Esprit.Point>();
            for (var i = 1; i <= Document.Group.Count; i++)
            {
                var graphicObject = Document.Group[i] as Esprit.IGraphicObject;
                if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espPoint)
                {
                    points.Add(graphicObject as Esprit.Point);
                }
            }

            if (points.Count != 0)
            {
                Document.Curves.Add(points.ToArray());
                Document.Refresh();
            }
        }

        //! [Code snippet grouped to curve]

        private void OnGroupedPointsToCurveButtonClick(object sender, EventArgs e)
        {
            GroupedPointsToCurve();
        }

        //! [Code snippet scan]

        private void ScanCurves()
        {
            var outputWindow = EspritApplication.EventWindow;

            outputWindow.Clear();
            outputWindow.Visible = true;

            foreach(Esprit.Curve curve in Document.Curves)
            {
                outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ThrownForCurveTutorial", $"Curve {curve.Key} IsClosed = {curve.IsClosed}");
                outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ThrownForCurveTutorial", $"Curve {curve.Key} IsPeriodic = {curve.IsPeriodic}");
                outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ThrownForCurveTutorial", $"Curve {curve.Key} Layer Name = {curve.Layer.Name}");
                outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ThrownForCurveTutorial", $"Curve {curve.Key} Length = {Math.Round(curve.Length, 5)}");
                outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ThrownForCurveTutorial", $"Curve {curve.Key} MaxCurvature = {Math.Round(curve.MaxCurvature, 5)}");
                outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ThrownForCurveTutorial", $"Curve {curve.Key} MinCurvature = {Math.Round(curve.MinCurvature, 5)}");
            }
        }

        //! [Code snippet scan]

        private void OnScanButtonClick(object sender, EventArgs e)
        {
            ScanCurves();
        }

        //! [Code snippet segment]

        private void ConstantRadiusHelixFromCenterAxisSegment()
        {
            Esprit.Segment segment = null;
            try
            {
                segment = Document.GetSegment("Select Center Axis Segment");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if (segment == null)
            {
                return;
            }

            var revolutions = 2.0;
            if (!RequestUserInput("Enter Number of Revolutions", "Revolutions", ref revolutions) || revolutions == 0)
            {
                return;
            }

            var startRadius = 10.0;
            if (!RequestUserInput("Enter Radius", "Radius", ref startRadius) || startRadius == 0)
            {
                return;
            }

            var point1 = segment.Extremity(EspritConstants.espExtremityType.espExtremityStart);
            var point2 = segment.Extremity(EspritConstants.espExtremityType.espExtremityEnd);
            var sPoint = Document.GetPoint(point1.X + startRadius, point1.Y, point1.Z);
            var lead = segment.Length / revolutions;

            try
            {
                GraphicObjectHelper.SetDefaultAttributes(Document.Curves.AddHelix(point1, point2, sPoint, lead, 0) as Esprit.GraphicObject, EspritApplication.Configuration);
            }
            catch (System.Runtime.InteropServices.COMException)
            { }
        }

        //! [Code snippet segment]

        private void OnConstantRadiusButtonClick(object sender, EventArgs e)
        {
            ConstantRadiusHelixFromCenterAxisSegment();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _form = null;
        }

        public ThrownForCurveTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "Thrown for a Curve";
        public override string HtmlPath => "html/thrown_for_curve_tutorial.html";

    }
}
