using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class CheckingSecurityTutorial : BaseTutorial
    {
        public CheckingSecurityTutorial(Esprit.Application app): base(app)
        {
        }

        // [Code snippet]

        private string LicenseTypeName(int type)
        {
            switch(type)
            {
                case 0:
                    return "Normal";
                case 1:
                    return "Demonstration";
                case 2:
                    return "Dealer";
                case 3:
                    return "Education";
                case 4:
                    return "Student";
            }

            return "Error";
        }

        public void DisplayLicenseOptions()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;
            
            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "CheckingSecurityTutorial", $"License type: {LicenseTypeName(EspritApplication.LicenseType)}");

            var message = EspritApplication.OptionIsValid(715)
                ? "STL Import/Export option is included in this license."
                : "STL Import/Export option is not included in this license.";

            MessageBox.Show(message, "CheckingSecurityTutorial", MessageBoxButton.OK);

            for (var i = 0; i < 50; i++)
            {
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "CheckingSecurityTutorial", $"Option {i} is {EspritApplication.OptionIsValid(i)}");
            }
        }

        // [Code snippet]

        public override void Execute()
        {
            DisplayLicenseOptions();
        }

        public override string Name => "Checking Security";
        public override string HtmlPath => "html/checking_security_tutorial.html";

    }
}
