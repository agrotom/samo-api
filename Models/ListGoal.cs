using System.ComponentModel.DataAnnotations.Schema;

namespace Samo.Models;

[Table("list_goals")]
public class ListGoal : Goal
{
    public bool Favorite { get; set; }

    public List List { get; set; }
    public int ListId { get; set; }

    public ListGoal() {}

    public ListGoal(bool favorite, bool completed, string text, List list, User user) : base(completed, text, user)
    {
        Favorite = favorite;
        List = list;
        ListId = list.Id;
    }
}