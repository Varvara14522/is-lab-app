using IsLabApp.Models;
using IsLabApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace IsLabApp.Endpoints;

public static class NotesEndpoints
{
    public static void MapNotesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/notes").WithTags("Notes");

        // GET /api/notes — список всех
        group.MapGet("/", (InMemoryNotesService service) =>
            Results.Ok(service.GetAll()));

        // GET /api/notes/{id} — одна заметка
        group.MapGet("/{id:int}", (int id, InMemoryNotesService service) =>
        {
            var note = service.GetById(id);
            return note is not null ? Results.Ok(note) : Results.NotFound();
        });

        // POST /api/notes — создать
        group.MapPost("/", (Note note, InMemoryNotesService service) =>
        {
            if (string.IsNullOrWhiteSpace(note.Title))
                return Results.BadRequest(new { message = "Заголовок не может быть пустым" });
            
            if (note.Title.Length < 3 || note.Title.Length > 200)
                return Results.BadRequest(new { message = "Заголовок от 3 до 200 символов" });
            
            var created = service.Create(note);
            return Results.Created($"/api/notes/{created.Id}", created);
        });

        // DELETE /api/notes/{id} — удалить
        group.MapDelete("/{id:int}", (int id, InMemoryNotesService service) =>
        {
            return service.Delete(id) ? Results.NoContent() : Results.NotFound();
        });
    }
}
