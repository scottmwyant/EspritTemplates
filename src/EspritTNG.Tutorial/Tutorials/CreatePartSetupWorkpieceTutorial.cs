using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class CreatePartSetupWorkpieceTutorial : BaseTutorial
    {
        public CreatePartSetupWorkpieceTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet Util]

        private void ClearWorkpiece(Esprit.Workpiece wp)
        {
            //Document.PartSetup.BeginEdit

            for (var i = wp.Stocks.Count; i > 0; i--)
            {
                wp.Stocks.Remove(i);
            }

            for (var i = wp.Parts.Count; i > 1; i--)
            {
                //Workpiece must contain 1 part as minimum
                wp.Parts.Remove(i);
            }

            //Document.PartSetup.EndEdit
        }

        private Esprit.Workpiece GetTestWorkpiece(bool isEmpty)
        {
            Esprit.Workpiece wp = null;

            foreach (Esprit.Workpiece item in Document.Workpieces)
            {
                if (item.Name == "Test workpiece")
                {
                    if (isEmpty)
                    {
                        ClearWorkpiece(item);
                    }

                    wp = item;
                    break;
                }
            }

            //Document.PartSetup.BeginEdit
            if (wp == null)
            {
                wp = Document.Workpieces.Add();
                wp.Name = "Test workpiece";
            }
            //Document.PartSetup.EndEdit
            return wp;
        }

        //! [Code snippet Util]
        //! [Code snippet Create Items]

        private Esprit.IStock AddBarStock()
        {
            //Document.PartSetup.BeginEdit
            var wp = GetTestWorkpiece(false);

            var bs = wp.Stocks.AddBarStock();
            bs.Tolerance = 0.1;
            bs.OutsideDiameter = 50;

            var alignment = new EspritConstants.STOCKALIGNMENT();
            alignment.AlignmentType = EspritConstants.espStockAlignmentType.espStockAlignmentTypeCentered;
            alignment.Size = 200;
            bs.set_LengthAlignment(alignment);

            //Document.PartSetup.EndEdit
            return bs;
        }

        private Esprit.IStock AddBlockStock()
        {
            //Document.PartSetup.BeginEdit
            var wp = GetTestWorkpiece(false);
            var bs = wp.Stocks.AddBlockStock();

            var alignment = new EspritConstants.STOCKALIGNMENT();
            var alignment1 = new EspritConstants.STOCKALIGNMENT();
            var alignment2 = new EspritConstants.STOCKALIGNMENT();

            alignment.AlignmentType = EspritConstants.espStockAlignmentType.espStockAlignmentTypeCentered;
            alignment.Size = 200;

            alignment1.AlignmentType = EspritConstants.espStockAlignmentType.espStockAlignmentTypeCentered;
            alignment1.Size = 100;

            alignment2.AlignmentType = EspritConstants.espStockAlignmentType.espStockAlignmentTypeCentered;
            alignment2.Size = 300;

            bs.set_LengthAlignment(alignment);
            bs.set_WidthAlignment(alignment1);
            bs.set_HeightAlignment(alignment2);
            //Document.PartSetup.EndEdit
            return bs;
        }

        private Esprit.IStock AddSolidStock()
        {
            if (Document.Solids.Count == 0)
            {
                return null;
            }

            Esprit.Solid solid = Document.Solids[1];

            //Document.PartSetup.BeginEdit
            var wp = GetTestWorkpiece(false);
            var ss = wp.Stocks.AddSolidStock();

            ss.Name = "Test Solid";
            ss.Tolerance = 0.1;
            ss.Solid = solid.SolidBody;
            //Document.PartSetup.EndEdit

            return ss;
        }

        private void CreatePart(Esprit.Workpiece wp)
        {
            //Document.PartSetup.BeginEdit
            var part = wp.Parts.Add();
            part.Name = "Test part";

            MessageBox.Show($"Part Name: {part.Name}", "Create Part Setup Workpiece Tutorial");

            foreach(Esprit.Solid solid in Document.Solids)
            {
                part.Geometry.Add(solid);
                MessageBox.Show($"{part.Name} add geometry: {solid.TypeName} {solid.Key}", "Create Part Setup Workpiece Tutorial");
            }
            //Document.PartSetup.EndEdit
        }

        //! [Code snippet Create Items]
        //! [Code snippet]

        private void CreateStocks(Esprit.Workpiece wp)
        {
            Esprit.IStock stock;
            stock = AddSolidStock();

            if (stock == null)
            {
                MessageBox.Show("We do not create a solid stock because your document does not have any solids", "Create Part Setup Workpiece Tutorial");
            }
            else
            {
                MessageBox.Show($"Stock: {stock.Name}; StockType: {stock.StockType}", "Create Part Setup Workpiece Tutorial");
            }

            stock = AddBarStock();
            MessageBox.Show($"Stock: {stock.Name}; StockType: {stock.StockType}", "Create Part Setup Workpiece Tutorial");

            stock = AddBlockStock();
            MessageBox.Show($"Stock: {stock.Name}; StockType: {stock.StockType}", "Create Part Setup Workpiece Tutorial");
        }

        private void CreateWorkpiece()
        {
            Document.PartSetup.BeginEdit();

            var wp = GetTestWorkpiece(true);

            CreatePart(wp);
            CreateStocks(wp);

            Document.PartSetup.EndEdit();
        }

        //! [Code snippet]

        public override void Execute()
        {
            CreateWorkpiece();
        }

        public override string Name => "Create Part Setup Workpiece";
        public override string HtmlPath => "html/create_part_setup_workpiece_tutorial.html";

    }
}
