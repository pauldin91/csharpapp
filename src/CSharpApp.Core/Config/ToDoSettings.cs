using CSharpApp.Core.Interfaces;

namespace CSharpApp.Core.Config
{
    public class ToDoSettings : IClientSettings
    {
        public string BaseUrl { get; set; }
        public string ItemRootUrl { get; set; }
    }
}