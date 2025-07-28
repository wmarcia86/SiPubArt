using Application.Common.Authentication;
using Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authentication;

/// <summary>
/// Service. Generates JWT tokens for authenticated users.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public class JwtTokenGenerator : ITokenGenerator
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the JwtTokenGenerator class.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the token.</param>
    /// <returns>A JWT token string.</returns>
    public string GenerateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        if (jwtSettings == null || !jwtSettings.Exists())
        {
            throw new InvalidOperationException("La sección JwtSettings no está configurada en appsettings.");
        }

        var issuer = jwtSettings["Issuer"];

        if (string.IsNullOrEmpty(issuer))
        {
            throw new InvalidOperationException("JwtSettings:Issuer is not configured.");
        }

        var audience = jwtSettings["Audience"];

        if (string.IsNullOrEmpty(issuer))
        {
            throw new InvalidOperationException("JwtSettings:Audience is not configured.");
        }

        var secret = jwtSettings["Secret"];

        if (string.IsNullOrEmpty(secret))
        {
            throw new InvalidOperationException("JwtSettings:Secret is not configured.");
        }

        var expires = DateTime.UtcNow.AddHours(2);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new Claim("fullName", user.FullName),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username.Value),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
