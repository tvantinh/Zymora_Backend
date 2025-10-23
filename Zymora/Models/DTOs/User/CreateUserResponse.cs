namespace Zymora.Models.DTOs.User
{
  public class CreateUserResponse
  {
    public required Guid UserId { get; set; }
    public required string UserName { get; set; }
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}
