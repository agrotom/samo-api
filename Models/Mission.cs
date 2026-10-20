namespace Samo.Models;

public class Mission : UserDependency
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Mission() {}

    public Mission(string text, User user) : base(user)
    {
        Text = text;
    }
}