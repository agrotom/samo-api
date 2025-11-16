using System.Text.Json.Serialization;

namespace Samo.Contracts;

public class TodayDeedsRequest
{
    public int Level { get; set; }
    public string Text { get; set; }
}