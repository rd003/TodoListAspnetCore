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
}
