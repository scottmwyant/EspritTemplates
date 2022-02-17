using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class CreateMachineSetupTutorial : BaseTutorial
    {
        public CreateMachineSetupTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet AddBarStock]

        private void AddBarStock()
        {
            Document.PartSetup.BeginEdit();

            Esprit.Workpiece workpiece = null;
            if (Document.Workpieces.Count > 0)
            {
                workpiece = Document.Workpieces[1];
            }
            else
            {
                workpiece = Document.Workpieces.Add();
            }

            var barStock = workpiece.Stocks.AddBarStock();
            barStock.Tolerance = 0.1;
            barStock.OutsideDiameter = 50;

            var alignment = new EspritConstants.STOCKALIGNMENT { AlignmentType = EspritConstants.espStockAlignmentType.espStockAlignmentTypeCentered, Size = 200 };
            barStock.set_LengthAlignment(alignment);

            Document.PartSetup.EndEdit();
        }

        //! [Code snippet AddBarStock]
        //! [Code snippet CreateMachineSetup]

        private void CreateMachineSetup()
        {
            Document.InitialMachineSetup.BeginEdit();

            for(var i = Document.InitialMachineSetup.MachineItems.Count; i > 0; i--)
            {
                Document.InitialMachineSetup.MachineItems.Remove(i);
            }

            try
            {
                Document.InitialMachineSetup.MachineFileName = EspritApplication.Configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeMachineSetup) + "/Mori_Seiki_NLX2500/Mori_Seiki_NLX2500.mprj";
            }
            catch(System.Runtime.InteropServices.COMException)
            {
                MessageBox.Show("Fail on changing machine setup. To run this tutorial create new document and try again.");
                return;
            }

            var fixtureResourceDirectoryPath = EspritApplication.Configuration.GetFileDirectory(EspritConstants.espFileType.espFileTypeFixtures);

            try
            { 
                var fixture = Document.InitialMachineSetup.MachineItems.AddFixture(fixtureResourceDirectoryPath + "\\Chuck_OD_D5toD200.gdml") as Esprit.IFixture;
                fixture.Name = "Fixture";
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                MessageBox.Show("Fail on adding fixture.");
                return;
            }


            var workpieceItem = Document.InitialMachineSetup.MachineItems[1].MachineItems.AddWorkpieceInstance();
            workpieceItem.Name = "Chuck_OD_D5toD200_Workpiece";
            workpieceItem.WorkOffsets[1].WorkOffsetType = EspritConstants.espWorkOffsetType.espWorkOffsetTypeCustom;

            Document.InitialMachineSetup.EndEdit();

            if (MessageBox.Show("Create Bar Stock?", "Load Machine Setup Tutorial", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                AddBarStock();
            }

            if (MessageBox.Show("Create other fixtures?", "Load Machine Setup Tutorial", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Document.InitialMachineSetup.BeginEdit();

                try
                {
                    var tailstock = Document.InitialMachineSetup.MachineItems.AddTailstock(fixtureResourceDirectoryPath + "\\Chuck_OD_D5toD200.gdml") as Esprit.IFixture;
                    tailstock.Name = "Tailstock";
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    MessageBox.Show("Fail on adding TailStock.");
                }

                try
                {
                    var steadyRest = Document.InitialMachineSetup.MachineItems.AddSteadyRest(fixtureResourceDirectoryPath + "\\Chuck_OD_D5toD200.gdml") as Esprit.IFixture;
                    steadyRest.Name = "SteadyRest";
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    MessageBox.Show("Fail on adding SteadyRest.");
                }

                try
                {
                    var rotaryTable = Document.InitialMachineSetup.MachineItems.AddRotaryTable(fixtureResourceDirectoryPath + "\\Samples\\Rotary Tables\\HRT160.gdml") as Esprit.IFixture;
                    rotaryTable.Name = "RotaryTable";
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    MessageBox.Show("Fail on adding RotaryTable.");
                }

                Document.InitialMachineSetup.EndEdit();
            }
        }

        //! [Code snippet CreateMachineSetup]

        public override void Execute()
        {
            CreateMachineSetup();
        }

        public override string Name => "Create Machine Setup";
        public override string HtmlPath => "html/create_machine_setup_tutorial.html";

    }
}
