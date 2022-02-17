using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using EspritConstants;

namespace TutorialCSharp.Tutorials
{
    class CustomOperationSetupSheetTutorial : BaseTutorial
    {
        private Tutorial_CustomOperationSetupSheet.OperationSetupSheetDialog _dialog;

        private Dictionary<string, List<string>> _codeNames = new Dictionary<string, List<string>>();
        private List<string> _outputToolPropertyNames = new List<string>();

        public CustomOperationSetupSheetTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            CustomOperationSetupSheet();
        }

        // [Code snippet execute]

        private void CustomOperationSetupSheet()
        {
            FindToolPropertyNames();

            _dialog = new Tutorial_CustomOperationSetupSheet.OperationSetupSheetDialog();
            _dialog.OkButton.Click += OnApplyButtonClick;
            _dialog.AddButton.Click += AddButtonClick;
            _dialog.RemoveButton.Click += RemoveButtonClick;
            _dialog.CycleType.SelectionChanged += CycleType_SelectionChanged;
            _dialog.UpButton.Click += OnUpButtonClick;
            _dialog.DownButton.Click += OnDownButtonClick;

            SetAvailablePropertiesList(_dialog.CycleType.Text);

            _dialog.ShowDialog();
        }

        // [Code snippet execute]

        // [Code snippet]

        private void OnUpButtonClick(object sender, RoutedEventArgs e)
        {
            if (_dialog.OutputProperties.SelectedIndex > 0)
            {
                var i = _dialog.OutputProperties.SelectedIndex;
                var tempItem = _dialog.OutputProperties.Items[i - 1];
                _dialog.OutputProperties.Items[i - 1] = _dialog.OutputProperties.Items[i];
                _dialog.OutputProperties.Items[i] = tempItem;

                _dialog.OutputProperties.SelectedIndex = i - 1;
            }
        }

        private void OnDownButtonClick(object sender, RoutedEventArgs e)
        {
            if (_dialog.OutputProperties.SelectedIndex >= 0 && _dialog.OutputProperties.SelectedIndex != _dialog.OutputProperties.Items.Count)
            {
                var i = _dialog.OutputProperties.SelectedIndex;
                var tempItem = _dialog.OutputProperties.Items[i + 1];
                _dialog.OutputProperties.Items[i + 1] = _dialog.OutputProperties.Items[i];
                _dialog.OutputProperties.Items[i] = tempItem;

                _dialog.OutputProperties.SelectedIndex = i + 1;
            }
        }

        private void CycleType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var comboBox = sender as System.Windows.Controls.ComboBox;
            var comboBoxItem = comboBox.SelectedItem as System.Windows.Controls.ComboBoxItem;

            SetAvailablePropertiesList(comboBoxItem.Content.ToString());
        }

        private void SetAvailablePropertiesList(string type)
        {
            _dialog.AvailableProperties.Items.Clear();
            var codes = _codeNames[type];

            foreach (var item in codes)
            {
                _dialog.AvailableProperties.Items.Add(item);
            }
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

            var filePath = Path.Combine(EspritApplication.TempDir, Guid.NewGuid().ToString() + ".csv");
            using (var outStream = new StreamWriter(filePath))
            {
                outStream.WriteLine(header);

                var tools = Document.Tools as EspritTools.Tools;
                foreach (Esprit.PartOperation operation in Document.PartOperations)
                {
                    var opTech = operation.Technology as EspritTechnology.Technology;
                    String dataItem = string.Empty;
                    var operationData = string.Empty;

                    for (var i = 0; i < _outputToolPropertyNames.Count; i++)
                    {
                        if (operationData.Length != 0)
                        {
                            operationData += ", ";
                        }

                        var param = opTech.FindParameter(_outputToolPropertyNames[i]);
                        if (param == null)
                        {
                            try
                            {
                                var toolTech = tools[opTech.FindParameter("ToolID").Value] as EspritTechnology.Technology;
                                param = toolTech.FindParameter(_outputToolPropertyNames[i]);
                            }
                            catch (Exception)
                            { }
                        }

                        if (param == null)
                        {
                            switch(_outputToolPropertyNames[i])
                            {
                                case "CycleTime":
                                    dataItem = operation.CycleTime.ToString();
                                    break;
                                case "LayerName":
                                    dataItem = operation.Layer.Name;
                                    break;
                                case "OperationNumber":
                                    dataItem = operation.Key;
                                    break;
                            }
                        }
                        else
                        {
                            dataItem = param.Value.ToString();
                        }
                            
                        if (dataItem != string.Empty)
                        {
                            operationData += dataItem;
                        }
                    }

                    if (operationData.Length != 0)
                    {
                        outStream.WriteLine(operationData);
                    }
                }

                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "CustomOperationSetupSheetTutorial", $"File created: {filePath}");
            }
        }

        private void FindToolPropertyNames()
        {
            _codeNames.Clear();
            _outputToolPropertyNames.Clear();

            _codeNames.Add("All Codes", new List<string>());
            _codeNames.Add("All Operations", new List<string>());
            _codeNames.Add("Lathe Operations", new List<string>());
            _codeNames.Add("Mill Operations", new List<string>());
            _codeNames.Add("All Tools", new List<string>());
            _codeNames.Add("Lathe Tools", new List<string>());
            _codeNames.Add("Mill Tools", new List<string>());
            _codeNames.Add("Other", new List<string>()
            {
                "CycleTime",
                "LayerName",
                "OperationNumber"
            });

            var groups = new Dictionary<string, string>()
            {
                {"T", "All Codes" },
                {"Tech", "All Operations" },
                {"TechLathe", "Lathe Operations" },
                {"TechMill", "Mill Operations" },
                {"Tool", "All Tools" },
                {"ToolLathe", "Lathe Tools" },
                {"ToolMill", "Mill Tools" }
            };

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

                        foreach (var name in groups.Keys)
                        {
                            if (techName.StartsWith(name))
                            {
                                var list = _codeNames[groups[name]];
                                for (var j = 1; j <= technology.Count; j++)
                                {
                                    if (!list.Contains(technology[j].Name))
                                    {
                                        list.Add(technology[j].Name);
                                    }
                                }

                                _codeNames[groups[name]] = list;
                            }
                        }
                    }
                }
                catch (Exception)
                { }
            }
        }

        // [Code snippet]

        public override string Name => "Custom Operation Setup Sheet";
        public override string HtmlPath => "html/custom_operation_setup_sheet_tutorial.html";

    }
}
