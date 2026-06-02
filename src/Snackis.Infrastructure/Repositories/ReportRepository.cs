using Microsoft.EntityFrameworkCore;
using Snackis.Domain.Entities;
using Snackis.Domain.Interface;
using Snackis.Infrastructure.Data;


namespace Snackis.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly SnackisDbContext _db;

        public ReportRepository(SnackisDbContext db)
        {
            _db = db;
        }


        public async Task<List<Report>> GetAllAsync() =>
        await _db.Reports
            .Include(r => r.Post)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        public async Task<Report?> GetOneAsync(int id) =>
      await _db.Reports
          .Include(r => r.Post)
          .FirstOrDefaultAsync(r => r.Id == id);

        public async Task CreateAsync(Report report)
        {
            _db.Reports.Add(report);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Report report)
        {
            _db.Reports.Update(report);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Report report)
        {
            _db.Reports.Remove(report);
            await _db.SaveChangesAsync();
        }



    }
}
