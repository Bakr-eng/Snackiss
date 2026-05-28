using Snackis.Domain.Entities;

namespace Snackis.Application.Services
{
    public interface IComentService
    {
        Task<List<Coment>> GetByPostAsync(int postId);
        Task CreateAsync(string Content, int postId, string userId);
        Task DeleteAsync(int id);
    }
}