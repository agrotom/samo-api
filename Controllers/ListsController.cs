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
[Route($"{Constants.API_V1}/lists/")]
[Authorize(AuthenticationSchemes = "Bearer", Policy = Constants.REQUIRE_NAME)]
public class ListsController(SamoDbContext dbContext) : BaseController(dbContext)
{
    [HttpPost]
    public async Task<IActionResult> AddList([FromBody] ListDataRequest request, CancellationToken ct)
    {
        if (request.Header == null)
        {
            return BadRequest("One of the property of request is empty!");
        }

        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        List list = new List(request.Header, user);

        await _context.Lists.AddAsync(list, ct);

        await _context.SaveChangesAsync(ct);

        return Ok(new
        {
            id = list.Id
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ModifyList(int id, [FromBody] ListDataRequest request, CancellationToken ct)
    {
        if (request.Header == null)
        {
            return BadRequest("One of the property of request is empty!");
        }

        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        List? list = await _context.Lists.FirstOrDefaultAsync(obj => obj.UserId == user.Id && obj.Id == id, ct);

        if (list == null)
        {
            return NoContent();
        }

        list.Header = request.Header;

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveList(int id, CancellationToken ct)
    {
        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        List? list = await _context.Lists.FirstOrDefaultAsync(obj => obj.UserId == user.Id && obj.Id == id, ct);

        if (list == null)
        {
            return NoContent();
        }

        _context.Lists.Remove(list);

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetLists([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, CancellationToken ct)
    {
        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        List[] lists = await _context.Lists.Where(obj => obj.UserId == user.Id).ToArrayAsync(ct);

        return Ok( new {
            data = lists.Select(obj => new ListDataDTO
            {
                Id = obj.Id,
                Header = obj.Header,
                CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(obj.CreatedAt, timeZone)
            })
        });
    }

    [HttpPost("goals/{listId}")]
    public async Task<IActionResult> AddListGoal(int listId, [FromBody] ListGoalDataRequest request, CancellationToken ct)
    {
        if (request.Text == null)
        {
            return BadRequest("One of the property of request is empty!");
        }

        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        List? list = await _context.Lists.FirstOrDefaultAsync(obj => obj.UserId == user.Id && obj.Id == listId, ct);

        if (list == null)
        {
            return BadRequest("No objective with ID: " + listId);
        }

        ListGoal goal = new ListGoal(request.Favorite, request.Completed, request.Text, list, user);

        await _context.ListGoals.AddAsync(goal, ct);

        await _context.SaveChangesAsync(ct);

        return Ok(new
        {
            id = goal.Id
        });
    }

    [HttpPut("goals/{goalId}")]
    public async Task<IActionResult> ModifyListGoal(int goalId, [FromBody] ListGoalDataRequest request, CancellationToken ct)
    {
        if (request.Text == null)
        {
            return BadRequest("One of the property of request is empty!");
        }

        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        ListGoal? goal = await _context.ListGoals.FirstOrDefaultAsync(goal => goal.UserId == user.Id && goal.Id == goalId, ct);

        if (goal == null)
        {
            return NoContent();
        }

        goal.Text = request.Text;
        goal.Completed = request.Completed;
        goal.Favorite = request.Favorite;

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpDelete("goals/{goalId}")]
    public async Task<IActionResult> RemoveListGoal(int goalId, CancellationToken ct)
    {
        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        ListGoal? goal = await _context.ListGoals.FirstOrDefaultAsync(goal => goal.UserId == user.Id && goal.Id == goalId, ct);

        if (goal == null)
        {
            return NoContent();
        }

        _context.ListGoals.Remove(goal);

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet("goals/{id}")]
    public async Task<IActionResult> GetListGoals([FromHeader(Name = Constants.TIME_ZONE_HEADER_NAME)] string timeZone, int id, CancellationToken ct)
    {
        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        List? list = await _context.Lists.FirstOrDefaultAsync(obj => obj.UserId == user.Id && obj.Id == id, ct);

        if (list == null)
        {
            return BadRequest("No list with ID: " + id);
        }

        ListGoal[] goal = await _context.ListGoals.Where(goal => goal.UserId == user.Id && goal.ListId == id).ToArrayAsync(ct);

        return Ok(new {
            data = goal.Select(goal => new ListGoalDataDTO
            {
                Id = goal.Id,
                Text = goal.Text,
                Completed = goal.Completed,
                Favorite = goal.Favorite,
                CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(goal.CreatedAt, timeZone)
            })
        });
    }
}