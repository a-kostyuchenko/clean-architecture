namespace Infrastructure.Authorization;

public class JwtOptions
{
    public const string ConfigurationSection = "Jwt";
    
    public string Issuer { get; init; }

    public string Audience { get; init; }

    public string SecretKey { get; init; }

    public int LifeTime { get; set; }
}