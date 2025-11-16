using System.ComponentModel.DataAnnotations.Schema;

namespace Samo.Models;

[Table("objective_goal")]
public class ObjectiveGoal : Goal
{
    public DateTime DeadLine { get; set; }

    public Objective Objective { get; set; }
    public int ObjectiveId { get; set; }

    public ObjectiveGoal() {}

    public ObjectiveGoal(bool completed, string text, DateTime deadLine, Objective objective, User user) : base(completed, text, user)
    {
        DeadLine = deadLine;
        Objective = objective;
        ObjectiveId = objective.Id;
    }
}