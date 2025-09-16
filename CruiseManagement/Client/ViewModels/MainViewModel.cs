using Shared.Domain;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Client.Services;
using System.Windows;

namespace Client.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly CruiseServiceProxy _service;

        public ObservableCollection<VoyageViewModel> Voyages { get; set; } = new();
        public ObservableCollection<Ship> Ships { get; set; } = new();
        public ObservableCollection<Port> Ports { get; set; } = new();

        private VoyageViewModel _selectedVoyage;

        public VoyageViewModel SelectedVoyage
        {
            get => _selectedVoyage;
            set { _selectedVoyage = value; OnPropertyChanged(nameof(SelectedVoyage)); }
        }

        private string _searchTerm;

        public string SearchTerm
        {
            get => _searchTerm;
            set { _searchTerm = value; OnPropertyChanged(nameof(SearchTerm)); SearchVoyages(); }
        }

        public ICommand AddVoyageCommand { get; }
        public ICommand EditVoyageCommand { get; }
        public ICommand DeleteVoyageCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand AddShipCommand { get; }
        public ICommand AddPortCommand { get; }

        public MainViewModel()
        {
            _service = ServiceClientFactory.CreateClient();

            AddVoyageCommand = new RelayCommand(_ => AddVoyage());
            EditVoyageCommand = new RelayCommand(_ => EditVoyage(), _ => SelectedVoyage != null);
            DeleteVoyageCommand = new RelayCommand(_ => DeleteVoyage(), _ => SelectedVoyage != null);
            SearchCommand = new RelayCommand(_ => SearchVoyages());
            RefreshCommand = new RelayCommand(_ => LoadData());
            AddShipCommand = new RelayCommand(_ => AddShip());
            AddPortCommand = new RelayCommand(_ => AddPort());

            LoadData();
        }

        public void LoadData()
        {
            try
            {
                Voyages.Clear();
                Ships.Clear();
                Ports.Clear();

                var voyages = _service.GetAllVoyages();
                foreach (var v in voyages)
                    Voyages.Add(new VoyageViewModel(v));

                var ships = _service.GetAllShips();
                foreach (var s in ships)
                    Ships.Add(s);

                var ports = _service.GetAllPorts();
                foreach (var p in ports)
                    Ports.Add(p);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddVoyage()
        {
            try
            {
                var voyageVm = new VoyageViewModel(new Voyage());
                var form = new Views.VoyageFormWindow(voyageVm, Ships, Ports);

                if (form.DataContext is VoyageFormViewModel formViewModel)
                {
                    formViewModel.RequestAddNewShip += ShowShipForm;
                    formViewModel.RequestAddNewPort += ShowPortForm;
                }

                if (form.ShowDialog() == true)
                {
                    // Validacija na serveru
                    var errors = _service.GetVoyageValidationErrors(voyageVm.Model);
                    if (errors.Any())
                    {
                        MessageBox.Show(string.Join("\n", errors), "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (_service.AddVoyage(voyageVm.Model))
                    {
                        Voyages.Add(voyageVm);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add voyage.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Ukloni event handlere
                if (form.DataContext is VoyageFormViewModel formViewModelAfter)
                {
                    formViewModelAfter.RequestAddNewShip -= ShowShipForm;
                    formViewModelAfter.RequestAddNewPort -= ShowPortForm;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding voyage: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditVoyage()
        {
            if (SelectedVoyage == null) return;

            try
            {
                var voyageVm = new VoyageViewModel(SelectedVoyage.Model.Clone());
                var form = new Views.VoyageFormWindow(voyageVm, Ships, Ports);

                // Preuzmi VoyageFormViewModel i dodaj event handlere
                if (form.DataContext is VoyageFormViewModel formViewModel)
                {
                    formViewModel.RequestAddNewShip += ShowShipForm;
                    formViewModel.RequestAddNewPort += ShowPortForm;
                }

                if (form.ShowDialog() == true)
                {
                    var errors = _service.GetVoyageValidationErrors(voyageVm.Model);
                    if (errors.Any())
                    {
                        MessageBox.Show(string.Join("\n", errors), "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (_service.UpdateVoyage(voyageVm.Model))
                    {
                        // Osveži prikaz
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update voyage.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Ukloni event handlere
                if (form.DataContext is VoyageFormViewModel formViewModelAfter)
                {
                    formViewModelAfter.RequestAddNewShip -= ShowShipForm;
                    formViewModelAfter.RequestAddNewPort -= ShowPortForm;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing voyage: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteVoyage()
        {
            if (SelectedVoyage == null) return;

            if (MessageBox.Show("Are you sure you want to delete this voyage?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if (_service.DeleteVoyage(SelectedVoyage.Code))
                    {
                        Voyages.Remove(SelectedVoyage);
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete voyage.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting voyage: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SearchVoyages()
        {
            try
            {
                Voyages.Clear();
                var voyages = string.IsNullOrWhiteSpace(SearchTerm)
                    ? _service.GetAllVoyages()
                    : _service.SearchVoyages(SearchTerm);

                foreach (var v in voyages)
                    Voyages.Add(new VoyageViewModel(v));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching voyages: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddShip()
        {
            try
            {
                var shipForm = new Views.ShipFormWindow();
                if (shipForm.ShowDialog() == true && shipForm.ShipViewModel != null)
                {
                    var errors = _service.GetShipValidationErrors(shipForm.ShipViewModel.Model);
                    if (errors.Any())
                    {
                        MessageBox.Show(string.Join("\n", errors), "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (_service.AddShip(shipForm.ShipViewModel.Model))
                    {
                        Ships.Add(shipForm.ShipViewModel.Model);
                        MessageBox.Show("Ship added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add ship.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding ship: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddPort()
        {
            try
            {
                var portForm = new Views.PortFormWindow();
                if (portForm.ShowDialog() == true && portForm.PortViewModel != null)
                {
                    var errors = _service.GetPortValidationErrors(portForm.PortViewModel.Model);
                    if (errors.Any())
                    {
                        MessageBox.Show(string.Join("\n", errors), "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (_service.AddPort(portForm.PortViewModel.Model))
                    {
                        Ports.Add(portForm.PortViewModel.Model);
                        MessageBox.Show("Port added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add port.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding port: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowShipForm()
        {
            AddShip();
        }

        private void ShowPortForm()
        {
            AddPort();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}