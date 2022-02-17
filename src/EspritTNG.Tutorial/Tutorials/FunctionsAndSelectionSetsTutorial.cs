using System;
using System.Windows.Forms;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class FunctionsAndSelectionSetsTutorial : BaseTutorial
    {
        private Form _form;

        private void Show()
        {
            _form = new Form();
            Button createBoltHoleButton = new Button();
            Button createMoldHoleButton = new Button();
            Button createChainCirclesButton = new Button();

            _form.Text = "FunctionsAndSelectionSetsTutorial form";
            createBoltHoleButton.Text = "Bolt Hole";
            createMoldHoleButton.Text = "Mold Hole";
            createChainCirclesButton.Text = "Chain Circles";

            createBoltHoleButton.SetBounds(9, 16, 75, 23);
            createMoldHoleButton.SetBounds(89, 16, 75, 23);
            createChainCirclesButton.SetBounds(169, 16, 75, 23);

            createChainCirclesButton.AutoSize = createMoldHoleButton.AutoSize = createBoltHoleButton.AutoSize = true;
            createChainCirclesButton.Anchor = createMoldHoleButton.Anchor = createBoltHoleButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            _form.ClientSize = new System.Drawing.Size(270, 40);
            _form.Controls.AddRange(new Control[] { createBoltHoleButton, createMoldHoleButton, createChainCirclesButton });
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            createBoltHoleButton.Click += OnCreateBoltHoleButtonClick;
            createMoldHoleButton.Click += OnCreateMoldHoleButtonClick;
            createChainCirclesButton.Click += OnCreateChainCirclesButtonClick;
            _form.FormClosed += (o, e) => { _form = null; };

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet chain demo]

        private void ChainCirclesDemo()
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
                var holeRadius = circle.Radius / 10.0;
                var spacing = circle.Radius / 100.0;

                if (RequestUserInput("Enter Hole Radius", "Hole Radius", ref holeRadius) &&
                    RequestUserInput("Enter Spacing", "Spacing", ref spacing))
                {
                    var pattern = FeatureHelper.MoldHolePattern(Document, circle.CenterPoint, circle.Radius, holeRadius, spacing);
                    FeatureHelper.ChainCircles(Document, pattern, 0);
                }
            }

            Document.Refresh();

            _form.Show();
        }

        //! [Code snippet chain demo]

        private void OnCreateChainCirclesButtonClick(object sender, EventArgs e)
        {
            ChainCirclesDemo();
        }

        //! [Code snippet mold demo]

        private void MoldHoleDemo()
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
            }

            if (circle != null)
            {
                var holeRadius = circle.Radius / 10.0;
                var spacing = circle.Radius / 100.0;

                if (RequestUserInput("Enter Hole Radius", "Hole Radius", ref holeRadius) &&
                    RequestUserInput("Enter Spacing", "Spacing", ref spacing))
                {
                    FeatureHelper.MoldHolePattern(Document, circle.CenterPoint, circle.Radius, holeRadius, spacing);
                }
            }

            Document.Refresh();

            _form.Show();
        }

        //! [Code snippet mold demo]

        private void OnCreateMoldHoleButtonClick(object sender, EventArgs e)
        {
            MoldHoleDemo();
        }

        //! [Code snippet bolthole demo]

        private void BoltHoleDemo()
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
                    FeatureHelper.BoltHolePattern(Document, point, patternRadius, holeRadius, numberOfHoles);
                }
            }

            Document.Refresh();

            _form.Show();
        }

        //! [Code snippet bolthole demo]

        private void OnCreateBoltHoleButtonClick(object sender, EventArgs e)
        {
            BoltHoleDemo();
        }

        public FunctionsAndSelectionSetsTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "More Fun With Functions and SelectionSets";
        public override string HtmlPath => "html/functions_and_selectionsets_tutorial.html";

    }
}
