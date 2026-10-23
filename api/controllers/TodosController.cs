using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/todos")]
public class TodosController(ITodoService todoService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() => Ok(todoService.GetAll());

    [HttpPost]
    public IActionResult Create(NewTodo input)
    {
        if (string.IsNullOrWhiteSpace(input.Title))
            return BadRequest(new { error = "Title is required" });

        var todo = todoService.Create(input.Title);
        return Created($"/api/todos/{todo.Id}", todo);
    }

    [HttpPut("{id:int}/toggle")]
    public IActionResult Toggle(int id)
    {
        var todo = todoService.Toggle(id);
        return todo is null ? NotFound() : Ok(todo);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        return todoService.Delete(id) ? NoContent() : NotFound();
    }
}

public record NewTodo(string Title);
