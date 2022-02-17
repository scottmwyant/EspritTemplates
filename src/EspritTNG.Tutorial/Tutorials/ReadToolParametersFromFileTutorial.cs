using System.IO;

namespace TutorialCSharp.Tutorials
{
    class ReadToolParametersFromFileTutorial : BaseTutorial
    {
        public ReadToolParametersFromFileTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void ReadToolParameters()
        {
            var folderPath = EspritApplication.Configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeToolPage);
            var tools = Document.Tools as EspritTools.Tools;

            var dirInfo = new DirectoryInfo(folderPath);

            var filePaths = dirInfo.GetFiles("*.test");
            foreach (var filePath in filePaths)
            {
                tools.LoadTools(filePath.FullName);
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            ReadToolParameters();
        }

        public override string Name => "Read Tool Parameters from a File";
        public override string HtmlPath => "html/read_tool_param_from_file_tutorial.html";

    }
}
