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
    [HttpGet("profile"), Authorize]

    public ActionResult<object> GetProfile()
    {
        var user = User?.Identity.Name; // From JWT token
        var email = User?.FindFirstValue(ClaimTypes.Email);
        var roles = User?.FindAll(ClaimTypes.Role);

        return Ok(new { user, email, roles = string.Join(',', roles.Select(c => c.Value))});

    }
}
