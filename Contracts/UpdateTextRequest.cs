using System.ComponentModel.DataAnnotations;

namespace Samo.Models.Contracts;

public class UpdateTextRequest
{
    public string Text { get; set; } = "";
}