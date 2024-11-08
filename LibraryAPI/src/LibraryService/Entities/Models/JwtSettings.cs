namespace LibraryAPI.LibraryService.Entities.Models;

public class JwtSettings
{
    public JwtSettings(string? secretKey, string? issuer, string? audience, int accessTokenExpirationMinutes,
        int refreshTokenExpirationDays)
    {
        this.Issuer = issuer;
        this.SecretKey = secretKey;
        this.Audience = audience;
        this.AccessTokenExpirationMinutes = accessTokenExpirationMinutes;
        this.RefreshTokenExpirationDays = refreshTokenExpirationDays;
    }

    public JwtSettings()
    {
    }
    
    public string? SecretKey { get; set; }
    public string? Audience { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
    public string? Issuer { get; set; }
}