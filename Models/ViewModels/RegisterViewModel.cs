using System.ComponentModel.DataAnnotations;

namespace Samo.Models.ViewModels;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long!", MinimumLength = 8)]
    public string Password { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string SureName { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    public string Achieve { get; set; }
}