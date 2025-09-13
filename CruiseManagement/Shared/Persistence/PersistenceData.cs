using Shared.Domain;

namespace Shared.Persistence
{
    [Serializable]
    public class PersistenceData
    {
        public IEnumerable<Voyage> Voyages { get; set; } = new List<Voyage>();
        public IEnumerable<Ship> Ships { get; set; } = new List<Ship>();
        public IEnumerable<Port> Ports { get; set; } = new List<Port>();
    }
}