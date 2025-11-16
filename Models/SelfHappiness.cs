using System.ComponentModel.DataAnnotations.Schema;

namespace Samo.Models;

[Table("self_happiness")]
public class SelfHappiness : Happiness
{
    public SelfHappiness() {}

    public SelfHappiness(int level, User user) : base(level, user)
    {
    }
}