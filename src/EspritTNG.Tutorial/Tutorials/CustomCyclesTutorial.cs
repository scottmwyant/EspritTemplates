using System;
using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    class CustomCyclesTutorial : BaseTutorial
    {
        //! [Code snippet]

        private void CreateCustomCycleForBoltHole()
        {
            double patternDiameter = 10;
            int numberOfHoles = 6;
            double startAngle = 10;
            
            if (!RequestUserInput("Enter Pattern Diameter", "Pattern Diameter", ref patternDiameter) ||
                !RequestUserInput("Enter Number of Holes", "Holes", ref numberOfHoles) ||
                !RequestUserInput("Enter Start Angle", "Start Angle", ref startAngle))
            {
                return;
            }

            Esprit.Point centerPoint = null;
            try
            {
                centerPoint = Document.GetPoint("Pick Pattern Center");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Point was not set
                return;
            }

            var millCustom = new EspritTechnology.TechMillCustom
            {
                ChannelID = 1,
                OperationName = "Mill Custom Technology",
                CustomSetting1 = 9020,
                CustomSetting2 = centerPoint.X,
                CustomSetting3 = centerPoint.Y,
                CustomSetting4 = patternDiameter,
                CustomSetting5 = numberOfHoles,
                CustomSetting6 = startAngle,
                Comment = "BOLT HOLE CANNED CYCLE"
            };

            try
            {
                Document.PartOperations.Add(millCustom);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Invalid technology");
            }

        }

        //! [Code snippet]

        public CustomCyclesTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            CreateCustomCycleForBoltHole();
        }

        public override string Name => "Custom Cycles";
        public override string HtmlPath => "html/custom_cycles_tutorial.html";

    }
}
