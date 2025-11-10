using System.ComponentModel.DataAnnotations;

namespace Samo.Models;

public class DateUnique() : UserDependency
{
    [Key]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
}