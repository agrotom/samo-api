namespace Samo.DTOs;

public class ListDataDTO : DateDTO
{
    public int Id { get; set; }
    public string Header { get; set; }
    public DateTime CreatedAt { get; set; }
}