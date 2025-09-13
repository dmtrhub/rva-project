using CsvHelper;
using CsvHelper.Configuration;
using Shared.Domain;
using System.Globalization;

namespace Shared.Persistence
{
    public class CsvPersistenceStrategy : IDataPersistenceStrategy
    {
        private readonly string _filePath;

        public CsvPersistenceStrategy(string filePath)
        {
            _filePath = filePath;
        }

        public void SaveData(IEnumerable<Voyage> voyages, IEnumerable<Ship> ships, IEnumerable<Port> ports)
        {
            // Kreiraj direktorijum ako ne postoji
            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Koristi tri različite CSV datoteke za svaku entitet
            SaveVoyagesToCsv(voyages, Path.Combine(directory, "voyages.csv"));
            SaveShipsToCsv(ships, Path.Combine(directory, "ships.csv"));
            SavePortsToCsv(ports, Path.Combine(directory, "ports.csv"));
        }

        public (IEnumerable<Voyage>, IEnumerable<Ship>, IEnumerable<Port>) LoadData()
        {
            var directory = Path.GetDirectoryName(_filePath);

            var voyages = File.Exists(Path.Combine(directory, "voyages.csv"))
                ? LoadVoyagesFromCsv(Path.Combine(directory, "voyages.csv"))
                : new List<Voyage>();

            var ships = File.Exists(Path.Combine(directory, "ships.csv"))
                ? LoadShipsFromCsv(Path.Combine(directory, "ships.csv"))
                : new List<Ship>();

            var ports = File.Exists(Path.Combine(directory, "ports.csv"))
                ? LoadPortsFromCsv(Path.Combine(directory, "ports.csv"))
                : new List<Port>();

            return (voyages, ships, ports);
        }

        private void SaveVoyagesToCsv(IEnumerable<Voyage> voyages, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(voyages);
            }
        }

        private void SaveShipsToCsv(IEnumerable<Ship> ships, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(ships);
            }
        }

        private void SavePortsToCsv(IEnumerable<Port> ports, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(ports);
            }
        }

        private List<Voyage> LoadVoyagesFromCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<Voyage>().ToList();
            }
        }

        private List<Ship> LoadShipsFromCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<Ship>().ToList();
            }
        }

        private List<Port> LoadPortsFromCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<Port>().ToList();
            }
        }
    }
}