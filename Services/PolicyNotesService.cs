using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Data;
using PolicyNotesService.Models;

namespace PolicyNotesService.Services;

public interface IPolicyNotesService
{
    Task<PolicyNote> CreateNoteAsync(PolicyNoteCreateDto dto);
    Task<IEnumerable<PolicyNote>> GetAllNotesAsync();
    Task<PolicyNote?> GetNoteByIdAsync(int id);
}

public class PolicyNotesService : IPolicyNotesService
{
    private readonly PolicyNotesDbContext _context;

    public PolicyNotesService(PolicyNotesDbContext context)
    {
        _context = context;
    }

    public async Task<PolicyNote> CreateNoteAsync(PolicyNoteCreateDto dto)
    {
        var note = new PolicyNote
        {
            PolicyNumber = dto.PolicyNumber,
            Note = dto.Note,
            CreatedAt = DateTime.UtcNow
        };

        _context.PolicyNotes.Add(note);
        await _context.SaveChangesAsync();
        return note;
    }

    public async Task<IEnumerable<PolicyNote>> GetAllNotesAsync()
    {
        return await _context.PolicyNotes.ToListAsync();
    }

    public async Task<PolicyNote?> GetNoteByIdAsync(int id)
    {
        return await _context.PolicyNotes.FindAsync(id);
    }
}