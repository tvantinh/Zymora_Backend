using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zymora.Models.DTOs.User;
using Zymora.Models.Settings;
using Zymora_BE.Contract.Repositories.Entities;
using Zymora_BE.Contract.Services.IService;

namespace Zymora.Controllers
{
    //test authentication
    [Route("api/[controller]/[action]")]
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
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest user)
    {
        bool userExists = await _userService.CheckUserExists(user.Email, user.UserName);
        if (userExists)
        {
            return Conflict(new
            {
                error = "User already exists",
                message = "A user with the same email or username already exists"
            });
        }
        string PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
        User newUser = new User
        {
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            PasswordHash = PasswordHash
        };
        User UserResponse = await _userService.CreateUser(newUser);
        
        return Ok();
    }
  }
}