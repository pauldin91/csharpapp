using CSharpApp.Core.Interfaces;

namespace CSharpApp.Core.Config
{
    public class ClientSettings : IClientSettings
    {
        public string BaseUrl { get; set; }=string.Empty;
    }
}