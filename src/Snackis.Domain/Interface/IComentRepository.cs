using Snackis.Domain.Entities;

namespace Snackis.Domain.Interface
{
    public interface IComentRepository
    {
        Task<List<Coment>> GetAllAsync();
        Task<List<Coment>> GetByPostAsync(int postId);
        Task<Coment?> GetOneAsync(int id);
        Task CreateAsync(Coment coment);
        Task UpdateAsync(Coment coment);
        Task DeleteAsync(Coment coment);
    }
}