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
        try
        {
            var addedTodoItem = await _todoRepo.AddTodoItem(todoItem);
            return Ok(addedTodoItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTodos()
    {
        try
        {
            var todos = await _todoRepo.GetTodoItems();
            return Ok(todos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}
