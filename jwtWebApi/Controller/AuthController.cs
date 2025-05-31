
using jwtWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            _logger.LogWarning("Invalid registration. Erros: {Errors: }", string.Join(", ", errors));
            return Problem("Invalid input: " + string.Join(", ", errors));

        }
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        //Check if login already exist
        if (users.SingleOrDefault(u => u.Login == request.Login) is not null)
        {

            _logger.LogWarning("Registration attempt with existent login: {Login} ", request.Login);
            return Problem("User with given login already exists.");
        }

        var user = new User { Login = request.Login, Email = request.Email, PasswordHash = hashedPassword, UserName = request.Username!, Roles = request.Roles };

        users.Add(user);
        Console.WriteLine(user);
        return Ok(Results.Created());


    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto request)
    {
        if (users?.SingleOrDefault(u => u.Login == request.Login) is not User user)
        {
            _logger.LogWarning("User with login: {Login} not found. ", request.Login);
            return BadRequest("User doesn´t exist");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            BadRequest("Passoword is wrong! ");

        var token = service.GenerateToken(user);

        return Ok(token);

    }
}


