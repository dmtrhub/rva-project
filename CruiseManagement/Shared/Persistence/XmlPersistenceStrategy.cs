using Shared.Domain;
using System.Xml.Serialization;

namespace Shared.Persistence
{
    public class XmlPersistenceStrategy : IDataPersistenceStrategy
    {
        private readonly string _filePath;

        public XmlPersistenceStrategy(string filePath)
        {
            _filePath = filePath;
        }

        public void SaveData(List<Voyage> voyages, List<Ship> ships, List<Port> ports)
        {
            var data = new PersistenceData { Voyages = voyages, Ships = ships, Ports = ports };
            var serializer = new XmlSerializer(typeof(PersistenceData));

            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var writer = new StreamWriter(_filePath))
            {
                serializer.Serialize(writer, data);
            }
        }

        public (List<Voyage>, List<Ship>, List<Port>) LoadData()
        {
            if (!File.Exists(_filePath))
                return (new List<Voyage>(), new List<Ship>(), new List<Port>());

            var serializer = new XmlSerializer(typeof(PersistenceData));
            using (var reader = new StreamReader(_filePath))
            {
                var data = (PersistenceData)serializer.Deserialize(reader);
                return (data.Voyages, data.Ships, data.Ports);
            }
        }
    }
}