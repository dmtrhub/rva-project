namespace Shared.Domain
{
    public class Voyage
    {
        public string Code { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public string CaptainMessage { get; set; }
        public double Distance { get; set; }

        public string VoyageSpecs()
        {
            return $"Voyage {Code} | {DepartureTime} - {ArrivalTime} | Distance: {Distance} km";
        }
    }
}