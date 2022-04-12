namespace api.Models
{
    public class JwtSettings
    {
        // for jwt auth
        public const string Jwt = "Jwt";
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Subject { get; set; } = null!;
    }
}
