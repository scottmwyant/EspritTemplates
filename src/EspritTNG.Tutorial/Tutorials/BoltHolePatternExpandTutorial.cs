using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    //! [Code snippet declare]

    class BoltHolePatternExpandHandler : IDisposable
    {
        private readonly Esprit.Application _espritApplication;
        public double HoleDiameter;
        public double PatternDiameter;
        public double StartAngleRad;
        public int NumberOfHoles;
        public bool IsBoltHoleCreated;
        public Esprit.Point LocalPatternCenter;
        public List<Esprit.Circle> Holes;

        public BoltHolePatternExpandHandler(Esprit.Application app)
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
            StartAngleRad = 0;
            LocalPatternCenter = null;
            Holes = new List<Esprit.Circle>();
        }

        public void Dispose()
        {
            RemoveHoles();
        }

        public void RemoveHoles()
        {
            foreach(var hole in Holes)
            {
                _espritApplication.Document.Circles.Remove(hole.Key);
            }

            Holes.Clear();
            _espritApplication.Document.Refresh();
        }

        public void Move(Esprit.Point point)
        {
            var xMove = point.X - LocalPatternCenter.X;
            var yMove = point.Y - LocalPatternCenter.Y;

            foreach(var hole in Holes)
            {
                hole.X += xMove;
                hole.Y += yMove;
            }

            LocalPatternCenter = point;
            _espritApplication.Document.Refresh();
        }

        public List<Esprit.Circle> DrawPattern(Esprit.Point point)
        {
            RemoveHoles();
            if (point != null)
            {
                LocalPatternCenter = point;
            }

            if (LocalPatternCenter != null)
            {
                for (var i = 0; i < NumberOfHoles; i++)
                {
                    var angle = StartAngleRad + i * 2 * Math.PI / NumberOfHoles;
                    var x = (PatternDiameter / 2) * Math.Cos(angle) + LocalPatternCenter.X;
                    var y = (PatternDiameter / 2) * Math.Sin(angle) + LocalPatternCenter.Y;
                    Holes.Add(_espritApplication.Document.Circles.Add(_espritApplication.Document.GetPoint(x, y, 0), HoleDiameter / 2.0));
                }
            }
            _espritApplication.Document.Refresh();
            return Holes;
        }
    }

    //! [Code snippet declare]

    class BoltHolePatternExpandTutorial : BaseTutorial
    {
        //! [Code snippet use1]

        private BoltHolePatternExpandHandler _boltHolePattern = null;

        //! [Code snippet use1]

        private Form _form;

        private void Show()
        {
            _form = new Form();
            Button createButton = new Button();
            Button moveButton = new Button();
            Button deleteButton = new Button();

            _form.Text = "BoltHolePatternExpandTutorial form";
            createButton.Text = "Create/Update";
            moveButton.Text = "Move";
            deleteButton.Text = "Delete";

            createButton.SetBounds(9, 16, 75, 23);
            moveButton.SetBounds(118, 16, 75, 23);
            deleteButton.SetBounds(200, 16, 75, 23);

            createButton.AutoSize = moveButton.AutoSize = deleteButton.AutoSize = true;
            createButton.Anchor = moveButton.Anchor = deleteButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            _form.ClientSize = new System.Drawing.Size(290, 40);
            _form.Controls.AddRange(new Control[] { createButton, moveButton, deleteButton });
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            createButton.Click += OnCreateButtonClick;
            moveButton.Click += OnMoveButtonClick;
            deleteButton.Click += OnDeleteButtonClick;
            _form.FormClosed += (o, e) => { _form = null; };

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet use]

        private void OnDeleteButtonClick(object sender, EventArgs e)
        {
            if (_boltHolePattern != null)
            {
                _boltHolePattern.Dispose();
                _boltHolePattern = null;
            }
        }

        private void OnMoveButtonClick(object sender, EventArgs e)
        {
            Esprit.Point point = null;
            try
            {
                point = Document.GetPoint("Pick The New Pattern Center");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Point was not set
                return;
            }

            _boltHolePattern?.Move(point);
        }

        private void OnCreateButtonClick(object sender, EventArgs e)
        {
            MakeHole();
        }

        private void MakeHole()
        {
            if (_boltHolePattern == null)
            {
                _boltHolePattern = new BoltHolePatternExpandHandler(EspritApplication);
                _boltHolePattern.IsBoltHoleCreated = false;
            }
            
            var holeDiameter = _boltHolePattern.HoleDiameter;
            var patternDiameter = _boltHolePattern.PatternDiameter;
            var numberOfHoles = _boltHolePattern.NumberOfHoles;
            var startAngle = (_boltHolePattern.StartAngleRad * 180/ Math.PI);
            
            if (RequestUserInput("Enter Hole Diameter", "Hole Diameter", ref holeDiameter) &&
                RequestUserInput("Enter Pattern Diameter", "Pattern Diameter", ref patternDiameter) &&
                RequestUserInput("Enter Number of Holes", "Holes", ref numberOfHoles) &&
                RequestUserInput("Enter New Start Angle in Degrees", "Start Angle", ref startAngle) )
            {
                if (holeDiameter > 0 && patternDiameter > 0 && numberOfHoles > 0)
                {
                    _boltHolePattern.HoleDiameter = holeDiameter;
                    _boltHolePattern.NumberOfHoles = numberOfHoles;
                    _boltHolePattern.StartAngleRad = startAngle * Math.PI / 180;
                    _boltHolePattern.HoleDiameter = holeDiameter;

                    Esprit.Point point = null;

                    if (!_boltHolePattern.IsBoltHoleCreated)
                    {
                        try
                        {
                            point = Document.GetPoint("Pick The Pattern Center");
                        }
                        catch (System.Runtime.InteropServices.COMException)
                        {
                            // Point was not set
                            _boltHolePattern.IsBoltHoleCreated = false;
                            return;
                        }
                        _boltHolePattern.IsBoltHoleCreated = true;
                    }
                    _boltHolePattern.DrawPattern(point);
                }
            }
        }

        //! [Code snippet use]

        public BoltHolePatternExpandTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "Expanding the Bolt Hole Pattern Class";
        public override string HtmlPath => "html/bolt_hole_pattern_expand_tutorial.html";

    }
}
