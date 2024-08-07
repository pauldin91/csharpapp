using CSharpApp.Core.Dtos;
using System.Collections.ObjectModel;

namespace CSharpApp.Core.Interfaces
{
    public interface IPostService 
    {
        Task<PostRecord?> AddPost(PostRecord post);

        Task<PostRecord?> DeletePostById(int id);

        Task<ReadOnlyCollection<PostRecord>> GetAllPosts();

        Task<PostRecord?> GetPostById(int id);
    }
}