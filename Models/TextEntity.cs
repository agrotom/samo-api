using System.ComponentModel.DataAnnotations.Schema;

namespace Samo.Models;

public class TextEntity : DateUnique
{

    public TextEntity()
    {

    }

    public TextEntity(string text, User user) : base(user)
    {
        Text = text;
    }

    public string Text { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

[Table("insights")]
public class Insight : TextEntity
{
    public Insight() { }

    public Insight(string text, User user) : base(text, user) {}
}

[Table("focuses")]
public class Focus : TextEntity
{
    public Focus() { }

    public Focus(string text, User user) : base(text, user) {}
}

[Table("thanks")]
public class Thanks : TextEntity
{
    public Thanks() { }

    public Thanks(string text, User user) : base(text, user) {}
}

[Table("problems")]
public class Problems : TextEntity
{
    public Problems() { }

    public Problems(string text, User user) : base(text, user) {}
}

[Table("results")]
public class Results : TextEntity
{
    public Results() { }

    public Results(string text, User user) : base(text, user) {}
}

[Table("tomorrowMainTasks")]
public class TomorrowMainTasks : TextEntity
{
    public TomorrowMainTasks() { }

    public TomorrowMainTasks(string text, User user) : base(text, user) {}
}