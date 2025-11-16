namespace Samo.Contracts;

public class ObjectiveDataRequest
{
    public string Label { get; set; }
    public string Text { get; set; }
    public bool Favorite { get; set; }
    public string BalanceType { get; set; }
    public DateTime DeadLine { get; set; }
    public bool Completed { get; set; }
    public string GoalsLabel { get; set; }
}