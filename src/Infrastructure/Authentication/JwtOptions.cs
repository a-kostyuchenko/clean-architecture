namespace Infrastructure.Authentication;

public class JwtOptions
{
    public const string ConfigurationSection = "Jwt";
    
    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public string SecretKey { get; init; } = string.Empty;

    public int LifeTime { get; set; }
}