namespace LibraryAPI.LibraryService.Entities.Models;

public record JwtSettings
{
    public JwtSettings(string? SecretKey, string? Issuer, string? Audience, int AccessTokenExpirationMinutes,
        int RefreshTokenExpirationDays)
    {
        this.Issuer = Issuer;
        this.SecretKey = SecretKey;
        this.Audience = Audience;
        this.AccessTokenExpirationMinutes = AccessTokenExpirationMinutes;
        this.RefreshTokenExpirationDays = RefreshTokenExpirationDays;
    }

    public JwtSettings()
    {
    }

    protected internal string? SecretKey { get; }
    protected internal string? Audience { get; }
    protected internal int AccessTokenExpirationMinutes { get; }
    protected internal int RefreshTokenExpirationDays { get; }
    public string? Issuer { get; init; }

    public void Deconstruct(out string? SecretKey, out string? Issuer, out string? Audience,
        out int AccessTokenExpirationMinutes, out int RefreshTokenExpirationDays)
    {
        SecretKey = this.SecretKey;
        Issuer = this.Issuer;
        Audience = this.Audience;
        AccessTokenExpirationMinutes = this.AccessTokenExpirationMinutes;
        RefreshTokenExpirationDays = this.RefreshTokenExpirationDays;
    }
}