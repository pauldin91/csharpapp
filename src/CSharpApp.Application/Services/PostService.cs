using CSharpApp.Core.Config;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Json;

namespace CSharpApp.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IHttpClientWrapper<ToDoSettings> _clientWrapper;
        private readonly ILogger<PostService> _logger;
        private readonly PostSettings _postSettings;

        public PostService(ILogger<PostService> logger, PostSettings postSettings,
            IHttpClientWrapper<ToDoSettings> clientWrapper)
        {
            _logger = logger;
            _postSettings = postSettings;
            _clientWrapper = clientWrapper;
        }

        public async Task<PostRecord?> AddPost(PostRecord post)
        {
            return await _clientWrapper.Post(_postSettings.ItemRootUrl, post);
        }

        public async Task<PostRecord?> DeletePostById(int id)
        {
            return await _clientWrapper.Delete<PostRecord>(_postSettings.ItemRootUrl, id);
        }

        public async Task<ReadOnlyCollection<PostRecord>> GetAllPosts()
        {
            var response = await _clientWrapper.Get<List<PostRecord>>(_postSettings.ItemRootUrl);

            return response!.AsReadOnly();
        }

        public async Task<PostRecord?> GetPostById(int id)
        {
            return await _clientWrapper.Get<PostRecord>(_postSettings.ItemRootUrl, id);
        }
    }
}