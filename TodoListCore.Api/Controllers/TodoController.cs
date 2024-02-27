using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Data.Models;
using TodoList.Data.Repositories;

namespace TodoListCore.Api.Controllers;

[Route("api/todos")]
[ApiController]
public class TodoController : ControllerBase
{
    private ILogger<TodoController> _logger;
    private ITodoRepository _todoRepo;
    public TodoController(ILogger<TodoController> logger, ITodoRepository todoRepo)
    {
        _logger = logger;
        _todoRepo = todoRepo;
    }

    [HttpPost]
    public async Task<IActionResult> AddTodo(TodoItem todoItem)
    {
        return Ok();
    }
}
