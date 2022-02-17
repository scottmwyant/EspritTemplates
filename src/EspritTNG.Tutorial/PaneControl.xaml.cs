using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TutorialCSharp.ViewModels;

namespace TutorialCSharp
{
    /// <summary>
    /// Interaction logic for PaneControl.xaml
    /// </summary>
    public partial class PaneControl : UserControl
    {
        private PaneViewModel Model => DataContext as PaneViewModel;
        public PaneControl()
        {
            InitializeComponent();
        }

        private void TreeViewItemSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Model.SelectedItem = e.NewValue as TutorialModel;
        }
    }
}
