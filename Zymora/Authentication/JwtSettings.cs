namespace Zymora.Authentication
{
    public class JwtSettings
    {
        public int ExpirationMinutes { get; init; }
        public required string SecretKey { get; init; }
    }
}
