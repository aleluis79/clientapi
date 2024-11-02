using System.Text.Json.Serialization;

namespace TodoApp.Models;

public class TodoModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("completed")]
    public bool Completed { get; set; }
}