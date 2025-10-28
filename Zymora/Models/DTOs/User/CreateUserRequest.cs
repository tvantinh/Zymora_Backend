using System.ComponentModel.DataAnnotations;

namespace Zymora.Models.DTOs.User
{
  public class CreateUserRequest
  {
    [Required(ErrorMessage = "Username is required")]
    [StringLength(256, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 256 characters")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit")]
    public string Password { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }

  }
}
