using System.Net;
using JwtWebApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jwtWebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{

    public UserController()
    {

    }
    [HttpGet("profile"), Authorize(Roles = "Admin, Premium")]
    public ActionResult<UserDto> GetProfile()
    {
        if (!User.IsInRole("Premium"))
              return Forbid();

        var name = User?.Identity.Name; // From JWT token
        var email = User?.FindFirstValue(ClaimTypes.Email); //// From JWT Claims token
        var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier)
        ; // From JWT Claims token
        var roles = User?.FindAll(ClaimTypes.Role);

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { Title = "Unauthorized", Detail = "User not authenticated or profile information is missing." });
        }

        return Ok(new { Username = name, Email = email, userId, Roles = roles?.Select(r => r.Value).ToArray() });

    }
}
