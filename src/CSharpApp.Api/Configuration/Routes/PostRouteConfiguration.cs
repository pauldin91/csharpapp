using CSharpApp.Core.Config;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Configuration.Routes
{
    public static class PostRouteConfiguration
    {
        public static WebApplication AddPostItemRoutes(this WebApplication app)
        {
            var postSettings = app.Services.GetRequiredService<PostSettings>();

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
                    return await postService.GetPostById(id) is PostRecord post ? Results.Ok(post) : Results.NotFound();
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
                return await postService.AddPost(post) is PostRecord created ? Results.Created($"/{postSettings.ItemRootUrl}", created) : Results.NotFound();
            })
                .WithName(nameof(IPostService.AddPost))
                .WithOpenApi();

            app.MapDelete($"/{postSettings.ItemRootUrl}" + "/{id:int}", async ([FromRoute] int id, IPostService postService) =>
            {
                try
                {
                    return await postService.DeletePostById(id) is PostRecord deleted ? Results.Ok() : Results.NotFound();
                }
                catch (Exception ex)
                {
                    return Results.NotFound();
                }
            })
                .WithName(nameof(IPostService.DeletePostById))
                .WithOpenApi();

            return app;
        }
    }
}