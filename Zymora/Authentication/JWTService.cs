using Microsoft.AspNetCore.Identity;
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
        public JWTService(IOptions<JwtSettings> options, IConfiguration configuration)
        {
            _settings = options.Value;
            _configuration = configuration;
          }
        public string GenerateToken(User user, List<Roles> rolesUser)
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
            return token;
        }
        
  }
}
