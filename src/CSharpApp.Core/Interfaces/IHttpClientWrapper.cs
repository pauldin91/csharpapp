
namespace CSharpApp.Core.Interfaces
{
    public interface IHttpClientWrapper<S>
        where S : IClientSettings, new()
    {
        Task<K?> Delete<K>(string itemRootUrl, int id);
        Task<K?> Get<K>(string itemRootUrl);
        Task<K?> Get<K>(string itemRootUrl, int id);
        Task<K?> Post<K>(string itemRootUrl, K item);
        Task<K?> Put<K>(string itemRootUrl, K item);
    }
}