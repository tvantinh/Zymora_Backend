using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Zymora.Authentication;
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
    public async Task<IActionResult> login([FromBody] FormLogin user)
    {
      //b1 tìm dưới db coi có account đó không 
      User? userExist = await _userService.CheckUserExistsByUserName(user.UserName);
      if (userExist is null)
      {
        throw new Exception("User không tồn tại");
      }
      else
      {
        //b2 nếu có kiểm tra lại password thì tạo token và trả về
        string token = _jwtService.GenerateToken(userExist);
      }
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
