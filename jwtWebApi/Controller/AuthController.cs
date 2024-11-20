using jwtWebApi.Models;
using jwtWebApi.Services.UserService;
using JwtWepApi.Dto;
using Microsoft.AspNetCore.Mvc;



namespace JwtWepApi.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{


    public static User user = new();

    [HttpPost("register")]
    public ActionResult<User> Register(UserDto request)
    {

        string pw_hash
               = BCrypt.Net.BCrypt.HashPassword(request.Password);


        user.Username = request.Username;
        user.PasswordHash = pw_hash;
        user.Roles = request.Roles;

        return Ok(user);

    }

    [HttpPost("login")]
    public ActionResult<User> Login(UserDto request, [FromServices] ITokenService service)
    {
        if (user.Username != request.Username)
        {
            return BadRequest("User Not Found");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return BadRequest("Wrong is Passoword");
        }

        string token = service.GenerateToken(user);

        return Ok(token);

    }
}


