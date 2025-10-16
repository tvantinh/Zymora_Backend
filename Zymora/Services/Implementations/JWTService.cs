using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Zymora.Models.DTOs.Authentication;
using Zymora.Models.Settings;
using Zymora.Services.Helper;
using Zymora.Services.Interfaces;
using Zymora_BE.Contract.Repositories.Entities;
using Zymora_BE.Repositories.DataContext;

namespace Zymora.Services.Implementations
{
  public class JWTService (JWTSettings settings, DatabaseContext db) : IJWTService
  {
    private readonly JWTSettings _settings = settings;
    private readonly DatabaseContext _context = db;

    public async Task<LoginResponse> GenerateToken(User user)
    {
      List<Claim> claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      };
      string? secretKey = _settings.SecretKey;
      if (string.IsNullOrEmpty(secretKey))
      {
        throw new ArgumentNullException(nameof(secretKey));
      }

      string token = JWTHelper.GenerateToken(
         claims: claims,
         expirationMinutes: Convert.ToInt32(_settings.ExpirationMinutes),
         secretKey: secretKey
     );

      string refreshToken = JWTHelper.GenerateRefreshTokenAsync(user.Id);

      await RevokeRefreshToken(refreshToken, user); // Handle adding a new refreshToken into the database and deleting old refreshToken.
      var expires = DateTime.UtcNow.AddMinutes(
      Convert.ToDouble(_settings.ExpirationMinutes)
      );

      return new LoginResponse { AccessToken = token, RefreshToken = refreshToken, ExpiresAt = expires };
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
      throw new NotImplementedException();
    }

    public string? GetUserIdFromToken(string token)
    {
      throw new NotImplementedException();
    }

    public Task<LoginResponse> RefreshToken(string token, string refreshToken)
    {
      throw new NotImplementedException();
    }

    public Task RemoveExpiredRefreshTokens()
    {
      throw new NotImplementedException();
    }

    public Task RevokeAllRefreshTokens(string userId)
    {
      throw new NotImplementedException();
    }

    public async Task RevokeRefreshToken(string refreshToken, User user)
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

    public Task<bool> ValidateAccessToken(string token)
    {
      throw new NotImplementedException();
    }

    public Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
    {
      throw new NotImplementedException();
    }

    ClaimsPrincipal? IJWTService.GetPrincipalFromExpiredToken(string token)
    {
      throw new NotImplementedException();
    }

    Task<LoginResponse> IJWTService.RefreshToken(string token, string refreshToken)
    {
      throw new NotImplementedException();
    }
  }
}
