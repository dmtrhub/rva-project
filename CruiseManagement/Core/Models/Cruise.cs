using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public class Cruise : BaseVoyage
{
    [Required(ErrorMessage = "Arrival time is required")]
    public DateTime ArrivalTime { get; set; }

    [Required(ErrorMessage = "Duration is required")]
    public TimeSpan Duration { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stop number must be positive")]
    public int StopNumber { get; set; }

    public Cruise() : base() { }

    public Cruise(string code) : base(code) { }

    public override string ToString() => $"{Code}: {ArrivalTime}, Duration: {Duration.TotalDays} days, Stops: {StopNumber}";
}