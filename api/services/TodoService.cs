using System.Collections.Concurrent;

namespace Api.Services;

public interface ITodoService
{
    IEnumerable<Todo> GetAll();
    Todo Create(string title);
    Todo? Toggle(int id);
    bool Delete(int id);
}

public class TodoService : ITodoService
{
    private readonly ConcurrentDictionary<int, Todo> _todos = new();
    private int _nextId;

    public IEnumerable<Todo> GetAll() => _todos.Values.OrderBy(todo => todo.Id);

    public Todo Create(string title)
    {
        var todo = new Todo(Interlocked.Increment(ref _nextId), title.Trim(), false);
        _todos[todo.Id] = todo;
        return todo;
    }

    public Todo? Toggle(int id)
    {
        if (!_todos.TryGetValue(id, out var todo))
            return null;

        var updated = todo with { Done = !todo.Done };
        _todos[id] = updated;
        return updated;
    }

    public bool Delete(int id) => _todos.TryRemove(id, out _);
}

public record Todo(int Id, string Title, bool Done);
