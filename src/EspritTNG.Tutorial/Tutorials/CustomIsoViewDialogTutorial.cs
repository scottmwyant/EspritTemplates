using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{

    class CustomIsoViewDialogTutorial : BaseTutorial
    {
        private Form _form;
        private CheckBox tglXYZMinus;
        private CheckBox tglXYZPlus;
        private CheckBox tglYZXMinus;
        private CheckBox tglYZXPlus;
        private CheckBox tglZXYMinus;
        private CheckBox tglZXYPlus;

        private CheckBox tglQuadrant1;
        private CheckBox tglQuadrant2;
        private CheckBox tglQuadrant3;
        private CheckBox tglQuadrant4;

        private bool _ignoreChanges;
        private int _quadrant;
        private int _orientation;
        private Esprit.Plane _plane;
        private List<Esprit.Line> _lines = new List<Esprit.Line>();

        public void Show()
        {
            _form = new Form();
            var left = new GroupBox();
            var right = new GroupBox();
            tglXYZMinus = new CheckBox();
            tglXYZMinus.Appearance = Appearance.Button;
            tglXYZPlus = new CheckBox();
            tglXYZPlus.Appearance = Appearance.Button;
            tglYZXMinus = new CheckBox();
            tglYZXMinus.Appearance = Appearance.Button;
            tglYZXPlus = new CheckBox();
            tglYZXPlus.Appearance = Appearance.Button;
            tglZXYMinus = new CheckBox();
            tglZXYMinus.Appearance = Appearance.Button;
            tglZXYPlus = new CheckBox();
            tglZXYPlus.Appearance = Appearance.Button;

            tglQuadrant1 = new CheckBox();
            tglQuadrant1.Appearance = Appearance.Button;
            tglQuadrant2 = new CheckBox();
            tglQuadrant2.Appearance = Appearance.Button;
            tglQuadrant3 = new CheckBox();
            tglQuadrant3.Appearance = Appearance.Button;
            tglQuadrant4 = new CheckBox();
            tglQuadrant4.Appearance = Appearance.Button;

            _form.Text = "CustomIsoViewDialogTutorial";

            tglXYZMinus.Text = "XYZ-";
            tglXYZPlus.Text = "XYZ+";
            tglYZXMinus.Text = "YZX-";
            tglYZXPlus.Text = "YZX+";
            tglZXYMinus.Text = "ZXY-";
            tglZXYPlus.Text = "ZXY+";

            tglQuadrant1.Text = "1";
            tglQuadrant2.Text = "2";
            tglQuadrant3.Text = "3";
            tglQuadrant4.Text = "4";

            left.Text = "Orientation";
            right.Text = "Ouadrant";

            left.SetBounds(10, 10, 120, 90);

            tglXYZPlus.SetBounds(20, 30, 50, 20);
            tglXYZMinus.SetBounds(72, 30, 50, 20);
            tglZXYPlus.SetBounds(20, 52, 50, 20);
            tglZXYMinus.SetBounds(72, 52, 50, 20);
            tglYZXPlus.SetBounds(20, 74, 50, 20);
            tglYZXMinus.SetBounds(72, 74, 50, 20);

            right.SetBounds(150, 10, 120, 90);

            tglQuadrant1.SetBounds(212, 30, 50, 30);
            tglQuadrant2.SetBounds(160, 30, 50, 30);
            tglQuadrant3.SetBounds(160, 62, 50, 30);
            tglQuadrant4.SetBounds(212, 62, 50, 30);

            _form.ClientSize = new System.Drawing.Size(300, 110);
            _form.Controls.AddRange(new Control[] { tglZXYPlus, tglZXYMinus, tglYZXPlus, tglXYZMinus, tglXYZPlus, tglYZXMinus, left,
            tglQuadrant1, tglQuadrant2, tglQuadrant3, tglQuadrant4, right });

            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            //! [Code snippet plane]

            _plane = PlaneHelper.GetPlane(Document, "Custom Iso View");

            if (_plane == null)
            {
                _form.Close();
            }

            _plane.IsWork = true;
            _plane.X = _plane.Y = _plane.Z = 0;
            var point = Document.GetPoint(0, 0, 0);
            _lines.Add(Document.GetLine(point, 1, 0, 0));
            _lines.Add(Document.GetLine(point, 0, 1, 0));
            _lines.Add(Document.GetLine(point, 0, 0, 1));

            //! [Code snippet plane]

            _ignoreChanges = false;

            _orientation = _quadrant = 0;

            _form.FormClosed += (o, e) => { _form = null; };
            tglQuadrant1.Click += OnTglQuadrant1Click;
            tglQuadrant2.Click += OnTglQuadrant2Click;
            tglQuadrant3.Click += OnTglQuadrant3Click;
            tglQuadrant4.Click += OnTglQuadrant4Click;

            tglXYZMinus.Click += OnTglXYZMinusClick;
            tglXYZPlus.Click += OnTglXYZPlusClick;
            tglYZXMinus.Click += OnTglYZXMinusClick;
            tglYZXPlus.Click += OnTglYZXPlusClick;
            tglZXYMinus.Click += OnTglZXYMinusClick;
            tglZXYPlus.Click += OnTglZXYPlusClick;

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet]

        private void SetPlane()
        {
            _ignoreChanges = false;

            if (_orientation == 0 || _quadrant == 0)
            {
                return;
            }

            switch (Math.Abs(_orientation))
            {
                case 1:
                    _plane.Ux = 0;
                    _plane.Uy = 0.70711;
                    _plane.Uz = -0.70711;
                    _plane.Vx = 0.8165;
                    _plane.Vy = 0.40825;
                    _plane.Vz = 0.40825;
                    _plane.Wx = 0.57735;
                    _plane.Wy = -0.57735;
                    _plane.Wz = -0.57735;
                    break;
                case 2:
                    _plane.Ux = -0.70711;
                    _plane.Uy = 0;
                    _plane.Uz = 0.70711;
                    _plane.Vx = 0.40825;
                    _plane.Vy = 0.8165;
                    _plane.Vz = 0.40825;
                    _plane.Wx = -0.57735;
                    _plane.Wy = 0.57735;
                    _plane.Wz = -0.57735;
                    break;
                case 3:
                    _plane.Ux = 0.70711;
                    _plane.Uy = -0.70711;
                    _plane.Uz = 0;
                    _plane.Vx = 0.40825;
                    _plane.Vy = 0.40825;
                    _plane.Vz = 0.8165;
                    _plane.Wx = -0.57735;
                    _plane.Wy = -0.57735;
                    _plane.Wz = 0.57735;
                    break;
            }

            switch (Math.Sign(_orientation))
            {
                case 1:
                    switch (_quadrant)
                    {
                        case 1:
                            _plane.Rotate(_lines[Math.Abs(_orientation)-1], Math.PI);
                            break;
                        case 2:
                            _plane.Rotate(_lines[Math.Abs(_orientation) - 1], -Math.PI/2);
                            break;
                        case 4:
                            _plane.Rotate(_lines[Math.Abs(_orientation) - 1], Math.PI / 2);
                            break;
                    }
                    break;
                case -1:
                    _plane.RotateUVW(Math.PI, 0, 0);
                    switch (_quadrant)
                    {
                        case 1:
                            _plane.Rotate(_lines[Math.Abs(_orientation) - 1], Math.PI / 2);
                            break;
                        case 2:
                            _plane.Rotate(_lines[Math.Abs(_orientation) - 1], Math.PI);
                            break;
                        case 4:
                            _plane.Rotate(_lines[Math.Abs(_orientation) - 1], -Math.PI / 2);
                            break;
                    }
                    break;
            }

            _plane.Activate();
            Document.Refresh();
        }

        private void OnTglXYZMinusClick(object sender, EventArgs e)
        {
            if (!_ignoreChanges)
            {
                _orientation = -3;
                SwitchOrientation(sender as CheckBox);
            }
        }

        private void OnTglXYZPlusClick(object sender, EventArgs e)
        {
            if (!_ignoreChanges)
            {
                _orientation = 3;
                SwitchOrientation(sender as CheckBox);
            }
        }

        private void OnTglYZXMinusClick(object sender, EventArgs e)
        {
            if (!_ignoreChanges)
            {
                _orientation = -1;
                SwitchOrientation(sender as CheckBox);
            }
        }

        private void OnTglYZXPlusClick(object sender, EventArgs e)
        {
            if (!_ignoreChanges)
            {
                _orientation = 1;
                SwitchOrientation(sender as CheckBox);
            }
        }

        private void OnTglZXYMinusClick(object sender, EventArgs e)
        {
            if (!_ignoreChanges)
            {
                _orientation = -2;
                SwitchOrientation(sender as CheckBox);
            }
        }

        private void OnTglZXYPlusClick(object sender, EventArgs e)
        {
            if (!_ignoreChanges)
            {
                _orientation = 2;
                SwitchOrientation(sender as CheckBox);
            }
        }

        private void SwitchQuadrant(CheckBox button)
        {
            _ignoreChanges = true;
            tglQuadrant1.Checked = false;
            tglQuadrant2.Checked = false;
            tglQuadrant3.Checked = false;
            tglQuadrant4.Checked = false;

            button.Checked = true;
            SetPlane();
        }

        private void SwitchOrientation(CheckBox button)
        {
            _ignoreChanges = true;
            tglXYZMinus.Checked = false;
            tglXYZPlus.Checked = false;
            tglYZXMinus.Checked = false;
            tglYZXPlus.Checked = false;
            tglZXYMinus.Checked = false;
            tglZXYPlus.Checked = false;

            button.Checked = true;
            SetPlane();
        }

        private void OnTglQuadrant1Click(object sender, EventArgs e)
        {
            if (!_ignoreChanges)
            {
                _quadrant = 1;
                SwitchQuadrant(sender as CheckBox);
            }
        }

        private void OnTglQuadrant2Click(object sender, EventArgs e)
        {
            if (!_ignoreChanges)
            {
                _quadrant = 2;
                SwitchQuadrant(sender as CheckBox);
            }
        }

        private void OnTglQuadrant3Click(object sender, EventArgs e)
        {
            if (!_ignoreChanges)
            {
                _quadrant = 3;
                SwitchQuadrant(sender as CheckBox);
            }
        }
        private void OnTglQuadrant4Click(object sender, EventArgs e)
        {
            if (!_ignoreChanges)
            {
                _quadrant = 4;
                SwitchQuadrant(sender as CheckBox);
            }
        }

        //! [Code snippet]

        public CustomIsoViewDialogTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "Custom Iso View Dialog";
        public override string HtmlPath => "html/custom_iso_view_dialog_tutorial.html";

    }
}
