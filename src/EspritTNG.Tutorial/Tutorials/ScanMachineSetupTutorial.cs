using System;
using System.Text;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class ScanMachineSetupTutorial : BaseTutorial
    {
        public ScanMachineSetupTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet Display functions]

        private string DisplayMachineItemType(EspritConstants.espMachineItemType value)
        {
            var result = "MachineItem";
            switch (value)
            {
                case EspritConstants.espMachineItemType.espMachineItemTypeFixture:
                    result = "Fixture";
                    break;
                case EspritConstants.espMachineItemType.espMachineItemTypeWorkpiece:
                    result = "Workpiece";
                    break;
            }

            return result;
        }

        private string DisplayAdaptorType(EspritConstants.espAdaptorType value)
        {
            var result = string.Empty;

            switch(value)
            {
                case EspritConstants.espAdaptorType.espAdaptorTypeFixture:
                    result = "Fixture adaptor";
                    break;
                case EspritConstants.espAdaptorType.espAdaptorTypeWorkpiece:
                    result = "Workpiece adaptor";
                    break;
                case EspritConstants.espAdaptorType.espAdaptorTypeJaw:
                    result = "Jaw adaptor";
                    break;
            }

            return result;
        }
    
        private string DisplayDoubleValue(double value)
        {
            return (Math.Abs(value) > 0.000001)
                ? value.ToString("F5")
                : "0";
        }

        private string DisplayMatrixData(EspritGeometryBase.IComMatrix matrix, string name)
        {
            string resultString = name;

            resultString += Environment.NewLine + "\tP.X: " + DisplayDoubleValue(matrix.P.X);
            resultString += Environment.NewLine + "\tP.Y: " + DisplayDoubleValue(matrix.P.Y);
            resultString += Environment.NewLine + "\tP.Z: " + DisplayDoubleValue(matrix.P.Z);

            resultString += Environment.NewLine + "\tU.X: " + DisplayDoubleValue(matrix.U.X);
            resultString += Environment.NewLine + "\tU.Y: " + DisplayDoubleValue(matrix.U.Y);
            resultString += Environment.NewLine + "\tU.Z: " + DisplayDoubleValue(matrix.U.Z);

            resultString += Environment.NewLine + "\tV.X: " + DisplayDoubleValue(matrix.V.X);
            resultString += Environment.NewLine + "\tV.Y: " + DisplayDoubleValue(matrix.V.Y);
            resultString += Environment.NewLine + "\tV.Z: " + DisplayDoubleValue(matrix.V.Z);

            resultString += Environment.NewLine + "\tW.X: " + DisplayDoubleValue(matrix.W.X);
            resultString += Environment.NewLine + "\tW.Y: " + DisplayDoubleValue(matrix.W.Y);
            resultString += Environment.NewLine + "\tW.Z: " + DisplayDoubleValue(matrix.W.Z);

            return resultString;
        }

        //! [Code snippet Display functions]
        //! [Code snippet Scan]

        private void ScanMachineItem(Esprit.MachineItem item)
        {
            var eventWindow = EspritApplication.EventWindow;

            var stringBuilder = new StringBuilder();

            var basicInfo = "Name: " + item.Name + Environment.NewLine;
            basicInfo += "Type: " + DisplayMachineItemType(item.MachineItemType);
            stringBuilder.AppendLine(basicInfo);

            if (item.ChildAdaptors.Count > 0)
            {
                var childAdaptorsInfo = "Child Adaprtors:";
                foreach (Esprit.Adaptor adaptor in item.ChildAdaptors)
                {
                    childAdaptorsInfo += Environment.NewLine + $"\tChild Adaptor {adaptor.Name} Type {DisplayAdaptorType(adaptor.AdaptorType)}";
                }

                stringBuilder.AppendLine(childAdaptorsInfo);
            }

            if (item.MountingAdaptors.Count > 0)
            {
                var mountingAdaptorsInfo = "Mounting Adaptors:";
                foreach (Esprit.Adaptor adaptor in item.MountingAdaptors)
                {
                    mountingAdaptorsInfo += Environment.NewLine + $"\tMounting Adaptor {adaptor.Name} Type {DisplayAdaptorType(adaptor.AdaptorType)}";
                }

                if (item.Mounting != null)
                {
                    mountingAdaptorsInfo += Environment.NewLine + "\tMounted Adaptor: " + item.Mounting.Name;
                }
                stringBuilder.AppendLine(mountingAdaptorsInfo);
            }

            stringBuilder.AppendLine(DisplayMatrixData(item.RelativeTransformation, "RelativeTransformation:"));

            stringBuilder.AppendLine(DisplayMatrixData(item.CumulativeTransformation, "CumulativeTransformation:"));

            switch(item.MachineItemType)
            {
                case EspritConstants.espMachineItemType.espMachineItemTypeFixture:
                    {
                        var fixture = item as Esprit.Fixture;
                        var fixtureInfo = "Fixture Data:";
                        fixtureInfo += Environment.NewLine + "\tClearance: " + fixture.SafetyDistance;
                        fixtureInfo += Environment.NewLine + "\tFileName: " + fixture.FileName;

                        stringBuilder.AppendLine(fixtureInfo);
                    }
                    break;
                case EspritConstants.espMachineItemType.espMachineItemTypeWorkpiece:
                    {
                        var workpieceItem  = item as Esprit.WorkpieceInstance;
                        var workpieceInfo = "Workpiece Data:";

                        if (workpieceItem.PartWorkpiece != null)
                        {
                            workpieceInfo += Environment.NewLine + "\tWorkpiece Name: " + workpieceItem.PartWorkpiece.Name;
                            workpieceInfo += Environment.NewLine + "\tParts Count: " + workpieceItem.PartWorkpiece.Parts.Count;
                        }

                        if (workpieceItem.WorkOffsets.Count > 0)
                        {
                            var offsetsInfo = "\tOffsets:";
                            foreach (Esprit.WorkOffset offset in workpieceItem.WorkOffsets)
                            {
                                offsetsInfo += Environment.NewLine + "\t\t" + offset.Name;
                            }

                            workpieceInfo += Environment.NewLine + offsetsInfo;
                        }

                        stringBuilder.AppendLine(workpieceInfo);

                    }
                    break;
            }

            eventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanMachineSetupTutorial", stringBuilder.ToString());
        }
        

        private void ScanMachineItemsReq(Esprit.MachineItem item)
        {
            ScanMachineItem(item);

            foreach(Esprit.MachineItem childItem in item.MachineItems)
            {
                ScanMachineItemsReq(childItem);
            }
        }

        private void ScanMachineItems()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            if (Document.InitialMachineSetup.MachineItems.Count == 0)
            {
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanMachineSetupTutorial", "There is empty machine");
            }
            else
            {
                foreach (Esprit.MachineItem item in Document.InitialMachineSetup.MachineItems)
                {
                    ScanMachineItemsReq(item);
                }
            }
        }

        //! [Code snippet Scan]

        public override void Execute()
        {
            ScanMachineItems();
        }

        public override string Name => "Scan Machine Setup";
        public override string HtmlPath => "html/scan_machine_setup_tutorial.html";

    }
}
