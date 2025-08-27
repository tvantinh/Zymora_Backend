using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Zymora.Authentication;
using Zymora_BE.Contract.Repositories.Entities;
using Zymora_BE.Contract.Services.IService;

namespace Zymora.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            IList<User> Users = await _userService.GetAll();
            return Ok(Users);
        }
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
        //    [HttpPost("login")]
        //public IActionResult Login(LoginRequest req)
        //{
        //    // TODO: validate user...
        //    var token = _jwt.GenerateToken(userId: "123", userName: "alice", role: "Admin");
        //    return Ok(new { access_token = token });
        //}


    }
}
