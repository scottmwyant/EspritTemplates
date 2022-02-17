using System;
using System.Text;
using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    class DimensioningParametersTutorial : BaseTutorial
    {
        private Form _form;

        private void Show()
        {
            _form = new Form();
            Button saveButton = new Button();
            Button loadButton = new Button();
            Button scaleButton = new Button();

            _form.Text = "DimensioningParametersTutorial form";
            saveButton.Text = "Save";
            loadButton.Text = "Load";
            scaleButton.Text = "Scale";

            saveButton.SetBounds(9, 16, 50, 23);
            loadButton.SetBounds(79, 16, 50, 23);
            scaleButton.SetBounds(149, 16, 50, 23);

            scaleButton.AutoSize = loadButton.AutoSize = saveButton.AutoSize = true;
            scaleButton.Anchor = loadButton.Anchor = saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            _form.ClientSize = new System.Drawing.Size(210, 40);
            _form.Controls.AddRange(new Control[] { saveButton, loadButton, scaleButton });
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            saveButton.Click += OnSaveButtonClick;
            loadButton.Click += OnLoadButtonClick;
            scaleButton.Click += OnScaleButtonClick;

            _form.FormClosed += (o, e) => { _form = null; };

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet scale]

        private void ScaleDimensioningParameters()
        {
            var scaleFactor = 1.0;

            if (RequestUserInput("Enter Dimensioning Factor", "Scale Factor", ref scaleFactor))
            {
                // only scale linear measurement values

                EspritApplication.Configuration.ConfigurationDimension.ArrowHeight *= scaleFactor;
                EspritApplication.Configuration.ConfigurationDimension.ArrowWidth *= scaleFactor;
                EspritApplication.Configuration.ConfigurationDimension.DatumDistance *= scaleFactor;
                EspritApplication.Configuration.ConfigurationDimension.LeaderDistance *= scaleFactor;
                EspritApplication.Configuration.ConfigurationDimension.ToleranceMinus *= scaleFactor;
                EspritApplication.Configuration.ConfigurationDimension.TolerancePlus *= scaleFactor;
                EspritApplication.Configuration.ConfigurationDimension.WitnessDistance *= scaleFactor;
                EspritApplication.Configuration.ConfigurationDimension.WitnessExtentDistance *= scaleFactor;
            }
        }

        //! [Code snippet scale]

        private void OnScaleButtonClick(object sender, EventArgs e)
        {
            ScaleDimensioningParameters();
        }

        //! [Code snippet open]

        private void OpenDimensioningParameters(String filePath)
        {
            if (filePath.Length != 0)
            {

                var config = EspritApplication.Configuration.ConfigurationDimension;

                using (var inStream = new System.IO.StreamReader(filePath))
                {
                    var charSeparators = new char[] { ',' };
                    var data = string.Empty;

                    while ((data = inStream.ReadLine()) != null)
                    {
                        var parameterData = data.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                        if (parameterData.Length != 2)
                        {
                            continue;
                        }

                        switch (parameterData[0])
                        {
                            case "AllowMajorAngle":
                                {
                                    if (bool.TryParse(parameterData[1], out var value))
                                        config.AllowMajorAngle = value;
                                    break;
                                }

                            case "AngleTrailingDigits":
                                {
                                    if (int.TryParse(parameterData[1], out var value))
                                        config.AngleTrailingDigits = value;
                                    break;
                                }
                            case "AngleUnit":
                                {
                                    if (Enum.TryParse(parameterData[1], out EspritConstants.espAngleUnitType value))
                                        config.AngleUnit = value;
                                    break;
                                }
                            case "ArcCenterLine":
                                {
                                    if (bool.TryParse(parameterData[1], out var value))
                                        config.ArcCenterLine = value;
                                    break;
                                }
                            case "ArcType":
                                {
                                    if (Enum.TryParse(parameterData[1], out EspritConstants.espDimensionRoundType value))
                                        config.ArcType = value;
                                    break;
                                }
                            case "ArrowHeight":
                                {
                                    if (double.TryParse(parameterData[1], out var value))
                                        config.ArrowHeight = value;
                                    break;
                                }
                            case "ArrowType":
                                {
                                    if (Enum.TryParse(parameterData[1], out EspritConstants.espArrowType value))
                                        config.ArrowType = value;
                                    break;
                                }
                            case "ArrowWidth":
                                {
                                    if (double.TryParse(parameterData[1], out var value))
                                        config.ArrowWidth = value;
                                    break;
                                }
                            case "AutoCenter":
                                {
                                    if (bool.TryParse(parameterData[1], out var value))
                                        config.AutoCenter = value;
                                    break;
                                }
                            case "AutoDimensionSide":
                                {
                                    if (Enum.TryParse(parameterData[1], out EspritConstants.espDimensionAutoPlacementType value))
                                        config.AutoDimensionSide = value;
                                    break;
                                }
                            case "CircleCenterLine":
                                {
                                    if (bool.TryParse(parameterData[1], out var value))
                                        config.CircleCenterLine = value;
                                    break;
                                }
                            case "CircleType":
                                {
                                    if (Enum.TryParse(parameterData[1], out EspritConstants.espDimensionRoundType value))
                                        config.CircleType = value;
                                    break;
                                }
                            case "DatumDistance":
                                {
                                    if (double.TryParse(parameterData[1], out var value))
                                        config.DatumDistance = value;
                                    break;
                                }
                            case "DatumType":
                                {
                                    if (Enum.TryParse(parameterData[1], out EspritConstants.espDimensionDatumType value))
                                        config.DatumType = value;
                                    break;
                                }
                            case "DrawingScale":
                                {
                                    if (double.TryParse(parameterData[1], out var value))
                                        config.DrawingScale = value;
                                    break;
                                }
                            case "LeaderDistance":
                                {
                                    if (double.TryParse(parameterData[1], out var value))
                                        config.LeaderDistance = value;
                                    break;
                                }
                            case "LengthTrailingDigits":
                                {
                                    if (int.TryParse(parameterData[1], out var value))
                                        config.LengthTrailingDigits = value;
                                    break;
                                }
                            case "LengthUnit":
                                {
                                    if (Enum.TryParse(parameterData[1], out EspritConstants.espUnitType value))
                                        config.LengthUnit = value;
                                    break;
                                }
                            case "Prefix":
                                {
                                    config.Prefix = parameterData[1];
                                    break;
                                }
                            case "Suffix":
                                {
                                    config.Suffix = parameterData[1];
                                    break;
                                }
                            case "ToleranceMinus":
                                {
                                    if (double.TryParse(parameterData[1], out var value))
                                        config.ToleranceMinus = value;
                                    break;
                                }
                            case "TolerancePlus":
                                {
                                    if (double.TryParse(parameterData[1], out var value))
                                        config.TolerancePlus = value;
                                    break;
                                }
                            case "ToleranceTextProportion":
                                {
                                    if (double.TryParse(parameterData[1], out var value))
                                        config.ToleranceTextProportion = value;
                                    break;
                                }
                            case "ToleranceType":
                                {
                                    if (Enum.TryParse(parameterData[1], out EspritConstants.espDimensionToleranceType value))
                                        config.ToleranceType = value;
                                    break;
                                }
                            case "WitnessDistance":
                                {
                                    if (double.TryParse(parameterData[1], out var value))
                                        config.WitnessDistance = value;
                                    break;
                                }
                            case "WitnessExtentDistance":
                                {
                                    if (double.TryParse(parameterData[1], out var value))
                                        config.WitnessExtentDistance = value;
                                    break;
                                }
                        }
                    }
                }
            }
        }

        //! [Code snippet open]

        private void OnLoadButtonClick(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = EspritApplication.Configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeEspritDrawings);
                openFileDialog.Filter = "Dimensioning Parameter Files (*.csv)|*.csv|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Title = "Open Dimensioning Parameter File";
                openFileDialog.ShowDialog();
                filePath = openFileDialog.FileName;
            }

            OpenDimensioningParameters(filePath);
        }

        //! [Code snippet save]

        private void SaveDimensioningParameters(String filePath)
        {
            if (filePath.Length != 0)
            {
                var text = new StringBuilder();
                var config = EspritApplication.Configuration.ConfigurationDimension;

                text.AppendLine($"AllowMajorAngle, {config.AllowMajorAngle}");
                text.AppendLine($"AngleTrailingDigits, {config.AngleTrailingDigits}");
                text.AppendLine($"AngleUnit, {config.AngleUnit}");
                text.AppendLine($"ArcCenterLine, {config.ArcCenterLine}");
                text.AppendLine($"ArcType, {config.ArcType}");
                text.AppendLine($"ArrowHeight, {config.ArrowHeight}");
                text.AppendLine($"ArrowType, {config.ArrowType}");
                text.AppendLine($"ArrowWidth, {config.ArrowWidth}");
                text.AppendLine($"AutoCenter, {config.AutoCenter}");
                text.AppendLine($"AutoDimensionSide, {config.AutoDimensionSide}");
                text.AppendLine($"CircleCenterLine, {config.CircleCenterLine}");
                text.AppendLine($"CircleType, {config.CircleType}");
                text.AppendLine($"DatumDistance, {config.DatumDistance}");
                text.AppendLine($"DatumType, {config.DatumType}");
                text.AppendLine($"DrawingScale, {config.DrawingScale}");
                text.AppendLine($"LeaderDistance, {config.LeaderDistance}");
                text.AppendLine($"LengthTrailingDigits, {config.LengthTrailingDigits}");
                text.AppendLine($"LengthUnit, {config.LengthUnit}");
                text.AppendLine($"Prefix, {config.Prefix}");
                text.AppendLine($"Suffix, {config.Suffix}");
                text.AppendLine($"ToleranceMinus, {config.ToleranceMinus}");
                text.AppendLine($"TolerancePlus, {config.TolerancePlus}");
                text.AppendLine($"ToleranceTextProportion, {config.ToleranceTextProportion}");
                text.AppendLine($"ToleranceType, {config.ToleranceType}");
                text.AppendLine($"WitnessDistance, {config.WitnessDistance}");
                text.AppendLine($"WitnessExtentDistance, {config.WitnessExtentDistance}");

                using (var outStream = new System.IO.StreamWriter(filePath))
                {
                    outStream.Write(text.ToString());
                }
            }
        }

        //! [Code snippet save]

        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (var saveFileDialog = new System.Windows.Forms.SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = EspritApplication.Configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeEspritDrawings);
                saveFileDialog.Filter = "Dimensioning Parameter Files (*.csv)|*.csv|All Files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.Title = "Save Dimensioning Parameter File";
                saveFileDialog.ShowDialog();
                filePath = saveFileDialog.FileName;
            }
            SaveDimensioningParameters(filePath);
        }

        public DimensioningParametersTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "Configuration: Scale, Save, and Open Dimensioning Parameters";
        public override string HtmlPath => "html/dimensioning_parameters_tutorial.html";

    }
}
