using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Samo.Common;
using Samo.DataAccess;
using Samo.DTOs;
using Samo.Models;
using Samo.Utility;

namespace Samo.Controllers;

[ApiController]
[Route($"{Constants.API_V1}/happiness/")]
[Authorize(AuthenticationSchemes = "Bearer", Policy = Constants.REQUIRE_NAME)]
public class HappinessController(SamoDbContext dbContext) : BaseController(dbContext)
{
    private async Task<(bool created, T entity)> 
    GetOrCreateTodayHappiness<T>(
        User user,
        Func<int, User, T> factory,
        DbSet<T> set,
        string timeZone,
        int level,
        bool replace,
        CancellationToken ct
    ) where T : Happiness
    {
        var happiness = await set.FirstOrDefaultAsync(wh => wh.UserId == user.Id && DbFuncs.ToLocalTime(wh.CreatedAt, timeZone) == DbFuncs.ToLocalTime(DateTime.Today.ToUniversalTime(), timeZone), ct);

        if (happiness == null)
        {
            happiness = factory(level, user);

            await set.AddAsync(happiness, ct);
            await _context.SaveChangesAsync(ct);

            return (true, happiness);
        }

        if (replace)
        {
            happiness.Level = level;
            await _context.SaveChangesAsync(ct);
        }

        return (false, happiness);
    }

    [HttpPost("{type}/{level}")]
    public async Task<IActionResult> ModifyHappiness(
        [FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone,
        string type,
        int level,
        CancellationToken ct)
    {
        if (level < 1 || level > 11)
            return BadRequest("Level must be 1–11");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == User.Identity!.Name);

        if (user == null)
            return Unauthorized();

        (bool created, Happiness entity) result = type switch
        {
            "work" => await GetOrCreateTodayHappiness(
                user, (lvl, u) => new WorkHappiness(lvl, u),
                _context.WorkHappiness, timeZone, level, true, ct
            ),

            "self" => await GetOrCreateTodayHappiness(
                user, (lvl, u) => new SelfHappiness(lvl, u),
                _context.SelfHappiness, timeZone, level, true, ct
            ),

            _ => default
        };

        if (result == default)
            return BadRequest("Unknown type");

        return result.created ? Created() : Ok();
    }

    [HttpGet("{type}")]
    public async Task<IActionResult> GetHappiness(
        [FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone,
        string type,
        CancellationToken ct)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == User.Identity!.Name);

        if (user == null)
            return Unauthorized();

        (bool created, Happiness entity) result = type switch
        {
            "work" => await GetOrCreateTodayHappiness(
                user, (lvl, u) => new WorkHappiness(lvl, u),
                _context.WorkHappiness, timeZone, 1, false, ct
            ),

            "self" => await GetOrCreateTodayHappiness(
                user, (lvl, u) => new SelfHappiness(lvl, u),
                _context.SelfHappiness, timeZone, 1, false, ct
            ),

            _ => default
        };

        if (result == default)
            return BadRequest("Unknown type");

        return Ok(new HappinessDTO
        {
            Level = result.entity.Level,
            CreatedAt = result.entity.CreatedAt
        });
    }
}