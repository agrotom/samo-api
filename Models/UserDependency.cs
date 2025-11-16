using System.Text.Json.Serialization;

namespace Samo.Models;

public class UserDependency
{

    public UserDependency() {}

    public UserDependency(User user)
    {
        User = user;
        UserId = user.Id;
    }

    [JsonIgnore]
    public string UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; }
}