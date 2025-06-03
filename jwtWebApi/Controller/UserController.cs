using JwtWebApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jwtWebApi.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{

    public UserController()
    {

    }
    [HttpGet("profile"), Authorize(Roles = "Premium, Admin")]
    public ActionResult<UserDto> GetProfile()
    {
        var user = User?.Identity.Name; // From JWT token
        var email = User?.FindFirstValue(ClaimTypes.Email); //// From JWT Claims token
        var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var roles = User?.FindAll(ClaimTypes.Role);

        return Ok(new UserDto { Username = user, Email = email, Roles = roles?.Select(r => r.Value).ToArray() });

    }
}
