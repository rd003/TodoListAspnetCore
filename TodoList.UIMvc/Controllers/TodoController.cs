using Microsoft.AspNetCore.Mvc;
using TodoList.Data.Models;
using TodoList.Data.Repositories;

namespace TodoList.UIMvc.Controllers;

public class TodoController : Controller
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodoRepository _todoRepo;

    public TodoController(ILogger<TodoController> logger, ITodoRepository todoRepo)
    {
        _logger = logger;
        _todoRepo = todoRepo;
    }

    // display todo list
    public async Task<IActionResult> Index()
    {
        var todos = await _todoRepo.GetTodoItems();
        return View(todos);
    }

    [HttpPost]
    public async Task<IActionResult> Index(TodoItem todo)
    {
        try
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");
            await _todoRepo.AddTodoItem(todo);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            TempData["msg"] = "Something went wrong";
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> DeleteTodo(int id)
    {
        try
        {
            var todoItem = await _todoRepo.GetTodoItemById(id);
            if (todoItem == null)
            {
                throw new Exception("This item does not exists");
            }
            await _todoRepo.DeleteTodoItem(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            TempData["msg"] = "Error on deleting item";
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ToggleCompletion(int id)
    {
        try
        {
            var todoItem = await _todoRepo.GetTodoItemById(id);
            if (todoItem == null)
            {
                throw new Exception("This item does not exists");
            }
            todoItem.IsCompleted = !todoItem.IsCompleted;
            await _todoRepo.UpdateTodoItem(todoItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            TempData["msg"] = "Error on deleting item";
        }
        return RedirectToAction(nameof(Index));
    }
}
