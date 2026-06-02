using Snackis.Domain.Entities;

namespace Snackis.Application.Services;

public interface IReportService
{
    Task<List<Report>> GetAllAsync();
    Task<Report?> GetByIdAsync(int id);
    Task CreateAsync(int postId, string reporterId, string reason);
    Task MarkAsHandledAsync(int id);
    Task DeleteAsync(int id);
}