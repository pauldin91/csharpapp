using CSharpApp.Core.Config;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CSharpApp.Infrastructure.Services;

public class TodoService : ITodoService
{
    private readonly IHttpClientWrapper _clientWrapper;
    private readonly ILogger<TodoService> _logger;
    private readonly ToDoSettings _toDoSettings;

    public TodoService(ILogger<TodoService> logger, ToDoSettings toDoSettings,
        IHttpClientWrapper clientWrapper)
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
        try
        {
            return await _clientWrapper.Get<TodoRecord>(_toDoSettings.ItemRootUrl, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ToDo with {Id} could be fetched", id);
        }
        return new TodoRecord(0, 0, "", false);
    }
}