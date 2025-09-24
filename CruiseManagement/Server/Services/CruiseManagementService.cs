using log4net;
using Shared.Contracts;
using Shared.Domain;
using Shared.Enums;
using Shared.Logging;
using Shared.Persistence;
using Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Server.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CruiseManagementService : ICruiseManagementService
    {
        private readonly List<Voyage> _voyages = new List<Voyage>();
        private readonly List<Ship> _ships = new List<Ship>();
        private readonly List<Port> _ports = new List<Port>();
        private readonly List<Cruise> _cruises = new List<Cruise>();
        private readonly IDataPersistenceStrategy _persistenceStrategy;
        private readonly IValidatorFactory _validatorFactory;
        private static readonly ILog log = LogManager.GetLogger(typeof(CruiseManagementService));

        public CruiseManagementService(IValidatorFactory validatorFactory, IDataPersistenceStrategy persistanceStrategy)
        {
            try
            {
                Logger.Info("Initializing CruiseManagementService");

                _persistenceStrategy = persistanceStrategy;
                _validatorFactory = validatorFactory;
                LoadData();

                if (!_voyages.Any())
                {
                    InitializeSampleData();
                    SaveData();
                }

                Logger.Info("CruiseManagementService initialized successfully");
            }
            catch (Exception ex)
            {
                Logger.Fatal($"Failed to initialize CruiseManagementService: {ex.Message}");
                throw;
            }
        }

        private void InitializeSampleData()
        {
            Logger.Info("Initializing sample data");

            var ship1 = new Ship
            {
                Name = "Ocean Queen",
                Capacity = 2000,
                Type = ShipType.Passenger,
                LengthInMeters = 300,
                DraftInMeters = 8.5
            };

            var port1 = new Port
            {
                Name = "Port of Rotterdam",
                Country = "Netherlands",
                Code = "ROT",
                Latitude = 51.8850,
                Longitude = 4.2690
            };

            var port2 = new Port
            {
                Name = "Port of Hamburg",
                Country = "Germany",
                Code = "HAM",
                Latitude = 53.5394,
                Longitude = 9.9872
            };

            _ships.Add(ship1);
            _ports.Add(port1);
            _ports.Add(port2);

            _voyages.Add(new Voyage
            {
                Code = "PCB-011-381",
                DepartureTime = DateTime.Now.AddDays(1),
                ArrivalTime = DateTime.Now.AddDays(5),
                CaptainMessage = "First voyage scheduled",
                Distance = 0,
                Ship = ship1,
                DeparturePort = port1,
                ArrivalPort = port2
            });

            _cruises.Add(new Cruise("CRS-001-003")
            {
                ArrivalTime = DateTime.Now.AddDays(3),
                Duration = TimeSpan.FromDays(2),
                StopsNumber = 5
            });

            _cruises.Add(new Cruise("KCZ-002-005")
            {
                ArrivalTime = DateTime.Now.AddDays(31),
                Duration = TimeSpan.FromDays(4),
                StopsNumber = 4
            });
        }

        private void LoadData()
        {
            try
            {
                Logger.Info("Loading data from persistence");
                var (voyages, ships, ports, cruises) = _persistenceStrategy.LoadData();

                _voyages.Clear();
                _voyages.AddRange(voyages);

                _ships.Clear();
                _ships.AddRange(ships);

                _ports.Clear();
                _ports.AddRange(ports);

                _cruises.Clear();
                _cruises.AddRange(cruises);

                Logger.Info($"Loaded {_voyages.Count} voyages, {_ships.Count} ships, {_ports.Count} ports");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to load data: {ex.Message}");
                throw;
            }
        }

        private void SaveData()
        {
            try
            {
                Logger.Info("Saving data to persistence");
                _persistenceStrategy.SaveData(_voyages, _ships, _ports, _cruises);
                Logger.Info("Data saved successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to save data: {ex.Message}");
                throw;
            }
        }

        public List<Voyage> GetAllVoyages()
        {
            Logger.Debug("Getting all voyages");
            return _voyages;
        }

        public List<Voyage> SearchVoyages(string searchTerm)
        {
            Logger.Info($"Searching voyages with term: {searchTerm}");

            if (string.IsNullOrWhiteSpace(searchTerm))
                return _voyages;

            string searchTermLower = searchTerm.ToLower();

            return _voyages.Where(v =>
                v != null &&
                ((v.Code != null && v.Code.ToLower().Contains(searchTermLower)) ||
                 (v.CaptainMessage != null && v.CaptainMessage.ToLower().Contains(searchTermLower)))
            ).ToList();
        }

        public bool AddVoyage(Voyage voyage)
        {
            try
            {
                Logger.Info($"Adding voyage: {voyage.Code}");

                var validator = new VoyageValidator(_voyages);
                var result = validator.Validate(voyage);

                if (!result.IsValid)
                {
                    Logger.Warn($"Validation failed for voyage: {voyage.Code}");
                    foreach (var error in result.Errors)
                    {
                        Logger.Warn($"Validation error: {error.ErrorMessage}");
                    }
                    return false;
                }

                if (_voyages.Any(v => v.Code == voyage.Code))
                {
                    Logger.Warn($"Voyage with code {voyage.Code} already exists");
                    return false;
                }

                _voyages.Add(voyage);
                SaveData();

                Logger.Info($"Voyage {voyage.Code} added successfully");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to add voyage {voyage.Code}: {ex.Message}");
                return false;
            }
        }

        public bool UpdateVoyage(Voyage voyage)
        {
            try
            {
                Logger.Info($"Updating voyage: {voyage.Code}");

                var otherVoyages = _voyages.Where(v => v.Code != voyage.Code).ToList();
                var validator = new VoyageValidator(otherVoyages);
                var result = validator.Validate(voyage);

                if (!result.IsValid)
                {
                    Logger.Warn($"Validation failed for voyage: {voyage.Code}");
                    foreach (var error in result.Errors)
                    {
                        Logger.Warn($"Validation error: {error.ErrorMessage}");
                    }
                    return false;
                }

                var existing = _voyages.FirstOrDefault(v => v.Code == voyage.Code);
                if (existing == null)
                {
                    Logger.Warn($"Voyage with code {voyage.Code} not found");
                    return false;
                }

                _voyages.Remove(existing);
                _voyages.Add(voyage);
                SaveData();

                Logger.Info($"Voyage {voyage.Code} updated successfully");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to update voyage {voyage.Code}: {ex.Message}");
                return false;
            }
        }

        public bool DeleteVoyage(string voyageCode)
        {
            try
            {
                Logger.Info($"Deleting voyage: {voyageCode}");

                var voyage = _voyages.FirstOrDefault(v => v.Code == voyageCode);
                if (voyage == null)
                {
                    Logger.Warn($"Voyage with code {voyageCode} not found");
                    return false;
                }

                _voyages.Remove(voyage);
                SaveData();

                Logger.Info($"Voyage {voyageCode} deleted successfully");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to delete voyage {voyageCode}: {ex.Message}");
                return false;
            }
        }

        public bool AddShip(Ship ship)
        {
            try
            {
                Logger.Info($"Adding ship: {ship.Name}");

                if (!ValidateShip(ship))
                {
                    Logger.Warn($"Validation failed for ship: {ship.Name}");
                    return false;
                }

                if (_ships.Any(s => s.Name == ship.Name))
                {
                    Logger.Warn($"Ship with name {ship.Name} already exists");
                    return false;
                }

                _ships.Add(ship);
                SaveData();

                Logger.Info($"Ship {ship.Name} added successfully");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to add ship {ship.Name}: {ex.Message}");
                return false;
            }
        }

        public bool AddPort(Port port)
        {
            try
            {
                Logger.Info($"Adding port: {port.Name}");

                var validator = new PortValidator(_ports);
                var result = validator.Validate(port);

                if (!result.IsValid)
                {
                    Logger.Warn($"Validation failed for port: {port.Name}");
                    foreach (var error in result.Errors)
                    {
                        Logger.Warn($"Validation error: {error.ErrorMessage}");
                    }
                    return false;
                }

                if (_ports.Any(p => p.Code == port.Code))
                {
                    Logger.Warn($"Port with code {port.Code} already exists");
                    return false;
                }

                _ports.Add(port);
                SaveData();

                Logger.Info($"Port {port.Name} added successfully");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to add port {port.Name}: {ex.Message}");
                return false;
            }
        }

        public List<Ship> GetAllShips()
        {
            Logger.Debug("Getting all ships");
            return _ships;
        }

        public List<Port> GetAllPorts()
        {
            Logger.Debug("Getting all ports");
            return _ports;
        }

        public bool SimulateStateChange(string voyageCode)
        {
            try
            {
                Logger.Info($"Simulating state change for voyage: {voyageCode}");

                var voyage = _voyages.FirstOrDefault(v => v.Code == voyageCode);
                if (voyage == null)
                {
                    Logger.Warn($"Voyage with code {voyageCode} not found");
                    return false;
                }

                voyage.SimulateStateChange();
                SaveData();

                Logger.Info($"State changed for voyage {voyageCode}. New status: {voyage.Status}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to simulate state change for voyage {voyageCode}: {ex.Message}");
                return false;
            }
        }

        public bool ValidateVoyage(Voyage voyage)
        {
            if (voyage == null) return false;

            var validator = _validatorFactory.GetVoyageValidator();
            var result = validator.Validate(voyage);
            return result.IsValid;
        }

        public bool ValidateShip(Ship ship)
        {
            if (ship == null) return false;

            var validator = _validatorFactory.GetShipValidator();
            var result = validator.Validate(ship);
            return result.IsValid;
        }

        public bool ValidatePort(Port port)
        {
            if (port == null) return false;

            var validator = _validatorFactory.GetPortValidator();
            var result = validator.Validate(port);
            return result.IsValid;
        }

        public List<string> GetVoyageValidationErrors(Voyage voyage)
        {
            if (voyage == null)
                return new List<string> { "Voyage cannot be null" };

            var validator = _validatorFactory.GetVoyageValidator();
            var result = validator.Validate(voyage);
            return result.Errors.Select(e => e.ErrorMessage).ToList();
        }

        public List<string> GetShipValidationErrors(Ship ship)
        {
            if (ship == null)
                return new List<string> { "Ship cannot be null" };

            var validator = _validatorFactory.GetShipValidator();
            var result = validator.Validate(ship);
            return result.Errors.Select(e => e.ErrorMessage).ToList();
        }

        public List<string> GetPortValidationErrors(Port port)
        {
            if (port == null)
                return new List<string> { "Port cannot be null" };

            var validator = _validatorFactory.GetPortValidator();
            var result = validator.Validate(port);
            return result.Errors.Select(e => e.ErrorMessage).ToList();
        }

        public List<Cruise> GetAllCruises()
        {
            return _cruises;
        }
    }
}