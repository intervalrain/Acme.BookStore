using System;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Acme.BookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : AbpControllerBase
{
    private readonly IAuthAppService _authAppService;

    public AuthController(IAuthAppService authAppService)
    {
        _authAppService = authAppService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "User login",
        Description = "Authenticate user with username, password and tenant to get JWT token"
    )]
    [SwaggerResponse(200, "Login successful", typeof(LoginResponseDto))]
    [SwaggerResponse(400, "Invalid credentials")]
    public async Task<LoginResponseDto> LoginAsync([FromBody] LoginDto input)
    {
        return await _authAppService.LoginAsync(input);
    }

    [HttpGet("me")]
    [Authorize(AuthenticationSchemes = "TempJwt")]
    [SwaggerOperation(
        Summary = "Get current user info",
        Description = "Get current authenticated user information from JWT token"
    )]
    [SwaggerResponse(200, "User information retrieved successfully")]
    [SwaggerResponse(401, "Unauthorized - Invalid or missing JWT token")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            Username = User.Identity!.Name,
            Tenant = User.FindFirst("tenant")?.Value,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }

    [HttpGet("protected")]
    [Authorize(AuthenticationSchemes = "TempJwt")]
    [SwaggerOperation(
        Summary = "Test protected endpoint",
        Description = "Test endpoint to verify JWT authentication is working"
    )]
    [SwaggerResponse(200, "Protected data retrieved successfully")]
    [SwaggerResponse(401, "Unauthorized - Invalid or missing JWT token")]
    public IActionResult GetProtectedData()
    {
        return Ok(new
        {
            Message = "This is protected data",
            User = User.Identity!.Name,
            Tenant = User.FindFirst("tenant")?.Value,
            Timestamp = DateTime.UtcNow
        });
    }
}