using WebApplication2.Dtos;
using WebApplication2.Mappers;

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
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Kullanıcı verisini User modeline dönüştür
        var newUser = registerRequest.ToUser();

        // Kullanıcıyı kaydet
        var result = await _userService.Register(newUser);
        if (!result)
        {
            return BadRequest(new { message = "Email already exists." });
        }
        // Kullanıcı bilgilerini Session'a kaydet
        HttpContext.Session.SetString("UserId", newUser.UserID.ToString());
        HttpContext.Session.SetString("Email", newUser.Email);
        return Ok(new { message = "User registered successfully." });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        _logger.LogInformation("Login endpoint called with email: {Email}", loginRequest?.Email);

        // Kullanıcıyı doğrulama işlemi
        var user = await _userService.Authenticate(loginRequest?.Email, loginRequest?.Password);
        if (user == null)
        {
            _logger.LogWarning("Invalid credentials for email: {Email}", loginRequest?.Email);
            return Unauthorized("Invalid credentials.");
        }

        // Kullanıcı bilgilerini Session'a kaydet
        HttpContext.Session.SetString("UserId", user.UserID.ToString());
        HttpContext.Session.SetString("Email", user.Email);

        _logger.LogInformation("User logged in successfully: {Email}", user?.Email);

        // Kullanıcı bilgilerini UserDto olarak döndür
        var userDto = new UserDto
        {
            UserID = user.UserID,
            UserName = user.UserName,
            Email = user.Email,
            TCKimlik = user.TCKimlik,
            Score = user.Score,

        };

        return Ok(userDto);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto requestDto)
    {
        if (string.IsNullOrEmpty(requestDto?.Email))
        {
            return BadRequest("Email is required.");
        }

        _logger.LogInformation("Forgot password request for email: {Email}", requestDto.Email);

        // Kullanıcıyı email ile bul
        var user = await _userService.FindUserByEmail(requestDto.Email);
        if (user == null)
        {
            _logger.LogWarning("User not found with email: {Email}", requestDto.Email);
            return BadRequest(new { message = "User not found." });
        }

        _logger.LogInformation("User found with email: {Email}, updating password to TC Kimlik number.", requestDto.Email);

        // Kullanıcının TC Kimlik bilgisi var mı kontrol edin
        if (string.IsNullOrEmpty(user.TCKimlik))
        {
            _logger.LogWarning("TC Kimlik number is missing for user with email: {Email}", requestDto.Email);
            return BadRequest(new { message = "TC Kimlik number not found for user." });
        }

        // Şifreyi TC Kimlik olarak güncelle
        user.Password = user.TCKimlik;

        // Kullanıcıyı güncelle (veritabanında)
        var updateResult = await _userService.UpdateUser(user);
        if (!updateResult)
        {
            _logger.LogError("Failed to update password for user with email: {Email}", requestDto.Email);
            return StatusCode(500, new { message = "Failed to update password." });
        }

        _logger.LogInformation("Password successfully updated to TC Kimlik for user with email: {Email}", requestDto.Email);

        // Yanıt döndür
        return Ok(new { tckimlik = user.TCKimlik, message = "Password successfully updated." });
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized(new { message = "User not authenticated." });
        }

        var user = await _userService.GetUserById(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }
        _logger.LogInformation("User profile get in successfully: {Email}", user?.Email);
    
    
        return Ok(user);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // Session'ı temizle
        HttpContext.Session.Clear();
        _logger.LogInformation("User logged out successfully.");
        return Ok(new { message = "Logout successful." });
    }

}


    

