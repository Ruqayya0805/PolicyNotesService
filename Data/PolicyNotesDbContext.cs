using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Models;

namespace PolicyNotesService.Data;

public class PolicyNotesDbContext : DbContext
{
    public PolicyNotesDbContext(DbContextOptions<PolicyNotesDbContext> options)
        : base(options)
    {
    }

    public DbSet<PolicyNote> PolicyNotes { get; set; }
}