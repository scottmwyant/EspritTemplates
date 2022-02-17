using System.IO;
using System.Text;

namespace TutorialCSharp.Tutorials
{
    class FilesAndFoldersLocationTutorial : BaseTutorial
    {
        public FilesAndFoldersLocationTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet get]

        public void ScanFileAndFolderLocations()
        {
            var outputWindow = EspritApplication.EventWindow;
            var configuration = EspritApplication.Configuration;

            outputWindow.Visible = true;
            outputWindow.Clear();

            outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FilesAndFoldersLocationTutorial", "ESPRIT Drawings folder is {configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeEspritDrawings)}");
            outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FilesAndFoldersLocationTutorial", $"Machine Setup folder is {configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeMachineSetup)}");
            outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FilesAndFoldersLocationTutorial", $"NC Code folder is {configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeNCcode)}");
            outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FilesAndFoldersLocationTutorial", $"NC Editor program is {configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeNCEditor)}");
            outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FilesAndFoldersLocationTutorial", $"Op Page folder is {configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeOpPage)}");
            outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FilesAndFoldersLocationTutorial", $"Post Processor folder is {configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypePostProcessor)}");
            outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FilesAndFoldersLocationTutorial", $"ESPRIT Template folder is {configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeTemplate)}");
            outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FilesAndFoldersLocationTutorial", $"Tool Library is {configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeToolLibrary)}");
            outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FilesAndFoldersLocationTutorial", $"Tool Page folder is {configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeToolPage)}");
            outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FilesAndFoldersLocationTutorial", "");
        }

        //! [Code snippet get]

        //! [Code snippet default tool]

        public void Load()
        {
            var configuration = EspritApplication.Configuration;

            var tools = Document.Tools as EspritTools.Tools;
            var toolPageFolder = configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeToolPage);

            var dirInfo = new DirectoryInfo(toolPageFolder);

            var filePaths = dirInfo.GetFiles("*.test");
            if (filePaths.Length > 0)
            {
                foreach (var filePath in filePaths)
                {
                    EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FilesAndFoldersLocationTutorial", filePath.FullName);
                }

                System.Windows.MessageBox.Show($"Loaded tools from {filePaths[0].FullName}", "FilesAndFoldersLocationTutorial");
                tools.LoadTools(filePaths[0].FullName);
            }
        }

        //! [Code snippet default tool]

        public override void Execute()
        {
            ScanFileAndFolderLocations();
            Load();
        }

        public override string Name => "Configuration: File and File Folder Locations";
        public override string HtmlPath => "html/files_and_folders_location_tutorial.html";

    }
}
