﻿namespace AuthHub.Infrastructure.Authentication;

public class OptionsToken
{
    public required string SecretKey { get; init; } = string.Empty;
    public required string Issuer { get; init; } = string.Empty;
    public required string Audience { get; init; } = string.Empty;
};
