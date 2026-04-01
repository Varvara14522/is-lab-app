using IsLabApp.Models;

namespace IsLabApp.Services;

public class InMemoryNotesService
{
    private readonly List<Note> _notes = new();
    private int _nextId = 1;
    private readonly object _lock = new();

    public IEnumerable<Note> GetAll()
    {
        lock (_lock) { return _notes.ToList(); }
    }

    public Note? GetById(int id)
    {
        lock (_lock) { return _notes.FirstOrDefault(n => n.Id == id); }
    }

    public Note Create(Note note)
    {
        lock (_lock)
        {
            note.Id = _nextId++;
            note.CreatedAt = DateTime.UtcNow;
            _notes.Add(note);
            return note;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var note = _notes.FirstOrDefault(n => n.Id == id);
            if (note is null) return false;
            _notes.Remove(note);
            return true;
        }
    }
}
