namespace Samo.DTOs;

public class TrackerDTO : DateDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CurrentStep { get; set; }
    public int TotalSteps { get; set; }

}