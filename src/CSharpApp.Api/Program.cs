using CSharpApp.Core;
using CSharpApp.Core.Config;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using CSharpApp.Infrastructure.Configuration;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpClients(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

var postSettings = app.Services.GetRequiredService<PostSettings>();
var toDoSettings = app.Services.GetRequiredService<ToDoSettings>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context
        => await Results.Problem()
                     .ExecuteAsync(context)));
}

app.UseHttpsRedirection();

app.MapGet($"/{toDoSettings.ItemRootUrl}", async (ITodoService todoService) =>
    {
        var todos = await todoService.GetAllTodos();
        return todos;
    })
    .WithName(nameof(ITodoService.GetAllTodos))
    .WithOpenApi();

app.MapGet($"/{toDoSettings.ItemRootUrl}" + "/{id:int}", async ([FromRoute] int id, ITodoService todoService) =>
    {
        try
        {
            return (await todoService.GetTodoById(id) is TodoRecord todo) ? Results.Ok(todo) : Results.NotFound();
        }
        catch (Exception ex)
        {
            return Results.NotFound();
        }
    })
    .WithName(nameof(ITodoService.GetTodoById))
    .WithOpenApi();

app.MapGet($"/{postSettings.ItemRootUrl}", async (IPostService postService) =>
{
    var todos = await postService.GetAllPosts();
    return todos;
})
    .WithName(nameof(IPostService.GetAllPosts))
    .WithOpenApi();

app.MapGet($"/{postSettings.ItemRootUrl}" + "/{id:int}", async ([FromRoute] int id, IPostService postService) =>
{
    try
    {
        return (await postService.GetPostById(id) is PostRecord post) ? Results.Ok(post) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.NotFound();
    }
})
    .WithName(nameof(IPostService.GetPostById))
    .WithOpenApi();

app.MapPost($"/{postSettings.ItemRootUrl}", async ([FromBody] PostRecord post, IPostService postService) =>
{
    return (await postService.AddPost(post) is PostRecord created) ? Results.Created($"/{postSettings.ItemRootUrl}", created) : Results.NotFound();
})
    .WithName(nameof(IPostService.AddPost))
    .WithOpenApi();


app.MapDelete($"/{postSettings.ItemRootUrl}" + "/{id:int}", async ([FromRoute] int id, IPostService postService) =>
{
    try
    {
        return (await postService.DeletePostById(id) is PostRecord deleted) ? Results.Ok() : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.NotFound();
    }
})
    .WithName(nameof(IPostService.DeletePostById))
    .WithOpenApi();


app.Run();