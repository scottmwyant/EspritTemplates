using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialCSharp.Tutorials.Helpers
{
    public static class WindowHelper
    {

        //! [Code snippet fit windows]

        public static void FitAllWindows(Esprit.Windows windows)
        {
            foreach (Esprit.Window window in windows)
            {
                window.Fit();
                window.Refresh();
            }
        }

        //! [Code snippet fit windows]
    }
}
