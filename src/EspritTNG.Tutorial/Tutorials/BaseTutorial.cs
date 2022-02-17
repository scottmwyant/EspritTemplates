using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Esprit;

namespace TutorialCSharp.Tutorials
{
    public abstract class BaseTutorial: ITutorial
    {
        protected BaseTutorial(Esprit.Application app)
        {
            EspritApplication = app;
        }

        protected Esprit.Application EspritApplication { get; }

        protected Document Document => EspritApplication.Document;

        protected bool RequestUserInput(string title, string promptText, ref string value)
        {
            return InputBoxDialog.Show(title, promptText, ref value) == DialogResult.OK;
        }

        protected bool RequestUserInput(string title, string promptText, ref double value, bool showConvertingErrorMessage = false)
        {
            var stringValue = value.ToString();
            if (InputBoxDialog.Show(title, promptText, ref stringValue) != DialogResult.OK)
            {
                return false;
            };

            var result = double.TryParse(stringValue, out value);
            if (!result && showConvertingErrorMessage)
            {
                System.Windows.MessageBox.Show("Error while converting to Double.", $"Tutorial {Name}", System.Windows.MessageBoxButton.OK);
            }

            return result;
        }

        protected bool RequestUserInput(string title, string promptText, ref int value, bool showConvertingErrorMessage = false)
        {
            var stringValue = value.ToString();
            if (InputBoxDialog.Show(title, promptText, ref stringValue) != DialogResult.OK)
            {
                return false;
            };

            var result = int.TryParse(stringValue, out value);
            if (!result && showConvertingErrorMessage)
            {
                System.Windows.MessageBox.Show("Error while converting to Int.", $"Tutorial {Name}", System.Windows.MessageBoxButton.OK);
            }

            return result;
        }

        public abstract string Name { get; }
        public abstract string HtmlPath { get; }
        public abstract void Execute();
    }
}
