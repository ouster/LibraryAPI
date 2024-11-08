using System;

namespace LibraryAPI.LibraryService.Entities.Models;

public class RefreshToken(int Id, string Token, string Username, DateTime ExpirationDate)
{
    public int Id { get; set; } = Id;
    public string Token { get; set; } = Token;
    public string Username { get; set; } = Username;
    public DateTime ExpirationDate { get; set; } = ExpirationDate;
}