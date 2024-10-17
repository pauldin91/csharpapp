using CSharpApp.Application.Services;
using CSharpApp.Core;
using CSharpApp.Core.Config;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using CSharpApp.Infrastructure.Configuration;
using CSharpApp.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Collections.ObjectModel;

namespace CSharpApp.Tests
{
    [TestFixture]
    public class Tests
    {
        private PostService _postService;
        private TodoService _todoService;
        private PostSettings _postSettings;
        private ToDoSettings _todoSettings;
        private ClientSettings _settings;
        private IHttpClientWrapper _wrapper;
        [OneTimeSetUp]
        public void Setup()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"ClientSettings:BaseUrl", "https://jsonplaceholder.typicode.com/"},
                {"PostSettings:ItemRootUrl", "posts"},
                {"ToDoSettings:ItemRootUrl", "todos"}
            };

            var conf = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration!)
                .Build();
            _postSettings = ConfigurationBinder.Get<PostSettings>(conf.GetRequiredSection(nameof(PostSettings)))!;
            _todoSettings = ConfigurationBinder.Get<ToDoSettings>(conf.GetRequiredSection(nameof(ToDoSettings)))!;
            _settings = ConfigurationBinder.Get<ClientSettings>(conf.GetRequiredSection(nameof(ClientSettings)))!;

            var svc = new ServiceCollection()
                .AddDefaultConfiguration()
                .AddLogging(conf =>
            {
                new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console().CreateLogger();
            })
                .AddSingleton<IConfiguration>(conf)
                .AddHttpClients(conf)
                .BuildServiceProvider();
            _wrapper = svc.GetRequiredService<IHttpClientWrapper>();
            var plogger = svc.GetRequiredService<ILogger<PostService>>();
            var tlogger = svc.GetRequiredService<ILogger<TodoService>>();
            var http = new HttpClientWrapper(conf);
            _postService = new PostService(plogger, _postSettings, _wrapper);
            _todoService = new TodoService(tlogger, _todoSettings, _wrapper);
        }


        [Test]
        public void TestClientSettings()
        {
            Assert.That(_settings.BaseUrl, Is.EqualTo("https://jsonplaceholder.typicode.com/"));
        }

        [Test]
        public void TestPostSettings()
        {
            Assert.Multiple(() =>
            {
                Assert.That(_postSettings.BaseUrl, Is.EqualTo(""));
                Assert.That(_postSettings.ItemRootUrl, Is.EqualTo("posts"));
            });
        }

        [Test]
        public void TestToDoSettings()
        {
            Assert.Multiple(() =>
            {
                Assert.That(_todoSettings.BaseUrl, Is.EqualTo(""));
                Assert.That(_todoSettings.ItemRootUrl, Is.EqualTo("todos"));
            });
        }

        [Test]
        public void TestPostServiceGetAll()
        {
            var expectedCount = 100;
            var actual = _postService.GetAllPosts().Result;
            Assert.Multiple(() =>
            {
                Assert.That(actual, Has.Count.EqualTo(expectedCount));
                Assert.That(actual.GetType(), Is.EqualTo(typeof(ReadOnlyCollection<PostRecord>)));
            });
        }

        [Test]
        public void TestPostServiceGetById()
        {
            var expected = new PostRecord(1, 1, "sunt aut facere repellat provident occaecati excepturi optio reprehenderit", "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto");
            var actual = _postService.GetPostById(1).Result;
            Assert.Multiple(() =>
            {
                Assert.That(actual?.Id, Is.EqualTo(expected.Id));
                Assert.That(actual?.UserId, Is.EqualTo(expected.UserId));
                Assert.That(actual?.Title, Is.EqualTo(expected.Title));
                Assert.That(actual?.Body, Is.EqualTo(expected.Body));
            });
            expected = new PostRecord(0,0,"","");
            actual = _postService.GetPostById(111111).Result;
            Assert.Multiple(() =>
            {
                Assert.That(actual?.Id, Is.EqualTo(expected.Id));
                Assert.That(actual?.UserId, Is.EqualTo(expected.UserId));
                Assert.That(actual?.Title, Is.EqualTo(expected.Title));
                Assert.That(actual?.Body, Is.EqualTo(expected.Body));
            });

        }

        [Test]
        public void TestPostServiceDeleteById()
        {
            var expected = new PostRecord(0, 0,null,null);
            var actual = _postService.DeletePostById(1).Result;
            Assert.Multiple(() =>
            {
                Assert.That(actual?.Id, Is.EqualTo(expected.Id));
                Assert.That(actual?.UserId, Is.EqualTo(expected.UserId));
                Assert.That(actual?.Title, Is.EqualTo(expected.Title));
                Assert.That(actual?.Body, Is.EqualTo(expected.Body));
            });
        }
        [Test]
        public void TestToDoServiceGetAll()
        {
            var expectedCount = 200;
            var actual = _todoService.GetAllTodos().Result;
            Assert.Multiple(() =>
            {
                Assert.That(actual, Has.Count.EqualTo(expectedCount));
                Assert.That(actual.GetType(), Is.EqualTo(typeof(ReadOnlyCollection<TodoRecord>)));
            });
        }
        [Test]
        public void TestPostServicePost()
        {
            var expected = new PostRecord(0, 101, "string", "string");
            var actual = _postService.AddPost(new PostRecord(0, 0, "string", "string")).Result;
            Assert.Multiple(() =>
            {
                Assert.That(actual?.Id, Is.EqualTo(expected.Id));
                Assert.That(actual?.UserId, Is.EqualTo(expected.UserId));
                Assert.That(actual?.Title, Is.EqualTo(expected.Title));
                Assert.That(actual?.Body, Is.EqualTo(expected.Body));
            });
        }

        [Test]
        public void TestTodoServiceGetById()
        {
            var expected = new TodoRecord(1, 1, "delectus aut autem", false);
            var actual = _todoService.GetTodoById(1).Result;
            Assert.Multiple(() =>
            {
                Assert.That(actual?.Id, Is.EqualTo(expected.Id));
                Assert.That(actual?.UserId, Is.EqualTo(expected.UserId));
                Assert.That(actual?.Title, Is.EqualTo(expected.Title));
                Assert.That(actual?.Completed, Is.EqualTo(expected.Completed));
            });
            expected = new TodoRecord(0, 0, "", false);
            actual = _todoService.GetTodoById(111111).Result;
            Assert.Multiple(() =>
            {
                Assert.That(actual?.Id, Is.EqualTo(expected.Id));
                Assert.That(actual?.UserId, Is.EqualTo(expected.UserId));
                Assert.That(actual?.Title, Is.EqualTo(expected.Title));
                Assert.That(actual?.Completed, Is.EqualTo(expected.Completed));
            });
        }
    }
}