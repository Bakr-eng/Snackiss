
using Snackis.Domain.Entities;
using Snackis.Domain.Interface;

namespace Snackis.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<List<Post>> GetAllAsync() =>
        await _postRepository.GetAllAsync();

    public async Task<Post?> GetByIdAsync(int id) =>
        await _postRepository.GetOneAsync(id);

    public async Task<List<Post>> GetByCategoryAsync(int categoryId) =>
        await _postRepository.GetByCategoryAsync(categoryId);

    public async Task CreateAsync(string title, string content, int categoryId, string userId, string? imageUrl = null)
    {
        var post = new Post
        {
            Title = title,
            Content = content,
            CategoryId = categoryId,
            UserId = userId,
            ImageUrl = imageUrl,
            CreatedAt = DateTime.Now

        };
        await _postRepository.CreateAsync(post);
    }

    public async Task UpdateAsync(int id, string title, string content)
    {
        var post = await _postRepository.GetOneAsync(id);
        if (post != null)
        {
            post.Title = title;
            post.Content = content;
            await _postRepository.UpdateAsync(post);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var post = await _postRepository.GetOneAsync(id);
        if (post != null)
        {
            await _postRepository.DeleteAsync(post);
        }
    }
}
