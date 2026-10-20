using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Samo.Common;
using Samo.DataAccess;
using Samo.DTOs;
using Samo.Models;
using Samo.Models.Contracts;
using Samo.Utility;

namespace Samo.Controllers;

[ApiController]
[Route($"{Constants.API_V1}/")]
[Authorize(AuthenticationSchemes = "Bearer", Policy = Constants.REQUIRE_NAME)]
public class TrackerController(SamoDbContext dbContext) : BaseController(dbContext)
{
    private const string BAD_REQUEST_TEXT = "Name property is null or current step and total steps have bad value!";

    [HttpPost("tracker")]
    public async Task<IActionResult> AddTracker([FromBody] TrackerDataRequest request, CancellationToken ct)
    {
        if (request.Name == null || request.CurrentStep > request.TotalSteps || request.CurrentStep < 0 || request.TotalSteps < 2)
        {
            return BadRequest(BAD_REQUEST_TEXT);
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name, ct);

        if (user == null)
        {
            return Unauthorized();
        }

        Tracker tracker = new Tracker(request.Name, request.CurrentStep, request.TotalSteps, user);

        await _context.Trackers.AddAsync(tracker, ct);

        await _context.SaveChangesAsync(ct);

        return Ok(new
        {
            id = tracker.Id
        });
    }

    [HttpPut("tracker/{id}")]
    public async Task<IActionResult> ModifyTracker([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, [FromBody] TrackerDataRequest request, int id, CancellationToken ct)
    {
        if (request.Name == null || request.CurrentStep > request.TotalSteps || request.CurrentStep < 0 || request.TotalSteps < 2)
        {
            return BadRequest(BAD_REQUEST_TEXT);
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name, ct);
        
        if (user == null)
        {
            return Unauthorized();
        }

        var limits = Dates.TodayLimit(timeZone);
        var trackers = await _context.Trackers.Where(t => user.Email == User.Identity!.Name && t.CreatedAt >= limits[0] && t.CreatedAt < limits[1]).ToArrayAsync(ct);

        Tracker? tracker = await _context.Trackers.FirstOrDefaultAsync(t => t.Id == id, ct);

        if (tracker == null)
        {
            return NoContent();
        }

        tracker.Name = request.Name;
        tracker.CurrentStep = request.CurrentStep;
        tracker.TotalSteps = request.TotalSteps;

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpDelete("tracker/{id}")]
    public async Task<IActionResult> RemoveTracker([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, int id, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name, ct);

        if (user == null)
        {
            return Unauthorized();
        }

        var limits = Dates.TodayLimit(timeZone);
        var tracker = await _context.Trackers.FirstOrDefaultAsync(t => user.Email == User.Identity!.Name && t.CreatedAt >= limits[0] && t.CreatedAt < limits[1] && t.Id == id, ct);

        if (tracker == null)
        {
            return NoContent();
        }

        _context.Remove(tracker);

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet("tracker")]
    public async Task<IActionResult> GetTrackers([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, [FromQuery] DateTime date, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name, ct);

        if (user == null)
        {
            return Unauthorized();
        }

        var limits = Dates.TodayLimit(date, timeZone);
        var trackers = await _context.Trackers.Where(t => user.Email == User.Identity!.Name)
            .Where(t => t.CreatedAt >= limits[0] && t.CreatedAt < limits[1]).ToArrayAsync(ct);
        var newTrackers = trackers.Select(t => new TrackerDTO
        {
            Id = t.Id,
            Name = t.Name,
            CurrentStep = t.CurrentStep,
            TotalSteps = t.TotalSteps,
            CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(t.CreatedAt, timeZone)
        });

        return Ok(new
        {
            data = newTrackers
        });
    }
}