using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Zymora.Authentication;
using Zymora_BE.Contract.Repositories.Entities;
using Zymora_BE.Contract.Services.IService;

namespace Zymora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly JWTService _jwt;

        public UserController(JWTService jwt)
        {
            //_userService = userService;
            _jwt = jwt;
        }
        //[HttpGet]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    IList<User> Users = await _userService.GetAll();
        //    return Ok(Users);

        //}
        //[HttpGet("exception")]
        //public IActionResult ThrowTestException()
        //{
        //    throw new InvalidOperationException("Đây là lỗi test cố tình ném ra.");
        //}

        //[HttpPost("From")]
        //public  async Task<IActionResult> Login([FromForm] Form login)
        //{
        //    if (login.Username == "admin" && login.Password == "123")
        //    {
        //        jwt.GenerateToken("admin","123","admin");
        //    }


        //}
        [HttpPost("login")]
        public IActionResult Login(LoginRequest req)
        {
            var token = _jwt.GenerateToken(userId: "123", userName: "alice", role: "Admin");
            return Ok(new { access_token = token });
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
