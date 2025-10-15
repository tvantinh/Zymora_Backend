using System.Security.Claims;
using Zymora.DTOs.Authentication;
using Zymora_BE.Contract.Repositories.Entities;

namespace Zymora.Authentication
{
  public interface IJWTService
  {
    // Authentication & Token Generation
    Task<LoginResponse> GenerateToken(User user);
    Task<LoginResponse> RefreshToken(string token, string refreshToken);

    // Token Validation
    Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken);
    Task<bool> ValidateAccessToken(string token); // Thiếu - validate access token

    // Token Revocation
    Task RevokeRefreshToken(string refreshToken, User user);
    Task RevokeAllRefreshTokens(string userId); // Thiếu - revoke tất cả token của user

    // Token Information Extraction
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token); // Thiếu - lấy claims từ expired token
    string? GetUserIdFromToken(string token); // Thiếu - extract userId

    // Token Cleanup
    Task RemoveExpiredRefreshTokens(); // Thiếu - dọn dẹp token hết hạn
  }
}
