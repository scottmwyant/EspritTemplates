using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TutorialCSharp.Tutorials.Tutorial_CustomOperationSetupSheet
{
    /// <summary>
    /// Interaction logic for BoltHolePattern.xaml
    /// </summary>
    public partial class OperationSetupSheetDialog : Window
    {
        public OperationSetupSheetDialog()
        {
            InitializeComponent();
        }

        private void OnApplyButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
