﻿using AuthHub.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthHub.Api.OptionsSetup;

internal sealed class JwtBearerParametersConfigureOptions(IOptions<OptionsToken> options)
    : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly OptionsToken optionsToken = options.Value;
    public void Configure(string? name, JwtBearerOptions options) => 
        Configure(options);

    public void Configure(JwtBearerOptions options) => 
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = optionsToken.Issuer,
            ValidAudience = optionsToken.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(optionsToken.SecretKey)),
        };
}
