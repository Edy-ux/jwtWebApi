
using jwtWebApi.Models;
using jwtWebApi.Services.Token;
using Microsoft.AspNetCore.Mvc;
using JwtWebApi.Dto;

namespace JwtWebApi.Controller;


[Route("api/v1/[controller]")]

[ApiController]
public class AuthController(ITokenService service, ILogger<AuthController> logger) : ControllerBase
{

    private readonly static List<User>? users = new List<User>();

    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("register")]
    public IActionResult Register([FromBody] UserDto request)
    {
        // Validate model using data annotamions
        if (!ModelState.IsValid)

        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            _logger.LogWarning("Invalid registration Erros: {Errors: }", string.Join(", ", errors));
            return Problem("Invalid input: " + string.Join(", ", errors));

        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        //Check if password and confirm password match
        if (request.Password != request.ConfirmePassword)
        {
            return BadRequest(new { Title = "Bad Request", Detail = "Password and Confirm Password do not match." });
        }
        //Check if login already exist
        if (users?.SingleOrDefault(u => u.Login == request.Login) is not null)
        {
            _logger.LogWarning("Registration attempted with existent login: {Login} ", request.Login);
            return Conflict(new ProblemDetails { Title = "Conflict", Detail = "User already exists." }); // ProblemDetails;
        }

        var user = new User { Login = request.Login, Email = request.Email, PasswordHash = hashedPassword, ConfirmePassword = request.ConfirmePassword, UserName = request.Username!, Roles = request.Roles };

        users.Add(user);
        _logger.LogInformation("User registered with login {Login} successfully", user.Login);
        return CreatedAtAction(nameof(Register), new { login = user.Login, userName = user.UserName, Title = "User registered successfully" });


    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDtoLogin request)
    {
        if (users?.SingleOrDefault(u => u.Login == request.Login) is not User user || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Invalid loggin atempt for username {Useraame}", request.Login);
            return BadRequest("Invalid username or password");
        }


        var token = service.GenerateToken(user);
        _logger.LogInformation("Successful login for username: {Useraame}", request.Login);

        Response.Headers["jwt-token"] = token;

        return Ok(token);

    }
}


