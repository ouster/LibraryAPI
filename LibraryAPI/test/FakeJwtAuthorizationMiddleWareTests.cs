namespace LibraryAPI.test;

using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LibraryAPI;
using LibraryAPI.LibraryService.Entities.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

public class FakeJwtAuthorizationMiddlewareTests
{
    [Fact]
    public async Task Middleware_Should_Add_Authorization_Header_When_Absent()
    {
        // Arrange
        var jwtSettings = new JwtSettings
        {
            SecretKey = "fakeSecretKeyfakeSecretKeyfakeSecretKeyfakeSecretKeyfakeSecretKeyfakeSecretKeyfakeSecretKeyfakeSecretKeyfakeSecretKeyfakeSecretKey",
            Issuer = "fakeIssuer",
            Audience = "fakeAudience"
        };

        // Mock IWebHostEnvironment without using extension methods
        var mockEnv = new Mock<IWebHostEnvironment>();
        mockEnv.SetupGet(env => env.EnvironmentName).Returns("Development");

        var mockJwtSettings = new Mock<IOptions<JwtSettings>>();
        mockJwtSettings.Setup(s => s.Value).Returns(jwtSettings);

        var mockNext = new Mock<RequestDelegate>();

        var middleware = new FakeJwtAuthorizationMiddleware(mockNext.Object, mockEnv.Object, mockJwtSettings.Object);

        var context = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        Assert.NotNull(authorizationHeader);  // Ensure the header is added
        Assert.StartsWith("Bearer ", authorizationHeader);  // Ensure the token starts with "Bearer"
        
        // Optional: Verify that the token has been generated and is in the expected format
        var token = authorizationHeader?.Substring(7);  // Remove "Bearer " prefix
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        Assert.NotNull(jwtToken);
        Assert.Equal(jwtSettings.Issuer, jwtToken.Issuer);
        Assert.Equal(jwtSettings.Audience, jwtToken.Audiences.FirstOrDefault());
    }

    [Fact]
    public async Task Middleware_Should_Not_Override_Existing_Authorization_Header()
    {
        // Arrange
        var jwtSettings = new JwtSettings
        {
            SecretKey = "fakeSecretKey",
            Issuer = "fakeIssuer",
            Audience = "fakeAudience"
        };

        // Mock IWebHostEnvironment without using extension methods
        var mockEnv = new Mock<IWebHostEnvironment>();
        mockEnv.SetupGet(env => env.EnvironmentName).Returns("Development");

        var mockJwtSettings = new Mock<IOptions<JwtSettings>>();
        mockJwtSettings.Setup(s => s.Value).Returns(jwtSettings);

        var mockNext = new Mock<RequestDelegate>();

        var middleware = new FakeJwtAuthorizationMiddleware(mockNext.Object, mockEnv.Object, mockJwtSettings.Object);

        var context = new DefaultHttpContext();
        context.Request.Headers.Authorization = "Bearer existingToken";

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        Assert.Equal("Bearer existingToken", authorizationHeader);  // Ensure it was not overridden
    }
}
