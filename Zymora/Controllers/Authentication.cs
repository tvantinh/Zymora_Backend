using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Zymora.Controllers
{
  [Route("auth/[action]")]
  [ApiController]
  public class Authentication : ControllerBase
  {
    [HttpPost("login")]
    public async Task<IActionResult> login([FromBody] FormLogin user)
    {
      return Ok(new { message = "Login successful", email = user.getEmail() } );
    }
    
  }
  public class FormLogin
  {
    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    private string Email { get; set; }
    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
    private string Password { get; set; }

    public FormLogin(string email,string password)
    {
      Email = email;
      Password = password;
    }
    public void setEmail(string email) => Email = email;
    public void setPassword(string password) => Password = password;
    public string getEmail() => Email;
    public string getPassword() => Password;

  }
}
