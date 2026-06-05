using Microsoft.AspNetCore.Mvc;
using Snackis.API.DTO;
using Snackis.Application.Services;

namespace Snackis.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postService.GetAllAsync();
            var result = new List<PostDto>();

            foreach (var post in posts)
            {
                var author = await _postService.GetAuthorAsync(post.UserId);

                result.Add(new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt,
                    AuthorName = author?.UserName ?? "Okänd",
                    CategoryName = post.Category?.Name ?? ""
                });
            }
            return Ok(result);
        }


        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var posts = await _postService.GetByCategoryAsync(categoryId);
            var result = new List<PostDto>();

            foreach (var post in posts)
            {
                var author = await _postService.GetAuthorAsync(post.UserId);

                result.Add(new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt,
                    AuthorName = author?.UserName ?? "Okänd",
                    CategoryName = post.Category?.Name ?? ""
                });
            }
            return Ok(result);
        }
    }
}
