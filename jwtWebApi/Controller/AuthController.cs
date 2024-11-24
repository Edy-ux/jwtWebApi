
using jwtWebApi.Services.Auth;
using Microsoft.AspNetCore.Authorization;

namespace JwtWebApi.Controller;

[Route("api/v1/[controller]")]

[ApiController]
public class AuthController(ITokenService service, IAuthService authService) : ControllerBase
{
    private readonly static User user = new();

    private readonly IAuthService _authService = authService;

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
    public ActionResult<object> Login(UserDto request)
    {

        if (user.Username != request.Username)
            BadRequest("User Not Found");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            BadRequest("Wrong is Passoword");
        //Gerar QR code if Login is sucessfull

        var token = service.GenerateToken(user);

        return Ok(token);

    }

    [HttpGet, Authorize]
    public ActionResult<object> GetMe()
    {
        var user = User?.Identity?.Name;
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var role = User.FindAll(ClaimTypes.Role);

        return Ok(new { user, userName, roles = string.Join(',', role.Select(c => c.Value)) });
    }

    [HttpGet("generateqr")]
    public ActionResult<string> GenerateQR(string email)
    {

        var (secretKey, qrCodeUrl) = _authService.GenerateTwoFactor(email);

        return qrCodeUrl;
    }

    [HttpPost("validatecode")]
    public ActionResult<bool> ValidateCode(string code, string key)
    {   
        return _authService.ValidateCode(code, key);
    }

}


