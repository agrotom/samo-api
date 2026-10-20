using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Samo.Models;

[Table("week_habit")]
public class WeekHabit : UserDependency
{
    public int Id { get; set; }
    public bool[] Checks { get; set; }
    public string Text { get; set; }
    public int Year { get; set; }
    public int Week { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public WeekHabit() {}

    public WeekHabit(bool[] checks, string text, int year, int week, User user) : base(user)
    {
        Checks = checks;
        Text = text;
        Year = year;
        Week = week;
    }
}