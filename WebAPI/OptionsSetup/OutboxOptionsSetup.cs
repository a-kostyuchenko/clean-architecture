using Infrastructure.Outbox;
using Microsoft.Extensions.Options;

namespace WebAPI.OptionsSetup;

public class OutboxOptionsSetup(IConfiguration configuration) 
    : IConfigureOptions<OutboxOptions>
{
    private const string SectionName = "Outbox";

    public void Configure(OutboxOptions options) => 
        configuration.GetSection(SectionName).Bind(options);
}