using Shared.Enums;

namespace Shared.Domain
{
    public class Ship
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public double LengthInMeters { get; set; }
        public ShipType Type { get; set; }
        public double DraftInMeters { get; set; }

        public string ShipSpecs()
        {
            return $"{Name} ({Type}) - Capacity: {Capacity}, Length: {LengthInMeters}m, Draft: {DraftInMeters}m";
        }
    }
}