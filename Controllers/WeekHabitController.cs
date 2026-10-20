using System.Globalization;
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
[Route($"{Constants.API_V1}/")]
[Authorize(AuthenticationSchemes = "Bearer", Policy = Constants.REQUIRE_NAME)]
public class WeekHabitsController(SamoDbContext dbContext) : BaseController(dbContext)
{
    [HttpPost("weekHabit/{id}")]
    public async Task<IActionResult> ModifyWeekHabit([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, [FromBody] WeekHabitRequest request, int id, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name);

        if (user == null)
        {
            return Unauthorized();
        }

        var userTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZone));

        var week = ISOWeek.GetWeekOfYear(userTime);
        var year = userTime.Year;

        WeekHabit? weekHabit = await _context.WeekHabits.FirstOrDefaultAsync(wh => wh.UserId == user.Id && wh.Id == id && wh.Year == year && wh.Week == week, ct);

        if (weekHabit == null)
        {
            return BadRequest("Use GET-method to recreate week habits!");
        }

        weekHabit.Text = request.Text;
        weekHabit.Checks = request.Checks;

        await _context.SaveChangesAsync(ct);

        return Ok();
    }  

    [HttpGet("weekHabit")]
    public async Task<IActionResult> GetWeekHabits([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, [FromQuery] DateTime date, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name);

        if (user == null)
        {
            return Unauthorized();
        }

        var userTime = TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.FindSystemTimeZoneById(timeZone));

        var week = ISOWeek.GetWeekOfYear(userTime);
        var year = userTime.Year;



        WeekHabit[] weekHabits = await _context.WeekHabits.Where(wh => wh.UserId == user.Id)
            .Where(t => t.Week == week && t.Year == year).ToArrayAsync(ct);

        if (TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date.ToUniversalTime(), timeZone).Date != TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, timeZone).Date)
        {
            return Ok(new
            {
                data = weekHabits
            });
        }

        if (weekHabits.Length < 3)
        {
            for (int i = 0; i < 3 - weekHabits.Length; i++)
            {
                await _context.AddAsync(new WeekHabit([ false, false, false, false, false, false, false ], "", year, week, user), ct);
            }

            await _context.SaveChangesAsync(ct);
            weekHabits = await _context.WeekHabits.Where(wh => wh.UserId == user.Id && DbFuncs.ToLocalTime(wh.CreatedAt, timeZone) == DbFuncs.ToLocalTime(DateTime.Today, timeZone)).ToArrayAsync(ct);
        }

        var newWeekHabits = weekHabits.Select(wh => new WeekHabitDTO
        {
            Id = wh.Id,
            Checks = wh.Checks,
            Text = wh.Text,
            CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(wh.CreatedAt, timeZone)
        });

        return Ok(new {
            data = newWeekHabits
        });
    }
}