using System;
using System.Collections.Generic;
using System.Text;
using EspritConstants;

namespace TutorialCSharp.Tutorials
{
    class FindToolParamsTutorial : BaseTutorial
    {
        private List<string> _toolLathePropertyNames = new List<string>();
        private List<string> _toolMillPropertyNames = new List<string>();

        public FindToolParamsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        private void FindToolPropertyNames()
        {
            _toolLathePropertyNames.Clear();
            _toolMillPropertyNames.Clear();

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
                            }
                        }
                    }
                }
                catch (Exception)
                { }
            }
        }

        private void DisplayToolLathePropertyNames()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("These are the Property Names for all Lathe Tools:");
            foreach (var name in _toolLathePropertyNames)
            {
                stringBuilder.AppendLine($"\t{name}");
            }
            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FindToolParamsTutorial", stringBuilder.ToString());
        }

        private void DisplayToolMillPropertyNames()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("These are the Property Names for all Mill Tools:");
            foreach (var name in _toolMillPropertyNames)
            {
                stringBuilder.AppendLine($"\t{name}");
            }
            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FindToolParamsTutorial", stringBuilder.ToString());
        }

        //! [Code snippet]

        public override void Execute()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            FindToolPropertyNames();
            DisplayToolLathePropertyNames();
            DisplayToolMillPropertyNames();
        }

        public override string Name => "Find Tool Parameters";
        public override string HtmlPath => "html/find_tool_params_tutorial.html";

    }
}
