using Core.Enums;
using Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public class Voyage : BaseVoyage
{
    [Required(ErrorMessage = "Arrival time is required")]
    public DateTime ArrivalTime { get; set; }

    [Required(ErrorMessage = "Departure time is required")]
    [DateRange("ArrivalTime", ErrorMessage = "Departure time must be before arrival time")]
    public DateTime DepartureTime { get; set; }

    [StringLength(500, ErrorMessage = "Captain message cannot exceed 500 characters")]
    public string CaptainMessage { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Distance must be positive")]
    public double Distance { get; set; }

    public VoyageStatus Status { get; set; } = VoyageStatus.Scheduled;

    // Asocijacije
    public Port DeparturePort { get; set; }
    public Port ArrivalPort { get; set; }
    public Ship Ship { get; set; }

    public Voyage() : base() { }

    public Voyage(string code) : base(code) { }

    public void VoyageSpecs()
    {
        Console.WriteLine($"Voyage {Code}: {DepartureTime} to {ArrivalTime}, Distance: {Distance} nautical miles");
    }

    public override string ToString() =>
        $"{Code}: {DeparturePort?.Code} -> {ArrivalPort?.Code} ({Status})";
}