using Snackis.Domain.Entities;
using Snackis.Domain.Interface;

namespace Snackis.Application.Services;
public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;

    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<List<Report>> GetAllAsync() =>
        await _reportRepository.GetAllAsync();

    public async Task<Report?> GetByIdAsync(int id) =>
        await _reportRepository.GetOneAsync(id);

    public async Task CreateAsync(int postId, string reporterId, string reason)
    {
        var report = new Report
        {
            PostId = postId,
            ReporterId = reporterId,
            Reason = reason,
            CreatedAt = DateTime.Now,
            IsHandled = false
        };

        await _reportRepository.CreateAsync(report);
    }

    public async Task MarkAsHandledAsync(int id)
    {
        var report = await _reportRepository.GetOneAsync(id);

        if (report is null)
        {
            return;
        }

        report.IsHandled = true;

        await _reportRepository.UpdateAsync(report);
    }

    public async Task DeleteAsync(int id)
    {
        var report = await _reportRepository.GetOneAsync(id);

        if (report is null)
            return;

        await _reportRepository.DeleteAsync(report);
    }
}


