namespace LibraryAPI.LibraryService.Entities.Models;

public class TokenRequest(string AccessToken, string RefreshToken)
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}