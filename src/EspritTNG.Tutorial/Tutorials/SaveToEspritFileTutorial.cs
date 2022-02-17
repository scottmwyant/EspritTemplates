using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class SaveToEspritFileTutorial : BaseTutorial
    {
        public SaveToEspritFileTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            var tempDir = EspritApplication.TempDir;
            var filePath = System.IO.Path.Combine(tempDir, "USER/Windows.CSV");

            using (var outStream = new System.IO.StreamWriter(filePath))
            {
                foreach (Esprit.Window window in Document.Windows)
                {
                    outStream.WriteLine($"{window.Caption} {window.Height} {window.Left} {window.Top} {window.Visible} {window.Width} {window.WindowState}");
                }
            }

            MessageBox.Show(filePath, "SaveToEspritFileTutorial");
        }

        public override string Name => "Saving Custom USER Information Within an ESPRIT File";
        public override string HtmlPath => "html/save_to_esprit_file_tutorial.html";

    }
}
