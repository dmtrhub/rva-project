using Shared.Domain;

namespace Shared.Persistence
{
    public interface IDataPersistenceStrategy
    {
        void SaveData(List<Voyage> voyages, List<Ship> ships, List<Port> ports, List<Cruise> cruises);

        (List<Voyage>, List<Ship>, List<Port>, List<Cruise>) LoadData();
    }
}