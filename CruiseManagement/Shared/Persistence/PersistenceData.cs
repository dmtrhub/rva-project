using Shared.Domain;

namespace Shared.Persistence
{
    [Serializable]
    public class PersistenceData
    {
        public List<Voyage> Voyages { get; set; } = new List<Voyage>();

        public List<Ship> Ships { get; set; } = new List<Ship>();

        public List<Port> Ports { get; set; } = new List<Port>();

        public List<Cruise> Cruises { get; set; } = new List<Cruise>();
    }
}