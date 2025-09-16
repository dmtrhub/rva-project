using Client.ViewModels;
using Shared.Domain;
using System.Windows;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for PortFormWindow.xaml
    /// </summary>
    public partial class PortFormWindow : Window
    {
        public PortFormWindow()
        {
            InitializeComponent();
            DataContext = new PortViewModel(new Port());
        }

        public PortViewModel PortViewModel => DataContext as PortViewModel;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}