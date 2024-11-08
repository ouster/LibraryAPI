namespace LibraryAPI.LibraryService.Entities.Models;

public record TokenRequest(string AccessToken, string RefreshToken)
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}