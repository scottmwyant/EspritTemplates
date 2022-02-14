using ESPRIT.NetApi.Extensions;
using System.ComponentModel;
using System.ComponentModel.Composition;

// To debug Extension

// 1) In the properties in the build tab set the output folder to 

// C:\Users\Public\Documents\D.P.Technology\ESPRIT (current version of Esprit)\Data\Extensions

// 2) In the debug tab set the "Start Action" -> "Start External Program" set the value to the Esprit.exe path.  

// 3) In the debug tab set the "Start Options" -> "Working Directory" to the value to the folder of the Esprit.exe.

// You can now start the project F5 and debug.

namespace EspritTNG.Basic
{
    [Export(typeof(IExtension))]
    [ExportMetadata("SupportBuild", 20)] //this must match the major build of Esprit you compile for
    public class Main : IExtension
    {
        public string Description => "EspritTNG.Basic description";
        public string Name => "EspritTNG.Basic";
        public string Publisher => "PublisherName goes Here";
        public string Url => @"http://www.espritcam.com";

        internal static Esprit.Application _espritApplication = null;

        public void Connect(object app)
        {
            _espritApplication = app as Esprit.Application;
        }

        public void Disconnect()
        {
        }
    }
}