using Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public class Port
{
    [Required(ErrorMessage = "Port name is required")]
    [StringLength(100, ErrorMessage = "Port name cannot exceed 100 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Country is required")]
    [StringLength(50, ErrorMessage = "Country name cannot exceed 50 characters")]
    public string Country { get; set; }

    [Required(ErrorMessage = "Port code is required")]
    [PortCode(ErrorMessage = "Port code must be exactly 3 uppercase letters")]
    public string Code { get; set; }

    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    public double Latitude { get; set; }

    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    public double Longitude { get; set; }

    // Navigacione properties - inicijalizacija u konstruktoru
    public ICollection<Voyage> DepartureVoyages { get; set; }
    public ICollection<Voyage> ArrivalVoyages { get; set; }

    public Port()
    {
        DepartureVoyages = [];
        ArrivalVoyages = [];
    }

    public override bool Equals(object obj) => obj is Port port && Code == port.Code;

    public override int GetHashCode() => Code.GetHashCode();

    public override string ToString() => $"{Name} ({Code})";
}