using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using PolicyNotesServiceApi.Data;
using PolicyNotesServiceApi.Models;

namespace PolicyNotesServiceApi.IntegrationTests;

public class PolicyNotesControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public PolicyNotesControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task PostNotes_ShouldReturn201Created()
    {
        var client = _factory.CreateClient();
        var request = new PolicyNoteCreateDto
        {
            PolicyNumber = "POL-12345",
            Note = "Customer called about premium payment"
        };

        var response = await client.PostAsJsonAsync("/notes", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);

        var note = await response.Content.ReadFromJsonAsync<PolicyNote>();
        Assert.NotNull(note);
        Assert.Equal("POL-12345", note.PolicyNumber);
        Assert.Equal("Customer called about premium payment", note.Note);
    }

    [Fact]
    public async Task GetNotes_ShouldReturn200OK()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/notes");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var notes = await response.Content.ReadFromJsonAsync<List<PolicyNote>>();
        Assert.NotNull(notes);
    }

    [Fact]
    public async Task GetNotesById_WhenNoteExists_ShouldReturn200OK()
    {
        var client = _factory.CreateClient();

        var createRequest = new PolicyNoteCreateDto
        {
            PolicyNumber = "POL-67890",
            Note = "Policy renewal reminder sent"
        };
        var createResponse = await client.PostAsJsonAsync("/notes", createRequest);
        var createdNote = await createResponse.Content.ReadFromJsonAsync<PolicyNote>();

        var response = await client.GetAsync($"/notes/{createdNote!.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var note = await response.Content.ReadFromJsonAsync<PolicyNote>();
        Assert.NotNull(note);
        Assert.Equal(createdNote.Id, note.Id);
        Assert.Equal("POL-67890", note.PolicyNumber);
    }

    [Fact]
    public async Task GetNotesById_WhenNoteMissing_ShouldReturn404NotFound()
    {
        var client = _factory.CreateClient();
        var nonExistentId = 99999;

        var response = await client.GetAsync($"/notes/{nonExistentId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetNotes_AfterCreatingMultipleNotes_ShouldReturnAllNotes()
    {
        var client = _factory.CreateClient();

        var note1 = new PolicyNoteCreateDto { PolicyNumber = "POL-001", Note = "First note" };
        var note2 = new PolicyNoteCreateDto { PolicyNumber = "POL-002", Note = "Second note" };

        await client.PostAsJsonAsync("/notes", note1);
        await client.PostAsJsonAsync("/notes", note2);

        var response = await client.GetAsync("/notes");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var notes = await response.Content.ReadFromJsonAsync<List<PolicyNote>>();
        Assert.NotNull(notes);
        Assert.True(notes.Count >= 2, $"Expected at least 2 notes, but got {notes.Count}");
    }
}

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<PolicyNotesDbContext>));

            services.AddDbContext<PolicyNotesDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PolicyNotesDbContext>();
            db.Database.EnsureCreated();
        });

        return base.CreateHost(builder);
    }
}