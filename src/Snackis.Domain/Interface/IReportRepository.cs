using Snackis.Domain.Entities;

namespace Snackis.Domain.Interface
{
    public interface IReportRepository
    {
        Task<List<Report>> GetAllAsync();
        Task<Report?> GetOneAsync(int id);
        Task CreateAsync(Report report);
        Task UpdateAsync(Report report);
        Task DeleteAsync(Report report);
    }
}