using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Samo.Common;
using Samo.DataAccess;
using Samo.DTOs;
using Samo.Models;
using Samo.Models.Contracts;

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
    public async Task<IActionResult> ModifyTracker([FromBody] TrackerDataRequest request, int id, CancellationToken ct)
    {
        Console.WriteLine("changed");
        if (request.Name == null || request.CurrentStep > request.TotalSteps || request.CurrentStep < 0 || request.TotalSteps < 2)
        {
            return BadRequest(BAD_REQUEST_TEXT);
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name, ct);
        
        if (user == null)
        {
            return Unauthorized();
        }

        var trackers = await _context.Trackers.Where(t => user.Email == User.Identity!.Name && t.CreatedAt == DateTime.Today.ToUniversalTime()).ToArrayAsync(ct);

        Models.Tracker? tracker = await _context.Trackers.FirstOrDefaultAsync(t => t.Id == id, ct);

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
    public async Task<IActionResult> RemoveTracker(int id, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name, ct);

        if (user == null)
        {
            return Unauthorized();
        }

        var tracker = await _context.Trackers.FirstOrDefaultAsync(t => user.Email == User.Identity!.Name && t.Id == id, ct);

        if (tracker == null)
        {
            return NoContent();
        }

        _context.Remove(tracker);

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet("tracker")]
    public async Task<IActionResult> GetTrackers(CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name, ct);

        if (user == null)
        {
            return Unauthorized();
        }

        var trackers = await _context.Trackers.Where(t => user.Email == User.Identity!.Name && t.CreatedAt == DateTime.Today.ToUniversalTime()).ToArrayAsync(ct);
        var newTrackers = trackers.Select(t => new TrackerDTO
        {
            Id = t.Id,
            Name = t.Name,
            CurrentStep = t.CurrentStep,
            TotalSteps = t.TotalSteps,
            CreatedAt = t.CreatedAt
        });

        return Ok(new
        {
            data = newTrackers
        });
    }
}