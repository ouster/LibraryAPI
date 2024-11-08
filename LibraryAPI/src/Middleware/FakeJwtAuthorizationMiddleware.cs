using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryAPI.LibraryService.Entities.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAPI;

public class FakeJwtAuthorizationMiddleware(RequestDelegate next, IWebHostEnvironment env, IOptions<JwtSettings> jwtSettings)
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task InvokeAsync(HttpContext context)
    {
        if (env.IsDevelopment())
        {
            // Check if the Authorization header already contains a Bearer token
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                // Fake JWT token to authorize all requests in development mode
                if (_jwtSettings is { Audience: not null, Issuer: not null, SecretKey: not null })
                {
                    var fakeToken = GenerateFakeJwtToken(_jwtSettings.SecretKey, _jwtSettings.Issuer,
                        _jwtSettings.Audience); // Replace with an actual fake token string
                    context.Request.Headers.Authorization = $"Bearer {fakeToken}";
                }
            }
        }

        // Continue processing the request
        await next(context);
    }

    private static string GenerateFakeJwtToken(string secretKey, string issuer, string audience)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "fakeUserId"),
            new Claim(JwtRegisteredClaimNames.Name, "Fake User"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}
