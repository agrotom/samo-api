using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
using Samo.Models;

[Table("objective")]
public class Objective : UserDependency
{
    public int Id { get; set; }
    public string Label { get; set; }
    public string Text { get; set; }
    public bool Favorite { get; set; }
    public string BalanceType { get; set; }
    public DateTime DeadLine { get; set; }
    public string ImageUrl { get; set; }
    public bool Completed { get; set; }
    public string GoalsLabel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Objective() {}

    public Objective(string label, string text, bool favorite, string balanceType,
        DateTime deadLine, string imageUrl, bool completed, string goalsLabel, User user) : base(user)
    {
        Label = label;
        Text = text;
        Favorite = favorite;
        BalanceType = balanceType;
        DeadLine = deadLine;
        ImageUrl = imageUrl;
        Completed = completed;
        GoalsLabel = goalsLabel;
    }
}