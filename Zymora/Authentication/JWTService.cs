using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Zymora.DTOs.Authentication;
using Zymora_BE.Contract.Repositories.Entities;
using Zymora_BE.Repositories.DataContext;

namespace Zymora.Authentication
{
  public class JWTService
  {
    private readonly JwtSettings _settings;
    private readonly IConfiguration _configuration;
    private readonly DatabaseContext _context;
    public JWTService(IOptions<JwtSettings> options, IConfiguration configuration, DatabaseContext context)
    {
      _settings = options.Value;
      _configuration = configuration;
      _context = context;
    }
    public async Task<LoginResponse> GenerateToken(User user, List<Roles> rolesUser)
    {

      List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            };

      foreach (var role in rolesUser)
      {
        claims.Add(new Claim(ClaimTypes.Role, role.Name ?? string.Empty));
      }

      string? secretKey = _configuration["JWT:secretKey"];

      if (string.IsNullOrEmpty(secretKey))
      {
        throw new ArgumentNullException(nameof(secretKey));
      }

      string token = JWTHelper.GenerateToken(
          claims: claims,
          expirationMinutes: Convert.ToInt32(_configuration["JWT:expirationMinutes"]),
          secretKey: secretKey
      );

      string refreshToken = JWTHelper.GenerateRefreshTokenAsync(user.Id);

      await RevokeRefreshToken(refreshToken, user);

      var expires = DateTime.UtcNow.AddMinutes(
      Convert.ToDouble(_configuration["Jwt:expirationMinutes"])
      );

      return new LoginResponse { AccessToken = token, RefreshToken = refreshToken, ExpiresAt = expires };
    }
    
    public async Task RevokeRefreshToken(string refreshToken, User user) // Handle adding a new refreshToken into the database and deleting old refreshToken.
    {
      UserToken userToken = new UserToken
      {
        UserId = user.Id,
        LoginProvider = "JWT",
        Name = "RefreshToken",
        Value = refreshToken,
        User = user
      };
      UserToken? exstingToken = _context.User_tokens.FirstOrDefault(ut => ut.Value == refreshToken && ut.LoginProvider == "JWT" && ut.Name == "RefreshToken");
      if (exstingToken != null)
      {
        _context.User_tokens.Remove(exstingToken);
      }
      _context.User_tokens.Add(userToken);
      await _context.SaveChangesAsync();
    }
    public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
    {
      var token = await _context.User_tokens
          .FirstOrDefaultAsync(t =>
              t.UserId == userId &&
              t.LoginProvider == "JWT" &&
              t.Name == "RefreshToken" &&
              t.Value == refreshToken);

      return token != null;
    }
  }
}
