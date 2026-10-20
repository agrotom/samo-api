using System.Text.Json.Serialization;
using Samo.DTOs;

public class ObjectiveDTO : DateDTO
{
    public int Id { get; set; }
    public string Label { get; set; }
    public string Text { get; set; }
    public bool Favorite { get; set; }
    public string BalanceType { get; set; }
    public DateTime DeadLine { get; set; }
    
    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; }
    public bool Completed { get; set; }
    public string GoalsLabel { get; set; }
}