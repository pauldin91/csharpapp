using CSharpApp.Core.Config;
using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CSharpApp.Application.Services
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper(IConfiguration configuration)
        {
            var _settings = ConfigurationBinder.Get<ClientSettings>(configuration.GetRequiredSection(typeof(ClientSettings).Name))!;
            _httpClient = new HttpClient
            {
               BaseAddress = new Uri(_settings.BaseUrl)
            };
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