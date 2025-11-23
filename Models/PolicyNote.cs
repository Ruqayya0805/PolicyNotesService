namespace PolicyNotesServiceApi.Models;

public class PolicyNote
{
    public int Id { get; set; }
    public string PolicyNumber { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}