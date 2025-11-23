namespace PolicyNotesServiceApi.Models;

public class PolicyNoteCreateDto
{
    public string PolicyNumber { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
}