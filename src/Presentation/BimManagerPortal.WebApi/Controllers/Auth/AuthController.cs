using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BimManagerPortal.Domain.Entities.Users;
using BimManagerPortal.Persistance.Contexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BimManagerPortal.WebApi.Controllers.Auth;

public record UpdateProfileRequest(string? WorkComputer, string? EmployeeId);

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(ApplicationDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpGet("google")]
    public IActionResult Login()
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = "/api/v1/auth/google/finalize"
        };
        return Challenge(props, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google/finalize")]
    public async Task<IActionResult> Finalize()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded)
            return Redirect($"{_config["WebApp:BaseUrl"]}/auth/error");

        var googleId = result.Principal!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value ?? "";
        var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value ?? "";
        var picture = result.Principal.FindFirst("picture")?.Value;

        var user = await _db.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId);
        if (user == null)
        {
            if (email.Contains(".softapro@"))
            {
                user = new User(googleId, email, name, picture);
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }
            else
            {
                return Redirect($"{_config["WebApp:BaseUrl"]}/auth/forbidden?googleId={Uri.EscapeDataString(googleId)}&email={Uri.EscapeDataString(email)}&name={Uri.EscapeDataString(name)}");
            }
        }

        user.Name = name;
        user.AvatarUrl = picture;
        await _db.SaveChangesAsync();

        var jwt = GenerateJwt(user);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Redirect($"{_config["WebApp:BaseUrl"]}/auth/callback?token={Uri.EscapeDataString(jwt)}");
    }

    [HttpGet("me")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Me()
    {
        var googleId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var user = await _db.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId);
        if (user == null) return NotFound();
        return Ok(new
        {
            user.Id,
            user.Email,
            user.Name,
            user.AvatarUrl,
            Role = user.Role.ToString(),
            user.WorkComputer,
            user.EmployeeId
        });
    }

    [HttpPut("profile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var googleId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var user = await _db.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId);
        if (user == null) return NotFound();

        user.WorkComputer = request.WorkComputer;
        user.EmployeeId = request.EmployeeId;
        await _db.SaveChangesAsync();

        return Ok();
    }

    private string GenerateJwt(User user)
    {
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.GoogleId),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim("picture", user.AvatarUrl ?? ""),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(30),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(descriptor));
    }
}
