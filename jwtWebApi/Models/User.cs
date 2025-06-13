using System.ComponentModel.DataAnnotations;
namespace jwtWebApi.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Login is required.")]
    public string Login { get; set; } = string.Empty;
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string PasswordHash { get; set; } = string.Empty;

    [Required(ErrorMessage = "ConfirmePassword is required.")]
    public string? ConfirmePassword { get; set; } = string.Empty;
    public bool? EmailConfirmed { get; set; } = false;
    public string[]? Roles { get; set; } = [];

    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;
    public List<RefreshToken> RefreshTokens { get; set; } = new();

}

