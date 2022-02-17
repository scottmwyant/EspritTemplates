using System.Text;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class FileImportExportTutorial : BaseTutorial
    {
        public FileImportExportTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet params]

        private void ScanFileOptions()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            var config = EspritApplication.Configuration;
            var acisFileTolerance = Document.SystemUnit == EspritConstants.espUnitType.espInch
                ? config.AcisFile.TrimToleranceInch
                : config.AcisFile.TrimToleranceMetric;
            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FileImportExportTutorial", $"AcisFile: Surfaces = {config.AcisFile.Surfaces}, TrimTolerance = {acisFileTolerance}");

            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FileImportExportTutorial", $"BmpFile: Height = {config.BmpFile.Height}, Width = {config.BmpFile.Width}");

            var catiaFileTolerance = Document.SystemUnit == EspritConstants.espUnitType.espInch
                ? config.CatiaFile.SewToleranceInch
                : config.CatiaFile.SewToleranceMetric;
            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FileImportExportTutorial", $"CatiaFile: Solid = {config.CatiaFile.Solid}, Surfaces = {config.CatiaFile.Surfaces}, SewTolerance = {catiaFileTolerance}");

            var dxfDwgFileTolerance = Document.SystemUnit == EspritConstants.espUnitType.espInch
                ? config.DxfDwgFile.TrimToleranceInch
                : config.DxfDwgFile.TrimToleranceMetric;
            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FileImportExportTutorial", $"DxfDwgFile: AutoScale = {config.DxfDwgFile.AutoScale}, BlockData = {config.DxfDwgFile.BlockData}, EllipseMode = {config.DxfDwgFile.EllipseMode}, " +
                $"FlatMode = {config.DxfDwgFile.FlatMode}, FileUnit = {config.DxfDwgFile.FileUnit}, TrimTolerance = {dxfDwgFileTolerance}, V13Compatible = {config.DxfDwgFile.V13Compatible}");

            var igesFileTolerance = Document.SystemUnit == EspritConstants.espUnitType.espInch
                ? config.IgesFile.SewToleranceInch
                : config.IgesFile.SewToleranceMetric;
            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FileImportExportTutorial", $"IgesFile: AutoScale = {config.IgesFile.AutoScale}, BlankStatus = {config.IgesFile.BlankStatus}, " +
                $"ClipMode = {config.IgesFile.ClipMode}, DrawingMode = {config.IgesFile.DrawingMode}, ImportLabel = {config.IgesFile.ImportLabel}, LabelName = {config.IgesFile.LabelName}, LogFileMode = {config.IgesFile.LogFileMode}, " +
                $"SmartDrawing = {config.IgesFile.SmartDrawing}, SewTolerance = {igesFileTolerance}");

            var parasolidFileTolerance = Document.SystemUnit == EspritConstants.espUnitType.espInch
                ? config.ParasolidFile.TrimToleranceInch
                : config.ParasolidFile.TrimToleranceMetric;
            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FileImportExportTutorial", $"ParasolidFile: ImportLabel = {config.ParasolidFile.ImportLabel}, LabelName = {config.ParasolidFile.LabelName}, " +
                $"Solid = {config.ParasolidFile.Solid}, Surfaces = {config.ParasolidFile.Surfaces}, Wireframe = {config.ParasolidFile.Surfaces}, TrimTolerance = {parasolidFileTolerance}");

            var solidWorksFileTolerance = Document.SystemUnit == EspritConstants.espUnitType.espInch
                ? config.SolidWorksFile.TrimToleranceInch
                : config.SolidWorksFile.TrimToleranceMetric;
            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FileImportExportTutorial", $"SolidWorksFile: ImportLabel = {config.SolidWorksFile.ImportLabel}, LabelName = {config.SolidWorksFile.LabelName}, " +
                $"Solid = {config.SolidWorksFile.Solid}, Surfaces = {config.ParasolidFile.Surfaces}, Wireframe = {config.ParasolidFile.Wireframe}, TrimTolerance = {solidWorksFileTolerance}");

            var stepFileTolerance = Document.SystemUnit == EspritConstants.espUnitType.espInch
                ? config.StepFile.SewToleranceInch
                : config.StepFile.SewToleranceMetric;
            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FileImportExportTutorial", $"StepFile: Solid = {config.StepFile.Solid}, Surfaces = {config.StepFile.Surfaces}, SewTolerance = {stepFileTolerance}");
        }

        //! [Code snippet params]

        //! [Code snippet iges]

        private void ImportIges()
        {
            EspritApplication.Configuration.IgesFile.LogFileMode = true;
            var igesFilePath = string.Empty;

            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                var espritDir = EspritApplication.Configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeEspritDrawings);
                openFileDialog.InitialDirectory = espritDir;
                openFileDialog.Filter = "Iges Files (*.igs)|*.igs|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Title = "Open Iges File";
                openFileDialog.ShowDialog();
                igesFilePath = openFileDialog.FileName;
            }

            if (igesFilePath == string.Empty)
            {
                return;
            }

            Document.MergeFile(igesFilePath);
            Document.Windows.ActiveWindow.Fit();
            Document.Refresh();
        }

        //! [Code snippet iges]

        public override void Execute()
        {
            ScanFileOptions();
            ImportIges();
        }

        public override string Name => "Configuration: File Import and Export Options";
        public override string HtmlPath => "html/file_import_export_tutorial.html";

    }
}
