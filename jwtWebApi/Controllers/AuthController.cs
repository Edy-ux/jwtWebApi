
using System.Globalization;
using jwtWebApi.Application.Interfaces;
using jwtWebApi.Models;
using jwtWebApi.Services.Token;
using JwtWebApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
namespace JwtWebApi.Controllers;


[Route("api/v1/[controller]")]

[ApiController]
public class AuthController : ControllerBase
{
    // private readonly static List<User>? users = new List<User>();
    private readonly IStringLocalizer _localizer;
    private readonly IUserService? _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger, IStringLocalizerFactory factory, IUserService userService)
    {
        _userService = userService;
        _logger = logger;
        _localizer = factory.Create("Controllers.AuthController", typeof(AuthController).Assembly.GetName().Name!);
    }

    [HttpPost("register")]

    public async Task<IActionResult> Register([FromBody] UserDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        // Validate model using data annotamions
        if (!ModelState.IsValid)
        {

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            //  _logger.LogWarning(string.Format(_localizer["InvalidLogin"], string.Join(", ", errors)));
            // return Problem("Invalid input: " + string.Join(", ", errors));

            return BadRequest(ModelState);

        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        //Check if password and confirm password match

        if (request.Password != request.ConfirmePassword)
        {
            _logger.LogWarning("Registration attempt with mismatched passwords for login: {Login} ", request.Login);
            return BadRequest(new { Title = "Bad Request", Detail = "Password and Confirm Password do not match." });
        }
        //Check if login already exist

        var user = new User
        {
            Login = request.Login,
            PasswordHash = hashedPassword,
            Email = request.Login,
            UserName = request.Username!,
            Roles = request.Roles ?? ["User"],
        };
        try
        {

            var createdUser = await _userService!.InsertUserAsync(user);

            if (createdUser is not null)
                _logger.LogInformation(string.Format(_localizer["ValidRegistration"], user.Login, DateTime.UtcNow));
            return CreatedAtAction(nameof(Register), new { login = createdUser?.Email, userName = createdUser?.UserName, Title = "User registered successfully" });

        }
        catch (Exception ex)
        {
            // TODO
            _logger.LogWarning("Registration attempt with existent login: {Login} ", request.Login);
            return Conflict(new ProblemDetails { Title = "Conflict", Detail = ex.Message }); // ProblemDetails;
        }


    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDtoLogin request)
    {
        if (await _userService?.GetUserByLogin(request.Login)! is not User user || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            _logger.LogWarning(_localizer["InvalidLogin"], request.Login);
            return BadRequest("Invalid username or password");
        }


        var (token, accessToken) = await _userService.AuthenticateAsync(user.Login, request.Password, HttpContext.Connection.RemoteIpAddress?.ToString()!);
        _logger.LogInformation("Successful login for username: {Username}", request.Login);

        Response.Headers["jwt-token"] = token;
        Response.Headers["access-token"] = accessToken;

        return Ok(new
        {
            token,
            accessToken,
            userId = user.Id,
            userName = user.UserName,
            roles = user.Roles ?? ["User"]
        });

    }

    [HttpGet("test-localizer")]
    public IActionResult Test()
    {
        var currentCulture = CultureInfo.CurrentUICulture.Name;
        var message = _localizer["ValidRegistration"];

        return Ok(new
        {
            translatedMessage = message.Value,
            currentCulture,
        });
    }


}


