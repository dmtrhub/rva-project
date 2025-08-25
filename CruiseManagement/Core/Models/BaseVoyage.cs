using Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public abstract class BaseVoyage
{
    [Required(ErrorMessage = "Code is required")]
    [VoyageCode(ErrorMessage = "Voyage code must be in format: ABC-123-456")]
    public string Code { get; protected set; }

    protected BaseVoyage(string code)
    {
        Code = code;
    }

    protected BaseVoyage() { }

    public override string ToString() => Code;
}
