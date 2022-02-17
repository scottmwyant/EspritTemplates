using System;
using System.Windows.Forms;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class ConfigurationObjectTutorial : BaseTutorial
    {
        private Form _form;

        private void Show()
        {
            _form = new Form();
            Button createBoltHoleButton = new Button();
            Button createChainCirclesButton = new Button();

            _form.Text = "ConfigurationObjectTutorial form";
            createBoltHoleButton.Text = "Bolt Hole";
            createChainCirclesButton.Text = "Mold Hole";

            createBoltHoleButton.SetBounds(9, 16, 75, 23);
            createChainCirclesButton.SetBounds(89, 16, 75, 23);

            createChainCirclesButton.AutoSize = createBoltHoleButton.AutoSize = true;
            createChainCirclesButton.Anchor = createBoltHoleButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            _form.ClientSize = new System.Drawing.Size(175, 50);
            _form.Controls.AddRange(new Control[] { createBoltHoleButton, createChainCirclesButton,  });
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            createBoltHoleButton.Click += BoltHoleImprovedDemo;
            createChainCirclesButton.Click += ChainCirclesImprovedDemo;

            _form.FormClosed += (o, e) => { _form = null; };

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet chain improved demo]

        private void ChainCirclesImprovedDemo(object sender, EventArgs e)
        {
            Esprit.Circle circle = null;

            _form.Hide();

            try
            {
                circle = Document.GetCircle("Pick Boundary Circle");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
            }

            if (circle != null)
            {
                var spacing = circle.Radius / 100.0;
                var holeRadius = circle.Radius / 10.0;
                var startAngle = 0.0;

                if (RequestUserInput("Enter Hole Radius", "Hole Radius", ref holeRadius) &&
                    RequestUserInput("Enter Spacing", "Spacing", ref spacing) &&
                    RequestUserInput("Enter Start Angle", "Start Angle", ref startAngle))
                {
                    try
                    {
                        var v = SelectionSetHelper.DefaultAttributesSet(FeatureHelper.MoldHolePattern(Document, circle.CenterPoint, circle.Radius, holeRadius, spacing), EspritApplication.Configuration);
                        SelectionSetHelper.DefaultAttributesSet(FeatureHelper.ChainCircles(Document, v, startAngle * Math.PI / 180), EspritApplication.Configuration);
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                    }
                }
            }

            _form.Show();

            Document.Refresh();
        }

        //! [Code snippet chain improved demo]

        //! [Code snippet bolthole improved demo]

        private void BoltHoleImprovedDemo(object sender, EventArgs e)
        {
            Esprit.Point point = null;

            _form.Hide();

            try
            {
                point = Document.GetPoint("Select Bolt Hole Center");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
            }

            if (point != null)
            {
                var patternRadius = 10.0;
                var holeRadius = 1.0;
                var numberOfHoles = 6;

                if (RequestUserInput("Enter Pattern Radius", "Pattern Radius", ref patternRadius) &&
                    RequestUserInput("Enter Hole Radius", "Hole Radius", ref holeRadius) &&
                    RequestUserInput("Enter Number of Holes", "Number of Holes", ref numberOfHoles))
                {
                    FeatureHelper.BoltHoleImproved(Document, EspritApplication.Configuration, point, patternRadius, holeRadius, numberOfHoles);
                }
            }

            _form.Show();

            Document.Refresh();
        }

        //! [Code snippet bolthole improved demo]

        public ConfigurationObjectTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "Introduction to the Configuration Object";
        public override string HtmlPath => "html/configuration_objects_tutorial.html";

    }
}
