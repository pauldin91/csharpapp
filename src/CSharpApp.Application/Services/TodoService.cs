namespace CSharpApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly ILogger<TodoService> _logger;
    private readonly HttpClient _client;

    public TodoService(ILogger<TodoService> logger, 
        IConfiguration configuration)
    {
        _logger = logger;
        _client = new HttpClient { 
            BaseAddress = new Uri(configuration["BaseUrl"])
        };

    }

    public async Task<TodoRecord?> GetTodoById(int id)
    {
        var response = await _client.GetFromJsonAsync<TodoRecord>($"todos/{id}");

        return response;
    }

    public async Task<ReadOnlyCollection<TodoRecord>> GetAllTodos()
    {
        var response = await _client.GetFromJsonAsync<List<TodoRecord>>($"todos");

        return response!.AsReadOnly();
    }
}