using System.ComponentModel;
using System.ComponentModel.Composition;
using ESPRIT.NetApi.Extensions;

// To debug Extension

// 1) In the properties in the build tab set the output folder to 

// C:\Users\Public\Documents\D.P.Technology\ESPRIT (current version of Esprit)\Data\Extensions

// 2) In the debug tab set the "Start Action" -> "Start External Program" set the value to the Esprit.exe path.  

// 3) In the debug tab set the "Start Options" -> "Working Directory" to the value to the folder of the Esprit.exe.

// You can now start the project F5 and debug.

namespace TutorialCSharp
{
    [Export(typeof(IExtension))]
    [ExportMetadata("SupportBuild", 20)] //this must match the major build of Esprit you compile for
    public class Main : IExtension
    {
        internal string ExtDescription = "ESPRIT API samples written in C#";
        internal string ExtName = "C# Tutorials";
        internal string ExtPublisher = "DP Technology";
        internal string ExtUrl = @"http://www.espritcam.com";

        internal static Esprit.Application EspritApplication;
        private AddinUi _addinUi;

        public void Connect(object app)
        {
            EspritApplication = app as Esprit.Application;
            _addinUi = new AddinUi(EspritApplication);
        }

        public string Description => ExtDescription;

        public void Disconnect()
        {
            _addinUi?.Dispose();
        }

        public string Name => ExtName;

        public string Publisher => ExtPublisher;

        public string Url => ExtUrl;
    }
}