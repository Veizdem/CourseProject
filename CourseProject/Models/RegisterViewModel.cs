using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Паролі не збігаються.")]
    public string ConfirmPassword { get; set; }
}