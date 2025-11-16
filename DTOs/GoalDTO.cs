namespace Samo.DTOs;

public class GoalDTO : DateDTO
{
    public int Id { get; set; }
    public bool Completed { get; set; }
    public string Text { get; set; }
}