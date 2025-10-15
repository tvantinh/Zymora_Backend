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
  public class JWTService (JWTSettings settings, IConfiguration config) : IJWTService
  {
    private readonly JWTSettings _settings = settings;
    private readonly IConfiguration _configuration = config;

    public Task<LoginResponse> GenerateToken(User user)
    {
      throw new NotImplementedException();
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

    public Task RevokeRefreshToken(string refreshToken, User user)
    {
      throw new NotImplementedException();
    }

    public Task<bool> ValidateAccessToken(string token)
    {
      throw new NotImplementedException();
    }

    public Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
    {
      throw new NotImplementedException();
    }
  }
}
