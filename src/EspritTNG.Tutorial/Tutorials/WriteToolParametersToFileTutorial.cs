using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class WriteToolParametersToFileTutorial : BaseTutorial
    {
        public WriteToolParametersToFileTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void WriteToolParameters()
        {
            var folderPath = EspritApplication.Configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeToolPage);

            var tools = Document.Tools as EspritTools.Tools;
            var index = 0;
            foreach (EspritTechnology.ITechnology technology in tools)
            {
                index++;
                var filePath = System.IO.Path.Combine(folderPath, $"WriteToolParametersToFileTutorial_{index}.test");
                technology.Save(filePath);
            }
            if (index == 0)
                MessageBox.Show("No tools to save", "WriteToolParametersToFileTutorial");
            else
                MessageBox.Show($"{index} files saved to {System.IO.Path.Combine(folderPath)}", "WriteToolParametersToFileTutorial");
        }

        //! [Code snippet]

        public override void Execute()
        {
            WriteToolParameters();
        }

        public override string Name => "Write Tool Parameters to a File";
        public override string HtmlPath => "html/write_tool_param_to_file_tutorial.html";

    }
}
