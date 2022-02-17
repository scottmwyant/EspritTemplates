using System;
using System.Windows.Forms;
using Microsoft.Win32;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class SaveValuesToRegistryTutorial : BaseTutorial
    {
        private Form _form;

        private void Show()
        {
            _form = new Form();
            Button createBoltHoleButton = new Button();
            Button createChainCirclesButton = new Button();

            _form.Text = "SaveValuesToRegistryTutorial form";
            createBoltHoleButton.Text = "Bolt Hole";
            createChainCirclesButton.Text = "Chain Circles";

            createBoltHoleButton.SetBounds(9, 16, 75, 23);
            createChainCirclesButton.SetBounds(89, 16, 75, 23);

            createChainCirclesButton.AutoSize = createBoltHoleButton.AutoSize = true;
            createChainCirclesButton.Anchor = createBoltHoleButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            _form.ClientSize = new System.Drawing.Size(190, 40);
            _form.Controls.AddRange(new Control[] { createBoltHoleButton, createChainCirclesButton,  });
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            createBoltHoleButton.Click += OnCreateBoltHoleButtonClick;
            createChainCirclesButton.Click += OnCreateChainCirclesButtonClick;

            _form.FormClosed += (o, e) => { _form = null; };

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet bolthole demo]

        private void BoltHoleDemoWithDefaults()
        {
            var reg = RegistryHelper.GetRegistry();
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
                double.TryParse(reg.GetValue("Pattern Radius", 10).ToString(), out var patternRadius);
                double.TryParse(reg.GetValue("Hole Radius", 1).ToString(), out var holeRadius);
                int.TryParse(reg.GetValue("Number of Holes", 6).ToString(), out var numberOfHoles);

                if (RequestUserInput("Enter Pattern Radius", "Pattern Radius", ref patternRadius) &&
                    RequestUserInput("Enter Hole Radius", "Hole Radius", ref holeRadius) &&
                    RequestUserInput("Enter Number of Holes", "Number of Holes", ref numberOfHoles))
                {
                    FeatureHelper.BoltHolePattern(Document, point, patternRadius, holeRadius, numberOfHoles);
                }

                reg.SetValue("Pattern Radius", patternRadius);
                reg.SetValue("Hole Radius", holeRadius);
                reg.SetValue("Number of Holes", numberOfHoles);
            }

            Document.Refresh();

            _form.Show();

        }

        //! [Code snippet bolthole demo]

        private void OnCreateBoltHoleButtonClick(object sender, EventArgs e)
        {
            BoltHoleDemoWithDefaults();
        }

        //! [Code snippet chain demo]

        private void ChainCirclesDemoWithDefaults()
        {
            var reg = RegistryHelper.GetRegistry();

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
                var spacing = double.Parse(reg.GetValue("Spacing", circle.Radius / 100).ToString());
                var holeRadius = double.Parse(reg.GetValue("Hole Radius", circle.Radius / 10).ToString());
                var startAngle = double.Parse(reg.GetValue("Start Angle", 0).ToString());

                if (RequestUserInput("Enter Hole Radius", "Hole Radius", ref holeRadius) &&
                    RequestUserInput("Enter Spacing", "Spacing", ref spacing) &&
                    RequestUserInput("Enter Start Angle", "Start Angle", ref startAngle))
                {
                    FeatureHelper.ChainCircles(
                        Document,
                        FeatureHelper.MoldHolePattern(Document, circle.CenterPoint, circle.Radius, holeRadius, spacing),
                        startAngle * Math.PI / 180
                    );
                }

                reg.SetValue("Spacing", spacing);
                reg.SetValue("Hole Radius", holeRadius);
                reg.SetValue("Start Angle", startAngle);
            }

            Document.Refresh();

            _form.Show();

        }

        //! [Code snippet chain demo]

        private void OnCreateChainCirclesButtonClick(object sender, EventArgs e)
        {
            ChainCirclesDemoWithDefaults();
        }

        public SaveValuesToRegistryTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "Saving Values to the Registry";
        public override string HtmlPath => "html/save_values_to_registry_tutorial.html";

    }
}
