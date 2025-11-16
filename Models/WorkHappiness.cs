using System.ComponentModel.DataAnnotations.Schema;

namespace Samo.Models;

[Table("work_happiness")]
public class WorkHappiness : Happiness
{

    public WorkHappiness() {}

    public WorkHappiness(int level, User user) : base(level, user)
    {
        
    }
}