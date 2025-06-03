
using System.ComponentModel.DataAnnotations;

namespace JwtWebApi.Dto;

public class UserDto
{
    [Required]
    public string Login { get; set; } = string.Empty;
    [Required]

    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = string.Empty;

    [Required]
    public string ConfirmePassword { get; set; } = string.Empty;
    [Required]

    public string Email { get; set; } = string.Empty;
    public string[]? Roles { get; set; } = [];

}

