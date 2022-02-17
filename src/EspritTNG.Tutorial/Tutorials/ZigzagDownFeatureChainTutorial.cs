using System;
using System.Windows;

using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    class ZigzagDownFeatureChainTutorial : BaseTutorial
    {
        private Form _form;
        private TextBox _totalDepthTextBox;
        private TextBox _maxIncrDepthTextBox;

        private RadioButton _optLevel;
        private RadioButton _optRamp;

        public void Show()
        {
            _form = new Form();
            _optLevel = new RadioButton();
            _optRamp = new RadioButton();
            var labelTotal = new Label();
            var labelMax = new Label();

            _totalDepthTextBox = new TextBox();
            _maxIncrDepthTextBox = new TextBox();

            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            _optLevel.Text = "Ramp - Level";
            _optRamp.Text = "Ramp - Ramp";
            _form.Text = "ZigzagDownFeatureChainTutorial form";

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            labelTotal.Text = "Total depth";
            labelMax.Text = "Max incremental depth";

            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            _optLevel.SetBounds(9, 10, 120, 30);
            _optRamp.SetBounds(150, 10, 120, 30);
            labelTotal.SetBounds(9, 46, 372, 13);
            _totalDepthTextBox.SetBounds(12, 66, 300, 20);
            labelMax.SetBounds(9, 96, 372, 13);
            _maxIncrDepthTextBox.SetBounds(12, 116, 300, 20);

            buttonOk.SetBounds(70, 162, 75, 23);
            buttonCancel.SetBounds(170, 162, 75, 23);

            labelTotal.AutoSize = labelMax.AutoSize = _totalDepthTextBox.AutoSize = _maxIncrDepthTextBox.AutoSize = _optRamp.AutoSize = _optLevel.AutoSize = true;

            labelTotal.Anchor = labelMax.Anchor = _totalDepthTextBox.Anchor = _maxIncrDepthTextBox.Anchor = _totalDepthTextBox.Anchor | AnchorStyles.Right;
            _optLevel.Anchor = _optRamp.Anchor = _optLevel.Anchor | AnchorStyles.Left;
            buttonOk.Anchor = buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            _form.ClientSize = new System.Drawing.Size(326, 190);
            _form.Controls.AddRange(new Control[] { labelTotal, _optLevel, _optRamp, labelMax, _totalDepthTextBox, _maxIncrDepthTextBox, buttonOk, buttonCancel });
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;
            _form.AcceptButton = buttonOk;
            _form.CancelButton = buttonCancel;

            _optLevel.Checked = true;

            //! [Code snippet init]

            _totalDepthTextBox.Text = (Document.SystemUnit == EspritConstants.espUnitType.espInch)
                ? "1"
                : "25";
            _maxIncrDepthTextBox.Text = (Document.SystemUnit == EspritConstants.espUnitType.espInch)
                ? "0.1"
                : "2.5";

            //! [Code snippet init]

            buttonCancel.Click += OnCancelButtonClick;
            buttonOk.Click += OnButtonOkClick;

            _form.FormClosed += (o, e) => { _form = null; };

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet OK]

        private void OnButtonOkClick(object sender, EventArgs e)
        {
            var totalDepth = 0.0;
            var maxIncrementalDepth = 0.0;
            if (!double.TryParse(_totalDepthTextBox.Text, out totalDepth))
            {
                System.Windows.MessageBox.Show("Total Depth must be a positive value", "ZigzagDownFeatureChainTutorial", MessageBoxButton.OK);
                return;
            }
            if (totalDepth <= 0)
            {
                System.Windows.MessageBox.Show("Total Depth must be a positive value", "ZigzagDownFeatureChainTutorial", MessageBoxButton.OK);
                return;
            }

            if (!double.TryParse(_maxIncrDepthTextBox.Text, out maxIncrementalDepth))
            {
                System.Windows.MessageBox.Show("Max Incremental Depth must be a positive value less then Total Depth", "ZigzagDownFeatureChainTutorial", MessageBoxButton.OK);
                return;
            }

            if (maxIncrementalDepth > totalDepth || maxIncrementalDepth < 0)
            {
                System.Windows.MessageBox.Show("Max Incremental Depth must be a positive value less then Total Depth", "ZigzagDownFeatureChainTutorial", MessageBoxButton.OK);
                return;
            }

            Esprit.FeatureChain featureChain = null;

            try
            {
                featureChain = Document.GetAnyElement("Pick Feature Chain to Zigzag Down", EspritConstants.espGraphicObjectType.espFeatureChain) as Esprit.FeatureChain;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                System.Windows.MessageBox.Show("Feature chain does not selected!", "ZigzagDownFeatureChainTutorial", MessageBoxButton.OK);
                return;
            }

            var totalSegmentLength = 0.0;
            for (var i = 1; i <= featureChain.Count; i++)
            {
                if (featureChain.Item[i].GraphicObjectType == EspritConstants.espGraphicObjectType.espSegment)
                {
                    var segment = featureChain.Item[i] as Esprit.Segment;
                    totalSegmentLength += segment.Length;
                }
            }

            if (totalSegmentLength == 0)
            {
                System.Windows.MessageBox.Show("Cannot zigzag down feature containing only arcs!", "ZigzagDownFeatureChainTutorial", MessageBoxButton.OK);
                return;
            }
            if (maxIncrementalDepth == 0)
            {
                maxIncrementalDepth = totalDepth;
            }
            var numberOfPasses = (int)(totalDepth / maxIncrementalDepth);
            if (totalDepth / maxIncrementalDepth > numberOfPasses)
            {
                numberOfPasses++;
            }

            maxIncrementalDepth = totalDepth / numberOfPasses;
            var passDepth = new double[2];
            if (_optLevel.Checked)
            {
                passDepth[0] = maxIncrementalDepth;
                passDepth[1] = 0;
                numberOfPasses *= 2;
            }
            else
            {
                passDepth[0] = maxIncrementalDepth / 2;
                passDepth[1] = passDepth[0];
                numberOfPasses = numberOfPasses * 2 + 1;
            }

            var zigzag = Document.FeatureChains.Add(featureChain.Extremity(EspritConstants.espExtremityType.espExtremityStart));
            zigzag.Color = featureChain.Color;
            zigzag.LineType = featureChain.LineType;
            zigzag.LineWeight = featureChain.LineWeight;
            var currentDepth = zigzag.Extremity(EspritConstants.espExtremityType.espExtremityStart).Z;

            for (var pass = 1; pass <= numberOfPasses; pass++)
            {
                if (pass % 2 > 0)
                {
                    for (var i = 1; i <= featureChain.Count; i++)
                    {
                        switch (featureChain.Item[i].GraphicObjectType)
                        {
                            case EspritConstants.espGraphicObjectType.espArc:
                                var arc = featureChain.Item[i] as Esprit.Arc;
                                var centerPoint = arc.CenterPoint;
                                centerPoint = Document.GetPoint(centerPoint.X, centerPoint.Y, currentDepth);

                                try
                                {
                                    arc = Document.GetArc(centerPoint, arc.Radius, arc.StartAngle, arc.EndAngle);
                                    zigzag.Add(arc);
                                }
                                catch (System.Runtime.InteropServices.COMException)
                                {
                                }
                                break;
                            case EspritConstants.espGraphicObjectType.espSegment:
                                var segment = featureChain.Item[i] as Esprit.Segment;
                                if (pass < numberOfPasses)
                                {
                                    currentDepth -= passDepth[0] * segment.Length / totalSegmentLength;
                                }
                                zigzag.Add(Document.GetPoint(segment.XEnd, segment.YEnd, currentDepth));
                                break;
                        }
                    }
                }
                else
                {
                    for (var i = featureChain.Count; i >= 1; i--)
                    {
                        switch (featureChain.Item[i].GraphicObjectType)
                        {
                            case EspritConstants.espGraphicObjectType.espArc:
                                var arc = featureChain.Item[i] as Esprit.Arc;
                                var centerPoint = arc.CenterPoint;
                                centerPoint = Document.GetPoint(centerPoint.X, centerPoint.Y, currentDepth);

                                try
                                {
                                    arc = Document.GetArc(centerPoint, arc.Radius, arc.StartAngle, arc.EndAngle);
                                    arc.Reverse();
                                    zigzag.Add(arc);
                                }
                                catch (System.Runtime.InteropServices.COMException)
                                {
                                }
                                break;
                            case EspritConstants.espGraphicObjectType.espSegment:
                                var segment = featureChain.Item[i] as Esprit.Segment;
                                if (pass < numberOfPasses)
                                {
                                    currentDepth -= passDepth[1] * segment.Length / totalSegmentLength;
                                }
                                zigzag.Add(Document.GetPoint(segment.XStart, segment.YStart, currentDepth));
                                break;
                        }
                    }
                }
            }

            CloseForm();
        }

        //! [Code snippet OK]

        //! [Code snippet cancel]

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            CloseForm();
        }
        private void CloseForm()
        {
            _form.Close();
            _form = null;
        }

        //! [Code snippet cancel]

        public ZigzagDownFeatureChainTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "Zigzag Down FeatureChain";
        public override string HtmlPath => "html/zigzag_down_featurechain_tutorial.html";

    }
}
