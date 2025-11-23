using Microsoft.EntityFrameworkCore;
using PolicyNotesServiceApi.Models;

namespace PolicyNotesServiceApi.Data;

public class PolicyNotesDbContext : DbContext
{
    public PolicyNotesDbContext(DbContextOptions<PolicyNotesDbContext> options)
        : base(options)
    {
    }

    public DbSet<PolicyNote> PolicyNotes { get; set; }
}