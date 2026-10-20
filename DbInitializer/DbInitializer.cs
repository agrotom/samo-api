using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Samo.DataAccess;
using Samo.Models;

namespace Samo.DbInitializer;

public class DbInitializer(SamoDbContext context, UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IDbInitializer
{

    private readonly SamoDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task Initialize()
    {
        try
        {
            if (_context.Database.GetPendingMigrations().Count() > 0)
            {
                _context.Database.Migrate();
            }
        }
        catch (Exception)
        {

        }

        if (!await _roleManager.RoleExistsAsync(Utility.Helper.Admin))
        {
            await _roleManager.CreateAsync(new IdentityRole(Utility.Helper.Admin));
            await _roleManager.CreateAsync(new IdentityRole(Utility.Helper.User));
        }

        if (await _userManager.FindByEmailAsync("gg@gmail.com") == null)
        {

            await _userManager.CreateAsync(new User(
                "gg@gmail.com",
                "Albert",
                "Sharafiev",
                "IT",
                new DateOnly(2005, 1, 29)
            ), "aB3456_");

            User user = _context.Users.FirstOrDefault(u => u.Email == "gg@gmail.com");
            await _userManager.AddToRoleAsync(user, Utility.Helper.Admin);
        }
    }
}