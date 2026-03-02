using CyberQuiz.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CyberQuiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _environment;

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IWebHostEnvironment environment)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _environment = environment;
    }

    public sealed record LoginDto(string UserName, string Password, bool RememberMe = false);

    // POST: api/account/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest("username and password are required");

        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user is null)
            return Unauthorized();

        var result = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.RememberMe, lockoutOnFailure: false);
        if (!result.Succeeded)
            return Unauthorized();

        return Ok();
    }

    // POST: api/account/logout
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    // GET: api/account/me
    // Simple endpoint to verify that cookie authentication + [Authorize] works.
    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = User.Identity?.Name;

        return Ok(new { userId, userName });
    }

    // GET: api/account/dev/seed-status
    // Development-only helper endpoint to verify that the seeded users exist.
    [HttpGet("dev/seed-status")]
    [AllowAnonymous]
    public async Task<IActionResult> DevSeedStatus()
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        var hasUser = await _userManager.FindByNameAsync("user") is not null;
        var hasAdmin = await _userManager.FindByNameAsync("admin") is not null;

        return Ok(new { hasUser, hasAdmin });
    }
}
