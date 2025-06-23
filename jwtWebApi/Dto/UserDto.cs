
using jwtWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace JwtWebApi.Dto;

public class UserDto
{

    [Required(ErrorMessage = "LoginRequired")]
    [EmailAddress(ErrorMessage = "EmailLogin")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "NameRequired")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "NameLength")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "PasswordRequired")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "PasswordLength")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "ConfirmPasswordRequired")]
    [Compare("Password", ErrorMessage = "PasswordMismatch")]
    public string ConfirmePassword { get; set; } = string.Empty;
    public string[]? Roles { get; set; } = [];

    public List<RefreshToken>? RefreshTokens { get; set; }

    [EmailAddress(ErrorMessage = "EmailLogin")]
    public string? Email { get; set; } = string.Empty;


}

