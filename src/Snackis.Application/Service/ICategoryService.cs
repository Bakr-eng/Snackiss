using Snackis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snackis.Application.Service
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
        Task CreateAsync(string name, int? parentId);
        Task UpdateAsync(int id, string name);
        Task DeleteAsync(int id);
    }
}
