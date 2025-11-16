using System.Text.Json.Serialization;

namespace Samo.Contracts;

public class RiteDataRequest
{
    public bool Completed { get; set; }
    public string Text { get; set; }

    [JsonPropertyName("sort_order")]
    public int SortOrder { get; set; }
}