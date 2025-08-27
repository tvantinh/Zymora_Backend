namespace Zymora.Authentication
{
    public class JwtSettings
    {
        public string Audience { get; init; } = default!;
        public int ExpirationMinutes { get; init; }
        public string Issuer { get; init; } = default!;
        public string SecretKey { get; init; } = default!;
    }
}
