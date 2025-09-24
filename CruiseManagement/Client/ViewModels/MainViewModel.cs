using Client.Commands;
using Client.Services;
using log4net;
using Shared.Domain;
using System.Collections.ObjectModel;
using System.Windows;
using Shared.Logging;

namespace Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CruiseServiceProxy _service;
        private readonly Shared.Commands.CommandManager _commandManager = new Shared.Commands.CommandManager();
        private readonly StateTracker _stateTracker;
        private ChartsViewModel _chartsViewModel;
        private static readonly ILog log = LogManager.GetLogger(typeof(MainViewModel));

        public ObservableCollection<VoyageViewModel> Voyages { get; set; } = new();
        public ObservableCollection<Ship> Ships { get; set; } = new();
        public ObservableCollection<Port> Ports { get; set; } = new();

        private VoyageViewModel _selectedVoyage;

        public VoyageViewModel SelectedVoyage
        {
            get => _selectedVoyage;
            set
            {
                if (SetProperty(ref _selectedVoyage, value))
                {
                    if (_selectedVoyage != null)
                    {
                        _selectedVoyage.RefreshAllProperties();
                    }
                }
            }
        }

        public ChartsViewModel ChartsVM
        {
            get => _chartsViewModel;
            private set => SetProperty(ref _chartsViewModel, value);
        }

        private string _searchTerm;

        public string SearchTerm
        {
            get => _searchTerm;
            set { SetProperty(ref _searchTerm, value); SearchVoyages(); }
        }

        private bool _showingCruises = false;
        public string ShowCruisesButtonText => _showingCruises ? "Show Voyages" : "Show Cruises";
        public string CurrentViewMessage => _showingCruises ? "Viewing Cruises (Adapted as Voyages)" : "Viewing Voyages";

        // Komande
        public System.Windows.Input.ICommand AddVoyageCommand { get; }

        public System.Windows.Input.ICommand EditVoyageCommand { get; }
        public System.Windows.Input.ICommand DeleteVoyageCommand { get; }
        public System.Windows.Input.ICommand SearchCommand { get; }
        public System.Windows.Input.ICommand AddShipCommand { get; }
        public System.Windows.Input.ICommand AddPortCommand { get; }
        public System.Windows.Input.ICommand UndoCommand { get; }
        public System.Windows.Input.ICommand RedoCommand { get; }
        public System.Windows.Input.ICommand SimulateStateChangeCommand { get; }
        public System.Windows.Input.ICommand ShowCruisesCommand { get; }

        public MainViewModel()
        {
            Logger.Info("MainViewModel initialized");

            _service = ServiceClientFactory.CreateClient();
            _stateTracker = new StateTracker(_service);
            ChartsVM = new ChartsViewModel(_stateTracker);

            // Inicijalizacija komandi
            AddVoyageCommand = new RelayCommand(_ => AddVoyage());
            EditVoyageCommand = new RelayCommand(_ => EditVoyage(), _ => SelectedVoyage != null);
            DeleteVoyageCommand = new RelayCommand(_ => DeleteVoyage(), _ => SelectedVoyage != null);
            SearchCommand = new RelayCommand(_ => SearchVoyages());
            AddShipCommand = new RelayCommand(_ => AddShip());
            AddPortCommand = new RelayCommand(_ => AddPort());
            UndoCommand = new RelayCommand(_ =>
            {
                _commandManager.Undo();
                ChartsVM?.RefreshCharts();
            }, _ => _commandManager.CanUndo);

            RedoCommand = new RelayCommand(_ =>
            {
                _commandManager.Redo();
                ChartsVM?.RefreshCharts();
            }, _ => _commandManager.CanRedo);
            SimulateStateChangeCommand = new RelayCommand(
                param => SimulateStateChange(param as VoyageViewModel),
                param => param is VoyageViewModel);
            //ShowCruisesCommand = new RelayCommand(_ => ShowCruises());

            LoadData();
        }

        public void LoadData()
        {
            try
            {
                Logger.Info("Loading data from service");

                Voyages.Clear();
                Ships.Clear();
                Ports.Clear();

                var voyages = _service.GetAllVoyages();
                Logger.Debug($"Retrieved {voyages.Count} voyages from service");
                foreach (var v in voyages)
                    Voyages.Add(new VoyageViewModel(v));

                var ships = _service.GetAllShips();
                Logger.Debug($"Retrieved {voyages.Count} ships from service");
                foreach (var s in ships)
                    Ships.Add(s);

                var ports = _service.GetAllPorts();
                Logger.Debug($"Retrieved {ports.Count} ports from service");
                foreach (var p in ports)
                    Ports.Add(p);

                ChartsVM?.RefreshCharts();

                Logger.Info("Data loading completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading data", ex);
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddVoyage()
        {
            try
            {
                Logger.Info("Starting voyage addition process");

                var voyageVm = new VoyageViewModel(new Voyage());
                var form = new Views.VoyageFormWindow(voyageVm, Ships, Ports);

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
                        Logger.Warn($"Voyage validation failed: {string.Join(", ", errors)}");
                        MessageBox.Show(string.Join("\n", errors), "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var cmd = new AddVoyageCommand(Voyages, voyageVm, _service);
                    _commandManager.ExecuteCommand(cmd);

                    ChartsVM?.OnVoyageStateChanged();
                    Logger.Info($"Voyage added successfully: {voyageVm.Code}");
                }

                if (form.DataContext is VoyageFormViewModel formViewModelAfter)
                {
                    formViewModelAfter.RequestAddNewShip -= ShowShipForm;
                    formViewModelAfter.RequestAddNewPort -= ShowPortForm;
                }

                Logger.Info("Voyage addition process completed");
            }
            catch (Exception ex)
            {
                Logger.Error("Error adding voyage", ex);
                MessageBox.Show($"Error adding voyage: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditVoyage()
        {
            if (SelectedVoyage == null) return;

            try
            {
                Logger.Info($"Starting voyage edit process: {SelectedVoyage.Code}");

                var oldVoyageVm = SelectedVoyage;
                var newVoyageVm = new VoyageViewModel(SelectedVoyage.Model.Clone());
                var form = new Views.VoyageFormWindow(newVoyageVm, Ships, Ports);

                if (form.DataContext is VoyageFormViewModel formViewModel)
                {
                    formViewModel.RequestAddNewShip += ShowShipForm;
                    formViewModel.RequestAddNewPort += ShowPortForm;
                }

                if (form.ShowDialog() == true)
                {
                    var errors = _service.GetVoyageValidationErrors(newVoyageVm.Model);
                    if (errors.Any())
                    {
                        MessageBox.Show(string.Join("\n", errors), "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var cmd = new EditVoyageCommand(Voyages, oldVoyageVm, newVoyageVm, this, _service);
                    _commandManager.ExecuteCommand(cmd);
                    SelectedVoyage = newVoyageVm;

                    ChartsVM?.OnVoyageStateChanged();
                }

                if (form.DataContext is VoyageFormViewModel formViewModelAfter)
                {
                    formViewModelAfter.RequestAddNewShip -= ShowShipForm;
                    formViewModelAfter.RequestAddNewPort -= ShowPortForm;
                }

                Logger.Info($"Voyage edited successfully: {SelectedVoyage.Code}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error editing voyage: {SelectedVoyage?.Code}", ex);
                MessageBox.Show($"Error editing voyage: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteVoyage()
        {
            if (SelectedVoyage == null) return;

            if (MessageBox.Show("Are you sure you want to delete this voyage?", "Delete",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Logger.Info($"Starting voyage deletion process: {SelectedVoyage.Code}");

                    var cmd = new DeleteVoyageCommand(Voyages, SelectedVoyage, _service);
                    _commandManager.ExecuteCommand(cmd);
                    SelectedVoyage = null;

                    ChartsVM?.OnVoyageStateChanged();
                    Logger.Info($"Voyage deleted successfully");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error deleting voyage: {SelectedVoyage?.Code}", ex);
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

        private void SimulateStateChange(VoyageViewModel voyageVm)
        {
            if (voyageVm == null) return;

            try
            {
                Logger.Info($"Simulating state change for voyage: {voyageVm.Code}");

                var oldState = voyageVm.Status;
                voyageVm.Model.SimulateStateChange();
                var newState = voyageVm.Status;

                if (_service.UpdateVoyage(voyageVm.Model))
                {
                    voyageVm.RefreshAllProperties();
                    ChartsVM?.OnVoyageStateChanged();

                    Logger.Info($"Voyage state changed: {voyageVm.Code} from {oldState} to {newState}");
                    MessageBox.Show($"State changed to: {voyageVm.Status}", "Success",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error simulating state change for voyage: {voyageVm.Code}", ex);
                MessageBox.Show($"Error simulating state change: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void ShowCruises()
        //{
        //    try
        //    {
        //        if (!_showingCruises)
        //        {
        //            // Prikaži cruise-ove kao voyage-ove
        //            var cruises = _service.GetAllCruises();
        //            Voyages.Clear();

        //            foreach (var cruise in cruises)
        //            {
        //                var adapter = new CruiseVoyageAdapter(cruise);
        //                Voyages.Add(new VoyageViewModel(adapter));
        //            }

        //            _showingCruises = true;
        //        }
        //        else
        //        {
        //            // Vrati se na standardne voyage-ove
        //            var voyages = _service.GetAllVoyages();
        //            Voyages.Clear();

        //            foreach (var v in voyages)
        //                Voyages.Add(new VoyageViewModel(v));

        //            _showingCruises = false;
        //        }

        //        SelectedVoyage = null;
        //        ChartsVM?.RefreshCharts();

        //        OnPropertyChanged(nameof(ShowCruisesButtonText));
        //        OnPropertyChanged(nameof(CurrentViewMessage));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error showing cruises: {ex.Message}", "Error",
        //                       MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
    }
}