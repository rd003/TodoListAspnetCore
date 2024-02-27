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
            return CreatedAtAction(nameof(AddTodo), addedTodoItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, TodoItem todoItemToUpdate)
    {
        try
        {
            if (id != todoItemToUpdate.Id)
            {
                return BadRequest();
            }
            var todoItem = await _todoRepo.GetTodoItemById(todoItemToUpdate.Id);
            if (todoItem == null)
                return NotFound("Resource not found");
            await _todoRepo.UpdateTodoItem(todoItemToUpdate);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        try
        {
            var todoItem = await _todoRepo.GetTodoItemById(id);
            if (todoItem == null)
                return NotFound("Resource not found");
            await _todoRepo.DeleteTodoItem(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoItem(int id)
    {
        try
        {
            var todoItem = await _todoRepo.GetTodoItemById(id);
            if (todoItem == null)
                return NotFound();
            return Ok(todoItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}
