namespace Shared.Domain
{
    public class Cruise : BaseVoyage
    {
        public DateTime ArrivalTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int StopsNumber { get; set; }
    }
}