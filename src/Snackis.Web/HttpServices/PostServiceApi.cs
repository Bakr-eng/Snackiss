using Snackis.Application.DTO;
using Snackis.Application.HttpServices;

namespace Snackis.Web.HttpServices
{
    public class PostServiceApi : IPostServiceApi
    {
        private readonly HttpClient _httpClient;

        public PostServiceApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PostDto>> GetAllPostsAsync()
        {
            var posts = await _httpClient.GetFromJsonAsync<List<PostDto>>("api/posts");
            return posts ?? new List<PostDto>();
        }
    }
}
