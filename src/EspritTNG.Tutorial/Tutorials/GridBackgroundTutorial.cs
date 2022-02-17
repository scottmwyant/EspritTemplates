using System;
using System.Windows.Forms;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class GridBackgroundTutorial : BaseTutorial
    {
        private Form _form;
        ScrollBar _rScrollBar;
        ScrollBar _gScrollBar;
        ScrollBar _bScrollBar;
        CheckBox _useGradientCheckBox;

        private void Show()
        {
            _form = new Form();
            _rScrollBar = new VScrollBar();
            _rScrollBar.Minimum = 0;
            _rScrollBar.Maximum = 255;
            _gScrollBar = new VScrollBar();
            _gScrollBar.Minimum = 0;
            _gScrollBar.Maximum = 255;
            _bScrollBar = new VScrollBar();
            _bScrollBar.Minimum = 0;
            _bScrollBar.Maximum = 255;
            _useGradientCheckBox = new CheckBox();

            Button scaleGridButton = new Button();
            scaleGridButton.Text = "Scale Grid";
            _useGradientCheckBox.Text = "Use Gradient";

            _form.Text = "Modify Background";

            var color = ColorHelper.UIntToColor(EspritApplication.Configuration.ConfigurationBackground.Color);
            _rScrollBar.Value = color.R;
            _gScrollBar.Value = color.G;
            _bScrollBar.Value = color.B;

            _useGradientCheckBox.Checked = EspritApplication.Configuration.ConfigurationBackground.Gradient;

            _rScrollBar.SetBounds(10, 16, 23, 100);
            _gScrollBar.SetBounds(50, 16, 23, 100);
            _bScrollBar.SetBounds(90, 16, 23, 100);
            _useGradientCheckBox.SetBounds(9, 140, 120, 23);
            scaleGridButton.SetBounds(40, 170, 75, 23);

            scaleGridButton.AutoSize = _rScrollBar.AutoSize = _gScrollBar.AutoSize = _bScrollBar.AutoSize = true;
            scaleGridButton.Anchor = _useGradientCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            _form.ClientSize = new System.Drawing.Size(160, 200);
            _form.Controls.AddRange(new Control[] { _rScrollBar, _gScrollBar, _bScrollBar, _useGradientCheckBox, scaleGridButton });
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            _rScrollBar.ValueChanged += OnScrollBarValueChanged;
            _gScrollBar.ValueChanged += OnScrollBarValueChanged;
            _bScrollBar.ValueChanged += OnScrollBarValueChanged;
            _useGradientCheckBox.CheckedChanged += OnUseGradientCheckBoxCheckedChanged;
            scaleGridButton.Click += OnButtonGridClick;

            _form.FormClosed += (o, e) => { _form = null; _rScrollBar = null; _gScrollBar = null; _bScrollBar = null; _useGradientCheckBox = null; };

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet value changed]

        private void OnScrollBarValueChanged(object sender, EventArgs e)
        {
            EspritApplication.Configuration.ConfigurationBackground.Color = ColorHelper.ColorToUInt(System.Drawing.Color.FromArgb(_rScrollBar.Value, _gScrollBar.Value, _bScrollBar.Value));
            Document.Refresh();
        }

        //! [Code snippet value changed]

        //! [Code snippet check]

        private void OnUseGradientCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            EspritApplication.Configuration.ConfigurationBackground.Gradient = _useGradientCheckBox.Checked;
            Document.Refresh();
        }

        //! [Code snippet check]

        //! [Code snippet grid]

        private void ScaleGrid()
        {
            var scaleFactor = 1.0;
            if (RequestUserInput("Enter Grid Scale Factor", "Scale Factor", ref scaleFactor))
            {
                EspritApplication.Configuration.EnableGridMode = true;
                EspritApplication.Configuration.GridAngle *= scaleFactor;

                if (EspritApplication.Document.SystemUnit == EspritConstants.espUnitType.espInch)
                {
                    EspritApplication.Configuration.GridDxInch *= scaleFactor;
                    EspritApplication.Configuration.GridDyInch *= scaleFactor;
                    EspritApplication.Configuration.GridDzInch *= scaleFactor;
                    EspritApplication.Configuration.GridRadiusInch *= scaleFactor;
                }
                if (EspritApplication.Document.SystemUnit == EspritConstants.espUnitType.espMetric)
                {
                    EspritApplication.Configuration.GridDxMetric *= scaleFactor;
                    EspritApplication.Configuration.GridDyMetric *= scaleFactor;
                    EspritApplication.Configuration.GridDzMetric *= scaleFactor;
                    EspritApplication.Configuration.GridRadiusMetric *= scaleFactor;
                }
            }
        }

        //! [Code snippet grid]

        private void OnButtonGridClick(object sender, EventArgs e)
        {
            ScaleGrid();
        }

        public GridBackgroundTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "More Configuration: Grid and Background";
        public override string HtmlPath => "html/grid_background_tutorial.html";

    }
}
