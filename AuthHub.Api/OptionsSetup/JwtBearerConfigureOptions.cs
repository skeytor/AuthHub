using AuthHub.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace AuthHub.Api.OptionsSetup;

internal class JwtBearerConfigureOptions(IConfiguration configuration) 
    : IConfigureNamedOptions<OptionsToken>
{
    private const string ConfigurationSectionName = "OptionsToken";
    public void Configure(string? name, OptionsToken options) => 
        Configure(options);
    public void Configure(OptionsToken options) => 
        configuration
            .GetSection(ConfigurationSectionName)
            .Bind(options);
}
