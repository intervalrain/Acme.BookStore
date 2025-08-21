using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Acme.BookStore.Auth;

[SwaggerSchema("Login request model")]
public class LoginDto
{
    [Required]
    [SwaggerSchema("Username for authentication")]
    public string Username { get; set; } = null!;

    [Required]
    [SwaggerSchema("Password for authentication")]
    public string Password { get; set; } = null!;

    [SwaggerSchema("Tenant identifier (optional, defaults to 'default')")]
    public string? Tenant { get; set; }
}

[SwaggerSchema("Login response model")]
public class LoginResponseDto
{
    [SwaggerSchema("JWT access token")]
    public string Token { get; set; } = null!;
    
    [SwaggerSchema("Authenticated username")]
    public string Username { get; set; } = null!;
    
    [SwaggerSchema("Tenant identifier")]
    public string? Tenant { get; set; }
    
    [SwaggerSchema("Token expiration time (UTC)")]
    public DateTime ExpiresAt { get; set; }
}