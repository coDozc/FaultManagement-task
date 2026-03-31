using FaultManagement.Api.Middleware;
using FaultManagement.Api.Services;
using FaultManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaultManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly FaultManagementDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(FaultManagementDbContext context, ITokenService tokenService, ILogger<AuthController> logger)
    {
        _context = context;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login attempt for user: {UserName}", request.UserName);

        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(ApiResponse.ErrorResponse("Username and password are required."));
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed for user: {UserName}", request.UserName);
            return BadRequest(ApiResponse.ErrorResponse("Invalid username or password."));
        }

        var token = _tokenService.GenerateToken(user);

        _logger.LogInformation("Login successful for user: {UserName}", request.UserName);

        return Ok(ApiResponse<object>.SuccessResponse(new { token }, "Login successful."));
    }
}

public class LoginRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
