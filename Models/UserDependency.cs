namespace Samo.Models;

public class UserDependency
{
    public required string UserId { get; set; }

    public required User User { get; set; }
}