using CSharpApp.Core.Config;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CSharpApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly IHttpClientWrapper<ToDoSettings> _clientWrapper;
    private readonly ILogger<TodoService> _logger;
    private readonly ToDoSettings _toDoSettings;

    public TodoService(ILogger<TodoService> logger, ToDoSettings toDoSettings,
        IHttpClientWrapper<ToDoSettings> clientWrapper)
    {
        _logger = logger;
        _toDoSettings = toDoSettings;
        _clientWrapper = clientWrapper;
    }

    public async Task<ReadOnlyCollection<TodoRecord>> GetAllTodos()
    {
        var response = await _clientWrapper.Get<List<TodoRecord>>(_toDoSettings.ItemRootUrl);

        return response!.AsReadOnly();
    }

    public async Task<TodoRecord?> GetTodoById(int id)
    {
        return await _clientWrapper.Get<TodoRecord>(_toDoSettings.ItemRootUrl, id);
    }
}