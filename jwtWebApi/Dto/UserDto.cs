
namespace JwtWebApi.Dto;

public class UserDto
{
    public string Login { get; set; } = string.Empty;
    public string? Username { get; set; } = String.Empty;
    public string? Password { get; set; } = string.Empty;
    public string? ConfirmePassword {  get; set; } = string.Empty;    
    public string  Email {  get; set; } = string.Empty;
    public string[]? Roles { get; set; } = [];
                                  
}

