using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Zymora.Models.Settings;
using Zymora.Models.DTOs.Authentication;
using Zymora.Services.Interfaces;
using Zymora_BE.Contract.Repositories.Entities;
using Zymora_BE.Contract.Services.IService;
using System.ComponentModel;

namespace Zymora.Controllers
{
  [Route("auth")]
  [ApiController]
  public class Authentication(IUserService userService, IJWTService JWTService) : ControllerBase
  {
    private readonly IUserService _userService = userService;
    private readonly IJWTService _jwtService = JWTService;
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] FormLogin user)
    {
      User? userExist = await _userService.CheckUserExistsByUserName(user.UserName);

      if (userExist is null)
      {
        return Unauthorized(new
        {
          error = "Invalid credentials",
          message = "Username or password is incorrect"
        });
      }

      bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, userExist.PasswordHash);

      if (!isPasswordValid)
      {
        return Unauthorized(new
        {
          error = "Invalid credentials",
          message = "Username or password is incorrect"
        });
      }

      LoginResponse token = await _jwtService.GenerateToken(userExist);
      return Ok(token);
    }


  }
  public class FormLogin
  {

    [Required(ErrorMessage = "UserName là bắt buộc")]
    [MinLength(4, ErrorMessage = "UserName phải có ít nhất 4 ký tự")]
    [DefaultValue("tinh1")]
    public required string UserName { get; set; }
    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
    [DefaultValue("Tinh123.")]
    public required string Password { get; set; }
  }
}
