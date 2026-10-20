using System.ComponentModel.DataAnnotations.Schema;

namespace Samo.Models;

[Table("today_deed")]
public class TodayDeed : UserDependency
{
    public int Id { get; set; }
    public int Level { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public TodayDeed() {}

    public TodayDeed(int level, string text, User user) : base(user)
    {
        Level = level;
        Text = text;
    }
}