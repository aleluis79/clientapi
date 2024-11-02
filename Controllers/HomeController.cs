using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using TodoApp.Services;

namespace clientapi.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{

    private readonly ILogger<HomeController> _logger;

    private readonly ITodoService _todoService;
    

    public HomeController(ILogger<HomeController> logger, ITodoService todoService)
    {
        _logger = logger;
        _todoService = todoService;
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("pong");
    }

    [HttpGet("todos")]
    public List<TodoModel> GetTodos()
    {
        return _todoService.GetTodos();
    }
}
