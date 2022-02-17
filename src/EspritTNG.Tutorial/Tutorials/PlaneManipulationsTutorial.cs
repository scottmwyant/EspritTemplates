using System;
using System.Collections.Generic;
using System.Windows.Forms;

using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{

    class PlaneManipulationsTutorial : BaseTutorial
    {
        public PlaneManipulationsTutorial(Esprit.Application app): base(app)
        {
        }

        private Form _form;

        public void Show()
        {
            _form = new Form();

            Button copyToPoint = new Button();
            Button rotatePlus = new Button();
            Button rotateMinus = new Button();
            Button flipUVW = new Button();
            Button flipWVU = new Button();
            Button copy = new Button();

            _form.Text = "PlaneManipulationsTutorial form";
            copyToPoint.Text = "Copy plane to point";
            rotatePlus.Text = "Rotate +90";
            rotateMinus.Text = "Rotate -90";
            flipUVW.Text = "Flip U V W";
            flipWVU.Text = "Flip W V U";
            copy.Text = "Copy planes around Z";

            copyToPoint.SetBounds(20, 10, 150, 20);
            rotatePlus.SetBounds(20, 40, 150, 20);
            rotateMinus.SetBounds(20, 70, 150, 20);
            flipUVW.SetBounds(20, 100, 150, 20);
            flipWVU.SetBounds(20, 130, 150, 20);
            copy.SetBounds(20, 160, 150, 20);

            _form.ClientSize = new System.Drawing.Size(210, 190);
            _form.Controls.AddRange(new Control[] { copyToPoint, rotatePlus, rotateMinus, flipUVW, flipWVU, copy });
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            copyToPoint.Click += OnCopyToPointButtonClick;
            rotatePlus.Click += OnRotatePlusClick;
            rotateMinus.Click += OnRotateMinusButtonClick;
            flipUVW.Click += OnFlipUVWButtonClick;
            flipWVU.Click += OnFlipWVUButtonClick;
            copy.Click += OnCopyButtonClick;

            _form.FormClosed += (o, e) => { _form = null; };

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        private void OnCopyToPointButtonClick(object sender, EventArgs e)
        {
            CopyWorkPlaneToPointDemo();
        }
        private void OnRotatePlusClick(object sender, EventArgs e)
        {
            RotateActivePlanePlus90();
        }

        private void OnRotateMinusButtonClick(object sender, EventArgs e)
        {
            RotateActivePlaneMinus90();
        }

        private void OnFlipUVWButtonClick(object sender, EventArgs e)
        {
            FlipPlaneUtoVtoW();
        }

        private void OnFlipWVUButtonClick(object sender, EventArgs e)
        {
            FlipPlaneWtoVtoU();
        }

        private void OnCopyButtonClick(object sender, EventArgs e)
        {
            CopyWorkPlanesAroundZ();
        }

        //! [Code snippet]

        private void CopyWorkPlaneToPointDemo()
        {
            for (var i = 1; i <= Document.Group.Count; i++)
            {
                var graphicObject = Document.Group[i] as Esprit.IGraphicObject;
                if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espPoint)
                {
                    PlaneHelper.CopyWorkPlaneToPoint(Document.Planes, Document.ActivePlane, graphicObject as Esprit.Point);
                }
            }
        }

        //! [Code snippet]

        //! [Code snippet rotateW]

        private void RotateActivePlanePlus90()
        {
            Document.ActivePlane.RotateUVW(0, 0, 90 * Math.PI / 180);
            Document.ActivePlane.Activate();
            Document.Refresh();
        }

        private void RotateActivePlaneMinus90()
        {
            Document.ActivePlane.RotateUVW(0, 0, -90 * Math.PI / 180);
            Document.ActivePlane.Activate();
            Document.Refresh();
        }

        //! [Code snippet rotateW]


        //! [Code snippet flip]

        private void FlipPlaneUtoVtoW()
        {
            Document.ActivePlane.RotateUVW(0, 0, 90 * Math.PI / 180);
            Document.ActivePlane.RotateUVW(90 * Math.PI / 180, 0, 0);
            Document.ActivePlane.Activate();
            Document.Refresh();
        }

        private void FlipPlaneWtoVtoU()
        {
            Document.ActivePlane.RotateUVW(-90 * Math.PI / 180, 0, 0);
            Document.ActivePlane.RotateUVW(0, 0, -90 * Math.PI / 180);
            Document.ActivePlane.Activate();
            Document.Refresh();
        }

        //! [Code snippet flip]

        //! [Code snippet copy]

        private void CopyWorkPlanesAroundZ()
        {
            Esprit.Plane [] planes = new Esprit.Plane [3];

            foreach (Esprit.Plane plane in Document.Planes)
            {
                if (plane.IsWork)
                {
                    if (plane.Name != "XYZ" && plane.Name != "ZXY" && plane.Name != "YZX")
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            planes[i] = PlaneHelper.AddUniquePlane(Document.Planes, plane.Name + " Copy ");

                            planes[i].IsWork = true;
                            planes[i].Ux = plane.Ux;
                            planes[i].Uy = plane.Uy;
                            planes[i].Uz = plane.Uz;
                            planes[i].Vx = plane.Vx;
                            planes[i].Vy = plane.Vy;
                            planes[i].Vz = plane.Vz;
                            planes[i].Wx = plane.Wx;
                            planes[i].Wy = plane.Wy;
                            planes[i].Wz = plane.Wz;
                            planes[i].X = plane.X;
                            planes[i].Y = plane.Y;
                            planes[i].Z = plane.Z;

                            planes[i].Rotate(Document.GetLine(Document.GetPoint(0, 0, 0), 0, 0, 1), i * 90 * Math.PI / 180);
                        }
                    }
                }
            }
        }

        //! [Code snippet copy]

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "More Work Plane Manipulations";
        public override string HtmlPath => "html/plane_manipulations_tutorial.html";

    }
}
