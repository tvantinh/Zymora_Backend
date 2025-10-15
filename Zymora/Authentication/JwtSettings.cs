namespace Zymora.Authentication
{
    public class JWTSettings
    {
        public int ExpirationMinutes { get; init; }
        public required string SecretKey { get; init; }
    }
}
