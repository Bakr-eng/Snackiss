using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Application.DTO;
using Snackis.Application.HttpServices;

namespace Snackis.Web.Pages.Admin
{
    [Authorize(Policy = "ShouldBeAdmin")]
    public class PostsPageModel : PageModel
    {
        private readonly IPostServiceApi _postServiceApi;

        public List<PostDto> Posts { get; set; } = new();

        public PostsPageModel(IPostServiceApi postServiceApi)
        {
            _postServiceApi = postServiceApi;
        }
        public async Task OnGetAsync()
        {
            Posts = await _postServiceApi.GetAllPostsAsync();
        }
    }
}
