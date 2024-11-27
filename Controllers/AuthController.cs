using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UMS1._0.Models;
using UMS1._0.Requests;
using LoginRequest = UMS1._0.Requests.LoginRequest;
using RegisterRequest = UMS1._0.Requests.RegisterRequest;

namespace UMS1._0.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UniversityDbContext _dbContext;
    private readonly JwtTokenService _jwtTokenService;
    private readonly HashingService _hashingService;

    public AuthController(UniversityDbContext dbContext, JwtTokenService jwtTokenService, HashingService hashingService)
    {
        _dbContext = dbContext;
        _jwtTokenService = jwtTokenService;
        _hashingService = hashingService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Username == request.Username);
        if (user == null)
        {
            return Unauthorized(new { Message = "Invalid username or password" });
        }

        var isValid = _hashingService.VerifyPassword(user.Username, request.Password, user.PasswordHash);
        if (!isValid)
        {
            return Unauthorized(new { Message = "Invalid username or password" });
        }

        // Generate JWT token
        var token = _jwtTokenService.GenerateJwtToken(user.Username, user.Role);

        return Ok(new
        {
            Token = token,
            Role = user.Role
        });
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (_dbContext.Users.Any(u => u.Username == request.Username))
        {
            return BadRequest(new { Message = "Username already exists" });
        }

        var hashedPassword = _hashingService.HashPassword(request.Username, request.Password);

        var user = new User
        {
            Username = request.Username,
            PasswordHash = hashedPassword,
            Role = request.Role
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        return Ok(new { Message = "User registered successfully" });
    }
}
