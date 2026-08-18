using Icof.Api.DTOs;
using Icof.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Icof.Api.Controllers;

// The bearer tokens issued by MapIdentityApi's /login are opaque (Data-Protection-protected),
// not JWTs — the frontend can't decode them to read claims like roles. This endpoint is how it
// finds out who's logged in and whether they're an Admin, right after login and on page reload.
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserDto>> Me()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return Unauthorized();
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new CurrentUserDto
        {
            Email = user.Email ?? string.Empty,
            Roles = roles.ToList()
        });
    }
}
