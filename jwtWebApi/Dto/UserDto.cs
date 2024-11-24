using JwtWebApi.Controller;
using System.Globalization;

namespace JwtWebApi.Dto;

public class UserDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string[]? Roles { get; set; } = [];

    public string  Email {  get; set; } = string.Empty;
                                  
}

