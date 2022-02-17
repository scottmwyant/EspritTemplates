using System;
using System.Windows.Forms;
using Microsoft.Win32;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class RegistryValuesWithUserFormsTutorial : BaseTutorial
    {
        private Form _form;
        private TextBox _txtHoleRadius;
        private TextBox _txtSpacing;
        private TextBox _txtStartAngle;

        private void Show()
        {
            _form = new Form();
            Label lblHoleRadius = new Label();
            _txtHoleRadius = new TextBox();
            Label lblSpacing = new Label();
            _txtSpacing = new TextBox();
            Label lblStartAngle = new Label();
            _txtStartAngle = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            _form.Text = "RegistryValuesWithUserFormsTutorial form";
            lblHoleRadius.Text = "Hole Radius";
            lblSpacing.Text = "Spacing";
            lblStartAngle.Text = "Start Angle";

            //! [Code snippet init]

            var scaleFactor = (Document.SystemUnit == EspritConstants.espUnitType.espMetric)
                ? 25.4
                : 1;

            var reg = RegistryHelper.GetRegistry();
            _txtHoleRadius.Text = reg.GetValue("Hole Radius", 0.5 * scaleFactor).ToString();
            _txtSpacing.Text = reg.GetValue("Spacing", 0.1 * scaleFactor).ToString();
            _txtStartAngle.Text = reg.GetValue("Start Angle", 0).ToString();

            //! [Code snippet init]

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            lblHoleRadius.SetBounds(9, 16, 372, 13);
            _txtHoleRadius.SetBounds(12, 36, 372, 20);
            lblSpacing.SetBounds(9, 60, 372, 13);
            _txtSpacing.SetBounds(12, 80, 372, 20);
            lblStartAngle.SetBounds(9, 106, 372, 13);
            _txtStartAngle.SetBounds(12, 128, 372, 20);
            buttonOk.SetBounds(229, 222, 75, 23);
            buttonCancel.SetBounds(309, 222, 75, 23);

            _txtStartAngle.AutoSize = _txtSpacing.AutoSize = _txtHoleRadius.AutoSize = lblStartAngle.AutoSize = lblHoleRadius.AutoSize = lblSpacing.AutoSize = true;
            buttonOk.Anchor = buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            _form.ClientSize = new System.Drawing.Size(396, 270);
            _form.Controls.AddRange(new Control[] { _txtStartAngle, _txtSpacing, _txtHoleRadius, lblStartAngle, lblHoleRadius, lblSpacing, buttonOk, buttonCancel });
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            buttonOk.Click += OnOkButtonClick;
            buttonCancel.Click += OnCancelButtonClick;

            //! [Code snippet close]

            _form.FormClosed += (o, e) => {
                reg.SetValue("Spacing", _txtSpacing.Text);
                reg.SetValue("Hole Radius", _txtHoleRadius.Text);
                reg.SetValue("Start Angle", _txtStartAngle.Text);

                _form = null;
            };

            //! [Code snippet close]

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet ok]

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            Esprit.Circle circle = null;

            _form.Hide();

            try
            {
                circle = Document.GetCircle("Pick BoundaryCircle");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                _form.Show();
                return;
            }

            double.TryParse(_txtHoleRadius.Text, out var holeRadius);
            double.TryParse(_txtSpacing.Text, out var spacing);
            int.TryParse(_txtStartAngle.Text, out var startAngle);

            if (circle != null)
            {
                FeatureHelper.ChainCircles(
                    Document,
                    FeatureHelper.MoldHolePattern(Document, circle.CenterPoint, circle.Radius, holeRadius, spacing), startAngle * Math.PI / 180
                );
            }

            Document.Refresh();

            CloseForm();
        }

        //! [Code snippet ok]

        //! [Code snippet cancel]

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            CloseForm();
        }

        void CloseForm()
        {
            _form.Close();
            _form = null;
        }

        //! [Code snippet cancel]

        public RegistryValuesWithUserFormsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet execute]

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        //! [Code snippet execute]

        public override string Name => "Using Registry Values with UserForms";
        public override string HtmlPath => "html/registry_values_with_userforms_tutorial.html";

    }
}
