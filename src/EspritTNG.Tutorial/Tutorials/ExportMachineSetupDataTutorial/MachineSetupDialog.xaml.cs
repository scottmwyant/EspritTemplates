using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TutorialCSharp.Tutorials.Tutorial_ExportMachineSetupData
{
    public partial class MachineSetupDialog : Window
    {
        public MachineSetupDialog()
        {
            InitializeComponent();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
