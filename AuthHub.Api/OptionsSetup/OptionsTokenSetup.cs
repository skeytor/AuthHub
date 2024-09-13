using AuthHub.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace AuthHub.Api.OptionsSetup;

public class OptionsTokenSetup(IConfiguration configuration) : IConfigureOptions<OptionsToken>
{
    public void Configure(OptionsToken options) => 
        configuration.GetSection("OptionsToken").Bind(options);
}
