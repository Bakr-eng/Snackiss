using Snackis.Domain.Entities;
using Snackis.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snackis.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        public Task CreateAsync(Report report)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Report report)
        {
            throw new NotImplementedException();
        }

        public Task<List<Report>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Report?> GetOneAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Report report)
        {
            throw new NotImplementedException();
        }
    }
}
