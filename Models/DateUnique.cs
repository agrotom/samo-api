using System.ComponentModel.DataAnnotations;

namespace Samo.Models;

public class DateUnique : UserDependency
{

    public DateUnique()
    {

    }
    
    public DateUnique(User user) : base(user)
    {
        
    }

    [Key]
    public DateTime Date { get; set; } = DateTime.UtcNow;
}