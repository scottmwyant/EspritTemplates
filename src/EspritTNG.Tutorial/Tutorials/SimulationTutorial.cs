using System;
using TutorialCSharp.Tutorials.Helpers;
using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    class SimulationTutorial : BaseTutorial
    {
        public SimulationTutorial(Esprit.Application app): base(app)
        {
        }

        private Form _form;
        private CheckBox _collisionsCheckBox;
        private CheckBox _ncCoordinateSystemCheckBox;
        private CheckBox _partViolationCheckBox;
        private CheckBox _stopCodeCheckBox;
        private CheckBox _traceToolpathCheckBox;
        private TrackBar _speedFeedTrackBar;
        private TrackBar _speedRapidTrackBar;

        private void Show()
        {
            _form = new Form();
            var playButton = new Button();
            var stopButton = new Button();
            var pauseButton = new Button();
            var stepButton = new Button();

            var changeFeed = new Label();
            var changeRapid = new Label();

            _speedFeedTrackBar = new TrackBar();
            _speedRapidTrackBar = new TrackBar();

            _collisionsCheckBox = new CheckBox();
            _ncCoordinateSystemCheckBox = new CheckBox();
            _partViolationCheckBox = new CheckBox();
            _stopCodeCheckBox = new CheckBox();
            _traceToolpathCheckBox = new CheckBox();

            _form.Text = "Simulation Tutorial";

            playButton.Text = "Play";
            stopButton.Text = "Stop";
            pauseButton.Text = "Pause";
            stepButton.Text = "Step";

            _collisionsCheckBox.Text = "Detect collisions";
            _ncCoordinateSystemCheckBox.Text = "NC coordinate system";
            _partViolationCheckBox.Text = "Part violation";
            _stopCodeCheckBox.Text = "Stop code";
            _traceToolpathCheckBox.Text = "Trace toolpath";
            changeFeed.Text = "Change play Feed speed:";
            changeRapid.Text = "Change play Rapid speed:";
            changeFeed.Visible = true;
            changeRapid.Visible = true;

            _speedFeedTrackBar.Minimum = -200;
            _speedFeedTrackBar.Maximum = 800;
            _speedFeedTrackBar.SmallChange = 10;
            _speedFeedTrackBar.TickFrequency = 1000;

            _speedRapidTrackBar.Minimum = -800;
            _speedRapidTrackBar.Maximum = 200;
            _speedRapidTrackBar.SmallChange = 10;
            _speedRapidTrackBar.TickFrequency = 1000;

            stepButton.AutoSize = changeRapid.AutoSize = changeFeed.AutoSize = _collisionsCheckBox.AutoSize = _stopCodeCheckBox.AutoSize =
                _ncCoordinateSystemCheckBox.AutoSize = _partViolationCheckBox.AutoSize = _traceToolpathCheckBox.AutoSize =
                _collisionsCheckBox.AutoSize = playButton.AutoSize = stopButton.AutoSize = pauseButton.AutoSize = true;
            stepButton.Anchor = changeRapid.Anchor = changeFeed.Anchor = _speedRapidTrackBar.Anchor = _speedFeedTrackBar.Anchor =
                _ncCoordinateSystemCheckBox.Anchor = _partViolationCheckBox.Anchor = _stopCodeCheckBox.Anchor = _traceToolpathCheckBox.Anchor =
                _collisionsCheckBox.Anchor = playButton.Anchor = stopButton.Anchor = pauseButton.Anchor = AnchorStyles.Left;

            playButton.SetBounds(9, 16, 50, 23);
            pauseButton.SetBounds(79, 16, 50, 23);
            stopButton.SetBounds(149, 16, 50, 23);
            stepButton.SetBounds(219, 16, 50, 23);

            _collisionsCheckBox.SetBounds(9, 70, 50, 13);
            _ncCoordinateSystemCheckBox.SetBounds(9, 90, 50, 13);
            _partViolationCheckBox.SetBounds(9, 110, 50, 13);
            _traceToolpathCheckBox.SetBounds(9, 130, 50, 13);
            _stopCodeCheckBox.SetBounds(9, 150, 50, 13);

            changeFeed.SetBounds(9, 180, 150, 13);
            _speedFeedTrackBar.SetBounds(9, 210, 150, 13);

            changeRapid.SetBounds(9, 260, 150, 13);
            _speedRapidTrackBar.SetBounds(9, 280, 150, 13);

            _form.ClientSize = new System.Drawing.Size(160, 320);
            _form.Controls.AddRange(new Control[] { playButton, stopButton, pauseButton, stepButton, _speedFeedTrackBar, _speedRapidTrackBar, _collisionsCheckBox,
                _ncCoordinateSystemCheckBox, _partViolationCheckBox, _traceToolpathCheckBox, _stopCodeCheckBox, changeFeed, changeRapid });
            _form.ClientSize = new System.Drawing.Size(Math.Max(300, stopButton.Right + 10), _form.ClientSize.Height);
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            _form.FormClosed += OnFormClosed;
            playButton.Click += OnPlayButtonClick;
            pauseButton.Click += OnPauseButtonClick;
            stopButton.Click += OnStopButtonClick;
            stepButton.Click += OnStepButtonClick;

            _collisionsCheckBox.Checked = Document.Simulation.CollisionDetection;
            _ncCoordinateSystemCheckBox.Checked = Document.Simulation.NCCoordinateSystem;
            _partViolationCheckBox.Checked = Document.Simulation.PartViolationDetection;
            _traceToolpathCheckBox.Checked = Document.Simulation.TraceToolpath;
            _stopCodeCheckBox.Checked = Document.Simulation.StopCode;
            _speedFeedTrackBar.Value = (int)(Document.Simulation.PlayFeedSpeed * 100);
            _speedRapidTrackBar.Value = (int)(Document.Simulation.PlayRapidSpeed * 100);

            _speedFeedTrackBar.ValueChanged += OnSpeedFeedTrackBarValueChanged;
            _speedRapidTrackBar.ValueChanged += OnSpeedRapidTrackBarValueChanged;
            _collisionsCheckBox.CheckedChanged += OnCollisionsCheckBoxCheckedChanged;
            _ncCoordinateSystemCheckBox.CheckedChanged += OnNCCoordinateSystemCheckBoxCheckedChanged;
            _partViolationCheckBox.CheckedChanged += OnPartViolationCheckBoxCheckedChanged;
            _traceToolpathCheckBox.CheckedChanged += OnTraceToolpathCheckBoxCheckedChanged;
            _stopCodeCheckBox.CheckedChanged += OnStopCodeCheckBoxCheckedChanged;

            SimulateWithChangingColor();

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet play]

        private void OnPlayButtonClick(object sender, EventArgs e)
        {
            Document.Simulation.Play();
        }

        //! [Code snippet play]

        //! [Code snippet stop]

        private void OnStopButtonClick(object sender, EventArgs e)
        {
            Document.Simulation.Stop();
        }

        //! [Code snippet stop]

        //! [Code snippet pause]

        private void OnPauseButtonClick(object sender, EventArgs e)
        {
            Document.Simulation.Pause();
        }

        //! [Code snippet pause]

        //! [Code snippet step]

        private void OnStepButtonClick(object sender, EventArgs e)
        {
            Document.Simulation.Step();
        }

        //! [Code snippet step]

        //! [Code snippet collision]

        private void OnCollisionsCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Document.Simulation.CollisionDetection = _collisionsCheckBox.Checked;
        }

        //! [Code snippet collision]

        //! [Code snippet StopCode]

        private void OnStopCodeCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Document.Simulation.StopCode = _stopCodeCheckBox.Checked;
        }

        //! [Code snippet StopCode]

        //! [Code snippet PartViolationDetection]

        private void OnPartViolationCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Document.Simulation.PartViolationDetection = _partViolationCheckBox.Checked;
        }

        //! [Code snippet PartViolationDetection]

        //! [Code snippet TraceToolpath]

        private void OnTraceToolpathCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Document.Simulation.TraceToolpath = _traceToolpathCheckBox.Checked;
        }

        //! [Code snippet TraceToolpath]

        //! [Code snippet NCCoordinateSystem]

        private void OnNCCoordinateSystemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Document.Simulation.NCCoordinateSystem = _ncCoordinateSystemCheckBox.Checked;
        }

        //! [Code snippet NCCoordinateSystem]

        private void OnSpeedFeedTrackBarValueChanged(object sender, EventArgs e)
        {
            double speed = _speedFeedTrackBar.Value / 1000.0;

            //! [Code snippet feed]

            Document.Simulation.PlayFeedSpeed = speed;

            //! [Code snippet feed]
        }

        private void OnSpeedRapidTrackBarValueChanged(object sender, EventArgs e)
        {
            double speed = _speedRapidTrackBar.Value / 1000.0;

            //! [Code snippet rapid]

            Document.Simulation.PlayRapidSpeed = speed;

            //! [Code snippet rapid]
        }

        //! [Code snippet color]

        private void SimulateWithChangingColor()
        {
            Random rnd = new Random();
            var tools = Document.Tools as EspritTools.Tools;
            foreach (EspritTechnology.Tool technology in tools)
            {
                if (technology == null)
                {
                    continue;
                }

                System.Drawing.Color color = System.Drawing.Color.FromArgb(0, 0, 0);
                var value = 255 - 100 * rnd.Next(rnd.Next(2));

                switch (rnd.Next(3))
                {
                    case 0:
                        color = System.Drawing.Color.FromArgb(value, 0, 0);
                        break;
                    case 1:
                        color = System.Drawing.Color.FromArgb(0, value, 0);
                        break;
                    case 2:
                        color = System.Drawing.Color.FromArgb(0, 0, value);
                        break;
                }

                technology.SimulationColor = (int)ColorHelper.ColorToUInt(color);
            }
        }

        //! [Code snippet color]

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _form = null;
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "Introduction to Simulation";
        public override string HtmlPath => "html/simulation_tutorial.html";

    }
}
