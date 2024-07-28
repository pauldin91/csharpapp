using CSharpApp.Core.Config;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace CSharpApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly HttpClient _client;
    private readonly ILogger<TodoService> _logger;
    private readonly ToDoSettings _toDoSettings;

    public TodoService(ILogger<TodoService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _toDoSettings = ConfigurationBinder.Get<ToDoSettings>(configuration.GetRequiredSection(typeof(ToDoSettings).Name))!;
        _client = new HttpClient
        {
            BaseAddress = new Uri(_toDoSettings.BaseUrl!)
        };
    }

    public async Task<ReadOnlyCollection<TodoRecord>> GetAllTodos()
    {
        var response = await _client.GetFromJsonAsync<List<TodoRecord>>(_toDoSettings.ItemRootUrl);

        return response!.AsReadOnly();
    }

    public async Task<TodoRecord?> GetTodoById(int id)
    {
        var response = await _client.GetFromJsonAsync<TodoRecord>($"{_toDoSettings.ItemRootUrl}/{id}");

        return response;
    }
}