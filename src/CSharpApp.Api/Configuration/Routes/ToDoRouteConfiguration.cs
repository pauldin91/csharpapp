using CSharpApp.Core.Config;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Configuration.Routes
{
    public static class ToDoRouteConfiguration
    {
        public static WebApplication AddToDoItemRoutes(this WebApplication app)
        {
            var toDoSettings = app.Services.GetRequiredService<ToDoSettings>();
            app.MapGet($"/{toDoSettings.ItemRootUrl}", async (ITodoService todoService) =>
            {
                var todos = await todoService.GetAllTodos();
                return todos;
            })
                .WithName(nameof(ITodoService.GetAllTodos))
                .WithOpenApi();

            app.MapGet($"/{toDoSettings.ItemRootUrl}" + "/{id:int}", async ([FromRoute] int id, ITodoService todoService) =>
            {
                return (await todoService.GetTodoById(id) is TodoRecord todo) ? Results.Ok(todo) : Results.NotFound();
            })
                .WithName(nameof(ITodoService.GetTodoById))
                .WithOpenApi();

            return app;
        }
    }
}