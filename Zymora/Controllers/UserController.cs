using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Zymora_BE.Contract.Repositories.Entities;
using Zymora_BE.Contract.Services.IService;

namespace Zymora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            IList<User> Users = await _userService.GetAll();
            return Ok(Users);

        }
        [HttpGet("exception")]
        public IActionResult ThrowTestException()
        {
            throw new InvalidOperationException("Đây là lỗi test cố tình ném ra.");
        }

        [HttpPost("From")]
        public  IActionResult Login([FromForm] Form login)
        {
            if (login.Username == "admin" && login.Password == "123")
            {
                return Ok(new
                {
                    success = true,
                    message = "Đăng nhập thành công"
                });
            }

                return BadRequest();
            

        }

    }
    public class Form
    {
        [Required(ErrorMessage = "Username không được để trống")]
        public required string Username { get; set; }
        [Required(ErrorMessage = "Password không được để trống")]
        [MinLength(3, ErrorMessage = "Password phải ít nhất 6 ký tự")]
        public required string Password { get; set; }
    }
}
