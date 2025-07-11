﻿
using jwtWebApi.Application.Interfaces;
using jwtWebApi.Dto.User;
using jwtWebApi.Models;
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
    private readonly IUserService _userService;
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
            _logger.LogWarning(string.Format(_localizer["InvalidLogin"], string.Join(", ", errors)));
            // return Problem("Invalid input: " + string.Join(", ", errors));
            return BadRequest(ModelState);

        }


        if (request.Password != request.ConfirmePassword)
        {
            _logger.LogWarning("Registration attempt with mismatched passwords for login: {Login} ", request.Login);
            return BadRequest(new { Title = "Bad Request", Detail = "Password and Confirm Password do not match." });
        }

        try
        {
            var createdUser = await _userService!.InsertUserAsync(request);

            if (createdUser is not null)
                _logger.LogInformation(string.Format(_localizer["ValidRegistration"], request.Login, DateTime.UtcNow));
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
    public async Task<IActionResult> Login(UserDtoLogin request)
    {

        try
        {
            var user = await _userService.GetUserByLogin(request);
            if (user is null)
            {
                _logger.LogWarning(_localizer["InvalidLogin"], request.Login);
                return NotFound("user not found");
            }

            var (accessToken, refreshToken) = await _userService.AuthenticateAsync(user.Login, request.Password, HttpContext.Connection.RemoteIpAddress?.ToString()!);
            _logger.LogInformation("Successful login for username: {Username}", request.Login);

            Response.Cookies.Append("refresh-token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, // Use Strict or Lax based on your requirements
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            Response.Headers["access-token"] = accessToken;

            return Ok(new
            {
                accessToken,
                userId = user.Id,
                userName = user.UserName,
                roles = user.Roles ?? ["User"]
            });

        }
        catch (UnauthorizedAccessException ex)
        {
            return BadRequest("Invalid username or password");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {Error}", ex.Message);
            return StatusCode(500, ex.Message);
            // TODO
        }

    }


    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {

        var refreshToken = Request.Cookies["refresh-token"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            _logger.LogWarning("Refresh token not provided in the request.");
            return BadRequest(new ProblemDetails { Title = "Bad Request", Detail = "Refresh token is required.", Status = 400 });
        }
        var result = await _userService.RenewAccessTokenAsync(refreshToken, HttpContext.Connection.RemoteIpAddress?.ToString()!);
        if (result == null)
            return NotFound(new ProblemDetails { Title = "Not Found", Detail = "Refresh token not found or expired." });

        Response.Cookies.Append("refresh-token", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

        Response.Headers["jwt-token"] = result.AccessToken;

        return Ok(new { accessToken = result.AccessToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {

        var refreshToken = Request.Cookies["refresh-token"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            _logger.LogWarning("Refresh token not provided in the request.");
            return BadRequest(new ProblemDetails { Title = "Bad Request", Detail = "Refresh token is required.", Status = 400 });
        }

        try
        {
            var user = await _userService.GetUserByRefreshTokenAsync(refreshToken);

            if (user is null)
            {
                return BadRequest(new
                {
                    Title = "Invalid or Revoked Token",
                    Detail = "User with the given token not found or token is revoked. Please re-authenticate to obtain a new access token."
                });

            }
            await _userService.LogoutAsync(refreshToken);
            //delete the cookie
            Response.Cookies.Delete("refresh-token");

            _logger.LogInformation("User {UserName} logged out successfully.", user?.UserName);
            return Ok(new { Title = "Logout successful", Detail = "User logged out successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "Error occurred while logging out.");
            return StatusCode(500, new ProblemDetails { Title = "Internal Server Error", Detail = "An error occurred while processing your request." });

        }
        finally
        {
            Response.Cookies.Delete("refresh-token");
        }
    }

}
