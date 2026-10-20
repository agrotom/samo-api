using System.ComponentModel.DataAnnotations.Schema;

namespace Samo.Models;

[Table("rite")]
public class Rite : UserDependency
{
    public int Id { get; set; }
    public bool Completed { get; set; }
    public string Text { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Rite() {}

    public Rite(bool completed, string text, int sortOrder, User user) : base(user)
    {
        Completed = completed;
        Text = text;
        SortOrder = sortOrder;
    }
}