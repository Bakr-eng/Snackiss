using Microsoft.EntityFrameworkCore;
using Snackis.Application.Service;
using Snackis.Domain.Entities;
using Snackis.Infrastructure.Data;

namespace Snackis.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly SnackisDbContext _db;

    public CategoryService(SnackisDbContext db)
    {
        _db = db;
    }

    public async Task<List<Category>> GetAllAsync() =>
        await _db.Categories
            .Include(c => c.SubCategories)
            .Where(c => c.ParentCategoryId == null)
            .ToListAsync();

    public async Task CreateAsync(string name, int? parentId)
    {
        _db.Categories.Add(new Category { Name = name, ParentCategoryId = parentId });
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, string name)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat != null)
        {
            cat.Name = name;
            await _db.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat != null)
        {
            _db.Categories.Remove(cat);
            await _db.SaveChangesAsync();
        }
    }
}