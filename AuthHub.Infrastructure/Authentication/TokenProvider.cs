using AuthHub.Domain.Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthHub.Infrastructure.Authentication;

public class TokenProvider(IOptions<OptionsToken> options) : ITokenProvider
{
    private readonly OptionsToken _optionsToken = options.Value;
    public AccessTokenResponse GetAccesToken(User user)
    {
        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(JwtRegisteredClaimNames.GivenName, user.LastName),
        ];

        SigningCredentials signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_optionsToken.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        DateTime expirationTime = DateTime.UtcNow.AddHours(1);

        JwtSecurityToken securityToken = new(
            _optionsToken.Issuer, 
            _optionsToken.Audience, 
            claims,
            null,
            expirationTime,
            signingCredentials);

        string accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        string refreshToken = GenerateRefreshToken();
        long expiresIn = (long)(expirationTime - DateTime.UtcNow).TotalSeconds;
        return new AccessTokenResponse
        {
            AccessToken = accessToken,
            ExpiresIn = expiresIn,
            RefreshToken = refreshToken
        };
    }
    private static string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[60];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
