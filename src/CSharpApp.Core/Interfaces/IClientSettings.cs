namespace CSharpApp.Core.Interfaces
{
    public interface IClientSettings
    {
        string BaseUrl { get; set; }
        string ItemRootUrl { get; set; }
    }
}