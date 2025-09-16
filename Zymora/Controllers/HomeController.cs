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
    public class HomeController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

    [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            IList<User> Users = await _userService.GetAll();
            return Ok(Users);
        }
    }
}
