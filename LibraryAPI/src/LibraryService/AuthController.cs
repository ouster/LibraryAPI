using System.Security.Claims;
using LibraryAPI.LibraryService.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = LibraryAPI.LibraryService.Entities.Models.LoginRequest;

namespace LibraryAPI.LibraryService;

[ApiController]
[Route("api/[controller]")]
public class AuthController(JwtTokenService jwtTokenService) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Assuming the user is authenticated successfully
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username),
            // Add other claims as needed
        };

        var accessToken = jwtTokenService.GenerateAccessToken(claims);
        var refreshToken = jwtTokenService.GenerateRefreshToken();

        // Save or update the refresh token in the database

        return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] TokenRequest request)
    {
        // Validate the refresh token (retrieve the stored refresh token from the database)
        var storedRefreshToken = GetStoredRefreshToken(request.RefreshToken);

        if (storedRefreshToken == null || storedRefreshToken.ExpirationDate < DateTime.UtcNow)
        {
            return Unauthorized("Invalid or expired refresh token.");
        }

        // Assuming you have the username or user ID stored with the refresh token
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, storedRefreshToken.Username)
            // Add other claims as needed
        };

        var newAccessToken = jwtTokenService.GenerateAccessToken(claims);
        var newRefreshToken = jwtTokenService.GenerateRefreshToken();

        // Update the stored refresh token
        storedRefreshToken.Token = newRefreshToken;
        storedRefreshToken.ExpirationDate = DateTime.UtcNow.AddDays(jwtTokenService.RefreshTokenExpirationDays());
        SaveRefreshToken(storedRefreshToken);

        return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
    }
    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            // Remove the refresh token from the database
            RemoveRefreshToken(refreshToken);

            // Clear the HTTP-only cookie
            Response.Cookies.Delete("refreshToken");
        }

        return NoContent();
    }

    private void RemoveRefreshToken(string refreshToken)
    {
        // TODO Implement your logic to retrieve the refresh token from the database
    }

    private RefreshToken? GetStoredRefreshToken(string refreshToken)
    {
        // TODO Implement your logic to retrieve the refresh token from the database
        return null;
    }

    private void SaveRefreshToken(RefreshToken refreshToken)
    {
        // TODO Implement your logic to save the refresh token to the database
    }
}