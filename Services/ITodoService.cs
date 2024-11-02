using TodoApp.Models;

namespace TodoApp.Services;

public interface ITodoService {
    List<TodoModel> GetTodos();
}