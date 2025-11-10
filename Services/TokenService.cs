using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Samo.Interfaces;
using Samo.Models;

namespace Samo.Services;

public class TokenService([NotNull] IConfiguration config) : ITokenService
{
    private readonly IConfiguration _config = config;

    public string CreateToken([NotNull] User user)
    {
        var secretKey = _config["AppSettings:TokenKey"];

        if (string.IsNullOrEmpty(secretKey))
        {
            throw new ArgumentNullException("Token key is missing from the configuration!");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

        var token = new JwtSecurityToken
            (
                issuer: null,

                audience: null,

                claims: claims,

                expires: DateTime.UtcNow.AddMinutes(60),

                signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}