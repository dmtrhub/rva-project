using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public class Ship
{
    [Required(ErrorMessage = "Ship name is required")]
    [StringLength(100, ErrorMessage = "Ship name cannot exceed 100 characters")]
    public string Name { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1")]
    public int Capacity { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Length must be positive")]
    public double LengthInMeters { get; set; }

    [Required(ErrorMessage = "Ship type is required")]
    public ShipType Type { get; set; } 

    [Range(0, double.MaxValue, ErrorMessage = "Draft must be positive")]
    public double DraftInMeters { get; set; }

    // Navigaciona property
    public ICollection<Voyage> Voyages { get; set; }

    public Ship()
    {
        Voyages = [];
    }

    public void ShipSpecs()
    {
        Console.WriteLine($"\nShip {Name}: \n\tCapacity: {Capacity}\n\tLength in meters: {LengthInMeters}\n\tType: {Type}\n\tDraft in meters: {DraftInMeters}\n");
    }

    public override bool Equals(object obj) => obj is Ship ship && Name == ship.Name;

    public override int GetHashCode() => Name.GetHashCode();

    public override string ToString() => $"{Name} ({Type})";
}
