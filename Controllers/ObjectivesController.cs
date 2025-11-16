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
[Route($"{Constants.API_V1}/objectives/")]
[Authorize(AuthenticationSchemes = "Bearer", Policy = Constants.REQUIRE_NAME)]
public class ObjectiveController(SamoDbContext dbContext) : BaseController(dbContext)
{

    [HttpPost]
    public async Task<IActionResult> AddObjective([FromBody] ObjectiveDataRequest request, CancellationToken ct)
    {
        if (request.BalanceType == null || request.GoalsLabel == null || request.Label == null || request.Text == null)
        {
            return BadRequest("One of the property of request is empty!");
        }

        if (request.DeadLine.ToUniversalTime() <= DateTime.Now.ToUniversalTime())
        {
            return BadRequest("Dead line property cannot be earlier than today!");
        }

        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        Objective objective = new Objective(
            request.Label, request.Text, request.Favorite,
            request.BalanceType, request.DeadLine.ToUniversalTime(), "",
            request.Completed, request.GoalsLabel, user
        );

        await _context.Objectives.AddAsync(objective, ct);

        await _context.SaveChangesAsync(ct);

        return Ok(new
        {
            id = objective.Id
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ModifyObjective(int id, [FromBody] ObjectiveDataRequest request, CancellationToken ct)
    {
        if (request.BalanceType == null || request.GoalsLabel == null || request.Label == null || request.Text == null)
        {
            return BadRequest("One of the property of request is empty!");
        }

        if (request.DeadLine.ToUniversalTime() <= DateTime.Now.ToUniversalTime())
        {
            return BadRequest("Dead line property cannot be earlier than today!");
        }

        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        Objective? objective = await _context.Objectives.FirstOrDefaultAsync(obj => obj.UserId == user.Id && obj.Id == id, ct);

        if (objective == null)
        {
            return NoContent();
        }

        objective.Label = request.Label;
        objective.Text = request.Text;
        objective.Favorite = request.Favorite;
        objective.BalanceType = request.BalanceType;
        objective.DeadLine = request.DeadLine;
        objective.Completed = request.Completed;
        objective.GoalsLabel = request.GoalsLabel;

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveObjective(int id, CancellationToken ct)
    {
        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        Objective? objective = await _context.Objectives.FirstOrDefaultAsync(obj => obj.UserId == user.Id && obj.Id == id, ct);

        if (objective == null)
        {
            return NoContent();
        }

        _context.Objectives.Remove(objective);

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetObjectives(CancellationToken ct)
    {
        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        Objective[] objectives = await _context.Objectives.Where(obj => obj.UserId == user.Id).ToArrayAsync(ct);

        return Ok( new {
            data = objectives.Select(obj => new ObjectiveDTO
            {
                Id = obj.Id,
                Label = obj.Label,
                Text = obj.Text,
                Favorite = obj.Favorite,
                BalanceType = obj.BalanceType,
                DeadLine = obj.DeadLine,
                ImageUrl = obj.ImageUrl,
                Completed = obj.Completed,
                GoalsLabel = obj.GoalsLabel,
                CreatedAt = obj.CreatedAt
            })
        });
    }

    [HttpPost("goals/{objId}")]
    public async Task<IActionResult> AddObjectiveGoal(int objId, [FromBody] ObjectiveGoalDataRequest request, CancellationToken ct)
    {
        if (request.Text == null)
        {
            return BadRequest("One of the property of request is empty!");
        }

        if (request.DeadLine.ToUniversalTime() <= DateTime.Now.ToUniversalTime())
        {
            return BadRequest("Dead line property cannot be earlier than today!");
        }

        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        Objective? objective = await _context.Objectives.FirstOrDefaultAsync(obj => obj.UserId == user.Id && obj.Id == objId, ct);

        if (objective == null)
        {
            return BadRequest("No objective with ID: " + objId);
        }

        await _context.ObjectiveGoals.AddAsync(new ObjectiveGoal(request.Completed, request.Text, request.DeadLine, objective, user), ct);

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpPut("goals/{objId}/{goalId}")]
    public async Task<IActionResult> ModifyObjectiveGoal(int objId, int goalId, [FromBody] ObjectiveGoalDataRequest request, CancellationToken ct)
    {
        if (request.Text == null)
        {
            return BadRequest("One of the property of request is empty!");
        }

        if (request.DeadLine.ToUniversalTime() <= DateTime.Now.ToUniversalTime())
        {
            return BadRequest("Dead line property cannot be earlier than today!");
        }

        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        Objective? objective = await _context.Objectives.FirstOrDefaultAsync(obj => obj.UserId == user.Id && obj.Id == objId, ct);

        if (objective == null)
        {
            return BadRequest("No objective with ID: " + objId);
        }

        ObjectiveGoal? objectiveGoal = await _context.ObjectiveGoals.FirstOrDefaultAsync(goal => goal.UserId == user.Id && goal.Id == goalId, ct);

        if (objectiveGoal == null)
        {
            return NoContent();
        }

        objectiveGoal.Text = request.Text;
        objectiveGoal.Completed = request.Completed;
        objectiveGoal.DeadLine = request.DeadLine;

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpDelete("goals/{objId}/{goalId}")]
    public async Task<IActionResult> RemoveObjectiveGoal(int objId, int goalId, CancellationToken ct)
    {
        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        Objective? objective = await _context.Objectives.FirstOrDefaultAsync(obj => obj.UserId == user.Id && obj.Id == objId, ct);

        if (objective == null)
        {
            return BadRequest("No objective with ID: " + objId);
        }

        ObjectiveGoal? objectiveGoal = await _context.ObjectiveGoals.FirstOrDefaultAsync(goal => goal.UserId == user.Id && goal.Id == goalId, ct);

        if (objectiveGoal == null)
        {
            return NoContent();
        }

        _context.ObjectiveGoals.Remove(objectiveGoal);

        await _context.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet("goals/{id}")]
    public async Task<IActionResult> GetObjectiveGoals(int id, CancellationToken ct)
    {
        User user = await _context.Users.FirstAsync(user => user.Email == User.Identity!.Name, ct);

        Objective? objective = await _context.Objectives.FirstOrDefaultAsync(obj => obj.UserId == user.Id && obj.Id == id, ct);

        Console.WriteLine(objective == null);

        if (objective == null)
        {
            return BadRequest("No objective with ID: " + id);
        }

        ObjectiveGoal[] objectiveGoals = await _context.ObjectiveGoals.Where(goal => goal.UserId == user.Id && goal.ObjectiveId == id).ToArrayAsync(ct);

        return Ok(new {
            data = objectiveGoals.Select(goal => new ObjectiveGoalDTO
            {
                Id = goal.Id,
                Text = goal.Text,
                Completed = goal.Completed,
                DeadLine = goal.DeadLine,
                CreatedAt = goal.CreatedAt
            })
        });
    }
}