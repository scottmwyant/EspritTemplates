using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class CrossSectionSurfaceMeshCurvesTutorial : BaseTutorial
    {
        private enum EspSectionNormalAxis
        {
            espSectionNormalAxisX = 1,
            espSectionNormalAxisY = 2,
            espSectionNormalAxisZ = 3
        }

        private Form _form;

        private void Show()
        {
            _form = new Form();
            var createCrossSectionCurvesNormalToXButton = new Button();
            var createCrossSectionCurvesNormalToYButton = new Button();
            var createXYMeshButton = new Button();

            _form.Text = "CrossSectionSurfaceMeshCurvesTutorial";
            createCrossSectionCurvesNormalToXButton.Text = "Create cross section curves normal to X";
            createCrossSectionCurvesNormalToYButton.Text = "Create cross section curves normal to Y";
            createXYMeshButton.Text = "Create XY Mesh";

            createCrossSectionCurvesNormalToXButton.AutoSize = createCrossSectionCurvesNormalToYButton.AutoSize = createXYMeshButton.AutoSize = true;
            createCrossSectionCurvesNormalToXButton.Anchor = createCrossSectionCurvesNormalToYButton.Anchor = createXYMeshButton.Anchor = createXYMeshButton.Anchor | AnchorStyles.Right;

            createCrossSectionCurvesNormalToXButton.SetBounds(5, 5, 172, 13);
            createCrossSectionCurvesNormalToYButton.SetBounds(5, 35, 172, 13);
            createXYMeshButton.SetBounds(5, 65, 172, 13);

            _form.ClientSize = new System.Drawing.Size(187, 137);
            _form.Controls.AddRange(new Control[] { createCrossSectionCurvesNormalToXButton, createCrossSectionCurvesNormalToYButton, createXYMeshButton });
            _form.ClientSize = new System.Drawing.Size(Math.Max(350, createCrossSectionCurvesNormalToXButton.Right + 10), _form.ClientSize.Height);
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            _form.FormClosed += OnFormClosed;
            createCrossSectionCurvesNormalToXButton.Click += OnCreateCrossSectionCurvesNormalToXButtonClick;
            createCrossSectionCurvesNormalToYButton.Click += OnCreateCrossSectionCurvesNormalToYButtonClick;
            createXYMeshButton.Click += OnCreateXYMeshButtonClick;

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet curves]

        private static void CreateCrossSectionCurves(Esprit.Application app,  EspSectionNormalAxis normalAxis, double start, double step, double deviation = 0)
        {
            var document = app.Document;
            Dictionary<int, Esprit.FeaturePtop> ptops = new Dictionary<int, Esprit.FeaturePtop>();
            Dictionary<int, double> min = new Dictionary<int, double>();
            Dictionary<int, double> max = new Dictionary<int, double>();

            for (var i = 1; i <= document.Group.Count; i++)
            {
                var graphicObject = document.Group[i] as Esprit.IGraphicObject;
                if (graphicObject == null || graphicObject.GraphicObjectType != EspritConstants.espGraphicObjectType.espPoint)
                {
                    continue;
                }

                var point = graphicObject as Esprit.Point;
                double pointCoordinate = 0;
                switch(normalAxis)
                {
                    case EspSectionNormalAxis.espSectionNormalAxisX:
                        pointCoordinate = point.X;
                        break;
                    case EspSectionNormalAxis.espSectionNormalAxisY:
                        pointCoordinate = point.Y;
                        break;
                    case EspSectionNormalAxis.espSectionNormalAxisZ:
                        pointCoordinate = point.Z;
                        break;
                }

                int sectionNumber = (int)(Math.Abs(pointCoordinate - start) / step);

                if (!min.ContainsKey(sectionNumber) && !max.ContainsKey(sectionNumber))
                {
                    min.Add(sectionNumber, start + sectionNumber * step - Math.Abs(deviation));
                    max.Add(sectionNumber, start + sectionNumber * step + Math.Abs(deviation));
                }

                if (pointCoordinate >= min[sectionNumber] && pointCoordinate <= max[sectionNumber])
                {
                    if (!ptops.ContainsKey(sectionNumber))
                    {
                        ptops.Add(sectionNumber, document.FeaturePtops.Add(point));
                    }
                    else
                    {
                        ptops[sectionNumber].Add(point);
                    }
                }

                foreach(var section in ptops.Values)
                {
                    try
                    {
                        if (section.Count > 1)
                        {
                            GraphicObjectHelper.SetDefaultAttributes(document.Curves.AddFromElement(section) as Esprit.GraphicObject, app.Configuration);
                        }

                        document.FeaturePtops.Remove(section.Key);
                    }
                    catch(Exception)
                    {
                    }
                }
            }
        }

        //! [Code snippet curves]

        //! [Code snippet mesh]

        private void CreateXYMesh()
        {
            if (Document.Group.Count == 0)
            {
                System.Windows.MessageBox.Show("Please group points before running this function.", "CrossSectionSurfaceMeshCurvesTutorial", MessageBoxButton.OK);
                return;
            }

            Esprit.Point prevPoint = null;
            Esprit.Point currentPoint = null;
            List<Esprit.FeaturePtop> featurePtops = new List<Esprit.FeaturePtop>();
            for (var i = 1; i <= Document.Group.Count; i++)
            {
                var graphicObject = Document.Group[i] as Esprit.IGraphicObject;
                if (graphicObject == null || graphicObject.GraphicObjectType != EspritConstants.espGraphicObjectType.espPoint)
                {
                    continue;
                }

                string primaryDirection = string.Empty;
                var point = graphicObject as Esprit.Point;
                if (prevPoint == null)
                {
                    prevPoint = point;
                    featurePtops.Add(Document.FeaturePtops.Add(prevPoint));
                }
                else
                {
                    if (currentPoint == null)
                    {
                        currentPoint = point;
                        var deltaX = currentPoint.X - prevPoint.X;
                        var deltaY = currentPoint.Y - prevPoint.Y;

                        if (Math.Abs(deltaX) > Math.Abs(deltaY))
                        {
                            primaryDirection = (deltaX > 0)
                                ? "+X"
                                : "-X";
                        }
                        else
                        {
                            primaryDirection = (deltaY > 0)
                                ? "+Y"
                                : "-Y";
                        }
                    }
                    else
                    {
                        prevPoint = currentPoint;
                        currentPoint = point;
                    }

                    if (currentPoint != null && prevPoint != null)
                    {
                        bool isDirectionChanged = false;
                        switch (primaryDirection)
                        {
                            case "+X": if (currentPoint.X < prevPoint.X) isDirectionChanged = true; break;
                            case "-X": if (currentPoint.X > prevPoint.X) isDirectionChanged = true; break;
                            case "+Y": if (currentPoint.Y < prevPoint.Y) isDirectionChanged = true; break;
                            case "-Y": if (currentPoint.Y > prevPoint.Y) isDirectionChanged = true; break;
                        }

                        if (isDirectionChanged)
                        {
                            featurePtops.Add(Document.FeaturePtops.Add(currentPoint));
                        }
                        else
                        {
                            featurePtops[featurePtops.Count-1].Add(currentPoint);
                        }
                    }
                    
                }
            }

            var maxCount = 0;
            for (var i = 0; i < featurePtops.Count; i++)
            {
                if (maxCount < featurePtops[i].Count)
                {
                    maxCount = featurePtops[i].Count;
                }
            }
            var featurePtops2 = new Esprit.FeaturePtop[maxCount];

            for (var i = 0; i < maxCount; i++)
            {
                if (i < featurePtops[0].Count)
                {
                    featurePtops2[i] = Document.FeaturePtops.Add(featurePtops[0].Item[i+1] as Esprit.Point);
                }
                else
                {
                    featurePtops2[i] = Document.FeaturePtops.Add(featurePtops[0].Item[featurePtops[0].Count] as Esprit.Point);
                }

                for (var j = 1; j < featurePtops.Count; j++)
                {
                    if (i < featurePtops[j].Count)
                    {
                        featurePtops2[i].Add(featurePtops2[j].Item[i + 1]);
                    }
                    else
                    {
                        featurePtops2[i].Add(featurePtops2[j].Item[featurePtops[0].Count]);
                    }
                }
            }

            for (var i = 0; i < featurePtops.Count; i++)
            {
                try
                {
                    GraphicObjectHelper.SetDefaultAttributes(Document.Curves.AddFromElement(featurePtops[i]) as Esprit.GraphicObject, EspritApplication.Configuration);
                    Document.FeaturePtops.Remove(featurePtops[i].Key);
                }
                catch(Exception)
                {
                    EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeWarning, "CrossSectionSurfaceMeshCurvesTutorial", $"Creating curve [{i}] from featurePtop failed");
                }
            }

            for (var i = 0; i < featurePtops2.Length; i++)
            {
                try
                {
                    GraphicObjectHelper.SetDefaultAttributes(Document.Curves.AddFromElement(featurePtops2[i]) as Esprit.GraphicObject, EspritApplication.Configuration);
                    Document.FeaturePtops.Remove(featurePtops2[i].Key);
                }
                catch (Exception)
                {
                    EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeWarning, "CrossSectionSurfaceMeshCurvesTutorial", $"Creating curve [{i}] from featurePtop failed");
                }
            }

            Document.Refresh();
        }

        //! [Code snippet mesh]

        private void OnCreateXYMeshButtonClick(object sender, EventArgs e)
        {
            CreateXYMesh();
        }

        //! [Code snippet normal]

        private void CreateCrossSectionCurvesNormalToX()
        {
            if (Document.Group.Count == 0)
            {
                System.Windows.MessageBox.Show("Please group points before running this function.", "CrossSectionSurfaceMeshCurvesTutorial", MessageBoxButton.OK);
                return;
            }

            var startX = 0.0;
            var stepX = 1.0;
            var deviation = 0.0;
            if (!RequestUserInput("Enter the starting X coordinate", "Start X", ref startX, true) ||
                !RequestUserInput("Enter the cross section step in X", "Step X", ref stepX, true) ||
                !RequestUserInput("Enter the maximum deviation", "Deviation", ref deviation, true))
            {
                return;
            }

            if (stepX == 0)
            {
                System.Windows.MessageBox.Show("The value of step in x cannot be zero.", "CrossSectionSurfaceMeshCurvesTutorial", MessageBoxButton.OK);
                return;
            }

            Document.OpenUndoTransaction();
            CreateCrossSectionCurves(EspritApplication, EspSectionNormalAxis.espSectionNormalAxisX, startX, stepX, deviation);
            Document.CloseUndoTransaction(true);
            Document.Refresh();
        }

        private void CreateCrossSectionCurvesNormalToY()
        {
            if (Document.Group.Count == 0)
            {
                System.Windows.MessageBox.Show("Please group points before running this function.", "CrossSectionSurfaceMeshCurvesTutorial", MessageBoxButton.OK);
                return;
            }

            var startY = 0.0;
            var stepY = 1.0;
            var deviation = 0.0;
            if (!RequestUserInput("Enter the starting Y coordinate", "Start Y", ref startY, true) ||
                !RequestUserInput("Enter the cross section step in Y", "Step Y", ref stepY, true) ||
                !RequestUserInput("Enter the maximum deviation", "Deviation", ref deviation, true))
            {
                return;
            }

            if (stepY == 0)
            {
                System.Windows.MessageBox.Show("The value of step in y cannot be zero.", "CrossSectionSurfaceMeshCurvesTutorial", MessageBoxButton.OK);
                return;
            }

            Document.OpenUndoTransaction();
            CreateCrossSectionCurves(EspritApplication, EspSectionNormalAxis.espSectionNormalAxisY, startY, stepY, deviation);
            Document.CloseUndoTransaction(true);
            Document.Refresh();
        }

        //! [Code snippet normal]

        private void OnCreateCrossSectionCurvesNormalToXButtonClick(object sender, EventArgs e)
        {
            CreateCrossSectionCurvesNormalToX();
        }

        private void OnCreateCrossSectionCurvesNormalToYButtonClick(object sender, EventArgs e)
        {
            CreateCrossSectionCurvesNormalToY();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _form = null;
        }

        public CrossSectionSurfaceMeshCurvesTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "Creating Cross Section and Surface Mesh Curves";
        public override string HtmlPath => "html/cross_section_surface_mesh_curves_tutorial.html";

    }
}
