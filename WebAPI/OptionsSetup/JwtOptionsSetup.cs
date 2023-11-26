using Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace WebAPI.OptionsSetup;

public class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "Jwt";

    public void Configure(JwtOptions options) => 
        configuration.GetSection(SectionName).Bind(options);
}