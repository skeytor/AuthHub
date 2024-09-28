using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuthHub.Api.OptionsSetup;

internal class ConfigureSwaggerGenOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure(string? name, SwaggerGenOptions options) => 
        Configure(options);

    public void Configure(SwaggerGenOptions options)
    {
        ConfigureDocInformation(options);
        ConfigureDocAuthentication(options);
    }


    private static void ConfigureDocInformation(SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Authentication Appplication",
            Description = "An ASP.NET Core Web API for managing authentication",
            TermsOfService = new("https://example.com/terms"),
            Contact = new()
            {
                Name = "Example contact",
                Url = new("https://example.com/contact"),
            },
            License = new()
            {
                Name = "Example Licence",
                Url = new("https://example.com/licence")
            }
        });
    }
    private static void ConfigureDocAuthentication(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Enter a valid Bearer Token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = JwtBearerDefaults.AuthenticationScheme,
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}
