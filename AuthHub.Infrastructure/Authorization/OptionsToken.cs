namespace AuthHub.Infrastructure.Authorization;

public record class OptionsToken
{
    public required string SecretKey { get; init; } = string.Empty;
    public required string Issuer { get; init; } = string.Empty;
    public required string Audience { get; init; } = string.Empty;
};
