using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        public string GenerateToken(string userId, string userName, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            };
            string? secretKey = _configuration["JWT:secretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }
            return JWTHelper.GenerateToken(
                issuer: _configuration["JWT:issuer"],
                audience: _configuration["JWT:audience"],
                claims: claims,
                expirationMinutes: Convert.ToInt32(_configuration["JWT:expirationMinutes"]),
                secretKey: secretKey
            );
        }
    }
}
