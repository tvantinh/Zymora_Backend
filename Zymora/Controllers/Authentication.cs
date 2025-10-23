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

namespace Zymora.Controllers
{
  [Route("auth/[action]")]
  [ApiController]
  public class Authentication(IUserService userService, IJWTService JWTService) : ControllerBase
  {
    private readonly IUserService _userService = userService;
    private readonly IJWTService _jwtService = JWTService;
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] FormLogin user)
    {
      // B1: Tìm user trong database
      User? userExist = await _userService.CheckUserExistsByUserName(user.UserName);

      if (userExist is null)
      {
        // User không tồn tại - trả về 401 Unauthorized
        return Unauthorized(new
        {
          error = "Invalid credentials",
          message = "Username or password is incorrect"
        });
      }

      // B2: Verify password (giả sử bạn có method VerifyPassword)
      bool isPasswordValid = await _userService.VerifyPassword(userExist.Id, user.Password);

      if (!isPasswordValid)
      {
        // Sai password - trả về 401 Unauthorized
        return Unauthorized(new
        {
          error = "Invalid credentials",
          message = "Username or password is incorrect"
        });
      }

      // B3: Tạo token và trả về khi thành công
      LoginResponse token = await _jwtService.GenerateToken(userExist);
      return Ok(token);
    }


  }
  public class FormLogin
  {
    [Required(ErrorMessage = "UserName là bắt buộc")]
    [MinLength(8, ErrorMessage = "UserName phải có ít nhất 8 ký tự")]
    public required string UserName { get; set; }
    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
    public required string Password { get; set; }
  }
}
