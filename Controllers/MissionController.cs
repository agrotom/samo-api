using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Samo.Common;
using Samo.Contracts;
using Samo.DataAccess;
using Samo.DTOs;
using Samo.Models;

namespace Samo.Controllers;

[ApiController]
[Route($"{Constants.API_V1}/mission")]
[Authorize(AuthenticationSchemes = "Bearer", Policy = Constants.REQUIRE_NAME)]
public class MissionController(SamoDbContext dbContext) : BaseController(dbContext)
{
    [HttpPut()]
    public async Task<IActionResult> ModifyMission([FromBody] MissionRequest request, CancellationToken ct)
    {
        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        Mission? mission = await _context.Missions.FirstOrDefaultAsync(m => m.UserId == user.Id, ct);

        if (mission == null)
        {
            await _context.Missions.AddAsync(new Mission(request.Text, user), ct);

            await _context.SaveChangesAsync(ct);

            return Created();
        }

        mission.Text = request.Text;

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet()]
    public async Task<IActionResult> GetMission(CancellationToken ct)
    {
        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        Mission? mission = await _context.Missions.FirstOrDefaultAsync(m => m.UserId == user.Id, ct);

        if (mission == null)
        {
            mission = new Mission("", user);
            await _context.Missions.AddAsync(mission, ct);

            await _context.SaveChangesAsync(ct);
        }

        return Ok( new
        {
            data = new MissionDTO
            {
                Text = mission.Text
            }
        });
    }
}