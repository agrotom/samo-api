using System.ComponentModel.DataAnnotations.Schema;

namespace Samo.Models;

[Table("trackers")]
public class Tracker : UserDependency
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CurrentStep { get; set; }
    public int TotalSteps { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Today.ToUniversalTime();

    public Tracker () {}

    public Tracker(string name, int currentStep, int totalSteps, User user) : base(user)
    {
        Name = name;
        CurrentStep = currentStep;
        TotalSteps = totalSteps;
    }
}