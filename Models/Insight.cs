using System.ComponentModel.DataAnnotations;

namespace Samo.Models;

public class Insight(string text) : DateUnique
{
    public string Text { get; set; } = text;
}