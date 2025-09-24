using System.Runtime.Serialization;

namespace Shared.Domain
{
    [DataContract]
    public class Cruise : BaseVoyage
    {
        [DataMember]
        public DateTime ArrivalTime { get; set; }

        [DataMember]
        public TimeSpan Duration { get; set; }

        [DataMember]
        public int StopsNumber { get; set; }

        [DataMember]
        public string CruiseName { get; set; }

        [DataMember]
        public string Description { get; set; }

        public Cruise() : base("TEM-111-111") // Pozovite base constructor sa default vrednošću
        {
        }

        public Cruise(string code) : base(code)
        {
        }

        public Cruise(string code, string cruiseName, DateTime arrivalTime, TimeSpan duration, int stopsNumber, string description = "")
            : base(code)
        {
            CruiseName = cruiseName;
            ArrivalTime = arrivalTime;
            Duration = duration;
            StopsNumber = stopsNumber;
            Description = description;
        }
    }
}