namespace Samo.Models;

public class Goal : UserDependency
{
    public int Id { get; set; }
    public bool Completed { get; set; }
    public string Text { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Goal() {}

    public Goal(bool completed, string text, User user) : base(user)
    {
        Completed = completed;
        Text = text;
    }
}