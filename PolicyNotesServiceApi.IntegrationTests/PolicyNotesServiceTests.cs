using Microsoft.EntityFrameworkCore;
using PolicyNotesServiceApi.Data;
using PolicyNotesServiceApi.Models;
using PolicyNotesServiceApi.Services;

namespace PolicyNotesServiceApi.IntegrationTests;

public class PolicyNotesServiceTests
{
    private PolicyNotesDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<PolicyNotesDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        return new PolicyNotesDbContext(options);
    }

    [Fact]
    public async Task CreateNoteAsync_ShouldAddNote_AndReturnIt()
    {
        
        var context = GetInMemoryDbContext();
        var service = new PolicyNotesService(context);
        var dto = new PolicyNoteCreateDto
        {
            PolicyNumber = "POL-TEST-001",
            Note = "Test note for policy"
        };

        
        var result = await service.CreateNoteAsync(dto);

        
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("POL-TEST-001", result.PolicyNumber);
        Assert.Equal("Test note for policy", result.Note);
        Assert.True(result.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task GetAllNotesAsync_ShouldReturnAllNotes()
    {
        
        var context = GetInMemoryDbContext();
        var service = new PolicyNotesService(context);

        await service.CreateNoteAsync(new PolicyNoteCreateDto { PolicyNumber = "POL-1", Note = "Note 1" });
        await service.CreateNoteAsync(new PolicyNoteCreateDto { PolicyNumber = "POL-2", Note = "Note 2" });

        
        var result = await service.GetAllNotesAsync();

        
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetNoteByIdAsync_WhenNoteExists_ShouldReturnNote()
    {
        
        var context = GetInMemoryDbContext();
        var service = new PolicyNotesService(context);
        var createdNote = await service.CreateNoteAsync(
            new PolicyNoteCreateDto { PolicyNumber = "POL-FIND", Note = "Find me" }
        );

       
        var result = await service.GetNoteByIdAsync(createdNote.Id);

        
        Assert.NotNull(result);
        Assert.Equal(createdNote.Id, result.Id);
        Assert.Equal("POL-FIND", result.PolicyNumber);
        Assert.Equal("Find me", result.Note);
    }

    [Fact]
    public async Task GetNoteByIdAsync_WhenNoteDoesNotExist_ShouldReturnNull()
    {
        
        var context = GetInMemoryDbContext();
        var service = new PolicyNotesService(context);

        
        var result = await service.GetNoteByIdAsync(999);

        
        Assert.Null(result);
    }
}
