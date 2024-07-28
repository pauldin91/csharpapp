using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CSharpApp.Core
{
    public class HttpClientWrapper<S> : IHttpClientWrapper<S>
        where S : IClientSettings, new()
    {
        private readonly HttpClient _httpClient;
        private readonly S _settings;

        public HttpClientWrapper(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _settings = ConfigurationBinder.Get<S>(configuration.GetRequiredSection(typeof(S).Name))!;
            _httpClient = httpClientFactory.CreateClient(typeof(S).Name);
        }

        public async Task<K?> Delete<K>(string itemRootUrl, int id)
        {
            return await _httpClient.DeleteFromJsonAsync<K>($"{itemRootUrl}/{id}");
        }

        public async Task<K?> Get<K>(string itemRootUrl)
        {
            return await _httpClient.GetFromJsonAsync<K>(itemRootUrl);
        }

        public async Task<K?> Get<K>(string itemRootUrl, int id)
        {
            return await _httpClient.GetFromJsonAsync<K>($"{itemRootUrl}/{id}");
        }

        public async Task<K?> Post<K>(string itemRootUrl, K item)
        {
            var response = await _httpClient.PostAsJsonAsync(itemRootUrl, item);

            return JsonConvert.DeserializeObject<K>(await response.Content.ReadAsStringAsync());
        }

        public async Task<K?> Put<K>(string itemRootUrl, K item)
        {
            var response = await _httpClient.PutAsJsonAsync(itemRootUrl, item);

            return JsonConvert.DeserializeObject<K>(await response.Content.ReadAsStringAsync());
        }
    }
}