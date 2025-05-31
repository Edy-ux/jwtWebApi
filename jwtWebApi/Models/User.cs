namespace jwtWebApi.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string UserName { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string? ConfirmePassword { get; set; } = string.Empty;
    public bool? EmailConfirmed { get; set; } = false;
    public string[]? Roles { get; set; } = [];
    public string Email { get; set; } = string.Empty;


}

