
using System.ComponentModel.DataAnnotations;

namespace JwtWebApi.Dto;

public class UserDto
{

    [Required(ErrorMessage = "LoginRequired")]
    [EmailAddress(ErrorMessage = nameof(Login))]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "NameRequired")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "NameLength")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "PasswordLength")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = nameof(Password))]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "ConfirmPasswordRequired")]
    [Compare("Password", ErrorMessage = "PasswordMismatch")]
    public string ConfirmePassword { get; set; } = string.Empty;
    public string[]? Roles { get; set; } = [];

}

