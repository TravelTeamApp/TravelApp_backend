namespace WebApplication2.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApplication2.Services;
using WebApplication2.Models;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(UserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User newUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.Register(newUser);
        if (!result)
        {
            return BadRequest(new { message = "Email already exists." });
        }

        return Ok(new { message = "User registered successfully." });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User loginRequest)
    {
        _logger.LogInformation("Login endpoint called with email: {Email}", loginRequest?.Email);

        var user = await _userService.Authenticate(loginRequest?.Email, loginRequest?.Password);
        if (user == null)
        {
            _logger.LogWarning("Invalid credentials for email: {Email}", loginRequest?.Email);
            return Unauthorized("Invalid credentials.");
        }

        _logger.LogInformation("User logged in successfully: {Email}", user?.Email);
        return Ok(user);
    }
}