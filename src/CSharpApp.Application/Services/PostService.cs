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
        private readonly HttpClient _client;
        private readonly ILogger<PostService> _logger;
        private readonly PostSettings _postSettings;

        public PostService(ILogger<PostService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _postSettings = ConfigurationBinder.Get<PostSettings>(configuration.GetRequiredSection(typeof(PostSettings).Name))!;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_postSettings.BaseUrl!)
            };
        }

        public async Task<PostRecord?> AddPost(PostRecord post)
        {
            var response = await _client.PostAsJsonAsync(_postSettings.ItemRootUrl, post);

            return JsonConvert.DeserializeObject<PostRecord>(await response.Content.ReadAsStringAsync());
        }

        public async Task<PostRecord?> DeletePostById(int id)
        {
            var response = await _client.DeleteFromJsonAsync<PostRecord>($"{_postSettings.ItemRootUrl}/{id}");

            return response;
        }

        public async Task<ReadOnlyCollection<PostRecord>> GetAllPosts()
        {
            var response = await _client.GetFromJsonAsync<List<PostRecord>>(_postSettings.ItemRootUrl);

            return response!.AsReadOnly();
        }

        public async Task<PostRecord?> GetPostById(int id)
        {
            var response = await _client.GetFromJsonAsync<PostRecord>($"{_postSettings.ItemRootUrl}/{id}");

            return response;
        }
    }
}