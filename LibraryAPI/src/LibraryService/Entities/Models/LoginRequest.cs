namespace LibraryAPI.LibraryService.Entities.Models;

public class LoginRequest(string Username, string Password)
{
    public string Username { get; set; } = Username;
    public string Password { get; set; } = Password;
}