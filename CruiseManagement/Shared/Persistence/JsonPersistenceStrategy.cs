using Shared.Domain;
using System.Text.Json;

namespace Shared.Persistence
{
    public class JsonPersistenceStrategy : IDataPersistenceStrategy
    {
        private readonly string _filePath;

        public JsonPersistenceStrategy(string filePath)
        {
            _filePath = filePath;
        }

        public void SaveData(List<Voyage> voyages, List<Ship> ships, List<Port> ports, List<Cruise> cruises)
        {
            var data = new PersistenceData { Voyages = voyages, Ships = ships, Ports = ports, Cruises = cruises };
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(_filePath, json);
        }

        public (List<Voyage>, List<Ship>, List<Port>, List<Cruise>) LoadData()
        {
            if (!File.Exists(_filePath))
                return (new List<Voyage>(), new List<Ship>(), new List<Port>(), new List<Cruise>());

            using (var stream = File.OpenRead(_filePath))
            {
                var data = JsonSerializer.DeserializeAsync<PersistenceData>(stream).Result;
                return (data.Voyages, data.Ships, data.Ports, data.Cruises);
            }
        }
    }
}