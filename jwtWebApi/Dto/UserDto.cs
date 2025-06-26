
using jwtWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace JwtWebApi.Dto;

public class UserDto
{

    public Guid? Id { get; set; }

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
    public bool EmailConfirmed { get; private set; }

    [EmailAddress(ErrorMessage = "EmailLogin")]
    public string Email { get; set; }

    public UserDto() { }
    public UserDto(string login, string email, string[] roles, string userName)
    {
        Login = login;
        Email = email;
        Roles = roles;
        UserName = userName;

    }


}