using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Samo.Common;
using Samo.DataAccess;
using Samo.Models;
using Samo.Models.Contracts;
using Samo.Utility;

namespace Samo.Controllers;

[ApiController]
[Route($"{Constants.API_V1}/diary/")]
[Authorize(AuthenticationSchemes = "Bearer", Policy = Constants.REQUIRE_NAME)]
public class DiaryController(SamoDbContext dbContext) : BaseController(dbContext)
{

    private async Task UpdateTextEntity<T>(User user, DbSet<T> set, string data, Func<string, User, T> factory, string timeZone, CancellationToken ct) where T : TextEntity
    {
        T? entity = await set.FirstOrDefaultAsync(entity => entity.User.Email == User.Identity!.Name && DbFuncs.ToLocalTime(entity.CreatedAt, timeZone).Date == DbFuncs.ToLocalTime(DateTime.Today, timeZone).Date, ct);

        if (entity == null)
        {
            await set.AddAsync(factory(data, user), ct);
        }
        else
        {
            entity.Text = data;
        }
    }

    private async Task<string> GetTextEntity<T>(DbSet<T> set, DateTime date, string timeZone) where T : TextEntity
    {
        var limits = Dates.TodayLimit(date, timeZone);
        T? entity = await set.Where(entity => entity.User.Email == User.Identity!.Name).FirstOrDefaultAsync(entity => entity.CreatedAt >= limits[0] && entity.CreatedAt < limits[1]);

        return entity?.Text ?? "";
    }

    [HttpPut("{type}")]
    public async Task<IActionResult> UpdateInsight([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, [FromBody] UpdateTextRequest request, string type, CancellationToken ct)
    {
        if (request.Text == null)
        {
            return BadRequest("Text property is null!");    
        }

        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);
        
        switch (type)
        {
            case Constants.INSIGHTS:
                await UpdateTextEntity(user, _context.Insights, request.Text, (data, user) => new Insight(data, user), timeZone, ct);
                break;
            case Constants.FOCUSES:
                await UpdateTextEntity(user, _context.Focuses, request.Text, (data, user) => new Focus(data, user), timeZone, ct);
                break;
            case Constants.THANKS:
                await UpdateTextEntity(user, _context.Thanks, request.Text, (data, user) => new Thanks(data, user), timeZone, ct);
                break;
            case Constants.PROBLEMS:
                await UpdateTextEntity(user, _context.Problems, request.Text, (data, user) => new Problems(data, user), timeZone, ct);
                break;
            case Constants.RESULTS:
                await UpdateTextEntity(user, _context.Results, request.Text, (data, user) => new Models.Results(data, user), timeZone, ct);
                break;
            case Constants.TOMORROW_MAIN_TASKS:
                await UpdateTextEntity(user, _context.TomorrowMainTasks, request.Text, (data, user) => new TomorrowMainTasks(data, user), timeZone, ct);
                break;
            default:
                return NotFound();
        }
        
        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet("{type}")]
    public async Task<IActionResult> GetInsight([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, [FromQuery] DateTime date, string type)
    {
        string data;

        switch (type)
        {
            case Constants.INSIGHTS:
                data = await GetTextEntity(_context.Insights, date, timeZone);
                break;
            case Constants.FOCUSES:
                data = await GetTextEntity(_context.Focuses, date, timeZone);
                break;
            case Constants.THANKS:
                data = await GetTextEntity(_context.Thanks, date, timeZone);
                break;
            case Constants.PROBLEMS:
                data = await GetTextEntity(_context.Problems, date, timeZone);
                break;
            case Constants.RESULTS:
                data = await GetTextEntity(_context.Results, date, timeZone);
                break;
            case Constants.TOMORROW_MAIN_TASKS:
                data = await GetTextEntity(_context.TomorrowMainTasks, date, timeZone);
                break;
            default:
                return NotFound();
        }

        return Ok(data);
    }
}