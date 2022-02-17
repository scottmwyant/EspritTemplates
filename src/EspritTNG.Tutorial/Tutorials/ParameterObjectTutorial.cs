using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EspritConstants;

namespace TutorialCSharp.Tutorials
{
    class ParameterObjectTutorial : BaseTutorial
    {
        public struct ClInfo
        {
            public int ClCode;
            public string Name;
            public string Caption;
            public string TechName;
        };

        public ParameterObjectTutorial(Esprit.Application app): base(app)
        {
        }

        // [Code snippet FindAllCLCodes]

        private void FindAllCLCodes()
        {
            var filePath = Path.Combine(EspritApplication.TempDir, Guid.NewGuid().ToString() + ".csv");
            using (var outStream = new StreamWriter(filePath))
            {
                outStream.WriteLine("Tech Type" + "," + "Technology Object Name" + "," + "CL Code" + "," + "Property Name" + "," + "Caption" + "," + "Default Value");
                var technologyUtility = Document.TechnologyUtility as EspritTechnology.TechnologyUtility;

                foreach (espTechnologyType technologyType in Enum.GetValues(typeof(espTechnologyType)))
                {
                    try
                    {
                        var technology = technologyUtility.CreateTechnology(technologyType, Document.SystemUnit);
                        if (technology != null)
                        {
                            var techName = Microsoft.VisualBasic.Information.TypeName(technology);
                            for (var j = 1; j <= technology.Count; j++)
                            {
                                var param = technology[j];
                                outStream.WriteLine($"{technologyType},{techName},{param.ClCode},{param.Name},{param.Caption},{param.Value}");
                            }
                        }
                    }
                    catch(Exception)
                    { }
                }

                EspritApplication.EventWindow.AddMessage(espMessageType.espMessageTypeInformation, "ParameterObjectTutorial", $"File with All CLCodes created: {filePath}");
            }
        }

        // [Code snippet FindAllCLCodes]

        // [Code snippet FindAllUniqueCLCodes]

        private void FindAllUniqueCLCodes()
        {
            var technologyUtility = Document.TechnologyUtility as EspritTechnology.TechnologyUtility;
            var infos = new Dictionary<int, ClInfo>();

            foreach (espTechnologyType technologyType in Enum.GetValues(typeof(espTechnologyType)))
            {
                try
                {
                    var technology = technologyUtility.CreateTechnology(technologyType, Document.SystemUnit);
                    if (technology != null)
                    {
                        var techName = Microsoft.VisualBasic.Information.TypeName(technology);
                        for (var j = 1; j <= technology.Count; j++)
                        {
                            var param = technology[j];
                            if (infos.Keys.Contains(param.ClCode))
                            {
                                var tmp = infos[param.ClCode];
                                tmp.TechName = $"{infos[param.ClCode].TechName }, {techName}";
                                infos[param.ClCode] = tmp;
                            }
                            else
                            {
                                ClInfo info = new ClInfo()
                                {
                                    ClCode = param.ClCode,
                                    Caption = param.Caption,
                                    Name = param.Name,
                                    TechName = techName
                                };

                                infos.Add(param.ClCode, info);
                            }
                        }
                    }
                }
                catch (Exception)
                { }
            }

            var filePath = Path.Combine(EspritApplication.TempDir, Guid.NewGuid().ToString() + ".csv");
            using (var outStream = new StreamWriter(filePath))
            {
                outStream.WriteLine("CL Code" + "," + "Property Name" + "," + "Caption" + "," + "Technology Object Names");
                foreach(var info in infos.Values)
                {
                    outStream.WriteLine($"{info.ClCode},{info.Name},{info.Caption},{info.TechName}");
                }

                EspritApplication.EventWindow.AddMessage(espMessageType.espMessageTypeInformation, "ParameterObjectTutorial", $"File with Unique CLCodes created: {filePath}");
            }
        }

        // [Code snippet FindAllUniqueCLCodes]

        public override void Execute()
        {
            FindAllCLCodes();
            FindAllUniqueCLCodes();
        }

        public override string Name => "A Closer Look at the Parameter Object";
        public override string HtmlPath => "html/parameter_object_tutorial.html";

    }
}
