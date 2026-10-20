using System.ComponentModel.DataAnnotations.Schema;

namespace Samo.Models;

public class Happiness : UserDependency
{
    public int Id { get; set; }
    public int Level { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Happiness() {}

    public Happiness(int level, User user) : base(user)
    {
        Level = level;
    }
}