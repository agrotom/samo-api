namespace Samo.Models.Contracts;

public class TrackerDataRequest
{
    public string Name { get; set; }
    public int CurrentStep { get; set; }
    public int TotalSteps { get; set; }

}