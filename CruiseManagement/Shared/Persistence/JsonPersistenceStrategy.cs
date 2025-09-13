using Shared.Domain;
using System.Text.Json;
using System.IO;

namespace Shared.Persistence
{
    public class JsonPersistenceStrategy : IDataPersistenceStrategy
    {
        private readonly string _filePath;

        public JsonPersistenceStrategy(string filePath)
        {
            _filePath = filePath;
        }

        public void SaveData(IEnumerable<Voyage> voyages, IEnumerable<Ship> ships, IEnumerable<Port> ports)
        {
            var data = new PersistenceData { Voyages = voyages, Ships = ships, Ports = ports };
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(_filePath, json);
        }

        public (IEnumerable<Voyage>, IEnumerable<Ship>, IEnumerable<Port>) LoadData()
        {
            if (!File.Exists(_filePath))
                return (new List<Voyage>(), new List<Ship>(), new List<Port>());

            using (var stream = File.OpenRead(_filePath))
            {
                var data = JsonSerializer.DeserializeAsync<PersistenceData>(stream).Result;
                return (data.Voyages, data.Ships, data.Ports);
            }
        }
    }
}