using Microsoft.AspNetCore.Mvc;
using PolicyNotesService.Models;
using PolicyNotesService.Services;

namespace PolicyNotesService.Controllers;

public static class PolicyNotesController
{
    public static void MapPolicyNotesEndpoints(this WebApplication app)
    {
        app.MapPost("/notes", async ([FromBody] PolicyNoteCreateDto dto, IPolicyNotesService service) =>
        {
            var note = await service.CreateNoteAsync(dto);
            return Results.Created($"/notes/{note.Id}", note);
        })
        .WithName("CreateNote");

        app.MapGet("/notes", async (IPolicyNotesService service) =>
        {
            var notes = await service.GetAllNotesAsync();
            return Results.Ok(notes);
        })
        .WithName("GetAllNotes");

        app.MapGet("/notes/{id}", async (int id, IPolicyNotesService service) =>
        {
            var note = await service.GetNoteByIdAsync(id);
            return note is not null ? Results.Ok(note) : Results.NotFound();
        })
        .WithName("GetNoteById");
    }
}