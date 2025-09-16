using Client.ViewModels;
using Shared.Domain;
using System.Collections.ObjectModel;
using System.Windows;

namespace Client.Views
{
    public partial class VoyageFormWindow : Window
    {
        public VoyageFormWindow(VoyageViewModel voyageVm, ObservableCollection<Ship> ships, ObservableCollection<Port> ports)
        {
            InitializeComponent();

            var viewModel = new VoyageFormViewModel(voyageVm, ships, ports);
            DataContext = viewModel;
        }

        public VoyageFormWindow(VoyageFormViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
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