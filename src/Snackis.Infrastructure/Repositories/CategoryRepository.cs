using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snackis.Domain.Interface;
using Snackis.Domain.Entities;
using Snackis.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Snackis.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SnackisDbContext _db;

        public CategoryRepository(SnackisDbContext db)
        {
            _db = db;
        }

        public async Task<List<Category>> GetAllAsync() =>
            await _db.Categories
                .Include(c => c.SubCategories)
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync();

        public async Task<Category?> GetOneAsync(int id) =>
            await _db.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task CreateAsync(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }
    }
}