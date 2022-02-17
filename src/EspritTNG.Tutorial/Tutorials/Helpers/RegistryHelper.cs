using Microsoft.Win32;

namespace TutorialCSharp.Tutorials.Helpers
{
    public static class RegistryHelper
    {
        public static RegistryKey GetRegistry()
        {
            return Registry.CurrentUser.CreateSubKey("Software\\D.P.Technology\\Esprit\\Tutorial");
        }
    }
}
