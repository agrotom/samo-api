using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Samo.Common;
using Samo.DataAccess;
using Samo.Interfaces;
using Samo.Models;
using Samo.Models.ViewModels;

namespace Samo.Controllers;

[Route($"{Constants.API_V1}/")]
[ApiController]
public class AccountController(SamoDbContext context,
    UserManager<User> userManager,
    ITokenService tokenService,
    RoleManager<IdentityRole> roleManager,
    SignInManager<User> signInManager) : ControllerBase
{
    private readonly SamoDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly SignInManager<User> _signInManager = signInManager;

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            return Unauthorized(new { message = "Lack of account." });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Invalid email or password!" });
        }

        //HttpContent.Session.SetString("email", user.Email);

        var token = _tokenService.CreateToken(user);

        return Ok(new
        {
            message = "Login successful!",
            email = user.Email,
            token = token
        });

    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _userManager.Users.AnyAsync(user => user.Email == model.Email))
        {
            return BadRequest(new
            {
                message = "User already exists!"
            });
        }

        var user = new User(model.Email, model.SureName, model.FirstName, model.Achieve, model.BirthDate.ToUniversalTime());

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = "Invalid email, uesrname or password!" });
        }

        return Ok(new
        {
            message = "Registration successful for new user",
            email = user.Email
        });
    }
}