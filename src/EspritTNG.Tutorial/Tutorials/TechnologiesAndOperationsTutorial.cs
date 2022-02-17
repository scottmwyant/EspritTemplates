using System.Windows.Forms;
using EspritConstants;
using EspritTechnology;

namespace TutorialCSharp.Tutorials
{
    class TechnologiesAndOperationsTutorial : BaseTutorial
    {
        public TechnologiesAndOperationsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void MultiPassContourImproved()
        {
            Esprit.IGraphicObject obj = null;

            try
            {
                obj = Document.GetAnyElement("Pick Rough Mill Contour",
                    espGraphicObjectType.espOperation);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            var espOperation = obj as Esprit.PartOperation;
            if (espOperation == null)
            {
                System.Windows.MessageBox.Show("Must pick mill contour operation.", "TechnologiesAndOperationsTutorial");
                return;
            }
            var roughOpTech = espOperation.Technology as EspritTechnology.Technology;

            if (roughOpTech.TechnologyType != espTechnologyType.espTechMillContour1)
            {
                System.Windows.MessageBox.Show("Must pick mill contour operation.", "TechnologiesAndOperationsTutorial");
                return;
            }
            
            var numberOfPasses = 4;

            if (RequestUserInput("Enter the Total Number of Passes", "Number of Passes", ref numberOfPasses) && numberOfPasses > 1)
            {
                var mc = new EspritTechnology.TechMillContour1();
                roughOpTech.CopyTo(mc as ITechnology, true);

                var finalStock = 0.0;

                if (!RequestUserInput("Enter the Final Stock Allowance", "Final Stock ", ref finalStock))
                {
                    return;
                }

                var stockIncrement = (mc.StockAllowance - finalStock) / (numberOfPasses - 1);
                for (int i = 0; i < numberOfPasses; i++)
                {
                    mc.StockAllowance -= stockIncrement;

                    Esprit.PartOperation operation = null;
                    try
                    {
                        operation = Document.PartOperations.Add(mc, espOperation.Feature);
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                    }
                    if (operation != null)
                    {
                        operation.Color = espOperation.Color;
                        operation.LineType = espOperation.LineType;
                        operation.LineWeight = espOperation.LineWeight;
                    }
                }
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            MultiPassContourImproved();
        }

        public override string Name => "Using Technology From An Existing Operation on New Operations";
        public override string HtmlPath => "html/technologies_and_operations_tutorial.html";

    }
}
