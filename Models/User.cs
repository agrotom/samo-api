using Microsoft.AspNetCore.Identity;

namespace Samo.Models;

public class User : IdentityUser
{
    public string SureName { get; set; }
    public string FirstName { get; set; }
    public string Achieve { get; set; }
    public DateOnly BirthDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User(string email, string sureName, string firstName, string achieve, DateOnly birthDate)
    {
        Email = email;
        UserName = email;
        SureName = sureName;
        FirstName = firstName;
        Achieve = achieve;
        BirthDate = birthDate;
    }
}