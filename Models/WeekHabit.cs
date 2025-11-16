using System.ComponentModel.DataAnnotations.Schema;

namespace Samo.Models;

[Table("week_habit")]
public class WeekHabit : UserDependency
{
    public int Id { get; set; }
    public bool[] Checks { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Today.ToUniversalTime();

    public WeekHabit() {}

    public WeekHabit(bool[] checks, string text, User user) : base(user)
    {
        Checks = checks;
        Text = text;
    }
}