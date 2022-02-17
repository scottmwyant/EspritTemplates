using System;
using System.Text;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class ScanPartSetupTutorial : BaseTutorial
    {
        public ScanPartSetupTutorial(Esprit.Application app) : base(app)
        {
        }

        //! [Code snippet Display functions]

        private string AlignmentTypeString(EspritConstants.espStockAlignmentType value)
        {
            var result = string.Empty;
            switch (value)
            {
                case EspritConstants.espStockAlignmentType.espStockAlignmentTypeCentered:
                    result = "Centered";
                    break;
                case EspritConstants.espStockAlignmentType.espStockAlignmentTypePositive:
                    result = "Left";
                    break;
                case EspritConstants.espStockAlignmentType.espStockAlignmentTypeNegative:
                    result = "Right";
                    break;
                case EspritConstants.espStockAlignmentType.espStockAlignmentTypeMargins:
                    result = "Left right margins";
                    break;
                case EspritConstants.espStockAlignmentType.espStockAlignmentTypePosition:
                    result = "Position";
                    break;
            }

            return result;
        }

        private void ScanPart(Esprit.IPart part, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine($"Part \"{part.Name}\" info:");
            stringBuilder.AppendLine($"\tMaterial:  {part.MaterialClassName}");
            stringBuilder.AppendLine($"\tMaterialCondition: {part.MaterialConditionName}");
            stringBuilder.AppendLine($"\tMaterialStandard: {part.MaterialStandard}");

            foreach (Esprit.Solid solid in part.Geometry)
            {
                stringBuilder.AppendLine($"\t{part.Name} add geometry: {solid.TypeName} {solid.Key}");
            }

            foreach (Esprit.PartOperation operation in part.PartOperations)
            {
                stringBuilder.AppendLine($"\tOperation: {operation.Name}");
            }

            Esprit.MachineSetup machine = null;

            try
            {
                machine = part.GetMachineSetupForNewOperation(1);
            }
            catch (Exception)
            {
            }

            if (machine != null)
            {
                stringBuilder.AppendLine($"\tMachine FilePath: {machine.MachineFileName}");
            }
        }

        private void ScanStock(Esprit.IStock stock, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine($"Stock \"{stock.Name}\" info:");
            stringBuilder.AppendLine($"\tColor: {stock.Color}");

            switch (stock.StockType)
            {
                case EspritConstants.espStockType.espStockTypeBar:
                    var barStock = stock as Esprit.BarStock;
                    stringBuilder.AppendLine($"\tType: Bar");
                    stringBuilder.AppendLine($"\tTolerance: {barStock.Tolerance}");
                    stringBuilder.AppendLine($"\tInside Diameter: {barStock.InsideDiameter}");
                    stringBuilder.AppendLine($"\tInside Diameter: {barStock.OutsideDiameter}");

                    stringBuilder.AppendLine($"\tAlignment Type: {AlignmentTypeString(barStock.get_LengthAlignment().AlignmentType)}");
                    stringBuilder.AppendLine($"\tAlignment Value: {barStock.get_LengthAlignment().Size}");
                    stringBuilder.AppendLine($"\tAlignment Margin: {barStock.get_LengthAlignment().Margin}");
                    break;

                case EspritConstants.espStockType.espStockTypeBlock:
                    var blockStock = stock as Esprit.BlockStock;
                    stringBuilder.AppendLine($"\tType: Block");

                    stringBuilder.AppendLine($"\tLength Alignment Type: {AlignmentTypeString(blockStock.get_LengthAlignment().AlignmentType)}");
                    stringBuilder.AppendLine($"\tLength Alignment Value: {blockStock.get_LengthAlignment().Size} ");
                    stringBuilder.AppendLine($"\tLength Alignment Margin: {blockStock.get_LengthAlignment().Margin}");

                    stringBuilder.AppendLine($"\tHeight Alignment Type:  {AlignmentTypeString(blockStock.get_HeightAlignment().AlignmentType)}");
                    stringBuilder.AppendLine($"\tHeight Alignment Value:  {blockStock.get_HeightAlignment().Size}");
                    stringBuilder.AppendLine($"\tHeight Alignment Margin: {blockStock.get_HeightAlignment().Margin}");

                    stringBuilder.AppendLine($"\tWidth Alignment Type: {AlignmentTypeString(blockStock.get_WidthAlignment().AlignmentType)}");
                    stringBuilder.AppendLine($"\tWidth Alignment Value: {blockStock.get_WidthAlignment().Size}");
                    stringBuilder.AppendLine($"\tWidth Alignment Margin: {blockStock.get_WidthAlignment().Margin}");
                    break;

                case EspritConstants.espStockType.espStockTypeSolid:
                    var solidStock = stock as Esprit.SolidStock;
                    stringBuilder.AppendLine($"\tType: Solid");
                    stringBuilder.AppendLine($"\tTolerance: {solidStock.Tolerance}");

                    if (solidStock.Solid != null)
                    {
                        stringBuilder.AppendLine($"\tStock solid: {solidStock.Solid.Type}");
                    }
                    break;

                case EspritConstants.espStockType.espStockTypeFile:
                    var fileStock = stock as Esprit.FileStock;

                    stringBuilder.AppendLine($"\tType: File");
                    stringBuilder.AppendLine($"\tStock file path: {fileStock.FileName}");

                    stringBuilder.AppendLine($"\tXTranslation: {fileStock.XTranslation}");
                    stringBuilder.AppendLine($"\tYTranslation: {fileStock.YTranslation}");
                    stringBuilder.AppendLine($"\tZTranslation: {fileStock.ZTranslation}");

                    stringBuilder.AppendLine($"\tXRotation: {fileStock.XRotation}");
                    stringBuilder.AppendLine($"\tYRotation: {fileStock.YRotation}");
                    stringBuilder.AppendLine($"\tZRotation: {fileStock.ZRotation}");
                    break;
            }
        }

        //! [Code snippet Display functions]
        //! [Code snippet Scan]

        private void ScanPartSetupTree()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            foreach (Esprit.Workpiece wp in Document.Workpieces)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Workpiece name: {wp.Name}");

                foreach (Esprit.IPart part in wp.Parts)
                {
                    ScanPart(part, stringBuilder);
                }

                foreach (Esprit.Stock stock in wp.Stocks)
                {
                    ScanStock(stock, stringBuilder);
                }

                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanPartSetupTutorial", stringBuilder.ToString());
            }
        }

        //! [Code snippet Scan]

        public override void Execute()
        {
            ScanPartSetupTree();
        }

        public override string Name => "Scan Part Setup Data";
        public override string HtmlPath => "html/scan_part_setup_tutorial.html";

    }
}
