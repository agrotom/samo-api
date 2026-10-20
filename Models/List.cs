using System.ComponentModel.DataAnnotations.Schema;

namespace Samo.Models;

[Table("lists")]
public class List : UserDependency
{
    public int Id { get; set; }
    public string Header { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List() {}

    public List(string header, User user) : base(user)
    {
        Header = header;
    }
}