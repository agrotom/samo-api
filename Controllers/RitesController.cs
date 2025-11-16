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
public class RitesController(SamoDbContext dbContext) : BaseController(dbContext)
{
    private const string BAD_REQUEST_TEXT = "Text property is null!";

    [HttpPost("rite")]
    public async Task<IActionResult> AddRite([FromBody] RiteDataRequest request, CancellationToken ct)
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

        await _context.Rites.AddAsync(new Rite(request.Completed, request.Text, request.SortOrder, user), ct);

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpPut("rite/{id}")]
    public async Task<IActionResult> ModifyRite([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, [FromBody] RiteDataRequest request, int id, CancellationToken ct)
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

        var rites = await _context.Rites.Where(t => user.Email == User.Identity!.Name && t.Id == id && DbFuncs.ToLocalTime(t.CreatedAt, timeZone) == DbFuncs.ToLocalTime(DateTime.Today.ToUniversalTime(), timeZone)).ToArrayAsync(ct);

        Rite? rite = await _context.Rites.FirstOrDefaultAsync(t => t.Id == id, ct);

        if (rite == null)
        {
            return NoContent();
        }

        rite.Completed = request.Completed;
        rite.Text = request.Text;
        rite.SortOrder = request.SortOrder;

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpDelete("rite/{id}")]
    public async Task<IActionResult> RemoveRite(int id, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name, ct);

        if (user == null)
        {
            return Unauthorized();
        }

        var rite = await _context.Rites.FirstOrDefaultAsync(t => user.Email == User.Identity!.Name && t.Id == id, ct);

        if (rite == null)
        {
            return NoContent();
        }

        _context.Remove(rite);

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet("rite")]
    public async Task<IActionResult> GetRites([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name, ct);

        if (user == null)
        {
            return Unauthorized();
        }

        var rites = await _context.Rites.Where(t => user.Email == User.Identity!.Name && DbFuncs.ToLocalTime(t.CreatedAt, timeZone) == DbFuncs.ToLocalTime(DateTime.Today.ToUniversalTime(), timeZone)).ToArrayAsync(ct);
        var newRites = rites.Select(r => new RiteDTO
            {
                Id = r.Id,
                Completed = r.Completed,
                Text = r.Text,
                SortOrder = r.SortOrder,
                CreatedAt = r.CreatedAt
            });

        return Ok(new
        {
            data = newRites
        });
    }
}