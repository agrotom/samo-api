using System.Text.Json.Serialization;

namespace Samo.DTOs;

public class RiteDTO : DateDTO
{
    public int Id { get; set; }
    public bool Completed { get; set; }
    public string Text { get; set; }

    [JsonPropertyName("sort_order")]
    public int SortOrder { get; set; }

}