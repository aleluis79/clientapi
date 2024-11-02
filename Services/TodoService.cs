using TodoApp.Models;

namespace TodoApp.Services;

public class TodoService : ITodoService
{

    private readonly ILogger<TodoService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public TodoService(ILogger<TodoService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        this._httpClientFactory = httpClientFactory;
    }

    public List<TodoModel> GetTodos()
    {
        _logger.LogInformation("--> Ejecutando GetTodos");
        
        using var client = _httpClientFactory.CreateClient("jsonplaceholder");

        try {
            var response = client.GetFromJsonAsync<List<TodoModel>>("todos").Result;
            return response ?? [];
        } catch (Exception ex) {
            _logger.LogError("Error al obtener los todos: {Error}", ex);
        }

        return [];
        
    }
}