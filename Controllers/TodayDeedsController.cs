using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Samo.Common;
using Samo.Contracts;
using Samo.DataAccess;
using Samo.DTOs;
using Samo.Models;
using Samo.Models.Contracts;
using Samo.Utility;

namespace Samo.Controllers;

[ApiController]
[Route($"{Constants.API_V1}/")]
[Authorize(AuthenticationSchemes = "Bearer", Policy = Constants.REQUIRE_NAME)]
public class TodayDeedsController(SamoDbContext dbContext) : BaseController(dbContext)
{
    private const string BAD_REQUEST_TEXT = "Text property is null!";

    [HttpPost("todayDeed")]
    public async Task<IActionResult> AddRite([FromBody] TodayDeedsRequest request, CancellationToken ct)
    {
        if (request.Text == null)
        {
            return BadRequest(BAD_REQUEST_TEXT);
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name, ct);

        if (user == null)
        {
            return Unauthorized();
        }

        TodayDeed todayDeed = new TodayDeed(request.Level, request.Text, user);

        await _context.TodayDeeds.AddAsync(todayDeed, ct);

        await _context.SaveChangesAsync(ct);

        return Ok(new
        {
            id = todayDeed.Id
        });
    }

    [HttpPut("todayDeed/{id}")]
    public async Task<IActionResult> ModifyRite([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, [FromBody] TodayDeedsRequest request, int id, CancellationToken ct)
    {
        if (request.Text == null)
        {
            return BadRequest(BAD_REQUEST_TEXT);
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name, ct);

        if (user == null)
        {
            return Unauthorized();
        }

        var limits = Dates.TodayLimit(timeZone);
        TodayDeed? todayDeed = await _context.TodayDeeds.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id && t.CreatedAt >= limits[0] && t.CreatedAt < limits[1], ct);

        if (todayDeed == null)
        {
            return NoContent();
        }

        todayDeed.Level = request.Level;
        todayDeed.Text = request.Text;

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpDelete("todayDeed/{id}")]
    public async Task<IActionResult> RemoveRite([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, int id, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name, ct);

        if (user == null)
        {
            return Unauthorized();
        }

        var limits = Dates.TodayLimit(timeZone);
        var todayDeed = await _context.TodayDeeds.FirstOrDefaultAsync(t => user.Email == User.Identity!.Name && t.CreatedAt >= limits[0] && t.CreatedAt < limits[1] && t.Id == id, ct);

        if (todayDeed == null)
        {
            return NoContent();
        }

        _context.Remove(todayDeed);

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet("todayDeed")]
    public async Task<IActionResult> GetRites([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, [FromQuery] DateTime date, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name, ct);

        if (user == null)
        {
            return Unauthorized();
        }

        var limits = Dates.TodayLimit(date, timeZone);
        var todayDeeds = await _context.TodayDeeds.Where(t => user.Email == User.Identity!.Name)
            .Where(t => t.CreatedAt >= limits[0] && t.CreatedAt < limits[1]).ToArrayAsync(ct);
        var newTodayDeeds = todayDeeds.Select(r => new TodayDeedDTO
            {
                Id = r.Id,
                Level = r.Level,
                Text = r.Text,
                CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(r.CreatedAt, timeZone)
            });

        return Ok(new
        {
            data = newTodayDeeds
        });
    }
}