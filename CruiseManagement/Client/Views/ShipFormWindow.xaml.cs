using System.Windows;
using Client.ViewModels;
using Shared.Domain;

namespace Client.Views
{
    public partial class ShipFormWindow : Window
    {
        public ShipFormWindow()
        {
            InitializeComponent();
            DataContext = new ShipViewModel(new Ship());
        }

        public ShipViewModel ShipViewModel => DataContext as ShipViewModel;

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