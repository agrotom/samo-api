using System.ComponentModel.DataAnnotations.Schema;

namespace Samo.DTOs;

public class WeekHabitDTO : DateDTO
{
    public int Id { get; set; }
    public bool[] Checks { get; set; }
    public string Text { get; set; }
}