namespace jwtWebApi.Models;

public class User
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string[]? Roles { get; set; } = [];
    public string Email { get; set; } = string.Empty;


}

