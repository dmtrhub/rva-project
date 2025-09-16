using Server.Services;
using Shared.Persistence;
using Shared.Validation;
using System.Configuration;
using System.ServiceModel;

internal class Program
{
    private static void Main(string[] args)
    {
        string dataFolder = ConfigurationManager.AppSettings["DataFolder"] ?? "data";
        string fileName = ConfigurationManager.AppSettings["DataFileName"] ?? "cruise_data";

        Console.WriteLine("Izaberite strategiju perzistencije (csv/xml/json):");
        var input = Console.ReadLine()?.Trim().ToLower();

        IDataPersistenceStrategy persistenceStrategy = input switch
        {
            "csv" => new CsvPersistenceStrategy(Path.Combine(dataFolder, $"{fileName}.csv")),
            "xml" => new XmlPersistenceStrategy(Path.Combine(dataFolder, $"{fileName}.xml")),
            "json" => new JsonPersistenceStrategy(Path.Combine(dataFolder, $"{fileName}.json")),
            _ => throw new InvalidOperationException("Nepoznata strategija!")
        };

        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }

        var validatorFactory = new ValidatorFactory();
        var serviceInstance = new CruiseManagementService(validatorFactory, persistenceStrategy);

        using (var host = new ServiceHost(serviceInstance))
        {
            host.Open();
            Console.WriteLine($"Service is running with {input} persistence. Press Enter to exit...");
            Console.ReadLine();
        }
    }
}