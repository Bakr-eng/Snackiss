using Snackis.Application.DTO;

namespace Snackis.Application.HttpServices
{
    public interface IPostServiceApi
    {
        Task<List<PostDto>> GetAllPostsAsync();
    }
}