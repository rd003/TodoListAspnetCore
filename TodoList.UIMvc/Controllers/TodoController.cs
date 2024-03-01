using Microsoft.AspNetCore.Mvc;
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
}
