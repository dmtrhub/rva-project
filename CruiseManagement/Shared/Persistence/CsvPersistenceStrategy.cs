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

        public void SaveData(List<Voyage> voyages, List<Ship> ships, List<Port> ports, List<Cruise> cruises)
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var writer = new StreamWriter(_filePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteField("VOYAGES_DATA");
                csv.NextRecord();
                csv.WriteRecords(voyages);

                csv.NextRecord();
                csv.WriteField("SHIPS_DATA");
                csv.NextRecord();
                csv.WriteRecords(ships);

                csv.NextRecord();
                csv.WriteField("PORTS_DATA");
                csv.NextRecord();
                csv.WriteRecords(ports);

                csv.NextRecord();
                csv.WriteField("CRUISES_DATA");
                csv.NextRecord();
                csv.WriteRecords(cruises);
            }
        }

        public (List<Voyage>, List<Ship>, List<Port>, List<Cruise>) LoadData()
        {
            if (!File.Exists(_filePath))
                return (new List<Voyage>(), new List<Ship>(), new List<Port>(), new List<Cruise>());

            var voyages = new List<Voyage>();
            var ships = new List<Ship>();
            var ports = new List<Port>();
            var cruises = new List<Cruise>();

            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                string currentSection = "";

                while (csv.Read())
                {
                    if (csv.GetField(0) == "VOYAGES_DATA")
                    {
                        currentSection = "VOYAGES";
                        csv.Read(); // Preskace header liniju
                    }
                    else if (csv.GetField(0) == "SHIPS_DATA")
                    {
                        currentSection = "SHIPS";
                        csv.Read();
                    }
                    else if (csv.GetField(0) == "PORTS_DATA")
                    {
                        currentSection = "PORTS";
                        csv.Read();
                    }
                    else if (csv.GetField(0) == "CRUISES_DATA")
                    {
                        currentSection = "CRUISES";
                        csv.Read();
                    }
                    else
                    {
                        switch (currentSection)
                        {
                            case "VOYAGES":
                                voyages.Add(csv.GetRecord<Voyage>());
                                break;

                            case "SHIPS":
                                ships.Add(csv.GetRecord<Ship>());
                                break;

                            case "PORTS":
                                ports.Add(csv.GetRecord<Port>());
                                break;

                            case "CRUISES":
                                cruises.Add(csv.GetRecord<Cruise>());
                                break;
                        }
                    }
                }
            }

            return (voyages, ships, ports, cruises);
        }
    }
}