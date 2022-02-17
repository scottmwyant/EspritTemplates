using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using EspritConstants;

namespace TutorialCSharp.Tutorials
{
    class ToolingCustomSetupSheetTutorial : BaseTutorial
    {
        private Tutorial_ToolingCustomSetupSheet.ToolSetupSheetDialog _dialog = null;

        private List<string> _toolLathePropertyNames = new List<string>();
        private List<string> _toolMillPropertyNames = new List<string>();
        private List<string> _allToolPropertyNames = new List<string>();
        private List<string> _outputToolPropertyNames = new List<string>();

        public ToolingCustomSetupSheetTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            CustomSetupSheetForTooling();
        }

        // [Code snippet execute]

        private void CustomSetupSheetForTooling()
        {
            FindToolPropertyNames();
            _dialog = new Tutorial_ToolingCustomSetupSheet.ToolSetupSheetDialog();
            _dialog.OkButton.Click += OnApplyButtonClick;
            _dialog.AddButton.Click += AddButtonClick;
            _dialog.RemoveButton.Click += RemoveButtonClick;
            _dialog.CycleType.SelectionChanged += CycleType_SelectionChanged;

            SetAvailablePropertiesList(_dialog.CycleType.Text);

            _dialog.ShowDialog();
        }

        // [Code snippet execute]

        // [Code snippet]

        private void CycleType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var comboBox = sender as System.Windows.Controls.ComboBox;
            var comboBoxItem = comboBox.SelectedItem as System.Windows.Controls.ComboBoxItem;

            SetAvailablePropertiesList(comboBoxItem.Content.ToString());
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < _dialog.OutputProperties.Items.Count; i++)
            {
                if (_dialog.OutputProperties.SelectedIndex == i)
                {
                    _outputToolPropertyNames.Remove(_dialog.OutputProperties.Items.GetItemAt(i).ToString());
                    _dialog.OutputProperties.Items.RemoveAt(i);
                    break;
                }
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < _dialog.AvailableProperties.Items.Count; i++)
            {
                if (_dialog.AvailableProperties.SelectedIndex == i)
                {
                    var value = _dialog.AvailableProperties.Items.GetItemAt(i).ToString();
                    if (!_outputToolPropertyNames.Contains(value))
                    {
                        _outputToolPropertyNames.Add(value);
                        _dialog.OutputProperties.Items.Add(value);
                    }
                    break;
                }
            }
        }

        private void SetAvailablePropertiesList(string type)
        {
            _dialog.AvailableProperties.Items.Clear();

            switch (type)
            {
                case "All Tools":
                    foreach(var item in _allToolPropertyNames)
                    {
                        _dialog.AvailableProperties.Items.Add(item);
                    }
                    break;
                case "Lathe Tools":
                    foreach (var item in _toolLathePropertyNames)
                    {
                        _dialog.AvailableProperties.Items.Add(item);
                    }
                    break;
                case "Mill Tools":
                    foreach (var item in _toolMillPropertyNames)
                    {
                        _dialog.AvailableProperties.Items.Add(item);
                    }
                    break;
            }
        }

        private void FindToolPropertyNames()
        {
            _toolLathePropertyNames.Clear();
            _toolMillPropertyNames.Clear();
            _allToolPropertyNames.Clear();
            _outputToolPropertyNames.Clear();

            var technologyUtility = Document.TechnologyUtility as EspritTechnology.TechnologyUtility;

            foreach (espTechnologyType technologyType in Enum.GetValues(typeof(espTechnologyType)))
            {
                try
                {
                    var technology = technologyUtility.CreateTechnology(technologyType, Document.SystemUnit);
                    if (technology != null)
                    {
                        var techName = Microsoft.VisualBasic.Information.TypeName(technology);
                        techName = techName.Substring(1);

                        if (techName.StartsWith("ToolLathe"))
                        {
                            for (var j = 1; j <= technology.Count; j++)
                            {
                                if (!_toolLathePropertyNames.Contains(technology[j].Name))
                                {
                                    _toolLathePropertyNames.Add(technology[j].Name);
                                }
                                if (!_allToolPropertyNames.Contains(technology[j].Name))
                                {
                                    _allToolPropertyNames.Add(technology[j].Name);
                                }
                            }
                        }
                        else if (techName.StartsWith("ToolMill"))
                        {
                            for (var j = 1; j <= technology.Count; j++)
                            {
                                if (!_toolMillPropertyNames.Contains(technology[j].Name))
                                {
                                    _toolMillPropertyNames.Add(technology[j].Name);
                                }
                                if (!_allToolPropertyNames.Contains(technology[j].Name))
                                {
                                    _allToolPropertyNames.Add(technology[j].Name);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                { }
            }
        }

        private void OnApplyButtonClick(object sender, RoutedEventArgs e)
        {
            _dialog.Close();

            var header = string.Empty;
            for (var i = 0; i < _outputToolPropertyNames.Count; i++)
            {
                if (header.Length != 0)
                {
                    header += ", ";
                }

                header += _outputToolPropertyNames[i];
            }

            var toolData = string.Empty;
            var tools = Document.Tools as EspritTools.Tools;
            foreach(EspritTechnology.Technology tech in tools)
            {
                for (var i = 0; i < _outputToolPropertyNames.Count; i++)
                {
                    if (toolData.Length != 0)
                    {
                        toolData += ", ";
                    }

                    var param = tech.FindParameter(_outputToolPropertyNames[i]);
                    if (param != null)
                    {
                        toolData += param.Value.ToString();
                    }
                }
            }

            var filePath = Path.Combine(EspritApplication.TempDir, Guid.NewGuid().ToString() + ".csv");
            using (var outStream = new StreamWriter(filePath))
            {
                outStream.WriteLine(header);
                outStream.WriteLine(toolData);

                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ToolingCustomSetupSheetTutorial", $"File created: {filePath}");
            }
        }

        // [Code snippet]

        public override string Name => "Custom Setup Sheet for Tooling";
        public override string HtmlPath => "html/tooling_custom_setup_sheet_tutorial.html";

    }
}
