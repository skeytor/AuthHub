using AuthHub.Persistence;
using AuthHub.Persistence.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AuthHub.Api.IntegrationTest.Initialization;

internal class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    IServiceScopeFactory serviceScopeFactory,
    ILoggerFactory logger, 
    UrlEncoder encoder) 
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        IAppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = context
            .Users
            .AsNoTracking()
            .Include(x => x.Role)
            .ThenInclude(x => x.Permissions)
            .FirstOrDefault(x => x.Username == "admin_1"); // See SampleUser class

        Claim[] claims =
        [
            new(ClaimTypes.NameIdentifier, user!.Id.ToString()),
            new(ClaimTypes.Role, user.Role.Name)
        ];

        ClaimsIdentity identiy = new(claims, "TestType");
        ClaimsPrincipal principal = new(identiy);
        AuthenticationTicket ticket = new(principal, "TestSchema");
        AuthenticateResult result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}
